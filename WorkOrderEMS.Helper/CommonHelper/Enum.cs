
namespace WorkOrderEMS.Helper
{
    public enum Result
    {
        Completed,
        Failed,
        DuplicateRecord,
        DuplicateRecordEmail,
        DoesNotExist,
        FreePlan,
        ExistRecord,
        LoginSuccessfully,
        LoginFailed,
        UpdatedSuccessfully,
        EmailSendSuccessfully,
        EmailIdDoesNotExist,
        Delete
    }
    public class StringValue : System.Attribute
    {
        private string _value;

        public StringValue(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

    }
    public enum GlobalCodename
    {
        [StringValue("PROJECTCATEGORY")]
        ProjectCategory = 1,
        [StringValue("EMPLOYEEPROFILE")]
        EmployeeProfile = 2,
        [StringValue("INVENTORYITEM")]
        InventoryItem = 3,
        [StringValue("TASKTYPE")]
        TaskType = 4,
        [StringValue("WORKPRIORITY")]
        WorkPriority = 5,
        [StringValue("ASSETCLASS")]
        AssetClass = 6
    }
    public enum UserType
    {
        [StringValue("GlobalAdmin")]
        GlobalAdmin = 1,
        [StringValue("Manager")]
        Manager = 2,
        [StringValue("Employee")]
        Employee = 3,
        [StringValue("Client")]
        Client = 4,
        [StringValue("IT Administrator")]
        ITAdministrator = 5,
        [StringValue("Administrator")]
        Administrator = 6,
        [StringValue("VendorUser")]
        VendorUser = 137,
        //[StringValue("Anonymous")]
        //Anonymous = 7,
        [StringValue("GuestUser")]
        GuestUser = 138,
        [StringValue("HR")]
        HR = 493,
    }
    public enum WorkRequestStatus
    {
        [StringValue("Assigned")]
        Assigned = 1,
        [StringValue("Pending")]
        Pending = 2,
    }
    public enum WorkOrderStatus
    {
        [StringValue("Pending")]
        Pending = 1,
        [StringValue("In Progress")]
        InProgress = 2,
        [StringValue("Complete")]
        Complete = 3,
        [StringValue("On Hold")]
        OnHold = 4
    }

    public enum ApprovalStatus
    {

        Accepted = 244,
        Pending = 245,
        Rejected = 246,
    }

    public class AlertMessageClass
    {
        public string Danger
        {
            get
            { return "text-danger"; }
        }

        public string Info
        {
            get
            { return "text-info"; }
        }

        public string Success
        {
            get
            { return "text-success"; }
        }
    }
    public enum TrashLevelValue
    {
        Full = 157,
        TwentyFive = 160

    }
    public enum QrcType
    {
        Vehicle = 36,
        TrashCan = 37,
        Elevator = 38,
        GateArm = 39,
        TicketSpitter = 40,
        BusStation = 41,
        EmergencyPhoneSystems = 42,
        MovingWalkway = 43,
        Escalators = 44,
        Bathroom = 45,
        Equipment = 46,
        Devices = 47,
        ParkingFacility = 101,
        ShuttleBus = 375,
        GTQRCScan = 340

    }


    public enum VEHICLETYPE
    {
        MotorVehicle = 53,
        ShuttleBus = 54
    }
    public enum ClientVendorType
    {
        IndividualClient = 99,
        VendorClient = 100
    }


    public static class CustomUserRoles
    {
        public const string GlobalAdmin = "GlobalAdmin";
        public const string ITAdministrator = "ITAdministrator";
        public const string Administrator = "Administrator";
        public const string Manager = "Manager";
        public const string Employee = "Employee";
        public const string Client = "Client";
    }

