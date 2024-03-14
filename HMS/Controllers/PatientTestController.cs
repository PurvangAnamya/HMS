using HMS.ConHelper;
using HMS.Data;
using HMS.Helpers;
using HMS.Models;
using HMS.Models.CheckupSummaryViewModel;
using HMS.Models.PatientTestDetailViewModel;
using HMS.Models.PatientTestViewModel;
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
    public class PatientTestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly IDBOperation _iDBOperation;

        public PatientTestController(ApplicationDbContext context, ICommon iCommon, IDBOperation iDBOperation)
        {
            _context = context;
            _iCommon = iCommon;
            _iDBOperation = iDBOperation;
        }

        [Authorize(Roles = Pages.MainMenu.PatientTest.RoleName)]
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

                var _GetGridItem = _iCommon.GetPatientTestGridItem();
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
                    || obj.PatientName.Contains(searchValue)
                    || obj.VisitId.Contains(searchValue)
                    || obj.TestDate.ToString().Contains(searchValue)
                    || obj.DeliveryDate.ToString().Contains(searchValue)
                    || obj.PaymentStatus.Contains(searchValue)
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

        public async Task<IActionResult> Details(Int64 id)
        {
            var result = await _iCommon.GetByPatientTestDetails(id);
            //PatientTestGridViewModel vm = await _iCommon.GetPatientTestGridItem().Where(x => x.VisitId == _VisitId).SingleOrDefaultAsync();
            if (result == null) return NotFound();
            return PartialView("_Details", result);
        }

        public async Task<IActionResult> AddEdit(Int64 id)
        {
            ViewBag._LoadddlPatientName = new SelectList(_iCommon.LoadddlPatientName(), "Id", "Name");
            ViewBag._LoadddlDoctorName = new SelectList(_iCommon.LoadddlDoctorName(), "Id", "Name");
            ViewBag._LoadddlLabTests = new SelectList(_iCommon.LoadddlLabTests(), "Id", "Name");

            ManagePatientTestViewModel _ManagePatientTestViewModel = new ManagePatientTestViewModel();
            if (id > 0)
            {
                _ManagePatientTestViewModel = await _iCommon.GetByPatientTestDetails(id);
            }
            return PartialView("_AddEdit", _ManagePatientTestViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(ManagePatientTestViewModel vm)
        {
            try
            {
                PatientTest _PatientTest = new();
                if (vm.PatientTestCRUDViewModel.Id > 0)
                {
                    vm.PatientTestCRUDViewModel.CreatedDate = _PatientTest.CreatedDate;
                    vm.PatientTestCRUDViewModel.CreatedBy = _PatientTest.CreatedBy;
                    vm.PatientTestCRUDViewModel.ModifiedDate = DateTime.Now;
                    vm.PatientTestCRUDViewModel.ModifiedBy = HttpContext.User.Identity.Name;
                    _context.Entry(_PatientTest).CurrentValues.SetValues(vm.PatientTestCRUDViewModel);
                    await _context.SaveChangesAsync();

                    foreach (var item in vm.listPatientTestResultUpdateViewModel)
                    {
                        var result = await UpdatePatientTestDetail(item);
                    }

                    var _AlertMessage = "Patient Test Updated Successfully. ID: " + _PatientTest.Id;
                    TempData["successAlert"] = _AlertMessage;
                    return new JsonResult(_AlertMessage);
                }
                else
                {
                    //Bug
                    //vm.PatientTestCRUDViewModel.ApplicationUserId = _context.DoctorsInfo.Where(x => x.Id == vm.PatientTestCRUDViewModel.Id).SingleOrDefault().ApplicationUserId;
                    _PatientTest = vm.PatientTestCRUDViewModel;
                    _PatientTest.CreatedDate = DateTime.Now;
                    _PatientTest.ModifiedDate = DateTime.Now;
                    _PatientTest.CreatedBy = HttpContext.User.Identity.Name;
                    _PatientTest.ModifiedBy = HttpContext.User.Identity.Name;
                    _context.Add(_PatientTest);
                    await _context.SaveChangesAsync();

                    foreach (var item in vm.listPatientTestDetailCRUDViewModel)
                    {
                        PatientTestDetail _PatientTestDetail = item;
                        _PatientTestDetail.PatientTestId = _PatientTest.Id;
                        _PatientTestDetail.CreatedBy = HttpContext.User.Identity.Name;
                        _PatientTestDetail.ModifiedBy = HttpContext.User.Identity.Name;
                        await _iCommon.CreatePatientTestDetail(_PatientTestDetail);
                    }
                    return new JsonResult(_PatientTest);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["errorAlert"] = "Operation failed.";
                if (!IsExists(vm.PatientTestCRUDViewModel.Id))
                    return NotFound();
                else
                    throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> SavePatientTestDetail(PatientTestDetailCRUDViewModel vm)
        {
            string strCurrentUserName = HttpContext.User.Identity.Name;
            PatientTestDetail _PatientTestDetail = vm;
            _PatientTestDetail.CreatedBy = strCurrentUserName;
            _PatientTestDetail.ModifiedBy = strCurrentUserName;
            var result = await _iCommon.CreatePatientTestDetail(_PatientTestDetail);

            //Add Payment Details
            if (result != null)
            {
                AddPaymentViewModel _AddPaymentViewModel = new AddPaymentViewModel();
                var _VisitId = _context.PatientTest.Where(x => x.Id == result.PatientTestId).SingleOrDefault().VisitId;
                var _PaymentId = _context.Payments.Where(x => x.VisitId == _VisitId).SingleOrDefault().Id;
                _AddPaymentViewModel.LabTestsId = _PatientTestDetail.LabTestsId;
                _AddPaymentViewModel.PaymentsId = _PaymentId;
                await _iDBOperation.CreatePaymentsDetails(_AddPaymentViewModel, PaymentItemType.LabTest, strCurrentUserName);
            }

            return new JsonResult(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePatientTestDetailDB(PatientTestResultUpdateViewModel vm)
        {
            var _PatientTestDetail = await UpdatePatientTestDetail(vm);
            return new JsonResult(_PatientTestDetail);
        }

        private async Task<PatientTestDetail> UpdatePatientTestDetail(PatientTestResultUpdateViewModel vm)
        {
            var _PatientTestDetail = await _context.PatientTestDetail.FindAsync(vm.Id);
            vm.ModifiedDate = DateTime.Now;
            vm.ModifiedBy = HttpContext.User.Identity.Name;
            _context.Entry(_PatientTestDetail).CurrentValues.SetValues(vm);
            _context.SaveChanges();
            return _PatientTestDetail;
        }

        [HttpPost]
        public async Task<IActionResult> DeletePatientTestDetail(PatientTestDetailCRUDViewModel vm)
        {
            try
            {
                var _PatientTestDetail = await _context.PatientTestDetail.FindAsync(vm.Id);
                //var _PatientTestDetail = await _context.PatientTestDetail.Where(x => x.LabTestsId == vm.LabTestsId && x.PatientTestId == vm.PatientTestId).SingleOrDefaultAsync();
                _PatientTestDetail.ModifiedDate = DateTime.Now;
                _PatientTestDetail.ModifiedBy = HttpContext.User.Identity.Name;
                _PatientTestDetail.Cancelled = true;

                _context.Update(_PatientTestDetail);
                await _context.SaveChangesAsync();
                return new JsonResult(_PatientTestDetail);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Int64 id)
        {
            try
            {
                var _PatientTest = await _context.PatientTest.FindAsync(id);
                _PatientTest.ModifiedDate = DateTime.Now;
                _PatientTest.ModifiedBy = HttpContext.User.Identity.Name;
                _PatientTest.Cancelled = true;

                _context.Update(_PatientTest);
                await _context.SaveChangesAsync();
                return new JsonResult(_PatientTest);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsExists(long id)
        {
            return _context.PatientTest.Any(e => e.Id == id);
        }
    }
}