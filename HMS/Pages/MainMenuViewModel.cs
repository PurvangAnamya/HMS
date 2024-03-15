namespace HMS.Pages
{
    public class MainMenuViewModel
    {
        public bool Admin { get; set; }
        public bool SuperAdmin { get; set; }
        public bool Settings { get; set; }
        public bool Dashboard { get; set; }
        public bool UserManagement { get; set; }
        public bool UserInfoFromBrowser { get; set; }
        public bool AuditLogs { get; set; }
        public bool UserProfile { get; set; }
        public bool EmailSetting { get; set; }
        public bool IdentitySetting { get; set; }
        public bool LoginHistory { get; set; }
        public bool CompanyInfo { get; set; }
        public bool Currency { get; set; }

        //Business       
        public bool PatientInfo { get; set; }
        public bool PatientAppointment { get; set; }
        public bool PatientPrescriptions { get; set; }
        public bool VitalSigns { get; set; }
        public bool Checkup { get; set; }
        public bool DoctorsInfo { get; set; }
        public bool MedicineCategories { get; set; }
        public bool Medicines { get; set; }
        public bool MedicineManufacture { get; set; }
        public bool MedicineHistory { get; set; }
        public bool Unit { get; set; }
        public bool MedicineLabel { get; set; }
        public bool BedCategories { get; set; }
        public bool Bed { get; set; }
        public bool BedAllotments { get; set; }
        public bool Payments { get; set; }
        public bool PatientPayments { get; set; }        
        public bool InsuranceCompanyInfo { get; set; }
        public bool Expenses { get; set; }
        public bool ExpenseCategories { get; set; }
        public bool PaymentCategories { get; set; }
        public bool LabTestCategories { get; set; }
        public bool LabTests { get; set; }
        public bool LabTestConfiguration { get; set; }
        public bool PatientTest { get; set; }

        public bool Nurse { get; set; }
        public bool Laboraties { get; set; }
        public bool Pharmacist { get; set; }
        public bool Accountants { get; set; }
        public bool ServicePaymentsReport { get; set; }
        public bool PaymentsReport { get; set; }
        public bool PaymentModeReport { get; set; }
        public bool ExpensesReport { get; set; }

        //New
        public bool ManageClinic { get; set; }
        public bool ManagePayments { get; set; }
        public bool ManageMedicine { get; set; }

        public bool SystemRole { get; set; }
        public bool ManageUserRoles { get; set; }
        public bool SampleChetnaManage { get; set; }
        public bool Designation { get; set; }
        public bool Department { get; set; }
        public bool SubDepartment { get; set; }
    }
}