    public enum TaskTypeCategory
    {
        [StringValue("USER CREATION")]
        UserCreation = 139,
        [StringValue("WORK ORDER REQUEST")]
        WorkOrderRequest = 141,
        [StringValue("WORK ORDER ASSIGNMENT")]
        WorkOrderAssignment = 142,
        [StringValue("WORK ORDER SUBMISSION")]
        WorkOrderSubmission = 143,
        [StringValue("QRC CREATION")]
        QRCCreation = 144,
        [StringValue("Create Location")]
        CreateLocation = 210,
        [StringValue("Update Loation")]
        UpdateLocation = 211,
        [StringValue("Delete Location")]
        DeleteLocation = 212,
        [StringValue("User Update")]
        UserUpdate = 213,
        [StringValue("User Delete")]
        UserDelete = 214,
        [StringValue("QRC Update")]
        QRCUpdate = 215,
        [StringValue("QRC Delete")]
        QRCDelete = 216,
        [StringValue("Work Order Update")]
        WorkOrderUpdate = 217,
        [StringValue("Work Order Delete")]
        WorkOrderDelete = 218,
        [StringValue("Create Rule")]
        CreateRule = 240,
        [StringValue("Update Rule")]
        UpdateRule = 241,
        [StringValue("Delete Rule")]
        DeleteRule = 242,
        [StringValue("Delete Email")]
        DeleteEmail = 243,
        [StringValue("Create Inventory")]
        CreateInventory = 249,
        [StringValue("Update Inventory")]
        UpdateInventory = 250,
        [StringValue("Delete Inventory")]
        DeleteInventory = 251,
        [StringValue("Assign location and permission")]
        AssignLocationPermission = 398,
        [StringValue("Code 26 - Shift End")]
        ShiftEnd = 280,
        [StringValue("Code 10 - Gate Repair")]
        GateRepair = 18,
        [StringValue("Code 11 - Customer Vehicle Locate")]
        CustomerVehicleLocate = 19,
        [StringValue("Code 12 - Customer Jump Start")]
        CustomerJumpStart = 20,
        [StringValue("Code 13 - Customer tire inflation")]
        Customertireinflation = 21,
        [StringValue("Code 14 - Customer Assistance")]
        CustomerAssistance = 22,
        [StringValue("Code 15 - Work Break")]
        WorkBreak = 23,
        [StringValue("Code 16 - Special Project")]
        SpecialProject = 24,
        [StringValue("Code 17 - Routine Checks")]
        RoutineChecks = 25,
        [StringValue("Code 18 - Space Count")]
        SpaceCount = 26,
        [StringValue("Code 19 - License Plate Inventory")]
        LicensePlateInventory = 27,
        [StringValue("Code 20 - Emergency")]
        Emergency = 28,
        [StringValue("Code 21 - Facility cleaning")]
        Facilitycleaning = 29,
        [StringValue("Code 22 - Facility Spill Response")]
        FacilitySpillResponse = 30,
        [StringValue("Code 23 - Snow Removal")]
        SnowRemoval = 31,
        [StringValue("Code 24 - Ticket Spitter Repair")]
        TicketSpitterRepair = 32,
        [StringValue("Code 25 - Miscellaneous Event")]
        MiscellaneousEvent = 33,

        //eFleet Fueling
        [StringValue("eFleet Fueling Submission")]
        EfleetFuelingSubmission = 462,

        //eFleet Incident for Service
        [StringValue("eFleet Incident Submission")]
        EfleetIncidentSubmission = 463,

        //Preventative Maitenance
        [StringValue("Preventative Maintenance Submission")]
        PreventativeMaintenanceSubmission = 446,
        [StringValue("Update Preventative Maintenance")]
        UpdatePreventativeMaintenance = 454,
        [StringValue("Delete eFleet Preventative Maintenance")]
        DeletePreventativeMaintenance = 458,

        //For eFleet Maintenance
        [StringValue("eFleet Maintenance Submission")]
        eFleetMaintenanceSubmission = 447,
        [StringValue("Update eFleet Maintenance")]
        UpdateeFleetMaintenance = 448,
        [StringValue("Delete eFleet Maintenance")]
        DeleteeFleetMaintenance = 459,

        //For eFleet Driver
        [StringValue("Fleet Driver Submission")]
        eFleetDriverSubmission = 449,
        [StringValue("Update Fleet Driver")]
        UpdateeFleetDriver = 450,
        [StringValue("Delete eFleet Driver")]
        DeleteeFleetDriver = 457,

