using HMS.Controllers;
using HMS.Helpers;
using HMS.Models;
using HMS.Models.DoctorsInfoViewModel;
using HMS.Models.PatientInfoViewModel;
using HMS.Models.UserProfileViewModel;

namespace HMS.Data
{
    public class SeedData
    {
        public IEnumerable<MedicineCategories> GetMedicineCategoriesList()
        {
            return new List<MedicineCategories>
            {
                new MedicineCategories { Name = "Fever", Description = "Fever"},
                new MedicineCategories { Name = "Severe Pain", Description = "Severe Pain"},
                new MedicineCategories { Name = "Infection", Description = "Infection"},
                new MedicineCategories { Name = "Anxiety", Description = "Anxiety"},
                new MedicineCategories { Name = "Depression", Description = "Depression"},
                new MedicineCategories { Name = "Cold", Description = "Cold"},

                new MedicineCategories { Name = "Nerve Pain", Description = "Nerve Pain"},
                new MedicineCategories { Name = "High BP", Description = "High BP"},
                new MedicineCategories { Name = "GERD", Description = "GERD"},
            };
        }
        public IEnumerable<Unit> GetUnitList()
        {
            return new List<Unit>
            {
                new Unit { Name = "ml", Description = "milliliter"},
                new Unit { Name = "mg", Description = "milligram"},
                new Unit { Name = "pc", Description = "pc"},
                new Unit { Name = "l", Description = "liter"},
            };
        }
        public IEnumerable<MedicineManufacture> GetMedicineManufactureList()
        {
            return new List<MedicineManufacture>
            {
                new MedicineManufacture { Name = "Johnson & Johnson", Address = "USA", Description = "TBD"},
                new MedicineManufacture { Name = "Pfizer", Address = "USA", Description = "TBD"},
                new MedicineManufacture { Name = "Sanofi", Address = "USA", Description = "TBD"},
                new MedicineManufacture { Name = "Novartis", Address = "USA", Description = "TBD"},
                new MedicineManufacture { Name = "GlaxoSmithKline", Address = "USA", Description = "TBD"},
            };
        }
        public IEnumerable<Medicines> GetMedicinesList()
        {
            return new List<Medicines>
            {
                new Medicines { Code=StaticData.GetUniqueID("M"), MedicineCategoryId = 1, PaymentItemCode=StaticData.GetUniqueID("MED"), MedicineName = "Paracetamol 500 mg", ManufactureId=1, UnitId=1, UnitPrice = 10, SellPrice = 12, Quantity=100, ExpiryDate=DateTime.Today.AddYears(1), Description="TBD" },
                new Medicines { Code=StaticData.GetUniqueID("M"), MedicineCategoryId = 1, PaymentItemCode=StaticData.GetUniqueID("MED"), MedicineName = "Paracetamol 600 mg", ManufactureId=1, UnitId=1, UnitPrice = 20, SellPrice = 22, Quantity=100, ExpiryDate=DateTime.Today.AddYears(1), Description="TBD" },
                new Medicines { Code=StaticData.GetUniqueID("M"), MedicineCategoryId = 1, PaymentItemCode=StaticData.GetUniqueID("MED"), MedicineName = "Paracetamol 700 mg", ManufactureId=2, UnitId=1, UnitPrice = 30, SellPrice = 32, Quantity=100, ExpiryDate=DateTime.Today.AddYears(1), Description="TBD" },
                new Medicines { Code=StaticData.GetUniqueID("M"), MedicineCategoryId = 2, PaymentItemCode=StaticData.GetUniqueID("MED"), MedicineName = "Oxycodone", ManufactureId=2, UnitId=1, UnitPrice = 30, SellPrice = 32, Quantity=100, ExpiryDate=DateTime.Today.AddYears(1), Description="TBD" },
                new Medicines { Code=StaticData.GetUniqueID("M"), MedicineCategoryId = 2, PaymentItemCode=StaticData.GetUniqueID("MED"), MedicineName = "Tylenol", ManufactureId=2, UnitId=1, UnitPrice = 40, SellPrice = 42, Quantity=100, ExpiryDate=DateTime.Today.AddYears(1), Description="TBD" },
            };
        }


