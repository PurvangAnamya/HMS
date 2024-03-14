using HMS.Data;
using HMS.Models;
using HMS.Models.CheckupMedicineDetailsViewModel;
using HMS.Models.CheckupSummaryViewModel;
using HMS.Models.CommonViewModel;
using HMS.Models.CompanyInfoViewModel;
using HMS.Models.DoctorsInfoViewModel;
using HMS.Models.ExpensesViewModel;
using HMS.Models.LabTestsViewModel;
using HMS.Models.MedicineHistoryViewModel;
using HMS.Models.MedicinesViewModel;
using HMS.Models.PatientAppointmentViewModel;
using HMS.Models.PatientInfoViewModel;
using HMS.Models.PatientTestDetailViewModel;
using HMS.Models.PatientTestViewModel;
using HMS.Models.PaymentsDetailsViewModel;
using HMS.Models.PaymentsViewModel;
using HMS.Models.ReportViewModel;
using HMS.Models.UserProfileViewModel;
using UAParser;

namespace HMS.Services
{
    public interface ICommon
    {
        string UploadedFile(IFormFile ProfilePicture);
        Task<SMTPEmailSetting> GetSMTPEmailSetting();
        Task<SendGridSetting> GetSendGridEmailSetting();
        UserProfile GetByUserProfile(Int64 id);
        Task<bool> InsertLoginHistory(LoginHistory _LoginHistory, ClientInfo _ClientInfo);
        CompanyInfoCRUDViewModel GetCompanyInfo();

        IQueryable<ItemDropdownListViewModel> LoadddlMedicineCategories();
        IQueryable<ItemDropdownListViewModel> LoadddlUnit();
        IQueryable<ItemDropdownListViewModel> LoadddlMedicineManufacture();
        IQueryable<ItemDropdownListViewModel> LoadddlMedicines();
        IQueryable<ItemDropdownListViewModel> LoadddBedCategories();
        IQueryable<ItemDropdownListViewModel> LoadddlBedNo(BedAllotments _BedAllotments);
        IQueryable<ItemDropdownListViewModel> LoadddlPatientName();
        IQueryable<ItemDropdownListViewModel> LoadddlDoctorName();
        IQueryable<ItemDropdownListViewModel> LoadddlLabTestCategories();
        IQueryable<ItemDropdownListViewModel> LoadddlLabTests();
        IQueryable<ItemDropdownListViewModel> LoadddlExpenseCategories();
        IQueryable<ItemDropdownListViewModel> LoadddlPaymentCategories();
        IQueryable<ItemDropdownListViewModel> LoadddlCurrencyItem();
        IQueryable<ItemDropdownListViewModel> LoadddlVisitID(Int64 PatintID);
        IQueryable<ItemDropdownListViewModel> LoadddlInsuranceCompanyInfo();


        IQueryable<DoctorsInfoCRUDViewModel> GetDoctorInfoList();

        Task<ManagePatientTestViewModel> GetByPatientTestDetails(Int64 id);
        IQueryable<PatientTestCRUDViewModel> GetPatientTestGridItem();
        Task<ManagePaymentsViewModel> GetByPatientPayments(Int64 id);

        Task<ManagePaymentsViewModel> GetByPaymentsDetails(Int64 id);
        IQueryable<PaymentsCRUDViewModel> GetPaymentDetails();
        IQueryable<PaymentsGridViewModel> GetPaymentGridList();
        IQueryable<PaymentModeReportViewModel> GetPaymentModeReportData();
        IQueryable<PatientPaymentViewModel> GetPatientPaymentsGridItem();


        Task<PaymentsReportViewModel> PrintPaymentInvoice(Int64 id);


        Task<ManageCheckupViewModel> GetByCheckupDetails(Int64 id);
        Task<ManageCheckupHistoryViewModel> GetByPatientHistory(Int64 id);
        IQueryable<CheckupSummaryCRUDViewModel> GetCheckupGridItem();
        IQueryable<PatientInfoCRUDViewModel> GetPatientInfoGridItem();
        IQueryable<ExpensesGridViewModel> GetExpensesGridItem();
        IQueryable<PatientAppointmentCRUDViewModel> GetPatientAppointmentGridItem();
        IQueryable<LabTestsCRUDViewModel> GetAllLabTests();
        IQueryable<CheckupMedicineDetailsCRUDViewModel> GetCheckupMedicineDetails();
        IQueryable<MedicinesCRUDViewModel> GetAllMedicines();
        public IQueryable<MedicineHistoryCRUDViewModel> GetAllMedicineHistory();
        Task<PatientTestDetail> CreatePatientTestDetail(PatientTestDetailCRUDViewModel vm);
        IQueryable<PaymentsDetailsCRUDViewModel> GetServicePaymentList();
        Task<List<ManageUserRolesDetails>> GetManageRoleDetailsList(Int64 id);
        IQueryable<ItemDropdownListViewModel> GetCommonddlData(string strTableName);
        IQueryable<UserProfileCRUDViewModel> GetUserProfileDetails();
        IEnumerable<T> GetTableData<T>(ApplicationDbContext dbContext) where T : class;
    }
}
