using HMS.Models.CheckupSummaryViewModel;
using HMS.Models.PaymentsViewModel;
using System.Collections.Generic;

namespace HMS.Models.DashboardViewModel
{
    public class DashboardDataViewModel
    {
        public DashboardDataViewModel()
        {
            // Initialize userRolesCountsModel to an empty list to avoid NullReferenceException
            userRolesCountsModel = new List<UserRoleCountsModel>();
        }

        public DashboardSummaryViewModel DashboardSummaryViewModel { get; set; }
        public List<PaymentsCRUDViewModel> listPaymentsCRUDViewModel { get; set; }
        public List<CheckupSummaryCRUDViewModel> listCheckupSummaryCRUDViewModel { get; set; }
        public List<UserRoleCountsModel> userRolesCountsModel { get; set;}
    }
}
