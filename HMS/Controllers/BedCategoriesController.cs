using HMS.Data;
using HMS.Models;
using HMS.Models.BedCategoriesViewModel;
using HMS.Pages;
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
    public class BedCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private string _hospitalId;
        private string _role;


        public BedCategoriesController(ApplicationDbContext context, ICommon iCommon)
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

        [Authorize(Roles = MainMenu.BedCategories.RoleName)]
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
                    || obj.CreatedDate.ToString().ToLower().Contains(searchValue)
                    || obj.ModifiedDate.ToString().ToLower().Contains(searchValue)
                    || obj.CreatedBy.ToLower().Contains(searchValue)
                    || obj.ModifiedBy.ToLower().Contains(searchValue)

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

        private IQueryable<BedCategoriesGridViewModel> GetGridItem(long hospitalId)
        {
            try
            {
                if (_role == "SuperAdmin")
                {
                    return (from _BedCategories in _context.BedCategories
                            join _hospital in _context.Hospital on _BedCategories.HospitalId equals _hospital.Id
                            into hospitalGroup
                            from _hospital in hospitalGroup.DefaultIfEmpty()
                            where _BedCategories.Cancelled == false
                            select new BedCategoriesGridViewModel
                            {
                                Id = _BedCategories.Id,
                                Name = _BedCategories.Name,
                                Description = _BedCategories.Description,
                                CreatedDate = _BedCategories.CreatedDate,
                                ModifiedDate = _BedCategories.ModifiedDate,
                                CreatedBy = _BedCategories.CreatedBy,
                                ModifiedBy = _BedCategories.ModifiedBy,
                                Hospital = _hospital.HospitalName

                            }).OrderByDescending(x => x.Id);
                }
                else
                {
                    return (from _BedCategories in _context.BedCategories
                            where _BedCategories.Cancelled == false && _BedCategories.HospitalId == hospitalId
                            select new BedCategoriesGridViewModel
                            {
                                Id = _BedCategories.Id,
                                Name = _BedCategories.Name,
                                Description = _BedCategories.Description,
                                CreatedDate = _BedCategories.CreatedDate,
                                ModifiedDate = _BedCategories.ModifiedDate,
                                CreatedBy = _BedCategories.CreatedBy,
                                ModifiedBy = _BedCategories.ModifiedBy,
                                Hospital = string.Empty

                            }).OrderByDescending(x => x.Id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            BedCategoriesCRUDViewModel vm = await _context.BedCategories.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            ViewBag.ddlHospital = new SelectList(_iCommon.GetTableData<Hospital>(_context), "Id", "HospitalName");
            ViewBag.Role = _role;
            BedCategoriesCRUDViewModel vm = new BedCategoriesCRUDViewModel();
            var data = await _context.BedCategories.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (id > 0)
                vm = await _context.BedCategories.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (data != null)
            {
                vm.HospitalId = data.HospitalId;
            }
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(BedCategoriesCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        BedCategories _BedCategories = new BedCategories();

                        if (vm.Id > 0)
                        {
                            _BedCategories = await _context.BedCategories.FindAsync(vm.Id);

                            vm.CreatedDate = _BedCategories.CreatedDate;
                            vm.CreatedBy = _BedCategories.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            if (_role == "SuperAdmin")
                            {
                                vm.HospitalId = _BedCategories.HospitalId;
                            }
                            else
                            {
                                vm.HospitalId = Convert.ToInt64(_hospitalId);
                            }
                            _context.Entry(_BedCategories).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "Bed Categories Updated Successfully. ID: " + _BedCategories.Id;
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            _BedCategories = vm;
                            _BedCategories.CreatedDate = DateTime.Now;
                            _BedCategories.ModifiedDate = DateTime.Now;
                            _BedCategories.CreatedBy = HttpContext.User.Identity.Name;
                            _BedCategories.ModifiedBy = HttpContext.User.Identity.Name;
                            if (_role == "SuperAdmin")
                            {
                                _BedCategories.HospitalId = vm.HospitalId;
                            }
                            else
                            {
                                _BedCategories.HospitalId = Convert.ToInt64(_hospitalId);
                            }

                            _context.Add(_BedCategories);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "Bed Categories Created Successfully. ID: " + _BedCategories.Id;
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var _BedCategories = await _context.BedCategories.FindAsync(id);
                _BedCategories.ModifiedDate = DateTime.Now;
                _BedCategories.ModifiedBy = HttpContext.User.Identity.Name;
                _BedCategories.Cancelled = true;

                _context.Update(_BedCategories);
                await _context.SaveChangesAsync();
                return new JsonResult(_BedCategories);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsExists(long id)
        {
            return _context.BedCategories.Any(e => e.Id == id);
        }
    }
}