        public IEnumerable<BedCategories> GetBedCategoriesList()
        {
            return new List<BedCategories>
            {
                new BedCategories { Name = "General", Description = "General Bed",BedPrice= 300},
                new BedCategories { Name = "ICU", Description = "ICU Bed",BedPrice= 700},
                new BedCategories { Name = "Semi Special", Description = "Semi Special Bed", BedPrice = 400},
                new BedCategories { Name = "Special Rooms", Description = "Special Rooms", BedPrice = 1000},
                new BedCategories { Name = "VIP", Description = "VIP"  , BedPrice = 500},
            };
        }
        public IEnumerable<Bed> GetBedList()
        {
            return new List<Bed>
            {
                new Bed { BedCategoryId = 1, No="B101", Description="General Bed"},
                new Bed { BedCategoryId = 2, No="B201", Description="ICU Bed"},
                new Bed { BedCategoryId = 3, No="B301", Description="Semi Special Bed"},
                new Bed { BedCategoryId = 4, No="B401", Description="Special Rooms Bed"},
                new Bed { BedCategoryId = 5, No="B501", Description="VIP Bed"},
            };
        }

        public IEnumerable<PatientInfoCRUDViewModel> GetPatientInfoList()
        {
            return new List<PatientInfoCRUDViewModel>
            {
                new PatientInfoCRUDViewModel { FirstName = "Mr", LastName = "Tom", DateOfBirth=DateTime.Now, Email="p1@gmail.com", MaritalStatus="Single", BloodGroup="AB+", Gender="Male", Phone="017889977000", Address="California, USA" },
                new PatientInfoCRUDViewModel { FirstName = "Mr", LastName = "Bin", DateOfBirth=DateTime.Now, Email="p2@gmail.com", MaritalStatus="Married", BloodGroup="A+", Gender="Male", Phone="017889977000", Address="California, USA" },
                new PatientInfoCRUDViewModel { FirstName = "Ms", LastName = "Gaga", DateOfBirth=DateTime.Now, Email="p3@gmail.com", MaritalStatus="Married", BloodGroup="O+", Gender="Female", Phone="017889977000", Address="California, USA" },
                new PatientInfoCRUDViewModel { FirstName = "Mr", LastName = "Trump", DateOfBirth=DateTime.Now, Email="p4@gmail.com", MaritalStatus="Single", BloodGroup="AB+", Gender="Male", Phone="017889977000", Address="California, USA" },
                new PatientInfoCRUDViewModel { FirstName = "Ms", LastName = "Clinton", DateOfBirth=DateTime.Now, Email="p5@gmail.com", MaritalStatus="Single", BloodGroup="AB+", Gender="Female", Phone="017889977000", Address="California, USA" },
            };
        }

        public IEnumerable<UserProfileCRUDViewModel> GetUserProfileList()
        {
            return new List<UserProfileCRUDViewModel>
            {
                new UserProfileCRUDViewModel { FirstName = "Nurse 01", LastName = "User", UserType = UserType.Nurse, Email = "nurse1@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/nurse1.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Nurse 02", LastName = "User", UserType = UserType.Nurse, Email = "nurse2@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/nurse2.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Nurse 03", LastName = "User", UserType = UserType.Nurse, Email = "nurse3@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/nurse3.png", Address = "California", Country = "USA", },

                new UserProfileCRUDViewModel { FirstName = "Laboraties 01", LastName = "User", UserType = UserType.Laboraties, Email = "laboraties1@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U4.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Laboraties 02", LastName = "User", UserType = UserType.Laboraties, Email = "laboraties2@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U5.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Laboraties 03", LastName = "User", UserType = UserType.Laboraties, Email = "laboraties3@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U6.png", Address = "California", Country = "USA", },

                new UserProfileCRUDViewModel { FirstName = "Pharmacist", LastName = "User", UserType = UserType.Pharmacist, Email = "pharmacist1@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U7.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Pharmacist", LastName = "User", UserType = UserType.Pharmacist, Email = "pharmacist2@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U8.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Pharmacist", LastName = "User", UserType = UserType.Pharmacist, Email = "pharmacist3@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U9.png", Address = "California", Country = "USA", },

                new UserProfileCRUDViewModel { FirstName = "Accountants", LastName = "User", UserType = UserType.Accountants, Email = "accountants1@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U10.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Accountants", LastName = "User", UserType = UserType.Accountants, Email = "accountants2@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U11.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Accountants", LastName = "User", UserType = UserType.Accountants, Email = "accountants3@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U12.png", Address = "California", Country = "USA", },
            };
        }

