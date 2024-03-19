using HMS.ConHelper;
using HMS.Data;
using HMS.Helpers;
using HMS.Models;
using HMS.Models.CheckupMedicineDetailsViewModel;
using HMS.Models.CheckupSummaryViewModel;
using HMS.Models.CommonViewModel;
using HMS.Models.MedicinesViewModel;
using HMS.Models.PaymentsViewModel;
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
    public class CheckupSummaryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly IDBOperation _iDBOperation;
        private readonly ILogger<CheckupSummaryController> _logger;

        public CheckupSummaryController(ApplicationDbContext context, ICommon iCommon, IDBOperation iDBOperation, ILogger<CheckupSummaryController> logger)
        {
            _context = context;
            _iCommon = iCommon;
            _iDBOperation = iDBOperation;
            _logger = logger;
        }
        [Authorize(Roles = Pages.MainMenu.Checkup.RoleName)]
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

                IQueryable<CheckupSummaryCRUDViewModel> _GetGridItem = null;
                var _UserProfile = _context.UserProfile.Where(x => x.Email == HttpContext.User.Identity.Name).SingleOrDefault();

                if (_UserProfile.UserType == UserType.Doctor)
                {
                    DoctorsInfo _DoctorsInfo = new();
                    _DoctorsInfo = _context.DoctorsInfo.Where(x => x.ApplicationUserId == _UserProfile.ApplicationUserId).SingleOrDefault();

                    if (_DoctorsInfo == null)
                    {
                        _GetGridItem = _iCommon.GetCheckupGridItem().Where(x => x.DoctorId == 0);
                    }
                    else
                    {
                        _GetGridItem = _iCommon.GetCheckupGridItem().Where(x => x.DoctorId == _DoctorsInfo.Id);
                    }
                }
                else
                {
                    _GetGridItem = _iCommon.GetCheckupGridItem();
                }

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
                    || obj.VisitId.ToLower().Contains(searchValue)
                    || obj.SerialNo.ToString().Contains(searchValue)
                    || obj.PatientName.ToLower().Contains(searchValue)
                    || obj.DoctorName.ToLower().Contains(searchValue)
                    || obj.Symptoms.ToLower().Contains(searchValue)
                    || obj.Diagnosis.ToLower().Contains(searchValue)
                    || obj.CheckupDate.ToString().ToLower().Contains(searchValue)
                    || obj.Advice.ToLower().Contains(searchValue)
                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                _logger.LogInformation("Error in getting Checkup Successfully .");
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting Checkup .");
                throw;
            }

        }

        public async Task<IActionResult> Details(long id)
        {
            var vm = await _iCommon.GetByCheckupDetails(id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> PatientHistory(long id)
        {
            var vm = await _iCommon.GetByPatientHistory(id);
            if (vm == null) return NotFound();
            return PartialView("_PatientHistory", vm);
        }
        public async Task<IActionResult> PrintCheckup(Int64 id)
        {
            var _GetByCheckupDetails = await _iCommon.GetByCheckupDetails(id);
            return View(_GetByCheckupDetails);
        }
        public IActionResult AddFromPatientInfo(long id)
        {
            LoadDDL();
            ManageCheckupViewModel vm = new ManageCheckupViewModel();
            CheckupSummaryCRUDViewModel _CheckupSummaryCRUDViewModel = new CheckupSummaryCRUDViewModel();
            _CheckupSummaryCRUDViewModel.PatientId = id;
            vm.CheckupSummaryCRUDViewModel = _CheckupSummaryCRUDViewModel;
            return PartialView("_AddEdit", vm);
        }

        public async Task<IActionResult> AddEdit(long id)
        {
            LoadDDL();
            ManageCheckupViewModel vm = new();
            CheckupSummaryCRUDViewModel _CheckupSummaryCRUDViewModel = new();
            vm.CheckupSummaryCRUDViewModel = _CheckupSummaryCRUDViewModel;

            if (id > 0)
            {
                vm = await _iCommon.GetByCheckupDetails(id);
                var _PatientAppointment = await _context.PatientAppointment.Where(x => x.VisitId == vm.CheckupSummaryCRUDViewModel.VisitId).SingleOrDefaultAsync();
                if (_PatientAppointment != null)
                {
                    vm.CheckupSummaryCRUDViewModel.PatientType = _PatientAppointment.PatientType;
                }
            }
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(ManageCheckupViewModel vm)
        {
            try
            {
                JsonResultViewModel _JsonResultViewModel = new();
                CheckupSummary _CheckupSummary = new();
                if (vm.CheckupSummaryCRUDViewModel.Id > 0)
                {
                    var result = await UpdateCheckupSummary(vm.CheckupSummaryCRUDViewModel);
                    _JsonResultViewModel.AlertMessage = "Checkup Updated Successfully. ID: " + result.Id;
                    _JsonResultViewModel.IsSuccess = true;
                    return new JsonResult(_JsonResultViewModel);
                }
                else
                {
                    string strCurrentUserName = HttpContext.User.Identity.Name;
                    _CheckupSummary = vm.CheckupSummaryCRUDViewModel;
                    _CheckupSummary.VisitId = StaticData.GetUniqueID("V");
                    _CheckupSummary.CreatedDate = DateTime.Now;
                    _CheckupSummary.ModifiedDate = DateTime.Now;
                    _CheckupSummary.CreatedBy = strCurrentUserName;
                    _CheckupSummary.ModifiedBy = strCurrentUserName;
                    _context.Add(_CheckupSummary);
                    await _context.SaveChangesAsync();

                    AddPaymentViewModel _AddPaymentViewModel = new AddPaymentViewModel
                    {
                        PatientId = _CheckupSummary.PatientId,
                        VisitId = _CheckupSummary.VisitId
                    };
                    //Add Payment
                    var _CreatePayments = await _iDBOperation.CreatePayments(_AddPaymentViewModel, strCurrentUserName);
                    _CheckupSummary.PaymentId = _CreatePayments.Id;
                    await UpdateCheckupSummary(_CheckupSummary);
                    _AddPaymentViewModel.PaymentsId = _CreatePayments.Id;
                    double? _GrandTotal = 0;

                    if (vm.listCheckupMedicineDetailsCRUDViewModel != null)
                    {
                        foreach (var item in vm.listCheckupMedicineDetailsCRUDViewModel)
                        {
                            CheckupMedicineDetails _CheckupMedicineDetails = item;
                            _CheckupMedicineDetails.MedicineId = item.CheckupMedicineDetailsId;
                            _CheckupMedicineDetails.VisitId = _CheckupSummary.VisitId;
                            var result = await CreateCheckupMedicineDetails(_CheckupMedicineDetails);

                            //Add Payment Details
                            _AddPaymentViewModel.ItemDetailId = result.Id;
                            _AddPaymentViewModel.MedicineId = _CheckupMedicineDetails.MedicineId;
                            _AddPaymentViewModel.WhentoTakeDayCount = item.WhentoTakeDayCount;
                            _AddPaymentViewModel.NoofDays = item.NoofDays;
                            var _PaymentsDetails = await _iDBOperation.CreatePaymentsDetails(_AddPaymentViewModel, PaymentItemType.Medicine, strCurrentUserName);

                            UpdateMedicineInventoryViewModel _UpdateMedicineInventoryViewModel = new UpdateMedicineInventoryViewModel();
                            _UpdateMedicineInventoryViewModel = _PaymentsDetails;
                            _UpdateMedicineInventoryViewModel.IsAddition = false;
                            _UpdateMedicineInventoryViewModel.Action = "Sell medicein. Payment Detail Id: " + _PaymentsDetails.Id;
                            _UpdateMedicineInventoryViewModel.CurrentUserName = strCurrentUserName;
                            await _iDBOperation.UpdateMedicineInventory(_UpdateMedicineInventoryViewModel);

                            _GrandTotal = _GrandTotal + _PaymentsDetails.TotalAmount;
                        }
                    }

                    var _PatientTest = await _iDBOperation.CreatePatientTest(_AddPaymentViewModel, strCurrentUserName);

                    if (vm.listPatientTestDetailCRUDViewModel != null)
                    {
                        foreach (var item in vm.listPatientTestDetailCRUDViewModel)
                        {
                            PatientTestDetail _PatientTestDetail = item;
                            _PatientTestDetail.Quantity = 1;
                            _PatientTestDetail.PatientTestId = _PatientTest.Id;
                            _PatientTestDetail.CreatedBy = strCurrentUserName;
                            _PatientTestDetail.ModifiedBy = strCurrentUserName;
                            await _iCommon.CreatePatientTestDetail(_PatientTestDetail);

                            //Add Payment Details
                            _AddPaymentViewModel.LabTestsId = _PatientTestDetail.LabTestsId;
                            var _PaymentsDetails = await _iDBOperation.CreatePaymentsDetails(_AddPaymentViewModel, PaymentItemType.LabTest, strCurrentUserName);
                            _GrandTotal = _GrandTotal + _PaymentsDetails.TotalAmount;
                        }
                    }


                    //Add Consultation Payment Details
                    _AddPaymentViewModel.PaymentCategoriesId = 3;
                    var _ConsultationPaymentDetails = await _iDBOperation.CreatePaymentsDetails(_AddPaymentViewModel, PaymentItemType.Consultation, strCurrentUserName);
                    _GrandTotal = _GrandTotal + _ConsultationPaymentDetails.TotalAmount;

                    //Uddate Payment
                    MedicineSellStateViewModel _MedicineSellStateViewModel = new()
                    {
                        Id = _CreatePayments.Id,
                        InvoiceNo = "INV" + _CreatePayments.Id.ToString(),
                        MedicineSellState = MedicineSellState.AddIntoCheckup,
                        ModifiedBy = strCurrentUserName,
                        GrandTotal = _GrandTotal
                    };
                    await _iDBOperation.UpdateMedicineSellState(_MedicineSellStateViewModel);

                    _JsonResultViewModel.AlertMessage = "Checkup Created Successfully. ID: " + _CheckupSummary.Id;
                    _JsonResultViewModel.IsSuccess = true;
                    return new JsonResult(_JsonResultViewModel);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error in Add Or Update Checkup .");
                return new JsonResult(ex.Message);
                throw;
            }
        }


        [HttpPost]
        public async Task<IActionResult> SaveCheckupMedicineDetails(CheckupMedicineDetailsCRUDViewModel vm)
        {
            var _CheckupId = vm.CheckupId;
            string strCurrentUserName = HttpContext.User.Identity.Name;
            vm.MedicineId = vm.CheckupMedicineDetailsId;
            var result = await CreateCheckupMedicineDetails(vm);
            //await UpdateCheckupSummary(vm.CheckupSummaryCRUDViewModel);
            //Add Payment Details
            if (vm != null)
            {
                AddPaymentViewModel _AddPaymentViewModel = new AddPaymentViewModel();
                _AddPaymentViewModel.PaymentsId = vm.PaymentId;
                _AddPaymentViewModel.ItemDetailId = result.Id;
                _AddPaymentViewModel.MedicineId = vm.MedicineId;
                _AddPaymentViewModel.WhentoTakeDayCount = vm.WhentoTakeDayCount;
                _AddPaymentViewModel.NoofDays = vm.NoofDays;
                var _PaymentsDetails = await _iDBOperation.CreatePaymentsDetails(_AddPaymentViewModel, PaymentItemType.Medicine, strCurrentUserName);

                UpdateMedicineInventoryViewModel _UpdateMedicineInventoryViewModel = new UpdateMedicineInventoryViewModel();
                _UpdateMedicineInventoryViewModel = _PaymentsDetails;
                _UpdateMedicineInventoryViewModel.IsAddition = false;
                _UpdateMedicineInventoryViewModel.Action = "Sell medicein. Payment Detail Id: " + _PaymentsDetails.Id;
                _UpdateMedicineInventoryViewModel.CurrentUserName = strCurrentUserName;
                await _iDBOperation.UpdateMedicineInventory(_UpdateMedicineInventoryViewModel);
            }

            vm.Id = result.Id;
            return new JsonResult(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCheckupMedicineDetails(CheckupMedicineDetailsCRUDViewModel vm)
        {
            try
            {
                var _CheckupMedicineDetails = await _context.CheckupMedicineDetails.Where(x => x.Id == vm.CheckupMedicineDetailsId).SingleOrDefaultAsync();
                _CheckupMedicineDetails.ModifiedDate = DateTime.Now;
                _CheckupMedicineDetails.ModifiedBy = HttpContext.User.Identity.Name;
                _CheckupMedicineDetails.Cancelled = true;

                _context.Update(_CheckupMedicineDetails);
                await _context.SaveChangesAsync();
                //await UpdateCheckupSummary(vm.CheckupSummaryCRUDViewModel);               
                var _PaymentsDetails = _context.PaymentsDetails.Where(x => x.ItemDetailId == _CheckupMedicineDetails.Id).SingleOrDefault();
                _PaymentsDetails.ModifiedDate = DateTime.Now;
                _PaymentsDetails.ModifiedBy = HttpContext.User.Identity.Name;
                _PaymentsDetails.Cancelled = true;

                _context.Update(_PaymentsDetails);
                await _context.SaveChangesAsync();

                UpdateMedicineInventoryViewModel _UpdateMedicineInventoryViewModel = new UpdateMedicineInventoryViewModel();
                _UpdateMedicineInventoryViewModel = _PaymentsDetails;
                _UpdateMedicineInventoryViewModel.IsAddition = true;
                _UpdateMedicineInventoryViewModel.Action = "Return Sell medicein. Payment Detail Id: " + _PaymentsDetails.Id;
                _UpdateMedicineInventoryViewModel.CurrentUserName = HttpContext.User.Identity.Name;
                await _iDBOperation.UpdateMedicineInventory(_UpdateMedicineInventoryViewModel);

                return new JsonResult(_CheckupMedicineDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Delete CheckupMedicineDetails.");
                throw;
            }
        }
        private async Task<CheckupSummaryCRUDViewModel> UpdateCheckupSummary(CheckupSummaryCRUDViewModel vm)
        {
            try
            {

                var _CheckupSummary = await _context.CheckupSummary.FindAsync(vm.Id);
                vm.CreatedDate = _CheckupSummary.CreatedDate;
                vm.CreatedBy = _CheckupSummary.CreatedBy;
                vm.ModifiedDate = DateTime.Now;
                vm.ModifiedBy = HttpContext.User.Identity.Name;
                _context.Entry(_CheckupSummary).CurrentValues.SetValues(vm);
                await _context.SaveChangesAsync();
                return vm;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Update UpdateCheckupSummary.");
                throw;
            }
        }

        private async Task<CheckupMedicineDetails> CreateCheckupMedicineDetails(CheckupMedicineDetails _CheckupMedicineDetails)
        {
            try
            {

                _CheckupMedicineDetails.CreatedDate = DateTime.Now;
                _CheckupMedicineDetails.ModifiedDate = DateTime.Now;
                _CheckupMedicineDetails.CreatedBy = HttpContext.User.Identity.Name;
                _CheckupMedicineDetails.ModifiedBy = HttpContext.User.Identity.Name;
                _context.Add(_CheckupMedicineDetails);
                await _context.SaveChangesAsync();
                return _CheckupMedicineDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Create CheckupMedicineDetails.");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Int64 id)
        {
            try
            {
                var _CheckupSummary = await _context.CheckupSummary.FindAsync(id);
                _CheckupSummary.ModifiedDate = DateTime.Now;
                _CheckupSummary.ModifiedBy = HttpContext.User.Identity.Name;
                _CheckupSummary.Cancelled = true;

                _context.Update(_CheckupSummary);
                await _context.SaveChangesAsync();
                return new JsonResult(_CheckupSummary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Delete Checkup.");
                throw;
            }
        }

        [HttpGet]
        public IActionResult LoadUnitPrice(Int64 Id)
        {
            var _LabTests = _context.LabTests.Where(x => x.Id == Id).SingleOrDefault();
            return new JsonResult(_LabTests);
        }
        private void LoadDDL()
        {
            ViewBag._LoadddlPatientName = new SelectList(_iCommon.LoadddlPatientName(), "Id", "Name");
            ViewBag._LoadddlDoctorName = new SelectList(_iCommon.LoadddlDoctorName(), "Id", "Name");
            ViewBag._LoadddlMedicines = new SelectList(_iCommon.LoadddlMedicines(), "Id", "Name");
            ViewBag._LoadddlLabTests = new SelectList(_iCommon.LoadddlLabTests(), "Id", "Name");
        }
        [HttpGet]
        public IActionResult LoadMedicine(string request)
        {
            var results = (from _Medicines in _context.Medicines.Where(x => x.Cancelled == false).OrderBy(x => x.Id)
                           where _Medicines.MedicineName != null && _Medicines.Cancelled == false
                           select new Select2ListViewModel
                           {
                               id = _Medicines.Id,
                               text = _Medicines.MedicineName + ", Available Quantity: " + _Medicines.Quantity
                           }).ToList();

            if (request == null)
            {
                return new JsonResult(results);
            }

            else
            {
                results = results.Where(x => x.text == request).ToList();
                return new JsonResult(results);
            }
            //return new JsonResult(result);
        }
        private bool IsExists(long id)
        {
            return _context.CheckupSummary.Any(e => e.Id == id);
        }
    }
}