        //For eFleet Vehicle Incident
        [StringValue("eFleet Vehicle Incident Submission")]
        eFleetVehicleIncidentSubmission = 451,
        [StringValue("Update Fleet Vehicle Incident")]
        UpdateeFleetIncidentVehicle = 452,
        [StringValue("Delete eFleet Vehicle Incident")]
        DeleteeFleetVehicleIncident = 460,

        //For eFleet Vehicle
        [StringValue("eFleet Vehicle Submission")]
        eFleetVehicleSubmission = 455,
        [StringValue("Update eFleet Vehicle")]
        UpdateeFleetVehicle = 456,
        [StringValue("Delete eFleet Vehicle")]
        DeleteeFleetVehicle = 461,

        //For Vendor
        [StringValue("Create Vendor")]
        CreateVendor = 479,
        [StringValue("Update Vendor")]
        UpdateVendor = 480,

        //For PO
        [StringValue("Create PO")]
        CreatePO = 481,
        [StringValue("Approve PO")]
        ApprovePO = 482,
        [StringValue("Reject PO")]
        RejectPO = 483,

        //for Cost Code allocation
        [StringValue("Allocate Costcode to location")]
        AllocateCostcode = 484,

        //For Miscellaneous
        [StringValue("Miscellaneous Created")]
        MiscellaneousCreated = 485,
        [StringValue("Miscellaneous Approved")]
        MiscellaneousApproved = 486,
        [StringValue("Miscellaneous Reject")]
        MiscellaneousReject = 487,

        //For Bill
        [StringValue("Bill Created")]
        BillCreated = 488,
        [StringValue("Bill Approved/Reject")]
        BillApprovedReject = 489,

        //For Payment
        [StringValue("Payment Approve/Cancel")]
        PaymentApporveCancel = 490,

        //For Seperatly Adding Insurance/License and Bank Account
        [StringValue("Add Insurance/License")]
        AddInsuranceLicese = 491,
        [StringValue("Add Bank Account")]
        AddBankAccount = 492,
        [StringValue("Add Facility")]
        AddFacility = 493,

        //[StringValue("Vehicle asfsf")]
        //Vehicle = 36,
        //TrashCan = 37,
        //Elevator = 38,
        //GateArm = 39,
        //TicketSpitter = 40,
        //BusStation = 41,
        //EmergencyPhoneSystems = 42,
        //MovingWalkway = 43,
        //Escalators = 44,
        //Bathroom = 45,
        //Equipment = 46,
        //Devices = 47,
        //ParkingFacility = 101,
        //ShuttleBus = 375,

    }

    public enum WorkRequestStatus1
    {
        Pending = 14,
        InProgress = 15,
        Complete = 16,
        OnHold = 17,
        Missed = 477
    }
    public enum WorkRequestPriority
    {
        Level1Urgent = 11,
        Level2Intermediate = 12,
        Level3Routine = 13
    }
    public enum WorkRequestProjectType
    {
        WorkRequest = 128,
        SpecialProject = 129,
        FacilityRequest = 256,
        ContinuousRequest = 279
    }

    public enum DeviceType
    {
        Windows = 133,
        IOS = 134,
        Andriod = 135,
        Others = 136
    }


    public enum ServiceResponse
    {
        SuccessResponse = 1,
        FailedResponse = 0,
        ExeptionResponse = -1,
        InvalidSessionResponse = -2,
        NoRecord = 2
    }
    public enum DARTASKTYPECATEGORY
    {
        GateRepair = 18,
        CustomerVehicleLocate = 19,
        CustomerJumpStart = 20,
        Customertireinflation = 21,
        CustomerAssistance = 22,
        WorkBreak = 23,
        SpecialProject = 24,
        RoutineChecks = 25,
        SpaceCount = 26,
        LicensePlateInventory = 27,
        Emergency = 28,
        Facilitycleaning = 29,
        FacilitySpillResponse = 30,
        SnowRemoval = 31,
        TicketSpitterRepair = 32,
        MiscellaneousEvent = 33,
        CustomerCall = 345,
        DARType = 205,
        QRCScan = 255,
        ContinuousRequestEnd = 281
    }
    public enum TimeZoneDetails
    {
        [StringValue("SQL")]
        SQL = 372,
        [StringValue("CSharp")]
        CSharp = 373,
        [StringValue("Both")]
        Both = 374
    }
    public enum PauseResume
    {
        [StringValue("Pause")]
        Pause = 329,
        [StringValue("Resume")]
        Resume = 330
    }
    public enum ContinuesPauseResume
    {
        [StringValue("Pause")]
        Pause = 322,
        [StringValue("Resume")]
        Resume = 323
    }
    public static class DashboardWidgetGlobalName
    {
        public const string DashboardSetting = "DashboardSetting";
        public const string DashboardSetting_M = "DashboardSettingManager";
        public const string DashboardSetting_C = "DashboardSettingClient";
        public const string DashboardSetting_E = "DashboardSettingEmployee";
    }

