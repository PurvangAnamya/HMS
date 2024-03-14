using System;

namespace HMS.Models.PaymentsViewModel
{
    public class MedicineSellStateViewModel
    {
        public Int64 Id { get; set; }
        public string InvoiceNo { get; set; }
        public int MedicineSellState { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public double? GrandTotal { get; set; }
    }
}


