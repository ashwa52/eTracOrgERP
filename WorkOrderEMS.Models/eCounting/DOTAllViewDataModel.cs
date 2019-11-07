using System;
using System.Collections.Generic;
namespace WorkOrderEMS.Models
{

    public class DOTManagementViewDataModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LastFourofSSN { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string LicenseNumber { get; set; }
        public string State { get; set; }
        public DateTime LicenseExpirationDate { get; set; }
        public string Licenseclasscommerciallicense { get; set; }
        public string Endorsementscommerciallicense { get; set; }
        public DateTime HazmatExpirationDate { get; set; }
        public string driverpassedtheirdrugtest { get; set; }
        public DateTime MVRDate { get; set; }
        public DateTime HireDate { get; set; }
        public string Notes { get; set; }
        public List<DOTManagementViewDataModel> NewDOTDetails { get; set; }
    }
}


