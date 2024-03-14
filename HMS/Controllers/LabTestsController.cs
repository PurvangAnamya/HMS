using HMS.ConHelper;
using HMS.Data;
using HMS.Helpers;
using HMS.Models;
using HMS.Models.LabTestConfigurationViewModel;
using HMS.Models.LabTestsViewModel;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class LabTestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly IDBOperation _iDBOperation;

        public LabTestsController(ApplicationDbContext context, ICommon iCommon, IDBOperation iDBOperation)
        {
            _context = context;
            _iCommon = iCommon;
            _iDBOperation = iDBOperation;
        }

        [Authorize(Roles = Pages.MainMenu.LabTests.RoleName)]
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

                var _GetGridItem = _iCommon.GetAllLabTests();
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
                    || obj.LabTestCategoryName.ToLower().Contains(searchValue)
                    || obj.LabTestName.ToLower().Contains(searchValue)
                    || obj.Unit.ToLower().Contains(searchValue)
                    || obj.ReferenceRange.ToLower().Contains(searchValue)
                    || obj.Status.ToString().ToLower().Contains(searchValue)
                    || obj.CreatedDate.ToString().ToLower().Contains(searchValue));
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

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            ManageLabTestConfigurationViewModel vm = new ManageLabTestConfigurationViewModel();
            vm.LabTestsCRUDViewModel = await _iCommon.GetAllLabTests().Where(x => x.Id == id).SingleOrDefaultAsync();
            vm.listLabTestConfiguration = _context.LabTestConfiguration.Where(x => x.LabTestsId == id && x.Cancelled == false).OrderBy(x => x.Sorting).ToList();
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            ViewBag._LoadddlLabTestCategories = new SelectList(_iCommon.LoadddlLabTestCategories(), "Id", "Name");

            LabTestsCRUDViewModel vm = new LabTestsCRUDViewModel();
            if (id > 0) vm = await _context.LabTests.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(LabTestsCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        LabTests _LabTests = new LabTests();
                        if (vm.Id > 0)
                        {
                            _LabTests = await _context.LabTests.FindAsync(vm.Id);

                            vm.CreatedDate = _LabTests.CreatedDate;
                            vm.CreatedBy = _LabTests.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Entry(_LabTests).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "Lab Tests Updated Successfully. ID: " + _LabTests.Id;
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            _LabTests = vm;
                            _LabTests.PaymentItemCode = StaticData.GetUniqueID("LAB");
                            _LabTests.CreatedDate = DateTime.Now;
                            _LabTests.ModifiedDate = DateTime.Now;
                            _LabTests.CreatedBy = HttpContext.User.Identity.Name;
                            _LabTests.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Add(_LabTests);
                            await _context.SaveChangesAsync();

                            TempData["successAlert"] = "Lab Tests Created Successfully. ID: " + _LabTests.Id;
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
                var _LabTests = await _context.LabTests.FindAsync(id);
                _LabTests.ModifiedDate = DateTime.Now;
                _LabTests.ModifiedBy = HttpContext.User.Identity.Name;
                _LabTests.Cancelled = true;

                _context.Update(_LabTests);
                await _context.SaveChangesAsync();
                return new JsonResult(_LabTests);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsExists(long id)
        {
            return _context.LabTests.Any(e => e.Id == id);
        }
    }
}