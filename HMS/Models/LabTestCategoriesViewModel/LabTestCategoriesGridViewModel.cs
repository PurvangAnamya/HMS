using System;

namespace HMS.Models.LabTestCategoriesViewModel
{
    public class LabTestCategoriesGridViewModel : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Hospital { get; set; }


    }
}
