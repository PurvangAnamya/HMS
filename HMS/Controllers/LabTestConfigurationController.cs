using HMS.Data;
using HMS.Models;
using HMS.Models.LabTestConfigurationViewModel;
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
    public class LabTestConfigurationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public LabTestConfigurationController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.LabTestConfiguration.RoleName)]
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
                    || obj.LabTestsId.ToString().ToLower().Contains(searchValue)
                    || obj.Sorting.ToString().ToLower().Contains(searchValue)
                    || obj.ReportGroup.ToLower().Contains(searchValue)
                    || obj.NameOfTest.ToLower().Contains(searchValue)
                    || obj.Result.ToLower().Contains(searchValue)
                    || obj.NormalValue.ToLower().Contains(searchValue)

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

        private IQueryable<LabTestConfigurationCRUDViewModel> GetGridItem()
        {
            try
            {
                return (from _LabTestConfiguration in _context.LabTestConfiguration
                        join _LabTests in _context.LabTests on _LabTestConfiguration.LabTestsId equals _LabTests.Id
                        join _LabTestCategories in _context.LabTestCategories on _LabTests.Id equals _LabTestCategories.Id
                        where _LabTestConfiguration.Cancelled == false
                        select new LabTestConfigurationCRUDViewModel
                        {
                            Id = _LabTestConfiguration.Id,
                            LabTestsId = _LabTestConfiguration.LabTestsId,
                            LabTestName = _LabTests.LabTestName,
                            LabTestCategoryName = _LabTestCategories.Name,
                            Sorting = _LabTestConfiguration.Sorting,
                            ReportGroup = _LabTestConfiguration.ReportGroup,
                            NameOfTest = _LabTestConfiguration.NameOfTest,
                            Result = _LabTestConfiguration.Result,
                            NormalValue = _LabTestConfiguration.NormalValue,

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
            ManageLabTestConfigurationViewModel vm = new ManageLabTestConfigurationViewModel();
            LabTestConfigurationCRUDViewModel _LabTestConfigurationCRUDViewModel = new LabTestConfigurationCRUDViewModel();
            _LabTestConfigurationCRUDViewModel = await GetGridItem().Where(x => x.Id == id).SingleOrDefaultAsync();

            vm.LabTestConfigurationCRUDViewModel = _LabTestConfigurationCRUDViewModel;
            vm.listLabTestConfiguration = _context.LabTestConfiguration.Where(x => x.LabTestsId == id && x.Cancelled == false).OrderByDescending(x => x.Id).ToList();

            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddConfigureTest(Int64 id)
        {
            ManageLabTestConfigurationViewModel vm = new ManageLabTestConfigurationViewModel();
            LabTestConfigurationCRUDViewModel _LabTestConfigurationCRUDViewModel = new LabTestConfigurationCRUDViewModel();
            vm.LabTestsCRUDViewModel = await _iCommon.GetAllLabTests().Where(x => x.Id == id).SingleOrDefaultAsync();
            vm.listLabTestConfiguration = _context.LabTestConfiguration.Where(x => x.LabTestsId == id && x.Cancelled == false).OrderBy(x => x.Sorting).ToList();
            vm.LabTestConfigurationCRUDViewModel = _LabTestConfigurationCRUDViewModel;
            return PartialView("_AddEdit", vm);
        }
        public async Task<IActionResult> EditConfigureTest(Int64 id)
        {
            ManageLabTestConfigurationViewModel vm = new ManageLabTestConfigurationViewModel();
            LabTestConfigurationCRUDViewModel _LabTestConfigurationCRUDViewModel = new LabTestConfigurationCRUDViewModel();

            vm.LabTestConfigurationCRUDViewModel = await GetGridItem().Where(x => x.Id == id).SingleOrDefaultAsync();
            vm.listLabTestConfiguration = _context.LabTestConfiguration.Where(x => x.LabTestsId == vm.LabTestConfigurationCRUDViewModel.LabTestsId && x.Cancelled == false).OrderBy(x => x.Sorting).ToList();
            vm.LabTestsCRUDViewModel = await _iCommon.GetAllLabTests().Where(x => x.Id == vm.LabTestConfigurationCRUDViewModel.LabTestsId).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(LabTestConfigurationCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    LabTestConfiguration _LabTestConfiguration = new LabTestConfiguration();
                    if (vm.Id > 0)
                    {
                        _LabTestConfiguration = await UpdateConfig(vm);
                        TempData["successAlert"] = "Lab Test Configuration Item Updated Successfully. Name Of Test: " + _LabTestConfiguration.NameOfTest;
                    }
                    else
                    {
                        _LabTestConfiguration = await AddConfig(vm);
                        TempData["successAlert"] = "Lab Test Configuration Created Successfully. Name Of Test: " + _LabTestConfiguration.NameOfTest;
                    }
                    return new JsonResult(_LabTestConfiguration);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            TempData["errorAlert"] = "Operation failed.";
            return View("Index");
        }

        private async Task<LabTestConfiguration> AddConfig(LabTestConfigurationCRUDViewModel vm)
        {
            LabTestConfiguration _LabTestConfiguration = vm;
            _LabTestConfiguration.CreatedDate = DateTime.Now;
            _LabTestConfiguration.ModifiedDate = DateTime.Now;
            _LabTestConfiguration.CreatedBy = HttpContext.User.Identity.Name;
            _LabTestConfiguration.ModifiedBy = HttpContext.User.Identity.Name;
            _context.Add(_LabTestConfiguration);
            await _context.SaveChangesAsync();
            return _LabTestConfiguration;
        }
        private async Task<LabTestConfiguration> UpdateConfig(LabTestConfigurationCRUDViewModel vm)
        {
            var _LabTestConfiguration = await _context.LabTestConfiguration.FindAsync(vm.Id);
            vm.ModifiedDate = DateTime.Now;
            vm.ModifiedBy = HttpContext.User.Identity.Name;
            _context.Entry(_LabTestConfiguration).CurrentValues.SetValues(vm);
            _context.SaveChanges();
            return _LabTestConfiguration;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Int64 id)
        {
            try
            {
                var _LabTestConfiguration = await _context.LabTestConfiguration.FindAsync(id);
                _LabTestConfiguration.ModifiedDate = DateTime.Now;
                _LabTestConfiguration.ModifiedBy = HttpContext.User.Identity.Name;
                _LabTestConfiguration.Cancelled = true;

                _context.Update(_LabTestConfiguration);
                await _context.SaveChangesAsync();
                return new JsonResult(_LabTestConfiguration);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsExists(long id)
        {
            return _context.LabTestConfiguration.Any(e => e.Id == id);
        }
    }
}
