using HMS.Data;
using HMS.Models;
using HMS.Models.BedCategoriesViewModel;
using HMS.Models.LabTestCategoriesViewModel;
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
    public class LabTestCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private string _hospitalId;
        private readonly ILogger<LabTestCategoriesController> _logger;
        private string _role;

        public LabTestCategoriesController(ApplicationDbContext context, ICommon iCommon, ILogger<LabTestCategoriesController> logger)
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


        [Authorize(Roles = Pages.MainMenu.LabTestCategories.RoleName)]
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
                    || obj.ModifiedBy.ToLower().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                _logger.LogInformation("Error in getting Successfully.");
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IQueryable<LabTestCategoriesGridViewModel> GetGridItem(long hospitalId)
        {
            try
            {
                if (_role == "SuperAdmin")
                {
                    return (from _LabTestCategories in _context.LabTestCategories
                            join _hospital in _context.Hospital on _LabTestCategories.HospitalId equals _hospital.Id
                            into hospitalGroup
                            from _hospital in hospitalGroup.DefaultIfEmpty()
                            where _LabTestCategories.Cancelled == false
                            select new LabTestCategoriesGridViewModel
                            {
                                Id = _LabTestCategories.Id,
                                Name = _LabTestCategories.Name,
                                Description = _LabTestCategories.Description,
                                CreatedDate = _LabTestCategories.CreatedDate,
                                ModifiedDate = _LabTestCategories.ModifiedDate,
                                CreatedBy = _LabTestCategories.CreatedBy,
                                ModifiedBy = _LabTestCategories.ModifiedBy,
                                Hospital = _hospital.HospitalName

                            }).OrderByDescending(x => x.Id);
                }
                else
                {
                    return (from _LabTestCategories in _context.LabTestCategories
                            where _LabTestCategories.Cancelled == false && _LabTestCategories.HospitalId == hospitalId
                            select new LabTestCategoriesGridViewModel
                            {
                                Id = _LabTestCategories.Id,
                                Name = _LabTestCategories.Name,
                                Description = _LabTestCategories.Description,
                                CreatedDate = _LabTestCategories.CreatedDate,
                                ModifiedDate = _LabTestCategories.ModifiedDate,
                                CreatedBy = _LabTestCategories.CreatedBy,
                                ModifiedBy = _LabTestCategories.ModifiedBy,
                                Hospital = string.Empty

                            }).OrderByDescending(x => x.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting Lab Test Categories.");
                throw;
            }
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            LabTestCategoriesCRUDViewModel vm = await _context.LabTestCategories.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            //LabTestCategoriesCRUDViewModel vm = new LabTestCategoriesCRUDViewModel();
            //if (id > 0) vm = await _context.LabTestCategories.Where(x => x.Id == id).SingleOrDefaultAsync();
            //return PartialView("_AddEdit", vm);

            ViewBag.ddlHospital = new SelectList(_iCommon.GetTableData<Hospital>(_context), "Id", "HospitalName");
            ViewBag.Role = _role;
            LabTestCategoriesCRUDViewModel vm = new LabTestCategoriesCRUDViewModel();
            var data = await _context.LabTestCategories.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (id > 0)
                vm = await _context.LabTestCategories.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (data != null)
            {
                vm.HospitalId = data.HospitalId;
            }
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(LabTestCategoriesCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        HMS.Models.LabTestCategories _LabTestCategories = new HMS.Models.LabTestCategories();
                        if (vm.Id > 0)
                        {
                            _LabTestCategories = await _context.LabTestCategories.FindAsync(vm.Id);

                            vm.CreatedDate = _LabTestCategories.CreatedDate;
                            vm.CreatedBy = _LabTestCategories.CreatedBy;
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
                       
                            _context.Entry(_LabTestCategories).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "Lab Test Categories Updated Successfully. ID: " + _LabTestCategories.Id;
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            _LabTestCategories = vm;
                            _LabTestCategories.CreatedDate = DateTime.Now;
                            _LabTestCategories.ModifiedDate = DateTime.Now;
                            _LabTestCategories.CreatedBy = HttpContext.User.Identity.Name;
                            _LabTestCategories.ModifiedBy = HttpContext.User.Identity.Name;
                            if (_role == "SuperAdmin")
                            {
                                _LabTestCategories.HospitalId = vm.HospitalId;
                            }
                            else
                            {
                                _LabTestCategories.HospitalId = Convert.ToInt64(_hospitalId);
                            }
                            _context.Add(_LabTestCategories);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "Lab Test Categories Created Successfully. ID: " + _LabTestCategories.Id;
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
                        _logger.LogError( "Error in Add Or Update Lab Test Categories.");
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
                var _LabTestCategories = await _context.LabTestCategories.FindAsync(id);
                _LabTestCategories.ModifiedDate = DateTime.Now;
                _LabTestCategories.ModifiedBy = HttpContext.User.Identity.Name;
                _LabTestCategories.Cancelled = true;

                _context.Update(_LabTestCategories);
                await _context.SaveChangesAsync();
                return new JsonResult(_LabTestCategories);
            }
            catch (Exception)
            {
                _logger.LogError( "Error in Delete Lab Test Categories.");
                throw;
            }
        }

        private bool IsExists(long id)
        {
            return _context.LabTestCategories.Any(e => e.Id == id);
        }
    }
}
