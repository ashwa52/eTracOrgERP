//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorkOrderEMS.Data.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Company
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Company()
        {
            this.Bills = new HashSet<Bill>();
            this.Tbl_Customer_Vehicle_Details = new HashSet<Tbl_Customer_Vehicle_Details>();
            this.Tbl_Customer_Payment_Details = new HashSet<Tbl_Customer_Payment_Details>();
            this.Tbl_Customer_Vehicle_Details1 = new HashSet<Tbl_Customer_Vehicle_Details>();
            this.Tbl_Customer_Payment_Details1 = new HashSet<Tbl_Customer_Payment_Details>();
            this.Tbl_Customer_Basic_Details = new HashSet<Tbl_Customer_Basic_Details>();
            this.Tbl_Log_Customer_Basic_Details = new HashSet<Tbl_Log_Customer_Basic_Details>();
            this.Tbl_Log_Customer_Vehicle_Details = new HashSet<Tbl_Log_Customer_Vehicle_Details>();
            this.Tbl_Log_Customer_Payment_Details = new HashSet<Tbl_Log_Customer_Payment_Details>();
            this.CompanyAccountDetails = new HashSet<CompanyAccountDetail>();
            this.CompanyAccountTransactions = new HashSet<CompanyAccountTransaction>();
            this.CompanyAccountTransactions1 = new HashSet<CompanyAccountTransaction>();
            this.CompanyDetails = new HashSet<CompanyDetail>();
            this.CompanyFacilityMappings = new HashSet<CompanyFacilityMapping>();
            this.CompanyQBKs = new HashSet<CompanyQBK>();
            this.Contracts = new HashSet<Contract>();
            this.Contracts1 = new HashSet<Contract>();
            this.Insurances = new HashSet<Insurance>();
            this.Licenses = new HashSet<License>();
            this.LocationCompanyMappings = new HashSet<LocationCompanyMapping>();
            this.LogBills = new HashSet<LogBill>();
            this.LogCompanies = new HashSet<LogCompany>();
            this.LogCompanyAccountDetails = new HashSet<LogCompanyAccountDetail>();
            this.LogCompanyDetails = new HashSet<LogCompanyDetail>();
            this.LogCompanyFacilityMappings = new HashSet<LogCompanyFacilityMapping>();
            this.LogContracts = new HashSet<LogContract>();
            this.LogContracts1 = new HashSet<LogContract>();
            this.LogInsurances = new HashSet<LogInsurance>();
            this.LogLicenses = new HashSet<LogLicense>();
            this.LogLocationCompanyMappings = new HashSet<LogLocationCompanyMapping>();
            this.LogMiscellaneous = new HashSet<LogMiscellaneou>();
            this.LogPreBills = new HashSet<LogPreBill>();
            this.Miscellaneous = new HashSet<Miscellaneou>();
            this.PreBills = new HashSet<PreBill>();
        }
    
        public long CMP_Id { get; set; }
        public string CMP_NameLegal { get; set; }
        public string CMP_NameDBA { get; set; }
        public Nullable<long> CMP_VDT_Id { get; set; }
        public long CMP_COT_Id { get; set; }
        public string CMP_CompanyDocument { get; set; }
        public string CMP_IsActive { get; set; }
        public Nullable<long> CMP_UserId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bill> Bills { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Customer_Vehicle_Details> Tbl_Customer_Vehicle_Details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Customer_Payment_Details> Tbl_Customer_Payment_Details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Customer_Vehicle_Details> Tbl_Customer_Vehicle_Details1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Customer_Payment_Details> Tbl_Customer_Payment_Details1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Customer_Basic_Details> Tbl_Customer_Basic_Details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Log_Customer_Basic_Details> Tbl_Log_Customer_Basic_Details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Log_Customer_Vehicle_Details> Tbl_Log_Customer_Vehicle_Details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Log_Customer_Payment_Details> Tbl_Log_Customer_Payment_Details { get; set; }
        public virtual CompanyType CompanyType { get; set; }
        public virtual VendorType VendorType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyAccountDetail> CompanyAccountDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyAccountTransaction> CompanyAccountTransactions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyAccountTransaction> CompanyAccountTransactions1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyDetail> CompanyDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyFacilityMapping> CompanyFacilityMappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyQBK> CompanyQBKs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contract> Contracts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contract> Contracts1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Insurance> Insurances { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<License> Licenses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocationCompanyMapping> LocationCompanyMappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogBill> LogBills { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogCompany> LogCompanies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogCompanyAccountDetail> LogCompanyAccountDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogCompanyDetail> LogCompanyDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogCompanyFacilityMapping> LogCompanyFacilityMappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogContract> LogContracts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogContract> LogContracts1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogInsurance> LogInsurances { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogLicense> LogLicenses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogLocationCompanyMapping> LogLocationCompanyMappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogMiscellaneou> LogMiscellaneous { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogPreBill> LogPreBills { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Miscellaneou> Miscellaneous { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PreBill> PreBills { get; set; }
    }
}
