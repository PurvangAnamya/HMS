using HMS.Data;
using HMS.Models.BedViewModel;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Dynamic.Core;

namespace HMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class BedController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private string _hospitalId;
        public BedController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _hospitalId = HttpContext.Session.GetString("HospitalId");
            base.OnActionExecuting(context);
        }


        [Authorize(Roles = Pages.MainMenu.Bed.RoleName)]
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
                    || obj.BedCategoryId.ToString().Contains(searchValue)
                    || obj.No.ToLower().Contains(searchValue)
                    || obj.Description.ToLower().Contains(searchValue)
                    || obj.CreatedDate.ToString().ToLower().Contains(searchValue)
                    || obj.ModifiedDate.ToString().ToLower().Contains(searchValue)
                    || obj.CreatedBy.ToLower().Contains(searchValue)

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

        private IQueryable<BedGridViewModel> GetGridItem(long hospitalId)
        {
            try
            {
                return (from _Bed in _context.Bed
                        join _BedCategories in _context.BedCategories on _Bed.BedCategoryId equals _BedCategories.Id
                        where _Bed.Cancelled == false && _BedCategories.HospitalId == hospitalId
                        select new BedGridViewModel
                        {
                            Id = _Bed.Id,
                            BedCategoryId = _Bed.BedCategoryId,
                            BedCategoryName = _BedCategories.Name,
                            No = _Bed.No,
                            Description = _Bed.Description,
                            CreatedDate = _Bed.CreatedDate,
                            ModifiedDate = _Bed.ModifiedDate,
                            CreatedBy = _Bed.CreatedBy,

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
            BedGridViewModel vm = await GetGridItem(Convert.ToInt64(_hospitalId)).Where(x => x.Id == id).SingleOrDefaultAsync();
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            ViewBag.ddlBedCategories = new SelectList(_iCommon.LoadddBedCategories(), "Id", "Name");
            BedCRUDViewModel vm = new BedCRUDViewModel();
            if (id > 0) vm = await _context.Bed.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> AddEdit(BedCRUDViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HMS.Models.Bed _Bed = new HMS.Models.Bed();
                    if (vm.Id > 0)
                    {
                        _Bed = await _context.Bed.FindAsync(vm.Id);

                        vm.CreatedDate = _Bed.CreatedDate;
                        vm.CreatedBy = _Bed.CreatedBy;
                        vm.ModifiedDate = DateTime.Now;
                        vm.ModifiedBy = HttpContext.User.Identity.Name;
                        _context.Entry(_Bed).CurrentValues.SetValues(vm);
                        await _context.SaveChangesAsync();
                        TempData["successAlert"] = "Bed Updated Successfully. ID: " + _Bed.Id;
                    }
                    else
                    {
                        //Check Duplicate Bed
                        var countBed = _context.Bed.Where(x => x.No == vm.No && x.BedCategoryId == vm.BedCategoryId && x.Cancelled != true).Count();
                        if (countBed > 0)
                        {
                            return new JsonResult("Bed no alredy exist. Bed no: " + vm.No + ", Description: " + vm.Description);
                        }
                        _Bed = vm;
                        _Bed.CreatedDate = DateTime.Now;
                        _Bed.ModifiedDate = DateTime.Now;
                        _Bed.CreatedBy = HttpContext.User.Identity.Name;
                        _Bed.ModifiedBy = HttpContext.User.Identity.Name;
                        _context.Add(_Bed);
                        await _context.SaveChangesAsync();
                        TempData["successAlert"] = "Bed Created Successfully. ID: " + _Bed.Id;
                    }
                }
                return new JsonResult("Success");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsExists(vm.Id))
                {
                    return new JsonResult(vm);
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Int64 id)
        {
            try
            {
                var _Bed = await _context.Bed.FindAsync(id);
                _Bed.ModifiedDate = DateTime.Now;
                _Bed.ModifiedBy = HttpContext.User.Identity.Name;
                _Bed.Cancelled = true;

                _context.Update(_Bed);
                await _context.SaveChangesAsync();
                return new JsonResult(_Bed);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsExists(long id)
        {
            return _context.Bed.Any(e => e.Id == id);
        }
    }
}
