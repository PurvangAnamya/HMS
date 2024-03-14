using Microsoft.EntityFrameworkCore;
using HMS.Models;
using HMS.Models.PaymentsViewModel;
using HMS.Models.CommonViewModel;

namespace HMS.Data
{
    public class ApplicationDbContext : AuditableIdentityContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<PaymentsGridViewModel>().HasNoKey();
            builder.Entity<ItemDropdownListViewModel>().HasNoKey();
            builder.Entity<Hospital>()
            .Property(h => h.Id)
            .ValueGeneratedOnAdd();

            //builder.Entity<ApplicationUser>()
            //.HasOne(u => u.Hospital)
            //.WithMany()
            //.HasForeignKey(u => u.Hospitalid)
            //.IsRequired(false);
        }

        public DbSet<PaymentsGridViewModel> PaymentsGridViewModel { get; set; }
        public DbSet<ItemDropdownListViewModel> ItemDropdownListViewModel { get; set; }


        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }

        public DbSet<SMTPEmailSetting> SMTPEmailSetting { get; set; }
        public DbSet<SendGridSetting> SendGridSetting { get; set; }
        public DbSet<DefaultIdentityOptions> DefaultIdentityOptions { get; set; }
        public DbSet<LoginHistory> LoginHistory { get; set; }
        public DbSet<CompanyInfo> CompanyInfo { get; set; }
        public DbSet<Currency> Currency { get; set; }

        //Business
        public DbSet<Bed> Bed { get; set; }
        public DbSet<BedAllotments> BedAllotments { get; set; }
        public DbSet<BedCategories> BedCategories { get; set; }
        public DbSet<CheckupSummary> CheckupSummary { get; set; }
        public DbSet<CheckupMedicineDetails> CheckupMedicineDetails { get; set; }
        public DbSet<PatientAppointment> PatientAppointment { get; set; }
        public DbSet<Expenses> Expenses { get; set; }
        public DbSet<ExpenseCategories> ExpenseCategories { get; set; }
        public DbSet<LabTests> LabTests { get; set; }
        public DbSet<LabTestConfiguration> LabTestConfiguration { get; set; }
        public DbSet<LabTestCategories> LabTestCategories { get; set; }
        public DbSet<PatientTest> PatientTest { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<PaymentsDetails> PaymentsDetails { get; set; }
        public DbSet<PaymentModeHistory> PaymentModeHistory { get; set; }
        public DbSet<PatientVisitPaymentHistory> PatientVisitPaymentHistory { get; set; }
        public DbSet<PaymentCategories> PaymentCategories { get; set; }
        public DbSet<PatientTestDetail> PatientTestDetail { get; set; }
        public DbSet<DoctorsInfo> DoctorsInfo { get; set; }
        public DbSet<PatientInfo> PatientInfo { get; set; }
        public DbSet<MedicineCategories> MedicineCategories { get; set; }
        public DbSet<Medicines> Medicines { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<MedicineManufacture> MedicineManufacture { get; set; }
        public DbSet<MedicineHistory> MedicineHistory { get; set; }
        public DbSet<InsuranceCompanyInfo> InsuranceCompanyInfo { get; set; }
        public DbSet<UserInfoFromBrowser> UserInfoFromBrowser { get; set; }

        public DbSet<ManageUserRoles> ManageUserRoles { get; set; }
        public DbSet<ManageUserRolesDetails> ManageUserRolesDetails { get; set; }
        public DbSet<Designation> Designation { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<SubDepartment> SubDepartment { get; set; }
        public DbSet<Hospital> Hospital { get; set; }
    }
}
