using HMS.ConHelper;
using HMS.Data;
using HMS.Helpers;
using HMS.Models;
using HMS.Models.MedicinesViewModel;
using HMS.Models.PaymentModeHistoryViewModel;
using HMS.Models.PaymentsDetailsViewModel;
using HMS.Models.PaymentsViewModel;
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
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly IDBOperation _iDBOperation;

        public PaymentsController(ApplicationDbContext context, ICommon iCommon, IDBOperation iDBOperation)
        {
            _context = context;
            _iCommon = iCommon;
            _iDBOperation = iDBOperation;
        }
        [Authorize(Roles = Pages.MainMenu.Payments.RoleName)]
        public IActionResult PaymentsList()
        {
            return View();
        }
        [Authorize(Roles = Pages.MainMenu.Payments.RoleName)]
        public IActionResult PaymentsListInBed()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetDataTabelData(string _PaymentsListType)
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

                IQueryable<PaymentsGridViewModel> _GetGridItem = null;
                if (_PaymentsListType == "PaymentsList")
                {
                    _GetGridItem = _iCommon.GetPaymentGridList().Where(x => x.PatientType == "Out Patient");
                }
                else if (_PaymentsListType == "PaymentsListInBed")
                {
                    _GetGridItem = _iCommon.GetPaymentGridList().Where(x => x.PatientType == "In Patient");
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
                    || obj.PatientName.ToLower().Contains(searchValue)
                    //|| obj.PatientType.ToLower().Contains(searchValue)                    
                    || obj.Discount.ToString().ToLower().Contains(searchValue)
                    || obj.Tax.ToString().ToLower().Contains(searchValue)
                    || obj.SubTotal.ToString().ToLower().Contains(searchValue)
                    || obj.GrandTotal.ToString().ToLower().Contains(searchValue)
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

        private IQueryable<PaymentsGridViewModel> GetPaymentGridList2()
        {
            try
            {
                var result = _context.PaymentsGridViewModel.FromSqlRaw(@"select A.Id,A.PatientId,A.VisitId,(B.FirstName + ' '+ B.LastName) 
                PatientName, C.PatientType,A.Discount,A.Tax,A.SubTotal,A.GrandTotal,A.CreatedDate from Payments A 
                inner join PatientInfo B on A.PatientId=B.Id 
                inner join CheckupSummary C on A.VisitId=C.VisitId 
                where A.Cancelled=0");
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> Details(Int64 id)
        {
            //PaymentsDataMigration();
            var result = await _iCommon.GetByPaymentsDetails(id);
            if (result == null) return NotFound();
            return PartialView("_Details", result);
        }

        public async Task<IActionResult> PrintPaymentInvoice(Int64 _PaymentId)
        {
            var _PrintPaymentInvoice = await _iCommon.PrintPaymentInvoice(_PaymentId);
            return View(_PrintPaymentInvoice);
        }
        public async Task<IActionResult> ThermalPrintPaymentInvoice(Int64 _PaymentId)
        {
            var _PrintPaymentInvoice = await _iCommon.PrintPaymentInvoice(_PaymentId);
            return View(_PrintPaymentInvoice);
        }

        public async Task<IActionResult> AddEdit(Int64 id)
        {
            ViewBag._LoadddlPaymentCategories = new SelectList(_iCommon.LoadddlPaymentCategories(), "Code", "Name");
            ViewBag.ddlCurrency = new SelectList(_iCommon.LoadddlCurrencyItem(), "Id", "Name");
            ViewBag._LoadddlInsuranceCompanyInfo = new SelectList(_iCommon.LoadddlInsuranceCompanyInfo(), "Id", "Name");

            ManagePaymentsViewModel vm = new ManagePaymentsViewModel();
            vm.PaymentsCRUDViewModel = new PaymentsCRUDViewModel();
            if (id > 0)
            {
                vm = await _iCommon.GetByPaymentsDetails(id);
                var _IsInRole = User.IsInRole("Admin");
                if(_IsInRole)
                {
                    vm.PaymentsCRUDViewModel.UserRole="Admin";
                }             
            }
            else
            {
                ViewBag._LoadddlPatientName = new SelectList(_iCommon.LoadddlPatientName(), "Id", "Name");
            }
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(ManagePaymentsViewModel vm)
        {
            try
            {
                Payments _Payments = new Payments();
                string strUserName = HttpContext.User.Identity.Name;
                if (vm.PaymentsCRUDViewModel.DueAmount < 1)
                    vm.PaymentsCRUDViewModel.PaymentStatus = PaymentStatus.Paid;
                else
                    vm.PaymentsCRUDViewModel.PaymentStatus = PaymentStatus.Unpaid;

                if (vm.PaymentsCRUDViewModel.Id > 0)
                {
                    _Payments = await UpdatePayments(vm.PaymentsCRUDViewModel);
                    //foreach (var item in vm.listPaymentsDetailsCRUDViewModel)
                    //{
                    //    var result = await UpdatePaymentsDetails(item);
                    //}

                    var _AlertMessage = "Payments Updated Successfully. ID: " + _Payments.Id;
                    TempData["successAlert"] = _AlertMessage;
                    return new JsonResult(_AlertMessage);
                }
                else
                {
                    _Payments = vm.PaymentsCRUDViewModel;
                    var _Id = _context.Payments.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
                    //_Payments.InvoiceNo = StaticData.GetUniqueID("INV");
                    _Payments.InvoiceNo = "INV" + _Id;
                    _Payments.CreatedDate = DateTime.Now;
                    _Payments.ModifiedDate = DateTime.Now;
                    _Payments.CreatedBy = strUserName;
                    _Payments.ModifiedBy = strUserName;
                    _context.Add(_Payments);
                    await _context.SaveChangesAsync();

                    PatientVisitPaymentHistory _PatientVisitPaymentHistory = new PatientVisitPaymentHistory
                    {
                        VisitId = vm.PaymentsCRUDViewModel.VisitId,
                        PaymentId = _Payments.Id,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = strUserName,
                        ModifiedBy = strUserName,
                    };
                    _context.Add(_PatientVisitPaymentHistory);

                    if (vm.listPaymentsDetailsCRUDViewModel != null)
                    {
                        foreach (var item in vm.listPaymentsDetailsCRUDViewModel)
                        {
                            PaymentsDetails _PaymentsDetails = item;
                            _PaymentsDetails.PaymentsId = _Payments.Id;
                            await CreatePaymentsDetails(_PaymentsDetails);
                        }
                    }

                    if (vm.listPaymentModeHistoryCRUDViewModel != null)
                    {
                        foreach (var item in vm.listPaymentModeHistoryCRUDViewModel)
                        {
                            PaymentModeHistory _PaymentModeHistory = item;
                            _PaymentModeHistory.PaymentId = _Payments.Id;
                            _PaymentModeHistory.CreatedBy = strUserName;
                            _PaymentModeHistory.ModifiedBy = strUserName;
                            await _iDBOperation.CreatePaymentModeHistory(_PaymentModeHistory);
                        }
                    }

                    var _AlertMessage = "Payments Created Successfully. ID: " + _Payments.Id;
                    TempData["successAlert"] = _AlertMessage;
                    return new JsonResult(_AlertMessage);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["errorAlert"] = "Operation failed.";
                if (!IsExists(vm.PaymentsCRUDViewModel.Id))
                    return NotFound();
                else
                    throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> SavePaymentsDetails(ManagePaymentsViewModel vm)
        {
            var _SavePaymentsDetails = await CreatePaymentsDetails(vm.PaymentsDetailsCRUDViewModel);
            await UpdatePayments(vm.PaymentsCRUDViewModel);
            return new JsonResult(_SavePaymentsDetails);
        }


        [HttpPost]
        public async Task<IActionResult> UpdatePaymentsDetailsDB(PaymentsDetailsCRUDViewModel vm)
        {
            var _PaymentsDetails = await UpdatePaymentsDetails(vm);
            return new JsonResult(_PaymentsDetails);
        }
        [HttpPost]
        public async Task<IActionResult> DeletePaymentsDetails(ManagePaymentsViewModel vm)
        {
            try
            {
                var _PaymentsDetails = await _context.PaymentsDetails.Where(x => x.Id == vm.PaymentsDetailsCRUDViewModel.Id).SingleOrDefaultAsync();
                _PaymentsDetails.ModifiedDate = DateTime.Now;
                _PaymentsDetails.ModifiedBy = HttpContext.User.Identity.Name;
                _PaymentsDetails.Cancelled = true;

                _context.Update(_PaymentsDetails);
                await _context.SaveChangesAsync();
                await UpdatePayments(vm.PaymentsCRUDViewModel);
                var _PaymentItemCode = _PaymentsDetails.PaymentItemCode.Substring(0, 3);
                if (_PaymentItemCode == "MED")
                {
                    UpdateMedicineInventoryViewModel _UpdateMedicineInventoryViewModel = new UpdateMedicineInventoryViewModel();

                    _UpdateMedicineInventoryViewModel = _PaymentsDetails;
                    _UpdateMedicineInventoryViewModel.IsAddition = true;
                    _UpdateMedicineInventoryViewModel.Action = "Return Sell medicein. Payment Detail Id: " + _PaymentsDetails.Id;
                    _UpdateMedicineInventoryViewModel.CurrentUserName = HttpContext.User.Identity.Name;
                    await _iDBOperation.UpdateMedicineInventory(_UpdateMedicineInventoryViewModel);
                }
                return new JsonResult(_PaymentsDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> SavePaymentModeHistory(PaymentModeHistoryCRUDViewModel vm)
        {
            vm.CreatedBy = HttpContext.User.Identity.Name;
            vm.ModifiedBy = HttpContext.User.Identity.Name;
            PaymentModeHistory _PaymentModeHistory = await _iDBOperation.CreatePaymentModeHistory(vm);
            return new JsonResult(_PaymentModeHistory);
        }
        [HttpPost]
        public async Task<IActionResult> DeletePaymentModeHistory(PaymentModeHistoryCRUDViewModel vm)
        {
            var _PaymentModeHistory = await _context.PaymentModeHistory.FindAsync(vm.Id);
            _PaymentModeHistory.ModifiedDate = DateTime.Now;
            _PaymentModeHistory.ModifiedBy = HttpContext.User.Identity.Name;
            _PaymentModeHistory.Cancelled = true;

            _context.Update(_PaymentModeHistory);
            await _context.SaveChangesAsync();
            return new JsonResult(_PaymentModeHistory);
        }


        private async Task<PaymentsDetails> UpdatePaymentsDetails(PaymentsDetailsCRUDViewModel vm)
        {
            var _PaymentsDetails = await _context.PaymentsDetails.FindAsync(vm.Id);
            double CurrentQuantity = _PaymentsDetails.Quantity;

            _PaymentsDetails.Quantity = vm.Quantity;
            _PaymentsDetails.UnitPrize = vm.UnitPrize;
            _PaymentsDetails.TotalAmount = vm.TotalAmount;
            _PaymentsDetails.ModifiedDate = DateTime.Now;
            _PaymentsDetails.ModifiedBy = HttpContext.User.Identity.Name;
            _context.Update(_PaymentsDetails);
            _context.SaveChanges();

            if (CurrentQuantity != vm.Quantity)
            {
                var _PaymentItemCode = _PaymentsDetails.PaymentItemCode.Substring(0, 3);
                if (_PaymentItemCode == "MED")
                {
                    UpdateMedicineInventoryViewModel _UpdateMedicineInventoryViewModel = new UpdateMedicineInventoryViewModel();
                    _UpdateMedicineInventoryViewModel = _PaymentsDetails;
                    if (CurrentQuantity < vm.Quantity)
                    {
                        _UpdateMedicineInventoryViewModel.TranQuantity = (int)(vm.Quantity - CurrentQuantity);
                        _UpdateMedicineInventoryViewModel.IsAddition = false;
                        _UpdateMedicineInventoryViewModel.Action = "Update Sell medicine Items by addition. Payment Detail Id: " + _PaymentsDetails.Id;
                    }
                    else
                    {
                        _UpdateMedicineInventoryViewModel.TranQuantity = (int)(CurrentQuantity - vm.Quantity);
                        _UpdateMedicineInventoryViewModel.IsAddition = true;
                        _UpdateMedicineInventoryViewModel.Action = "Update Sell medicine Items by return. Payment Detail Id: " + _PaymentsDetails.Id;
                    }
                    _UpdateMedicineInventoryViewModel.CurrentUserName = HttpContext.User.Identity.Name;
                    await _iDBOperation.UpdateMedicineInventory(_UpdateMedicineInventoryViewModel);
                }
            }

            return _PaymentsDetails;
        }

        private async Task<PaymentsCRUDViewModel> UpdatePayments(PaymentsCRUDViewModel vm)
        {
            var _Payments = await _context.Payments.FindAsync(vm.Id);
            vm.CreatedDate = _Payments.CreatedDate;
            vm.CreatedBy = _Payments.CreatedBy;
            vm.ModifiedDate = DateTime.Now;
            vm.ModifiedBy = HttpContext.User.Identity.Name;
            _context.Entry(_Payments).CurrentValues.SetValues(vm);
            await _context.SaveChangesAsync();
            return vm;
        }

        private async Task<PaymentsDetails> CreatePaymentsDetails(PaymentsDetails _PaymentsDetails)
        {
            _PaymentsDetails.CreatedDate = DateTime.Now;
            _PaymentsDetails.ModifiedDate = DateTime.Now;
            _PaymentsDetails.CreatedBy = HttpContext.User.Identity.Name;
            _PaymentsDetails.ModifiedBy = HttpContext.User.Identity.Name;
            _context.Add(_PaymentsDetails);
            var result = await _context.SaveChangesAsync();

            var _PaymentItemCode = _PaymentsDetails.PaymentItemCode.Substring(0, 3);
            if (_PaymentItemCode == "MED")
            {
                UpdateMedicineInventoryViewModel _UpdateMedicineInventoryViewModel = new UpdateMedicineInventoryViewModel();

                _UpdateMedicineInventoryViewModel = _PaymentsDetails;
                _UpdateMedicineInventoryViewModel.IsAddition = false;
                _UpdateMedicineInventoryViewModel.Action = "Sell medicein. Payment Detail Id: " + _PaymentsDetails.Id;
                _UpdateMedicineInventoryViewModel.CurrentUserName = HttpContext.User.Identity.Name;
                await _iDBOperation.UpdateMedicineInventory(_UpdateMedicineInventoryViewModel);
            }
            return _PaymentsDetails;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Int64 id)
        {
            try
            {
                var _Payments = await _context.Payments.FindAsync(id);
                _Payments.ModifiedDate = DateTime.Now;
                _Payments.ModifiedBy = HttpContext.User.Identity.Name;
                _Payments.Cancelled = true;

                _context.Update(_Payments);
                await _context.SaveChangesAsync();
                return new JsonResult(_Payments);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public IActionResult LoadVisitId(Int64 Id)
        {
            var _LoadddlVisitID = _iCommon.LoadddlVisitID(Id).ToList();
            return new JsonResult(_LoadddlVisitID);
        }
        private bool IsExists(long id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }

        private void PaymentsDataMigration()
        {
            var _PaymentsList = _context.Payments.Where(x => x.PaidAmount > 0).ToList();
            foreach (var item in _PaymentsList)
            {
                PaymentModeHistory _PaymentModeHistory = new PaymentModeHistory
                {
                    PaymentId = item.Id,
                    ModeOfPayment = "Cash",
                    Amount = item.PaidAmount,
                    CreatedDate = item.CreatedDate,
                    ModifiedDate = item.ModifiedDate,
                    CreatedBy = item.CreatedBy,
                    ModifiedBy = item.ModifiedBy
                };

                _context.Add(_PaymentModeHistory);
                var result = _context.SaveChanges();
            }
        }
    }
}
