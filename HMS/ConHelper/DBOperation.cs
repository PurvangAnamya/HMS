using HMS.Data;
using HMS.Helpers;
using HMS.Models;
using HMS.Models.CheckupSummaryViewModel;
using HMS.Models.MedicineHistoryViewModel;
using HMS.Models.MedicinesViewModel;
using HMS.Models.PaymentModeHistoryViewModel;
using HMS.Models.PaymentsViewModel;
using HMS.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.ConHelper
{
    public class DBOperation : IDBOperation
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        public DBOperation(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        public async Task<PatientTest> CreatePatientTest(AddPaymentViewModel _AddPaymentViewModel, string strUserName)
        {
            PatientTest _PatientTest = new PatientTest();
            _PatientTest.PatientId = _AddPaymentViewModel.PatientId;
            _PatientTest.VisitId = _AddPaymentViewModel.VisitId;

            _PatientTest.PaymentStatus = PaymentStatus.Unpaid;
            _PatientTest.TestDate = DateTime.Now;
            _PatientTest.DeliveryDate = DateTime.Now;
            _PatientTest.CreatedDate = DateTime.Now;
            _PatientTest.ModifiedDate = DateTime.Now;
            _PatientTest.CreatedBy = strUserName;
            _PatientTest.ModifiedBy = strUserName;
            _context.Add(_PatientTest);
            await _context.SaveChangesAsync();

            return _PatientTest;
        }

        public async Task<Payments> CreatePayments(AddPaymentViewModel _AddPaymentViewModel, string strUserName)
        {
            var _CurrencyId = _context.Currency.Where(x => x.IsDefault == true).FirstOrDefault().Id;
            Payments _Payments = new Payments();
            _Payments.PatientId = _AddPaymentViewModel.PatientId;
            _Payments.VisitId = _AddPaymentViewModel.VisitId;
            _Payments.CurrencyId = _CurrencyId;

            _Payments.PaymentStatus = PaymentStatus.Unpaid;
            //_Payments.InvoiceNo = StaticData.GetUniqueID("INV");
            _Payments.CreatedDate = DateTime.Now;
            _Payments.ModifiedDate = DateTime.Now;
            _Payments.CreatedBy = strUserName;
            _Payments.ModifiedBy = strUserName;
            _context.Add(_Payments);
            await _context.SaveChangesAsync();

            PatientVisitPaymentHistory _PatientVisitPaymentHistory = new PatientVisitPaymentHistory
            {
                VisitId = _AddPaymentViewModel.VisitId,
                PaymentId = _Payments.Id,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                CreatedBy = strUserName,
                ModifiedBy = strUserName,
            };

            _context.Add(_PatientVisitPaymentHistory);
            await _context.SaveChangesAsync();

            return _Payments;
        }

        public async Task<PaymentsDetails> CreatePaymentsDetails(AddPaymentViewModel _AddPaymentViewModel, int _PaymentItemType, string strUserName)
        {
            PaymentsDetails _PaymentsDetails = new PaymentsDetails();
            if (_PaymentItemType == PaymentItemType.LabTest)
            {
                var _LabTests = await _context.LabTests.FindAsync(_AddPaymentViewModel.LabTestsId);
                _PaymentsDetails.PaymentItemCode = _LabTests.PaymentItemCode;
                _PaymentsDetails.Quantity = 1;
                _PaymentsDetails.UnitPrize = _LabTests.UnitPrice;
            }
            else if (_PaymentItemType == PaymentItemType.Medicine)
            {
                var _Medicines = await _context.Medicines.FindAsync(_AddPaymentViewModel.MedicineId);
                _PaymentsDetails.PaymentItemCode = _Medicines.PaymentItemCode;
                _PaymentsDetails.Quantity = _AddPaymentViewModel.NoofDays * _AddPaymentViewModel.WhentoTakeDayCount;
                _PaymentsDetails.UnitPrize = _Medicines.SellPrice;
            }
            else if (_PaymentItemType == PaymentItemType.Consultation)
            {
                var _PaymentCategories = await _context.PaymentCategories.FindAsync(_AddPaymentViewModel.PaymentCategoriesId);
                _PaymentsDetails.PaymentItemCode = _PaymentCategories.PaymentItemCode;
                _PaymentsDetails.Quantity = 1;
                _PaymentsDetails.UnitPrize = _PaymentCategories.UnitPrice;
            }

            _PaymentsDetails.TotalAmount = (double)Math.Round((decimal)(_PaymentsDetails.Quantity * _PaymentsDetails.UnitPrize), 2);
            _PaymentsDetails.ItemDetailId = _AddPaymentViewModel.ItemDetailId;
            _PaymentsDetails.PaymentsId = _AddPaymentViewModel.PaymentsId;
            _PaymentsDetails.CreatedDate = DateTime.Now;
            _PaymentsDetails.ModifiedDate = DateTime.Now;
            _PaymentsDetails.CreatedBy = strUserName;
            _PaymentsDetails.ModifiedBy = strUserName;
            _context.Add(_PaymentsDetails);
            await _context.SaveChangesAsync();
            return _PaymentsDetails;
        }

        public async Task<Payments> UpdateMedicineSellState(MedicineSellStateViewModel _MedicineSellStateViewModel)
        {
            var _Payments = await _context.Payments.FindAsync(_MedicineSellStateViewModel.Id);
            _MedicineSellStateViewModel.ModifiedDate = DateTime.Now;
            _context.Entry(_Payments).CurrentValues.SetValues(_MedicineSellStateViewModel);
            await _context.SaveChangesAsync();
            return _Payments;
        }
        public async Task<MedicineHistoryCRUDViewModel> UpdateMedicineInventory(UpdateMedicineInventoryViewModel vm)
        {
            try
            {
                Medicines _Medicines = _context.Medicines.Where(x => x.PaymentItemCode == vm.PaymentItemCode).SingleOrDefault();
                double? _OldQuantity = _Medicines.Quantity;

                if (vm.IsAddition)
                {
                    _Medicines.Quantity = _Medicines.Quantity + vm.TranQuantity;
                }
                else
                {
                    _Medicines.Quantity = _Medicines.Quantity - vm.TranQuantity;
                }

                _Medicines.ModifiedDate = DateTime.Now;
                _Medicines.ModifiedBy = vm.CurrentUserName;
                _context.Medicines.Update(_Medicines);
                await _context.SaveChangesAsync();

                MedicinesCRUDViewModel _MedicinesCRUDViewModel = new MedicinesCRUDViewModel();
                _MedicinesCRUDViewModel = _Medicines;

                MedicineHistoryCRUDViewModel _MedicineHistoryCRUDViewModel = new MedicineHistoryCRUDViewModel();
                _MedicineHistoryCRUDViewModel = _MedicinesCRUDViewModel;

                _MedicineHistoryCRUDViewModel.TranQuantity = (int)vm.TranQuantity;
                _MedicineHistoryCRUDViewModel.OldQuantity = _OldQuantity;
                _MedicineHistoryCRUDViewModel.NewQuantity = _Medicines.Quantity;
                _MedicineHistoryCRUDViewModel.Action = vm.Action;

                _context.MedicineHistory.Add(_MedicineHistoryCRUDViewModel);
                await _context.SaveChangesAsync();

                return _MedicineHistoryCRUDViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<PaymentModeHistory> CreatePaymentModeHistory(PaymentModeHistoryCRUDViewModel vm)
        {
            try
            {
                PaymentModeHistory _PaymentModeHistory = new PaymentModeHistory();
                _PaymentModeHistory = vm;
                _PaymentModeHistory.CreatedDate = DateTime.Now;
                _PaymentModeHistory.ModifiedDate = DateTime.Now;
                _context.Add(_PaymentModeHistory);
                await _context.SaveChangesAsync();
                return _PaymentModeHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<PaymentModeHistory> UpdatePaymentModeHistory(PaymentModeHistoryCRUDViewModel vm)
        {
            try
            {
                var _PaymentModeHistory = await _context.PaymentModeHistory.FindAsync(vm.Id);
                _PaymentModeHistory.ModifiedDate = DateTime.Now;
                _context.Entry(_PaymentModeHistory).CurrentValues.SetValues(vm);
                await _context.SaveChangesAsync();
                return _PaymentModeHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
