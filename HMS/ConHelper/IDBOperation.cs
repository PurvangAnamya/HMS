using HMS.Models;
using HMS.Models.CheckupSummaryViewModel;
using HMS.Models.MedicineHistoryViewModel;
using HMS.Models.MedicinesViewModel;
using HMS.Models.PaymentModeHistoryViewModel;
using HMS.Models.PaymentsViewModel;
using System.Threading.Tasks;

namespace HMS.ConHelper
{
    public interface IDBOperation
    {
        Task<PatientTest> CreatePatientTest(AddPaymentViewModel _AddPaymentViewModel, string strUserName);
        Task<Payments> CreatePayments(AddPaymentViewModel _AddPaymentViewModel, string strUserName);
        Task<PaymentsDetails> CreatePaymentsDetails(AddPaymentViewModel _AddPaymentViewModel, int _PaymentItemType, string strUserName);
        Task<Payments> UpdateMedicineSellState(MedicineSellStateViewModel _MedicineSellStateViewModel);
        Task<MedicineHistoryCRUDViewModel> UpdateMedicineInventory(UpdateMedicineInventoryViewModel vm);
        Task<PaymentModeHistory> CreatePaymentModeHistory(PaymentModeHistoryCRUDViewModel vm);
        Task<PaymentModeHistory> UpdatePaymentModeHistory(PaymentModeHistoryCRUDViewModel vm);
    }
}
