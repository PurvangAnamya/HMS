using HMS.Data;
using HMS.Models;
using HMS.Models.MedicineLabelViewModel;
using HMS.Models.VitalSignsViewModel;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace HMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class MedicineLabelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly ILogger<MedicineLabelController> _logger;

        public MedicineLabelController(ApplicationDbContext context, ICommon iCommon, ILogger<MedicineLabelController> logger)
        {
            _context = context;
            _iCommon = iCommon;
            _logger = logger;
        }

        [Authorize(Roles = Pages.MainMenu.MedicineLabel.RoleName)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetDataTabelData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();

                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnAscDesc = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int resultTotal = 0;

                var _GetGridItem = _iCommon.GetCheckupGridItem();

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnAscDesc)))
                {
                    _GetGridItem = _GetGridItem.OrderBy(sortColumn + " " + sortColumnAscDesc);
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _GetGridItem = _GetGridItem.Where(obj => obj.PatientId.ToString().Contains(searchValue)
                    || obj.VisitId.ToLower().Contains(searchValue)
                    || obj.PatientName.ToLower().Contains(searchValue)
                    || obj.DoctorName.ToLower().Contains(searchValue)
                    || obj.CheckupDate.ToString().ToLower().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                _logger.LogInformation("Error in getting Successfully.");
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting Vital Signs.");
                throw;
            }

        }

        public async Task<IActionResult> PrintMedicineLabel(string VisitId)
        {
            MedicineLabelPrintViewModel _MedicineLabelPrintViewModel = new MedicineLabelPrintViewModel();
            var _CheckupSummary = _context.CheckupSummary.Where(x => x.VisitId == VisitId).SingleOrDefault();
            _MedicineLabelPrintViewModel.PatientId = _CheckupSummary.PatientId;
            _MedicineLabelPrintViewModel.HospitalName = _context.CompanyInfo.Where(x => x.Id == 1).SingleOrDefault().Name;
            var _PatientInfo = await _context.PatientInfo.Where(x => x.Id == _CheckupSummary.PatientId).SingleOrDefaultAsync();
            _MedicineLabelPrintViewModel.PatientName = _PatientInfo.FirstName + " " + _PatientInfo.LastName;

            _MedicineLabelPrintViewModel.listCheckupMedicineDetailsCRUDViewModel = _iCommon.GetCheckupMedicineDetails().Where(x => x.VisitId == _CheckupSummary.VisitId).ToList();

            return View(_MedicineLabelPrintViewModel);
        }

        public async Task<IActionResult> Details(long id)
        {
            var vm = await _iCommon.GetByCheckupDetails(id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            VitalSignsCRUDViewModel vm = new VitalSignsCRUDViewModel();
            if (id > 0) vm = await _context.CheckupSummary.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(VitalSignsCRUDViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CheckupSummary _VitalSigns = new CheckupSummary();
                    if (vm.CheckupSummaryId > 0)
                    {
                        _VitalSigns = await _context.CheckupSummary.FindAsync(vm.CheckupSummaryId);

                        vm.CreatedDate = _VitalSigns.CreatedDate;
                        vm.CreatedBy = _VitalSigns.CreatedBy;
                        vm.ModifiedDate = DateTime.Now;
                        vm.ModifiedBy = HttpContext.User.Identity.Name;
                        _context.Entry(_VitalSigns).CurrentValues.SetValues(vm);
                        await _context.SaveChangesAsync();
                        TempData["successAlert"] = "Vital Signs Updated Successfully. Visit Id: " + _VitalSigns.VisitId;
                        return RedirectToAction(nameof(Index));
                    }
                }
                TempData["errorAlert"] = "Operation failed.";
                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Add Or Update Vital Signs.");
                throw;
            }
        }
    }
}