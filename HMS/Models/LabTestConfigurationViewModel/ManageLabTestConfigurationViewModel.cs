using HMS.Models.LabTestsViewModel;
using System.Collections.Generic;

namespace HMS.Models.LabTestConfigurationViewModel
{
    public class ManageLabTestConfigurationViewModel
    {
        public LabTestsCRUDViewModel LabTestsCRUDViewModel { get; set; }
        public LabTestConfigurationCRUDViewModel LabTestConfigurationCRUDViewModel { get; set; }
        public List<LabTestConfiguration> listLabTestConfiguration { get; set; }
    }
}
