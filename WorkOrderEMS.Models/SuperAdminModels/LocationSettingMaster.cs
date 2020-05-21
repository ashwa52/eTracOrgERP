using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class LocationSettingMaster
    {
        public long? LocationID { get; set; }
        public int Id { get; set; }
        public decimal ParkerInvoicingCost { get; set; }
        public string StopParkingRegistration { get; set; }
        public decimal ParkeringRateIncreases { get; set; }
        public DateTime Transpireclosingprocesson { get; set; }
        public string InvoiceTemplate { get; set; }
        public string ReviewInvoiceBeforeSending { get; set; }
        public DateTime InvoicingDate { get; set; }
        public string Requestcollectionsemail { get; set; }
        public string AddBankAccount { get; set; }
        public string Settimefrequency { get; set; }
        public string Frequencyinvoices { get; set; }
        public string frequencyautomaticrecons { get; set; }
        public DateTime SetClosingDate { get; set; }
        public DateTime Automaticbillingdate { get; set; }
        public DateTime Cutoffcarddate { get; set; }

    }
}
