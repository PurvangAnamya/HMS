using HMS.Pages;
using System;

namespace HMS.Helpers
{
    public static class StaticData
    {
        public static string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }
        public static string GetUniqueID(string Prefix)
        {
            Random _Random = new Random();
            var result = Prefix + DateTime.Now.ToString("yyyyMMddHHmmss") + _Random.Next(1, 1000);
            return result;
        }
    }
    public static class ComplaintStatusTypes
    {
        public const int New = 1;
        public const int Submited = 2;
        public const int InProgress = 3;
        public const int Pending = 4;
        public const int Resolved = 5;
        public const int Rejected = 6;
        public const int Blocker = 7;
        public const int Closed = 8;
        public const int ToDo = 9;
    }

    public static class DefaultUserPage
    {
        public static readonly string[] DefaultPageCollection =
            {
                MainMenu.Dashboard.PageName,
                MainMenu.UserProfile.PageName,               
            };
        public static readonly string[] DoctorPageCollection =
            {
                MainMenu.Dashboard.PageName,
                MainMenu.UserProfile.PageName,
                MainMenu.ManageClinic.PageName,
                MainMenu.PatientInfo.PageName, 
                MainMenu.Checkup.PageName,              
            };
        public static readonly string[] NursePageCollection =
            {
                MainMenu.Dashboard.PageName,
                MainMenu.UserProfile.PageName,
                MainMenu.ManageClinic.PageName,
                MainMenu.PatientAppointment.PageName,
                MainMenu.PatientInfo.PageName,            
            };
        public static readonly string[] AccountantsPageCollection =
            {
                MainMenu.Dashboard.PageName,
                MainMenu.UserProfile.PageName,
                MainMenu.ManagePayments.PageName,
                MainMenu.Payments.PageName,
                MainMenu.PatientPayments.PageName,
                MainMenu.Expenses.PageName,
                MainMenu.ExpenseCategories.PageName,
                MainMenu.PaymentCategories.PageName,             
            };
    }

    public static class UserType
    {
        public const int Doctor = 1;
        public const int Nurse = 2;
        public const int Laboraties = 3;
        public const int Pharmacist = 4;
        public const int Accountants = 5;
        public const int General = 6;
        public const int Patient = 7;
    }
    public static class PaymentStatus
    {
        public const string Paid = "Paid";
        public const string Unpaid = "Unpaid";
    }

    public static class PatientType
    {
        public const string InPatient = "In Patient";
        public const string OutPatient = "Out Patient";
    }

    public static class PaymentItemType
    {
        public const int Medicine = 1;
        public const int LabTest = 2;
        public const int Common = 3;
        public const int Consultation = 4;
    }
    public static class MedicineSellState
    {
        public const int None = 0;
        public const int AddIntoCheckup = 1;
        public const int SellFromPayments = 2;
        public const int SellfromPatientPayments = 3;
    }
    public static class ConnectionStrings
    {
        public const string connMSSQLNoCred = "connMSSQLNoCred";
        public const string connMSSQL = "connMSSQL";
        public const string connPostgreSQL = "connPostgreSQL";
        public const string connMySQL = "connMySQL";
        public const string connDockerBase = "connDockerBase";
        public const string connMSSQLProd = "connMSSQLProd";
        public const string connOthers = "connOthers";
    }
}
