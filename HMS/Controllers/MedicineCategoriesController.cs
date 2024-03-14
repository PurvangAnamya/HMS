using HMS.Data;
using HMS.Models;
using HMS.Models.MedicineCategoriesViewModel;
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
    public class MedicineCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public MedicineCategoriesController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
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

        private IQueryable<MedicineCategoriesGridViewModel> GetGridItem()
        {
            try
            {
                return (from _MedicineCategories in _context.MedicineCategories
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
            MedicineCategoriesCRUDViewModel vm = await _context.MedicineCategories.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            MedicineCategoriesCRUDViewModel vm = new MedicineCategoriesCRUDViewModel();
            if (id > 0) vm = await _context.MedicineCategories.Where(x => x.Id == id).SingleOrDefaultAsync();
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
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsExists(long id)
        {
            return _context.MedicineCategories.Any(e => e.Id == id);
        }
    }
}
