using HMS.Data;
using HMS.Models;
using HMS.Models.BedCategoriesViewModel;
using HMS.Models.ExpenseCategoriesViewModel;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace HMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ExpenseCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private string _hospitalId;
        private string _role;

        public ExpenseCategoriesController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _hospitalId = HttpContext.Session.GetString("HospitalId");
            _role = HttpContext.Session.GetString("Role");
            base.OnActionExecuting(context);

        }
        [Authorize(Roles = Pages.MainMenu.ExpenseCategories.RoleName)]
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
                    || obj.Description.ToLower().Contains(searchValue)
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

        private IQueryable<ExpenseCategoriesGridViewModel> GetGridItem(long hospitalId)
        {
            try
            {
                if (_role == "SuperAdmin")
                {
                    return (from _ExpenseCategories in _context.ExpenseCategories
                            join _hospital in _context.Hospital on _ExpenseCategories.HospitalId equals _hospital.Id
                            into hospitalGroup
                            from _hospital in hospitalGroup.DefaultIfEmpty()
                            where _ExpenseCategories.Cancelled == false
                            select new ExpenseCategoriesGridViewModel
                            {
                                Id = _ExpenseCategories.Id,
                                Name = _ExpenseCategories.Name,
                                Description = _ExpenseCategories.Description,
                                CreatedDate = _ExpenseCategories.CreatedDate,
                                ModifiedDate = _ExpenseCategories.ModifiedDate,
                                CreatedBy = _ExpenseCategories.CreatedBy,
                                ModifiedBy = _ExpenseCategories.ModifiedBy,
                                Hospital = _hospital.HospitalName

                            }).OrderByDescending(x => x.Id);
                }
                else
                {
                    return (from _ExpenseCategories in _context.ExpenseCategories
                            where _ExpenseCategories.Cancelled == false && _ExpenseCategories.HospitalId == hospitalId
                            select new ExpenseCategoriesGridViewModel
                            {
                                Id = _ExpenseCategories.Id,
                                Name = _ExpenseCategories.Name,
                                Description = _ExpenseCategories.Description,
                                CreatedDate = _ExpenseCategories.CreatedDate,
                                ModifiedDate = _ExpenseCategories.ModifiedDate,
                                CreatedBy = _ExpenseCategories.CreatedBy,
                                ModifiedBy = _ExpenseCategories.ModifiedBy,
                                Hospital = string.Empty
                            }).OrderByDescending(x => x.Id);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            ExpenseCategoriesCRUDViewModel vm = await _context.ExpenseCategories.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            ViewBag.ddlHospital = new SelectList(_iCommon.GetTableData<Hospital>(_context), "Id", "HospitalName");
            ViewBag.Role = _role;
            ExpenseCategoriesCRUDViewModel vm = new ExpenseCategoriesCRUDViewModel();   
            var data = await _context.ExpenseCategories.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (id > 0)
                vm = await _context.ExpenseCategories.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (data != null)
            {
                vm.HospitalId = data.HospitalId;
            }
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(ExpenseCategoriesCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        ExpenseCategories _ExpenseCategories = new ExpenseCategories();
                        if (vm.Id > 0)
                        {
                            _ExpenseCategories = await _context.ExpenseCategories.FindAsync(vm.Id);

                            vm.CreatedDate = _ExpenseCategories.CreatedDate;
                            vm.CreatedBy = _ExpenseCategories.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            if (_role == "SuperAdmin")
                            {
                                vm.HospitalId = vm.HospitalId;
                            }
                            else
                            {
                                vm.HospitalId = Convert.ToInt64(_hospitalId);
                            }
                            _context.Entry(_ExpenseCategories).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "Expense Categories Updated Successfully. ID: " + _ExpenseCategories.Id;
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            _ExpenseCategories = vm;
                            _ExpenseCategories.CreatedDate = DateTime.Now;
                            _ExpenseCategories.ModifiedDate = DateTime.Now;
                            _ExpenseCategories.CreatedBy = HttpContext.User.Identity.Name;
                            _ExpenseCategories.ModifiedBy = HttpContext.User.Identity.Name;
                            if (_role == "SuperAdmin")
                            {
                                _ExpenseCategories.HospitalId = vm.HospitalId;
                            }
                            else
                            {
                                _ExpenseCategories.HospitalId = Convert.ToInt64(_hospitalId);
                            }
                            _context.Add(_ExpenseCategories);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "Expense Categories Created Successfully. ID: " + _ExpenseCategories.Id;
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
                var _ExpenseCategories = await _context.ExpenseCategories.FindAsync(id);
                _ExpenseCategories.ModifiedDate = DateTime.Now;
                _ExpenseCategories.ModifiedBy = HttpContext.User.Identity.Name;
                _ExpenseCategories.Cancelled = true;

                _context.Update(_ExpenseCategories);
                await _context.SaveChangesAsync();
                return new JsonResult(_ExpenseCategories);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsExists(long id)
        {
            return _context.ExpenseCategories.Any(e => e.Id == id);
        }
    }
}
