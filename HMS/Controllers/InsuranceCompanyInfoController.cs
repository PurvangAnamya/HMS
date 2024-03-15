using HMS.Data;
using HMS.Models;
using HMS.Models.InsuranceCompanyInfoViewModel;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static HMS.Pages.MainMenu;

namespace HMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class InsuranceCompanyInfoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private string _hospitalId;

        public InsuranceCompanyInfoController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _hospitalId = HttpContext.Session.GetString("HospitalId");
            base.OnActionExecuting(context);
        }

        [Authorize(Roles = Pages.MainMenu.InsuranceCompanyInfo.RoleName)]
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

                var _GetGridItem = GetGridItem(Convert.ToInt64(_hospitalId));

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
                    || obj.Name.ToLower().Contains(searchValue)
                    || obj.Address.ToLower().Contains(searchValue)
                    || obj.Phone.ToLower().Contains(searchValue)
                    || obj.Email.ToLower().Contains(searchValue)
                    || obj.CoverageDetails.ToLower().Contains(searchValue)
                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception)
            {
                throw;
            }

        }

        private IQueryable<InsuranceCompanyInfoGridViewModel> GetGridItem(long hospitalId)
        {
            try
            {
                return (from _InsuranceCompanyInfo in _context.InsuranceCompanyInfo
                        where _InsuranceCompanyInfo.Cancelled == false && _InsuranceCompanyInfo.HospitalId == hospitalId
                        select new InsuranceCompanyInfoGridViewModel
                        {
                            Id = _InsuranceCompanyInfo.Id,
                            Name = _InsuranceCompanyInfo.Name,
                            Address = _InsuranceCompanyInfo.Address,
                            Phone = _InsuranceCompanyInfo.Phone,
                            Email = _InsuranceCompanyInfo.Email,
                            CoverageDetails = _InsuranceCompanyInfo.CoverageDetails,
                            CreatedDate = _InsuranceCompanyInfo.CreatedDate,

                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            InsuranceCompanyInfoCRUDViewModel vm = await _context.InsuranceCompanyInfo.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            InsuranceCompanyInfoCRUDViewModel vm = new InsuranceCompanyInfoCRUDViewModel();
            if (id > 0) vm = await _context.InsuranceCompanyInfo.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(InsuranceCompanyInfoCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        HMS.Models.InsuranceCompanyInfo _InsuranceCompanyInfo = new HMS.Models.InsuranceCompanyInfo();
                        if (vm.Id > 0)
                        {
                            _InsuranceCompanyInfo = await _context.InsuranceCompanyInfo.FindAsync(vm.Id);

                            vm.CreatedDate = _InsuranceCompanyInfo.CreatedDate;
                            vm.CreatedBy = _InsuranceCompanyInfo.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            vm.HospitalId = Convert.ToInt64(_hospitalId);
                            _context.Entry(_InsuranceCompanyInfo).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "Insurance Company Info Updated Successfully. Name: " + _InsuranceCompanyInfo.Name;
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            _InsuranceCompanyInfo = vm;
                            _InsuranceCompanyInfo.CreatedDate = DateTime.Now;
                            _InsuranceCompanyInfo.ModifiedDate = DateTime.Now;
                            _InsuranceCompanyInfo.CreatedBy = HttpContext.User.Identity.Name;
                            _InsuranceCompanyInfo.ModifiedBy = HttpContext.User.Identity.Name;
                            _InsuranceCompanyInfo.HospitalId = Convert.ToInt64(_hospitalId);
                            _context.Add(_InsuranceCompanyInfo);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "Insurance Company Info Created Successfully. Name: " + _InsuranceCompanyInfo.Name;
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
                var _InsuranceCompanyInfo = await _context.InsuranceCompanyInfo.FindAsync(id);
                _InsuranceCompanyInfo.ModifiedDate = DateTime.Now;
                _InsuranceCompanyInfo.ModifiedBy = HttpContext.User.Identity.Name;
                _InsuranceCompanyInfo.Cancelled = true;

                _context.Update(_InsuranceCompanyInfo);
                await _context.SaveChangesAsync();
                return new JsonResult(_InsuranceCompanyInfo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsExists(long id)
        {
            return _context.InsuranceCompanyInfo.Any(e => e.Id == id);
        }
    }
}