    public enum LocationServiceMaster
    {
        LocationSetup = 1,
        ManageUsers = 2,
        eScanner = 3,
        eMaintenance = 4,
        GTTracker = 5,
        InventoryTracker = 6,
        Rules = 7,
        Reports = 8,
        DailyActivityReports = 9
    }
    public enum DashboardWidget
    {
        QRCScanned = 386,
        eCashInfraction = 385,
        ActiveUsers = 384,
        UnAssignedWorkOrder = 383,
        DailyActivityReport = 382,
        WorkOrderUser = 381,
        ProjectProgress = 380,
        VendorsVehicleCount = 379,
        UserCount = 378,
        WorkOrderCount = 377,
        QRCCount = 376
    }
    public enum ReportName
    {
        WorkOrderIssued,
        DailyActivityCode,
        AssignedWorkOrder,
        CompletedWorkOrder,
        CustomerAssistance,
        JumpStarts,
        TireInflation,
        SpaceCount,
        QRCScan,
        TrashLevel,
        TrashPickedUp,
        QRCAmount,
        Damage,
        QRCOwnedBy,
        GtScanReport,
        CheckInPermitScannedReport,
        InfractionReport,
        CleaningReport,
        RoutineCheck,
        eFleetDamage,
        eFleetInspection,
        eFleetDefect,
        IdleEmployee
    }
    public static class PDFReportNameHeading
    {
        public const string WorkOrderIssued = "Work Order Issued Report";
        public const string AssignedWorkOrder = "Assigned Work Order Report";
        public const string CompletedWorkOrder = "Completed Work Order Report";
        public const string CustomerAssistance = "Customer Assistance Report";
        public const string JumpStarts = "Jump Starts Report";
        public const string TireInflation = "Tire Inflation Report";
        public const string SpaceCount = "Space Count Report";
        public const string QRCScan = "QRC Scan Report";
        public const string TrashLevel = "Trash Level Report";
        public const string TrashPickedUp = "Trash Picked Up Report";
        public const string QRCAmount = "QRC Amounts Report";
        public const string Damage = "Damage Report";
        public const string QRCOwnedBy = "QRC Owned By Report";
        public const string GtScan = "GT Scan Report";
        public const string CheckInPermit = "CheckIn Permit Scanned Report";
        public const string Infraction = "Infraction Report";
        public const string CleaningReport = "Cleaning Report";
        public const string RoutineCheck = "Routine Check Report";
        public const string Defect = "Defect Report";
        public const string IdleEmployee = "Idle Employee Report";
    }
    public enum DarTaskTypeCategory
    {
        GateRepair = 18,
        CustomerVehicleLocate = 19,
        CustomerJumpStart = 20,
        Customertireinflation = 21,
        CustomerAssistance = 22,
        WorkBreak = 23,
        SpecialProject = 24,
        RoutineChecks = 25,
        SpaceCount = 26,
        LicensePlateInventory = 27,
        Emergency = 28,
        Facilitycleaning = 29,
        FacilitySpillResponse = 30,
        SnowRemoval = 31,
        TicketSpitterRepair = 32,
        MiscellaneousEvent = 33,
        ShiftEnd = 280,
        ContinuousRequestEnd = 281
    }
    public static class DarTaskTypeCategoryName
    {
        public const string GateRepair = "GateRepair";
        public const string CustomerVehicleLocate = "CustomerVehicleLocate";
        public const string CustomerJumpStart = "CustomerJumpStart";
        public const string Customertireinflation = "Customertireinflation";
        public const string CustomerAssistance = "CustomerAssistance";
        public const string WorkBreak = "WorkBreak";
        public const string SpecialProject = "SpecialProject";
        public const string RoutineChecks = "RoutineChecks";
        public const string SpaceCount = "SpaceCount";
        public const string LicensePlateInventory = "LicensePlateInventory";
        public const string Emergency = "Emergency";
        public const string Facilitycleaning = "Facilitycleaning";
        public const string FacilitySpillResponse = "FacilitySpillResponse";
        public const string SnowRemoval = "SnowRemoval";
        public const string TicketSpitterRepair = "TicketSpitterRepair";
        public const string MiscellaneousEvent = "MiscellaneousEvent";
        public const string ShiftEnd = "ShiftEnd";
        public const string ContinuousRequestEnd = "ContinuousRequestEnd";
    }
    public static class eFleetCheckInOut
    {
        public const string PreTrip = "PreTrip";
        public const string PostTrip = "PostTrip";
    }