        public IEnumerable<DoctorsInfoCRUDViewModel> GetDoctorsInfoList()
        {
            return new List<DoctorsInfoCRUDViewModel>
            {
                new DoctorsInfoCRUDViewModel { FirstName = "Dr.", LastName = "Robert", UserType = UserType.Doctor, Email="Robert@gmail.com", PasswordHash="123", ConfirmPassword="123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/DocIcon/U1.png" },
                new DoctorsInfoCRUDViewModel { FirstName = "Dr.", LastName = "Genefar", UserType = UserType.Doctor, Email="Genefar@gmail.com", PasswordHash="123", ConfirmPassword="123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/DocIcon/U2.png" },
                new DoctorsInfoCRUDViewModel { FirstName = "Dr.", LastName = "James", UserType = UserType.Doctor, Email="James@gmail.com", PasswordHash="123", ConfirmPassword="123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/DocIcon/U3.png" },
                new DoctorsInfoCRUDViewModel { FirstName = "Dr.", LastName = "Hasan", UserType = UserType.Doctor, Email="Hasan@gmail.com", PasswordHash="123", ConfirmPassword="123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/DocIcon/U4.png" },
                new DoctorsInfoCRUDViewModel { FirstName = "Dr.", LastName = "Smith", UserType = UserType.Doctor, Email="Smith@gmail.com", PasswordHash="123", ConfirmPassword="123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/DocIcon/U5.png" },

                new DoctorsInfoCRUDViewModel { FirstName = "Dr.", LastName = "Kate", UserType = UserType.Doctor, Email="Kate@gmail.com", PasswordHash="123", ConfirmPassword="123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/DocIcon/U6.png" },
                new DoctorsInfoCRUDViewModel { FirstName = "Dr.", LastName = "Ameen", UserType = UserType.Doctor, Email="Ameen@gmail.com", PasswordHash="123", ConfirmPassword="123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/DocIcon/U7.png" },
            };
        }

        public IEnumerable<ExpenseCategories> GetExpenseCategoriesList()
        {
            return new List<ExpenseCategories>
            {
                new ExpenseCategories { Name = "Laundry", Description = "Laundry"},
                new ExpenseCategories { Name = "Office Rent", Description = "Office Rent"},
                new ExpenseCategories { Name = "Staff Salary ", Description = "Staff Salary "},
                new ExpenseCategories { Name = "LAB Instrument", Description = "LAB Instrument"},
                new ExpenseCategories { Name = "Miscellaneous", Description = "Miscellaneous"}
            };
        }
        public IEnumerable<PaymentCategories> GetPaymentCategoriesList()
        {
            return new List<PaymentCategories>
            {
                new PaymentCategories { PaymentItemCode = StaticData.GetUniqueID("CMN"), Name = "Bed", UnitPrice = 100, Description = "Bed"},
                new PaymentCategories { PaymentItemCode = StaticData.GetUniqueID("CMN"), Name = "Blood Report", UnitPrice = 200, Description = "Blood Report"},
                new PaymentCategories { PaymentItemCode = StaticData.GetUniqueID("CMN"), Name = "Consulting Charge", UnitPrice = 300, Description = "Consulting Charge"},
                new PaymentCategories { PaymentItemCode = StaticData.GetUniqueID("CMN"), Name = "Injection Charge", UnitPrice = 400, Description = "Injection Charge"},
                new PaymentCategories { PaymentItemCode = StaticData.GetUniqueID("CMN"), Name = "Medicine Charge", UnitPrice = 500, Description = "Medicine Charge"},
                new PaymentCategories { PaymentItemCode = StaticData.GetUniqueID("CMN"), Name = "MRI Charge", UnitPrice = 400, Description = "MRI Charge"},
                new PaymentCategories { PaymentItemCode = StaticData.GetUniqueID("CMN"), Name = "X-ray Charge", UnitPrice = 200, Description = "X-ray Charge"},
                new PaymentCategories { PaymentItemCode = StaticData.GetUniqueID("CMN"), Name = "COVID", UnitPrice = 500, Description = "COVID"},
            };
        }

