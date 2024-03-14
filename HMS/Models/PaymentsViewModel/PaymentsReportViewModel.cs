using HMS.Models.CompanyInfoViewModel;
using HMS.Models.PatientInfoViewModel;
using HMS.Models.PaymentModeHistoryViewModel;
using HMS.Models.PaymentsDetailsViewModel;
using System.Collections.Generic;

namespace HMS.Models.PaymentsViewModel
{
    public class PaymentsReportViewModel
    {
        public PaymentsCRUDViewModel PaymentsCRUDViewModel { get; set; }
        public List<PaymentsDetailsCRUDViewModel> listPaymentsDetailsCRUDViewModel { get; set; }
        public List<PaymentModeHistoryCRUDViewModel> listPaymentModeHistoryCRUDViewModel { get; set; }
        public PatientInfoCRUDViewModel PatientInfoCRUDViewModel { get; set; }
        public CompanyInfoCRUDViewModel CompanyInfoCRUDViewModel { get; set; }
    }
}
