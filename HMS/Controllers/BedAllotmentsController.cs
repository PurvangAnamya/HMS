using HMS.Data;
using HMS.Models;
using HMS.Models.BedAllotmentsViewModel;
using HMS.Models.BedViewModel;
using HMS.Models.CommonViewModel;
using HMS.Pages;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace HMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class BedAllotmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly ILogger<BedAllotmentsController> _logger;

        public BedAllotmentsController(ApplicationDbContext context, ICommon iCommon
            , ILogger<BedAllotmentsController> logger)
        {
            _context = context;
            _iCommon = iCommon;
            _logger = logger;
        }

        [Authorize(Roles = MainMenu.BedAllotments.RoleName)]
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
                    || obj.PatientName.ToLower().Contains(searchValue)
                    || obj.BedCategoryName.ToLower().Contains(searchValue)
                    || obj.BedNo.ToLower().Contains(searchValue)
                    || obj.BedCategoryPrice.ToString().Contains(searchValue)
                    || obj.AllotmentDateDisplay.ToLower().Contains(searchValue)
                    || obj.DischargeDateDisplay.ToLower().Contains(searchValue)
                    || obj.ReleasedStatus.ToLower().Contains(searchValue)
                    || obj.CreatedDate.ToString().ToLower().Contains(searchValue)

                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                _logger.LogInformation("Error in getting Successfully.");
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting Bed Allotments.");
                throw;
            }

        }

        private IQueryable<BedAllotmentsGridViewModel> GetGridItem()
        {
            try
            {
                return (from _BedAllotments in _context.BedAllotments
                        join _BedCategories in _context.BedCategories on _BedAllotments.BedCategoryId equals _BedCategories.Id
                        join _PatientInfo in _context.PatientInfo on _BedAllotments.PatientId equals _PatientInfo.Id
                        join _Bed in _context.Bed on _BedAllotments.BedId equals _Bed.Id
                        where _BedAllotments.Cancelled == false
                        select new BedAllotmentsGridViewModel
                        {
                            Id = _BedAllotments.Id,
                            PatientName = _PatientInfo.FirstName + " " + _PatientInfo.LastName,
                            BedCategoryName = _BedCategories.Name,
                            BedId = _Bed.Id,
                            BedNo = _Bed.No,
                            BedCategoryPrice = _BedAllotments.BedCategoryPrice,
                            AllotmentDateDisplay = String.Format("{0:D}", _BedAllotments.AllotmentDate),
                            DischargeDateDisplay = String.Format("{0:D}", _BedAllotments.DischargeDate),
                            ReleasedStatus = _BedAllotments.IsReleased == false ? "No" : "Yes",
                            CreatedDate = _BedAllotments.CreatedDate,

                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Add Bed Allotments.");
                throw;
            }
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            BedAllotmentsGridViewModel vm = await GetGridItem().Where(x => x.Id == id).SingleOrDefaultAsync();
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public IActionResult GetBedPrice(int categoryId)
        {
            // Retrieve the bed price based on the categoryId
            double? bedPrice = _context.BedCategories
                .Where(bc => bc.Id == categoryId)
                .Select(bc => bc.BedPrice)
                .FirstOrDefault();

            // Check if the bed price is null
            if (bedPrice.HasValue)
            {
                // Convert the nullable double to string
                string actualBedPrice = bedPrice.ToString();
                return Ok(actualBedPrice);
            }
            else
            {
                // Handle the case where bed price is null
                // You can return a default value or handle it as per your application logic
                return NotFound(); // Or you can return BadRequest or any other appropriate status code
            }
        }
        public async Task<IActionResult> AddEdit(int id)
        {

            ViewBag.LoadddlPatientName = new SelectList(_iCommon.LoadddlPatientName(), "Id", "Name");
            ViewBag.LoadddBedCategories = new SelectList(_iCommon.LoadddBedCategories(), "Id", "Name");
            ViewBag._IsInAdminRole = User.IsInRole("Admin");
            BedAllotmentsCRUDViewModel vm = new BedAllotmentsCRUDViewModel();
            vm.listBedCategories = await _iCommon.GetBedCategorieslist();
            if (id > 0)
            {
                vm = await _context.BedAllotments.Where(x => x.Id == id)
                                      .Select(b => new BedAllotmentsCRUDViewModel
                                      {
                                          Id = b.Id,
                                          BedCategoryId = b.BedCategoryId,
                                          PatientId = b.PatientId,
                                          BedId = b.BedId,
                                          BedCategoryPrice = b.BedCategoryPrice,
                                          OldBedCategoryPrice = _context.BedCategories
                                                               .Where(bc => bc.Id == b.BedCategoryId)
                                                               .Select(bc => bc.BedPrice)
                                                               .FirstOrDefault()
                                      })
                                      .SingleOrDefaultAsync();
                ViewBag.LoadddlBedNo = new SelectList(_iCommon.LoadddlBedNo(vm, true), "Id", "Name");



            }


            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(BedAllotmentsCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        BedAllotments _BedAllotments = new BedAllotments();
                        if (vm.Id > 0)
                        {
                            _BedAllotments = await _context.BedAllotments.FindAsync(vm.Id);
                            //BedCategoryPrice Change By Admn User
                            if (User.IsInRole("Admin"))
                            {
                                _BedAllotments.BedCategoryPrice = vm.BedCategoryPrice;


                            }
                            else
                            {
                                _BedAllotments.BedCategoryPrice = vm.OldBedCategoryPrice;
                            }
                            vm.CreatedDate = _BedAllotments.CreatedDate;
                            vm.CreatedBy = _BedAllotments.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Entry(_BedAllotments).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();


                            TempData["successAlert"] = "BedAllotments Updated Successfully. ID: " + _BedAllotments.Id;
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            _BedAllotments = vm;
                            //BedCategoryPrice Change By Admn User
                            if (User.IsInRole("Admin"))
                            {
                                _BedAllotments.BedCategoryPrice = vm.BedCategoryPrice;


                            }
                            else
                            {
                                _BedAllotments.BedCategoryPrice = vm.OldBedCategoryPrice;
                            }
                            _BedAllotments.CreatedDate = DateTime.Now;
                            _BedAllotments.ModifiedDate = DateTime.Now;
                            _BedAllotments.CreatedBy = HttpContext.User.Identity.Name;
                            _BedAllotments.ModifiedBy = HttpContext.User.Identity.Name;

                            _context.Add(_BedAllotments);
                            await _context.SaveChangesAsync();


                            TempData["successAlert"] = "BedAllotments Created Successfully. ID: " + _BedAllotments.Id;
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
                        _logger.LogError("Error in Add or Update Bed Allotments.");
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
                var _BedAllotments = await _context.BedAllotments.FindAsync(id);
                _BedAllotments.ModifiedDate = DateTime.Now;
                _BedAllotments.ModifiedBy = HttpContext.User.Identity.Name;
                _BedAllotments.Cancelled = true;

                _context.Update(_BedAllotments);
                await _context.SaveChangesAsync();
                return new JsonResult(_BedAllotments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Delete Bed Allotments.");
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetAvailableBedList(Int64 id)
        {
            var _GetAvailableBedList = _context.Bed.Where(x => x.Cancelled == false && x.BedCategoryId == id).ToList();

            var _BookedBedList = _context.BedAllotments.Where(x => x.Cancelled == false
                        && x.BedCategoryId == id && x.IsReleased == false).ToList();

            foreach (var item in _BookedBedList)
            {
                var itemToRemove = _GetAvailableBedList.Where(x => x.Id == item.BedId).SingleOrDefault();
                if (itemToRemove != null)
                    _GetAvailableBedList.Remove(itemToRemove);
            }

            var result = (from _Bed in _GetAvailableBedList.OrderBy(x => x.Id)
                          join _BedCategories in _context.BedCategories on _Bed.BedCategoryId equals _BedCategories.Id
                          where _Bed.Cancelled == false
                          select new ItemDropdownListViewModel
                          {
                              Id = _Bed.Id,
                              Name = _Bed.No + "<>" + _BedCategories.Description,
                          }).ToList();
            return new JsonResult(result);
        }

        private bool IsExists(long id)
        {
            return _context.BedAllotments.Any(e => e.Id == id);
        }
    }
}
