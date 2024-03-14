using HMS.Models.PaymentModeHistoryViewModel;
using HMS.Models.PaymentsDetailsViewModel;
using System.Collections.Generic;

namespace HMS.Models.PaymentsViewModel
{
    public class ManagePaymentsViewModel
    {
        public PaymentsCRUDViewModel PaymentsCRUDViewModel { get; set; }
        public PaymentsDetailsCRUDViewModel PaymentsDetailsCRUDViewModel { get; set; }
        public List<PaymentsDetailsCRUDViewModel> listPaymentsDetailsCRUDViewModel { get; set; }             
        public List<PaymentModeHistoryCRUDViewModel> listPaymentModeHistoryCRUDViewModel { get; set; }
    }
}
