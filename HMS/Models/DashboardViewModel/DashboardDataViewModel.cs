using HMS.Models.CheckupSummaryViewModel;
using HMS.Models.PaymentsViewModel;
using System.Collections.Generic;

namespace HMS.Models.DashboardViewModel
{
    public class DashboardDataViewModel
    {
        public DashboardSummaryViewModel DashboardSummaryViewModel { get; set; }
        public List<PaymentsCRUDViewModel> listPaymentsCRUDViewModel { get; set; }
        public List<CheckupSummaryCRUDViewModel> listCheckupSummaryCRUDViewModel { get; set; }
    }
}
