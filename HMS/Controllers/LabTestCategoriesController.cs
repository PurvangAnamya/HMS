using HMS.Data;
using HMS.Models;
using HMS.Models.LabTestCategoriesViewModel;
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
    public class LabTestCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public LabTestCategoriesController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
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
                    || obj.Name.ToLower().Contains(searchValue)
                    || obj.Description.ToLower().Contains(searchValue)
                    || obj.CreatedDate.ToString().ToLower().Contains(searchValue)
                    || obj.ModifiedDate.ToString().ToLower().Contains(searchValue)
                    || obj.CreatedBy.ToLower().Contains(searchValue)
                    || obj.ModifiedBy.ToLower().Contains(searchValue));
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

        private IQueryable<LabTestCategoriesGridViewModel> GetGridItem()
        {
            try
            {
                return (from _LabTestCategories in _context.LabTestCategories
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
            LabTestCategoriesCRUDViewModel vm = await _context.LabTestCategories.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            LabTestCategoriesCRUDViewModel vm = new LabTestCategoriesCRUDViewModel();
            if (id > 0) vm = await _context.LabTestCategories.Where(x => x.Id == id).SingleOrDefaultAsync();
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
                        LabTestCategories _LabTestCategories = new LabTestCategories();
                        if (vm.Id > 0)
                        {
                            _LabTestCategories = await _context.LabTestCategories.FindAsync(vm.Id);

                            vm.CreatedDate = _LabTestCategories.CreatedDate;
                            vm.CreatedBy = _LabTestCategories.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
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
                throw;
            }
        }

        private bool IsExists(long id)
        {
            return _context.LabTestCategories.Any(e => e.Id == id);
        }
    }
}
