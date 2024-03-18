using HMS.Data;
using HMS.Models;
using HMS.Models.CheckupMedicineDetailsViewModel;
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
    public class CheckupMedicineDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly ILogger<CheckupMedicineDetailsController> _logger;

        public CheckupMedicineDetailsController(ApplicationDbContext context, ICommon iCommon, ILogger<CheckupMedicineDetailsController> logger)
        {
            _context = context;
            _iCommon = iCommon;
            _logger = logger;
        }

        [Authorize(Roles = Pages.MainMenu.Checkup.RoleName)]
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

                var _GetGridItem = GetGridItem();
                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnAscDesc)))
                {
                    _GetGridItem = _GetGridItem.OrderBy(sortColumn + " " + sortColumnAscDesc);
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _GetGridItem = _GetGridItem.Where(obj => obj.Id.ToString().Contains(searchValue)
                    || obj.VisitId.ToLower().Contains(searchValue)
                    || obj.MedicineId.ToString().ToLower().Contains(searchValue)
                    || obj.NoofDays.ToString().ToLower().Contains(searchValue)
                    || obj.WhentoTake.ToLower().Contains(searchValue)
                    || obj.IsBeforeMeal.ToString().ToLower().Contains(searchValue)
                    || obj.CreatedDate.ToString().ToLower().Contains(searchValue)

                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                _logger.LogInformation("Error in getting Successfully.");
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting CheckupMedicineDetails.");
                throw;
            }

        }

        private IQueryable<CheckupMedicineDetailsGridViewModel> GetGridItem()
        {
            try
            {
                return (from _CheckupMedicineDetails in _context.CheckupMedicineDetails
                        where _CheckupMedicineDetails.Cancelled == false
                        select new CheckupMedicineDetailsGridViewModel
                        {
                            Id = _CheckupMedicineDetails.Id,
                            VisitId = _CheckupMedicineDetails.VisitId,
                            MedicineId = _CheckupMedicineDetails.MedicineId,
                            NoofDays = _CheckupMedicineDetails.NoofDays,
                            WhentoTake = _CheckupMedicineDetails.WhentoTake,
                            IsBeforeMeal = _CheckupMedicineDetails.IsBeforeMeal,
                            CreatedDate = _CheckupMedicineDetails.CreatedDate,

                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Add CheckupMedicineDetails.");
                throw;
            }
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            CheckupMedicineDetailsCRUDViewModel vm = await _context.CheckupMedicineDetails.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            CheckupMedicineDetailsCRUDViewModel vm = new CheckupMedicineDetailsCRUDViewModel();
            if (id > 0) vm = await _context.CheckupMedicineDetails.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(CheckupMedicineDetailsCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        CheckupMedicineDetails _CheckupMedicineDetails = new CheckupMedicineDetails();
                        if (vm.Id > 0)
                        {
                            _CheckupMedicineDetails = await _context.CheckupMedicineDetails.FindAsync(vm.Id);

                            vm.CreatedDate = _CheckupMedicineDetails.CreatedDate;
                            vm.CreatedBy = _CheckupMedicineDetails.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Entry(_CheckupMedicineDetails).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "CheckupMedicineDetails Updated Successfully. ID: " + _CheckupMedicineDetails.Id;
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            _CheckupMedicineDetails = vm;
                            _CheckupMedicineDetails.CreatedDate = DateTime.Now;
                            _CheckupMedicineDetails.ModifiedDate = DateTime.Now;
                            _CheckupMedicineDetails.CreatedBy = HttpContext.User.Identity.Name;
                            _CheckupMedicineDetails.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Add(_CheckupMedicineDetails);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "CheckupMedicineDetails Created Successfully. ID: " + _CheckupMedicineDetails.Id;
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    TempData["errorAlert"] = "Operation failed.";
                    return View("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IsExists(vm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError("Error in Add or Update CheckupMedicineDetails.");
                        throw;
                    }
                }
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Int64 id)
        {
            try
            {
                var _CheckupMedicineDetails = await _context.CheckupMedicineDetails.FindAsync(id);
                _CheckupMedicineDetails.ModifiedDate = DateTime.Now;
                _CheckupMedicineDetails.ModifiedBy = HttpContext.User.Identity.Name;
                _CheckupMedicineDetails.Cancelled = true;

                _context.Update(_CheckupMedicineDetails);
                await _context.SaveChangesAsync();
                return new JsonResult(_CheckupMedicineDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Delete CheckupMedicineDetails.");
                throw;
            }
        }

        private bool IsExists(long id)
        {
            return _context.CheckupMedicineDetails.Any(e => e.Id == id);
        }
    }
}