        public IEnumerable<LabTestCategories> GetLabTestCategoriesList()
        {
            return new List<LabTestCategories>
            {
                new LabTestCategories { Name = "Blood", Description = "Blood"},
                new LabTestCategories { Name = "Sugar", Description = "Sugar"},
                new LabTestCategories { Name = "Hemoglobin A1C", Description = "Hemoglobin A1C"},
                new LabTestCategories { Name = "Urinalysis", Description = "Urinalysis"},
                new LabTestCategories { Name = "Thyroid Stimulating Hormone", Description = "Thyroid Stimulating Hormone"},
                new LabTestCategories { Name = "Liver Panel", Description = "Liver Panel"},
                new LabTestCategories { Name = "Basic Metabolic Panel", Description = "Basic Metabolic Panel"},
            };
        }

        public IEnumerable<LabTests> GetLabTestsList()
        {
            return new List<LabTests>
            {
                new LabTests { LabTestCategoryId = 1, PaymentItemCode = StaticData.GetUniqueID("LAB"), LabTestName = "Complete Blood Count(CBC)", Unit="Liter(L)", UnitPrice = 200, ReferenceRange="Male: 4.35-5.65 trillion cells/L*, Female: 3.92-5.13 trillion cells/L" },
                new LabTests { LabTestCategoryId = 1, PaymentItemCode = StaticData.GetUniqueID("LAB"), LabTestName = "Lipid panel ", Unit="mg/dL", UnitPrice = 300, ReferenceRange="Less than 150 mg/dL" },
                new LabTests { LabTestCategoryId = 1, PaymentItemCode = StaticData.GetUniqueID("LAB"), LabTestName = "Liver Panel", Unit="Liter(L)", UnitPrice = 400, ReferenceRange="The normal range of values for ALT (SGPT) is about 7 to 56 units per liter of serum." },
                new LabTests { LabTestCategoryId = 1, PaymentItemCode = StaticData.GetUniqueID("LAB"), LabTestName = "Hemoglobin A1C", Unit="Liter(L)", UnitPrice = 500, ReferenceRange="The normal range for the hemoglobin A1c level is between 4% and 5.6%." },
            };
        }

        public IEnumerable<Currency> GetCurrencyList()
        {
            return new List<Currency>
            {
                new Currency { Name = "US Dollar", Code = "USD", Symbol = "$", Country = "United States",  Description = "US Dollar", IsDefault = true},
                new Currency { Name = "Euro", Code = "EUR", Symbol = "€", Country = "European Union",  Description = "European Union Currency", IsDefault = false},
                new Currency { Name = "Pounds Sterling", Code = "GBD", Symbol = "£", Country = "UK",  Description = "British Pounds", IsDefault = false},
                new Currency { Name = "Yen", Code = "JPY", Symbol = "¥", Country = "Japan",  Description = "Japany Yen", IsDefault = false},
                new Currency { Name = "Taka", Code = "BDT", Symbol = "৳", Country = "Bangladesh",  Description = "Bangladeshi Taka", IsDefault = false},
                new Currency { Name = "Australia Dollars", Code = "AUD", Symbol = "A$", Country = "Australia",  Description = "Australia Dollar (AUD)", IsDefault = false},
            };
        }
        public IEnumerable<InsuranceCompanyInfo> GetInsuranceCompanyInfoList()
        {
            return new List<InsuranceCompanyInfo>
            {
                new InsuranceCompanyInfo { Name = "Berkshire Hathaway", Address = "USA", CoverageDetails = "TBD", Phone = "90980899", Email = "hathaway@gmail.com"},
                new InsuranceCompanyInfo { Name = "Ping An Insurance", Address = "China", CoverageDetails = "TBD", Phone = "101010101", Email = "ping@gmail.com"},
                new InsuranceCompanyInfo { Name = "China Life Insurance", Address = "China", CoverageDetails = "TBD", Phone = "202020202", Email = "chinalife@gmail.com"},
                new InsuranceCompanyInfo { Name = "Allianz", Address = "USA", CoverageDetails = "TBD", Phone = "303030303", Email = "allianz@gmail.com"},
                new InsuranceCompanyInfo { Name = "Metlife", Address = "USA", CoverageDetails = "TBD", Phone = "404040404", Email = "metlife@gmail.com"},
            };
        }
        public IEnumerable<ManageUserRoles> GetManageUserRolesList()
        {
            return new List<ManageUserRoles>
            {
                new ManageUserRoles { Name = "Admin", Description = "User Role: New"},
                new ManageUserRoles { Name = "General", Description = "User Role: General"},
                new ManageUserRoles { Name = "Doctor", Description = "User Role: Doctor"},
                new ManageUserRoles { Name = "Patient", Description = "User Role: Patient"},
            };
        }

