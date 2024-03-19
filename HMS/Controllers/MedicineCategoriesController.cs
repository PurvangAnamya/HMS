using HMS.Data;
using HMS.Models;
using HMS.Models.BedCategoriesViewModel;
using HMS.Models.MedicineCategoriesViewModel;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;


namespace HMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class MedicineCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private string _hospitalId;
        private readonly ILogger<MedicineCategoriesController> _logger;
        private string _role;

        public MedicineCategoriesController(ApplicationDbContext context, ICommon iCommon, ILogger<MedicineCategoriesController> logger)
        {
            _context = context;
            _iCommon = iCommon;
            _logger = logger;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _hospitalId = HttpContext.Session.GetString("HospitalId");
            _role = HttpContext.Session.GetString("Role");
            base.OnActionExecuting(context);
        }

        [Authorize(Roles = Pages.MainMenu.MedicineCategories.RoleName)]
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
                _logger.LogInformation("Error in getting Successfully.");
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting Medicine Categories.");
                throw;
            }

        }

        private IQueryable<MedicineCategoriesGridViewModel> GetGridItem(long hospitalId)
        {
            try
            {

                if (_role == "SuperAdmin")
                {
                    return (from _MedicineCategories in _context.MedicineCategories
                            join _hospital in _context.Hospital on _MedicineCategories.HospitalId equals _hospital.Id
                            into hospitalGroup
                            from _hospital in hospitalGroup.DefaultIfEmpty()
                            where _MedicineCategories.Cancelled == false
                            select new MedicineCategoriesGridViewModel
                            {
                                Id = _MedicineCategories.Id,
                                Name = _MedicineCategories.Name,
                                Description = _MedicineCategories.Description,
                                CreatedDate = _MedicineCategories.CreatedDate,
                                ModifiedDate = _MedicineCategories.ModifiedDate,
                                CreatedBy = _MedicineCategories.CreatedBy,
                                ModifiedBy = _MedicineCategories.ModifiedBy,
                                Hospital = _hospital.HospitalName

                            }).OrderByDescending(x => x.Id);
                }
                else
                {
                    return (from _MedicineCategories in _context.MedicineCategories
                            where _MedicineCategories.Cancelled == false && _MedicineCategories.HospitalId == hospitalId
                            select new MedicineCategoriesGridViewModel
                            {
                                Id = _MedicineCategories.Id,
                                Name = _MedicineCategories.Name,
                                Description = _MedicineCategories.Description,
                                CreatedDate = _MedicineCategories.CreatedDate,
                                ModifiedDate = _MedicineCategories.ModifiedDate,
                                CreatedBy = _MedicineCategories.CreatedBy,
                                ModifiedBy = _MedicineCategories.ModifiedBy,
                                Hospital = string.Empty

                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Add Medicine Categories.");
                throw;
            }
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            MedicineCategoriesCRUDViewModel vm = await _context.MedicineCategories.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            //MedicineCategoriesCRUDViewModel vm = new MedicineCategoriesCRUDViewModel();
            //if (id > 0) vm = await _context.MedicineCategories.Where(x => x.Id == id).SingleOrDefaultAsync();
            //return PartialView("_AddEdit", vm);

            ViewBag.ddlHospital = new SelectList(_iCommon.GetTableData<Hospital>(_context), "Id", "HospitalName");
            ViewBag.Role = _role;
            MedicineCategoriesCRUDViewModel vm = new MedicineCategoriesCRUDViewModel();
            var data = await _context.MedicineCategories.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (id > 0)
                vm = await _context.MedicineCategories.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (data != null)
            {
                vm.HospitalId = data.HospitalId;
            }
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(MedicineCategoriesCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        MedicineCategories _MedicineCategories = new MedicineCategories();
                        if (vm.Id > 0)
                        {
                            _MedicineCategories = await _context.MedicineCategories.FindAsync(vm.Id);

                            vm.CreatedDate = _MedicineCategories.CreatedDate;
                            vm.CreatedBy = _MedicineCategories.CreatedBy;
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
                           // vm.HospitalId = Convert.ToInt64(_hospitalId);
                            _context.Entry(_MedicineCategories).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "MedicineCategories Updated Successfully. ID: " + _MedicineCategories.Id;
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            _MedicineCategories = vm;
                            _MedicineCategories.CreatedDate = DateTime.Now;
                            _MedicineCategories.ModifiedDate = DateTime.Now;
                            _MedicineCategories.CreatedBy = HttpContext.User.Identity.Name;
                            _MedicineCategories.ModifiedBy = HttpContext.User.Identity.Name;
                            if (_role == "SuperAdmin")
                            {
                                _MedicineCategories.HospitalId = vm.HospitalId;
                            }
                            else
                            {
                                _MedicineCategories.HospitalId = Convert.ToInt64(_hospitalId);
                            }
                            //_MedicineCategories.HospitalId = Convert.ToInt64(_hospitalId);
                            _context.Add(_MedicineCategories);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "MedicineCategories Created Successfully. ID: " + _MedicineCategories.Id;
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
                        _logger.LogError( "Error in Add Or Update Medicine Categories.");
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
                var _MedicineCategories = await _context.MedicineCategories.FindAsync(id);
                _MedicineCategories.ModifiedDate = DateTime.Now;
                _MedicineCategories.ModifiedBy = HttpContext.User.Identity.Name;
                _MedicineCategories.Cancelled = true;

                _context.Update(_MedicineCategories);
                await _context.SaveChangesAsync();
                return new JsonResult(_MedicineCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Delete Medicine Categories.");
                throw;
            }
        }

        private bool IsExists(long id)
        {
            return _context.MedicineCategories.Any(e => e.Id == id);
        }
    }
}
