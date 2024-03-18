using HMS.Data;
using HMS.Models;
using HMS.Models.DepartmentViewModel;
using HMS.Pages;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace HMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly ILogger<DepartmentController> _logger;


        public DepartmentController(ApplicationDbContext context, ICommon iCommon, ILogger<DepartmentController> logger)
        {
            _context = context;
            _iCommon = iCommon;
            _logger = logger;
        }

        [Authorize(Roles = Pages.MainMenu.Department.RoleName)]
        [HttpGet]
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

                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                _logger.LogInformation("Error in getting Successfully.");
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error in getting Department.");
                throw;
            }
        }

        private IQueryable<DepartmentGridViewModel> GetGridItem()
        {
            try
            {
                return (from _Department in _context.Department
                        where _Department.Cancelled == false
                        select new DepartmentGridViewModel
                        {
                            Id = _Department.Id,
                            Name = _Department.Name,
                            Description = _Department.Description,
                            CreatedDate = _Department.CreatedDate,                           
                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error in Add Department.");
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            DepartmentCRUDViewModel vm = await _context.Department.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            DepartmentCRUDViewModel vm = new DepartmentCRUDViewModel();
            if (id > 0) vm = await _context.Department.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(DepartmentCRUDViewModel vm)
        {
            try
            {
                Department _Department = new Department();
                string _UserName = HttpContext.User.Identity.Name;

                if (vm.Id > 0)
                {
                    _Department = await _context.Department.FindAsync(vm.Id);

                    vm.CreatedDate = _Department.CreatedDate;
                    vm.CreatedBy = _Department.CreatedBy;
                    vm.ModifiedDate = DateTime.Now;
                    vm.ModifiedBy = _UserName;
                    _context.Entry(_Department).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "Department Updated Successfully. ID: " + _Department.Id;
                    return new JsonResult(_AlertMessage);
                }
                else
                {
                    _Department = vm;
                    _Department.CreatedDate = DateTime.Now;
                    _Department.ModifiedDate = DateTime.Now;
                    _Department.CreatedBy = _UserName;
                    _Department.ModifiedBy = _UserName;
                    _context.Add(_Department);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "Department Created Successfully. ID: " + _Department.Id;
                    return new JsonResult(_AlertMessage);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex,"Error in Add Or Update Department.");
                return new JsonResult(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Int64 id)
        {
            try
            {
                var _Department = await _context.Department.FindAsync(id);
                _Department.ModifiedDate = DateTime.Now;
                _Department.ModifiedBy = HttpContext.User.Identity.Name;
                _Department.Cancelled = true;

                _context.Update(_Department);
                await _context.SaveChangesAsync();
                return new JsonResult(_Department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error in Delete Department.");
                throw;
            }
        }

        private bool IsExists(long id)
        {
            return _context.Department.Any(e => e.Id == id);
        }
    }
}