        public IEnumerable<SampleChetnaManage> GetSampleChetnaManageList()
        {
            return new List<SampleChetnaManage>
            {
                new SampleChetnaManage { Title = "S1", Description = "SampleChetnaManage: New1", DateOfBirth=DateTime.Now},
                new SampleChetnaManage { Title = "S2", Description = "SampleChetnaManage: New2", DateOfBirth=DateTime.Now},
                new SampleChetnaManage { Title = "S3", Description = "SampleChetnaManage: New3", DateOfBirth = DateTime.Now},

            };

        }


        public IEnumerable<Department> GetDepartmentList()
        {
            return new List<Department>
            {
                new Department { Name = "IT", Description = "IT Department"},
                new Department { Name = "HR", Description = "HR Department"},
                new Department { Name = "Finance", Description = "Finance Department"},
                new Department { Name = "Procurement", Description = "Procurement Department"},
                new Department { Name = "Legal", Description = "Procurement Department"},
            };
        }
        public IEnumerable<SubDepartment> GetSubDepartmentList()
        {
            return new List<SubDepartment>
            {
                new SubDepartment { DepartmentId = 1, Name = "QA", Description = "QA Department"},
                new SubDepartment { DepartmentId = 1, Name = "Software Development", Description = "Software Development Department"},
                new SubDepartment { DepartmentId = 1, Name = "Operation", Description = "Operation Department"},
                new SubDepartment { DepartmentId = 1, Name = "PM", Description = "Project Management Department"},
                new SubDepartment { DepartmentId = 2, Name = "Recruitment", Description = "Recruitment Department"},
            };
        }
        public IEnumerable<Designation> GetDesignationList()
        {
            return new List<Designation>
            {
                new Designation { Name = "MBBS", Description = "MBBS"},
                new Designation { Name = "HR", Description = "HR"},
                new Designation { Name = "Accounts Manager", Description = "Accounts Manager"},
                new Designation { Name = "Accounts Manager", Description = "Accounts Manager"},
                new Designation { Name = "Pharmacist", Description = "Pharmacist"},
                new Designation { Name = "Software Engineer", Description = "Employee Job Designation"},
                new Designation { Name = "Head Of Engineering", Description = "Employee Job Designation"},
                new Designation { Name = "Software Architect", Description = "Employee Job Designation"},
                new Designation { Name = "QA Engineer", Description = "Employee Job Designation"},
                new Designation { Name = "DevOps Engineer", Description = "Employee Job Designation"},
            };
        }
        public CompanyInfo GetCompanyInfo()
        {
            return new CompanyInfo
            {
                Name = "XYZ Company Limited",
                ApplicationTitle = "Complaint Mng Sys",
                CompanyLogoImagePath = "/upload/blank_logo.png",
                Currency = "৳",
                Address = "Dhaka, Bangladesh",
                City = "Dhaka",
                Country = "Bangladesh",
                Phone = "132546789",
                Fax = "9999",
                Email = "XYZ@GMAIL.COM",
                Website = "www.wyx.com",
            };
        }
    }
}