    public enum eFleetEnum
    {
        Fueling = 442,//10439,
        VehicleScan = 443,//10440,
        Miles = 429,  //10426,
        Hours = 430,   //10427,
        Annual = 431,  //10428,
        [StringValue("Diesel Fuel")]
        DieselFuel = 441,
        [StringValue("Gasoline")]
        Gasoline = 440,
        [StringValue("Functional")]
        Functional = 419,
        [StringValue("Not Functional")]
        NotFunctional = 420,
        [StringValue("Good")]
        Good = 412,
        [StringValue("Fair")]
        Fair = 413,
        [StringValue("Poor")]
        Poor = 414,
        [StringValue("Required")]
        Required = 403,
        [StringValue("Full")]
        Full = 415,
        [StringValue("3/4")]
        ThreeByFour = 416,
        [StringValue("1/2")]
        OneByTwo = 417,
        [StringValue("1/4")]
        OneByFour = 418,
        [StringValue("Regular")]
        Regular = 464,
        [StringValue("Event")]
        Event = 465,
        [StringValue("N/A")]
        NA = 476,
        [StringValue("DamageTireDetails")]
        DamageTireDetails = 422,
        [StringValue("InteriorMileageDriverDetails")]
        InteriorMileageDriverDetails = 423,
        [StringValue("EngineExteriorDetails")]
        EngineExteriorDetails = 424,
        [StringValue("EmergencyAccessoriesDetails")]
        EmergencyAccessoriesDetails = 425
    }

    public static class UserJobTitle
    {
        public const string Client = "475";

    }
    public enum eCounting
    {
        COT_ID = 1
    }
    public enum GlobalAdmin
    {
        [StringValue("DaneGrey")]
        UserId = 3,
    }

    public enum PDFForm
    {
        [StringValue("W9Form")]
        W9Form = 1,
        [StringValue("BudgetTransferRequestForm")]
        BudgetTransferRequestForm = 2,
        [StringValue("CharitableContributionRequestForm")]
        CharitableContributionRequestForm = 3,
        //[StringValue("INVENTORYITEM")]
        //InventoryItem = 3,
        //[StringValue("TASKTYPE")]
        //TaskType = 4,
        //[StringValue("WORKPRIORITY")]
        //WorkPriority = 5,
        //[StringValue("ASSETCLASS")]
        //AssetClass = 6
    }
    public static class POStatus
    {
        public const string W = "Waiting";
        public const string Y = "Approved";
        public const string N = "Rejected";
    }
    public enum VendorTypeValue
    {
        [StringValue("InsuranceVander")]
        VendorType = 3,
    }
    public enum POType
    {
        [StringValue("NormalPO")]
        NormalPO = 1,
    }
    public enum FileType
    {
        [StringValue("FileTypeSSNLicense")]
        FileType = 2,
    }

    public enum POReccuringStaus
    {
        Pending,
        Completed,
    }
    public static class EmployeeStatusChnage
    {
        public const string L = "LocationTransfer";
        public const string S = "EmployeeStatus";
        public const string D = "Demotion";
    }
    public static class ApplicantStatus
    {
        public const string BackgroundCheck = "F";       
        public const string Screened = "S";
        public const string Reject = "R";
        public const string Applied = "A";
        public const string Shortlisted = "D";
        public const string Assessment = "E";
        public const string Onboarding = "I";
        public const string Offer = "O";
        public const string Hired = "H";
        public const string InterviewScheduled = "Hired";
    }
    public static class ApplicantIsActiveStatus
    {
        public const string Sent = "S";
        public const string Pass = "Y";
        public const string Fail = "N";
        public const string OfferAccepted = "A";
        public const string OfferRejected = "D";
        public const string OfferCounter = "C";
        public const string Onboarding = "I";
    }
    public static class VendorName
    {
        public const string BackgroundCheck = "Accredited Employment Screenin";
    }
    /// <summary>
    /// Created By : Ashwajit Bansod
    /// </summary>
    public enum InterviewQuestionsId
    {
        MasterId_1 = 1,
        QuestionId_2 = 2,
        QuestionId_3 = 3,
        MasterId_2 = 2,
        QuestionId_4 = 4,
        QuestionId_6 = 6,
        MasterId_3 = 3,
        QuestionId_7 = 7,
        QuestionId_8 = 8,
        QuestionId_10 = 10,
        QuestionId_11 = 11,
        MasterId_4 = 4,
        QuestionId_12 = 12,
        QuestionId_13 = 13,
        QuestionId_15 = 15,
        QuestionId_16 = 16,
        MasterId_5 = 5,
        QuestionId_22 = 22,
        QuestionId_23 = 23,
        MasterId_6 = 6,
        QuestionId_26 = 26,
        MasterId = 11,
        QuestionId_45 = 45,
        QuestionId_46 = 46,
        QuestionId_47 = 47,
        QuestionId_48 = 48,

    }
    public static class SelfIdentification
    {
        public const string HL = "Hispanic or Latino";
        public const string S = "EmployeeStatus";
        public const string D = "Demotion";
    }
    public enum FileTypeId
    {
        I9 = 17,
        W4 = 19
    }
    public static class ActiveStatus
    {
        public const string N = "N";
        public const string Y = "Y";
    }
    public static class ModuleSubModule
    {
        //Epeople
        public const string ePeople = "ePeople";
        public const string OnBoarded = "OnBoarded";
        public const string OnBoarding = "OnBoarding";
        public const string Interviewer = "Interviewer";
        public const string InterviewerAcceptDeny = "InterviewAcceptDeny";
        public const string AssessmentStatus = "AssessmentStatus";
        public const string OnBoardingComplete = "OnBoardingComplete";
        public const string AssessmentStart = "AssessmentStart";

        //Performance
        public const string Performance = "Performance";
        public const string EvaluationStart = "EvaluationStart";
        public const string EvaluationComplete = "EvaluationComplete";
    }
    public static class Priority
    {
        public const string High = "H";
        public const string Medium = "M";
        public const string Low = "L";
    }
    public static class APIName
    {
        public const string I9AuthenticationAPI= "I9Authentication";
        public const string I9AuthenticationLink = "/authentication/login";
        public const string I9CaseAPI = "I9CaseAPI";
        public const string I9CaseAPILink = "https://stage-everify.uscis.gov/api/v30/cases";
        public const string I9refreshTokenLink = "/authentication/refresh";
        public const string I9CaseSubmitLink = "https://stage-everify.uscis.gov/api/v30/cases/";
        public const string I9refreshToken = "I9TokenRefresh";
        public const string I9PostDataCase = "I9PostDataCase";
        public const string I9CaseSubmit = "I9CaseSubmit";
        public const string FloridaBlueAuthentication = "FloridaBlueAuthentication";
        public const string FloridaBlueAuthenticationLink = "https://api.availity.com/availity/v1/token";
        public const string FloridaBlueGetLink = "https://api.availity.com/availity/v1/configurations?type=270";
        public const string BackGroudScreeningPostLink = "https://uat-api.applicantinsight.net/v1/api/Screenings";
    }
}
