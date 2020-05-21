﻿using System.Globalization;

namespace WorkOrderEMS.Helper
{
    /// <summary>
    /// Created by: Roshan Rahood
    /// Created on: 09-Jan-2015
    /// Purpose: This class will be used to create the common message for the DAR.
    /// </summary>
    public static class DarMessage
    {
        public static string SaveSuccessLocationDar(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "New Location : {0} has been created sucessfully.", locationName);
        }

        public static string UpdateSuccessLocationDar(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Location : {0} has been updated.", locationName);
        }

        public static string DeleteSuccessLocationDar(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Location : {0} has been deleted.", locationName);
        }

        public static string AdministratorDeleteDar(string locationName)
        {
            return "Administrator for the location :" + locationName + " has been deleted.";
        }

        public static string NewManagerCreatedDar(string locationName)
        {
            return "New manager for the location :" + locationName + " has been created.";
        }

        public static string ClientUpdatedDar(string locationName)
        {
            return "Client for the location :" + locationName + " has been updated.";
        }

        public static string NewClientCreatedDar(string locationName)
        {
            return "New Client for the location :" + locationName + " has been created.";
        }

        public static string ManagerUpdatedDar(string locationName)
        {
            return "Manager for the location :" + locationName + " has been updated.";
        }

        public static string NewEmployeeCreatedDar(string locationName)
        {
            return "New employee for the location :" + locationName + " has been Created.";
        }

        public static string EmployeeUpdatedDar(string locationName)
        {
            if (string.IsNullOrEmpty(locationName))
            { return "Employee without location has been updated."; }
            else
            {
                return "Employee for the location :" + locationName + " has been updated.";
            }
        }

        public static string NewAdministratorCreatedDar(string locationName)
        {
            return "Administrator for the location :" + locationName + " has been Created.";
        }

        public static string AdministratorUpdatedDar(string locationName)
        {
            return "Administrator for the location :" + locationName + " has been updated.";
        }

        public static string NewITAdministratorCreatedDar(string locationName)
        {
            return "IT Administrator for the location :" + locationName + " has been Created.";
        }

        public static string ITAdministratorUpdatedDar(string locationName)
        {
            return "IT Administrator for the location :" + locationName + " has been updated.";
        }

        public static string ITAdministratorDeletedDar(string locationName)
        {
            return "IT Administrator for the location :" + locationName + " has been deleted.";
        }

        public static string UserDeleteDar(string userName)
        {
            return string.Format(CultureInfo.InvariantCulture, "User : {0}  has been deleted.", userName);
        }

        public static string UserVerifiedDar(string userName)
        {
            return string.Format(CultureInfo.InvariantCulture, "User : {0} has been manually verified.", userName);
        }
        public static string UpdateVehicle(string QRCCodeID,string locationName)
        {          
            return "Vehicle: " + QRCCodeID + " for the location :" + locationName + " has been updated.";
        }       
        public static string DarUpdateTaskStatus(string userName, string clientUserName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Employee :{0} has accepted the work order requested by client:{1}.", userName, clientUserName);
        }
        public static string DarUpdateStartTimeTaskStatus(string userName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "DAR has been updated for Work Order Assignment - Start  at location {1}, for {0}.", userName, locationName);
        }
        public static string DarUpdateEndTimeTaskStatus(string userName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "DAR has been updated for Work Order Assignment - End  at location {1}, for {0}.", userName, locationName);
        }
        public static string QrcVehicleCleaning(string userName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Employee :{0} has clean the vehicle", userName);
        }
        public static string UserSuccessfullyCreated(string UserName, string CreatedBy, string UserType)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1} is created by {2}.", UserType, UserName, CreatedBy);
        }
        public static string DeleteLocationMapping(string username, string mappingWith, string userType, string operationBy)
        {
            return string.Format(CultureInfo.InvariantCulture, "{1} {0} mapping with {2} location has been deleted by {3} Admin User.", userType, username, mappingWith, operationBy);
        }
        public static string DarJumpStartEndTimeStatus(string userName, string locationName, string desc)
        {
            return string.Format(CultureInfo.InvariantCulture, "DAR has been updated at location {1} for {2}.", userName, locationName, desc);
        }
        public static string FacilityRequestAccept(string userName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "DAR has been updated for Facility Request accept at location {1}, for {0}", userName, locationName);
        }
        public static string UrgentWOAcceptbyEmp(string userName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "DAR has been updated for Urgent Work Request accept at location {1}, for {0}", userName, locationName);
        }
        public static string DarCustomerCall(string userName, string locationName, string desc)
        {
            return string.Format(CultureInfo.InvariantCulture, "Employee :{0} has called to {1} at location :{2}", userName, desc, locationName);
        }
        public static string SaveQRC(string qrcName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "New QRC : {0} for the location :{1} has been created.", qrcName, locationName);
        }
        public static string UpdateQRC(string qrcName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "QRC : {0} for the location :{1} has been updated.", qrcName, locationName);
        }
        public static string DeleteQRC(string qrcName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "QRC : {0} for the location :{1} has been deleted.", qrcName, locationName);
        }
        public static string CreateWorkRequest(string workOrderCode, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Work Order : {0} for the location :{1} has been created.", workOrderCode, locationName);
        }
        public static string UpdateWorkRequest(string workOrderCode, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Work Order : {0} for the location :{1} has been updated.", workOrderCode, locationName);
        }
        public static string DeleteWorkRequest(string workOrderCode, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Work Order : {0} for the location :{1} has been deleted.", workOrderCode, locationName);
        }
        public static string QRCScanMessage(string userName, string qrcName, string qrcCode)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} has scanned the QR code {1}({2}) ", userName, qrcName, qrcCode);
        }
        public static string VehicleScanMessage(string userName, string vehicleName, string vehicleCode)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} has scanned the Vehicle QR code {1}({2}) ", userName, vehicleName, vehicleCode);
        }
        public static string ShiftEnd(string userName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Employee :{0} has end the shift for the day", userName);
        }
        public static string CRTaskStart(string userName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "DAR has been updated for Continuous Request - Start  at location {1}, for {0}.", userName, locationName);
        }
        public static string CRTaskEnd(string userName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "DAR has been updated for Continuous Request - End  at location {1}, for {0}.", userName, locationName);
        }
        public static string CheckIn(string qrcName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "QRC : {0} for the location :{1} has been check in through web.", qrcName, locationName);
        }
        public static string LocationAssignedAdmin(string userName, string location)
        {
            return string.Format(CultureInfo.InvariantCulture, "Administrator: {0} for the location:{1} has been assigned.", userName, location);
        }
        public static string LocationAssignedEmployee(string userName, string location)
        {
            return string.Format(CultureInfo.InvariantCulture, "Employee: {0} for the location:{1} has been assigned.", userName, location);
        }
        public static string LocationAssignedManager(string userName, string location)
        {
            return string.Format(CultureInfo.InvariantCulture, "Manager: {0} for the location:{1} has been assigned.", userName, location);

        }
        public static string LocationAssigned(string userName, string location)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} for the location:{1} has been assigned.", userName, location);

        }
        public static string NeweFleetPMCreated(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "New eFleet Preventative Maintenance for the location :{0} has been created.", locationName);
        }
        public static string NeweFleetPMUpdated(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Preventative Maintenance for the location :{0} has been Updated.", locationName);
        }
        public static string DeleteFleetPM(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Prevantative Maintenance for the location :{0} has been deleted.", locationName);
        }
        public static string RegisterNeweFleetMaintenance(string locationName)
        {
            return "New Vehicle Maintenance for the location :" + locationName + " has been registered.";
        }
        public static string UpdateeFleetMaintenance(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Maintenance for the location :{0} has been Updated.", locationName);
        }
        public static string DeleteFleetMaintenance(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Maintenance for the location :{0} has been deleted.", locationName);
        }
        public static string RegisterNeweFleetDriver(string locationName)
        {
            return "New eFleet driver for the location :" + locationName + " has been registered.";
        }
        public static string UpdateeFleetDriver(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Driver for the location :{0} has been Updated.", locationName);
        }
        public static string DeleteFleetDriver(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Driver for the location :{0} has been deleted.", locationName);
        }
        public static string RegisterNeweFleetIncidentVehicle(string locationName)
        {
            return "New eFleet Vehicle Incident for the location :" + locationName + " has been registered.";
        }
        public static string UpdateeFleetVehicleIncident(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Vehicle Incident for the location :{0} has been Updated.", locationName);
        }
        public static string DeleteFleetVehicleIncident(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Vehicle Incident for the location :{0} has been deleted.", locationName);
        }
        public static string RegisterNeweFleetVehicle(string locationName)
        {
            return "New eFleet Vehicle for the location :" + locationName + " has been registered.";
        }
        public static string UpdateeFleetVehicle(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Vehicle for the location :{0} has been Updated.", locationName);
        }
        public static string DeleteFleetVehicle(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Vehicle for the location :{0} has been deleted.", locationName);
        }
        public static string RegisterNeweFleetFueling(string locationName)
        {
            return "New eFleet Fueling for the location :" + locationName + " has been registered.";
        }

        public static string CreateVendor(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Vendor is created for the location :{0}.", locationName);
        }
        public static string UpdateVendor()
        {
            return string.Format(CultureInfo.InvariantCulture, "Vendor is Updated");
        }
        public static string CreatePO(string locationName,long PONumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "PO :{1} is created for the location :{0}.", locationName, PONumber);
        }
        public static string ApproveRejectPO(string locationName, long PONumber,string ApptoveRemoveSatus)
        {
            return string.Format(CultureInfo.InvariantCulture, "PO :{1} is {2} for the location :{0}.", locationName, PONumber, ApptoveRemoveSatus);
        }
        public static string AllocateCostCodeForLocation(string locationName, string CostCode)
        {
            return string.Format(CultureInfo.InvariantCulture, "Cost Code :{1} is allocated for the location :{0}.", locationName, CostCode);
        }
        public static string MiscellaneousCreated(string locationName, string MISID)
        {
            return string.Format(CultureInfo.InvariantCulture, "Miscellaneous :{1} is Created for the location :{0}.", locationName, MISID);
        }
        public static string MiscellaneousApproveReject(string locationName, string MISID, string Status)
        {
            return string.Format(CultureInfo.InvariantCulture, "Miscellaneous :{1} is {2} for the location :{0}.", locationName, MISID, Status);
        }
        public static string BillCreated(string UserName,string locationName, long POID)
        {
            return string.Format(CultureInfo.InvariantCulture, "Bill :{1} is Created for the location :{0} by {2}.", locationName, POID, UserName);
        }
        public static string BillApprovedReject(string UserName, string locationName, string Status)
        {
            return string.Format(CultureInfo.InvariantCulture, "Bill is {2} for the location :{0} by {1}.", locationName, Status, UserName);
        }
        public static string PaymentPaidCancel(string UserName, string locationName, string Status, long? BillNo)
        {
            return string.Format(CultureInfo.InvariantCulture, "Bill {0} is {2} for the location :{0} by {1}.", BillNo, locationName, Status, UserName);
        }
        public static string VendorApprovedCancel(string UserName, string locationName, string Status)
        {
            return string.Format(CultureInfo.InvariantCulture, "Vendor {2} is {1} for the location :{0}",  locationName, Status, UserName);
        }
        public static string AddInsuranceAndLicense(string VendorName, string UserName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Insurance/Lisense is added for vendor {0} by user {1}", VendorName, UserName);
        }
        public static string AddBankAccountForVendor(string VendorName, string UserName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Bank account details is added for vendor {0} by user {1}", VendorName, UserName);
        }
        public static string DarUpdatePauseResumeTaskStatus(string userName, string locationName, string Status)
        {
            return string.Format(CultureInfo.InvariantCulture, "DAR has been updated for Work Order Assignment - {2}  at location {1}, for {0}.", userName, locationName, Status);
        }
        public static string AddCompanyFacility(string VendorName, string UserName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Facility is added for vendor {0} by user {1}", VendorName, UserName);
        }
        public static string AddAssetsForHiredApplicant(string ApplicantName, string HiringManagerName, string LocationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} has been hired by {1}  for location {2} please add assets for him.", ApplicantName, HiringManagerName, LocationName);
        }
        public static string OfferCouterByAppicant(string ApplicantName, string JobTitle, string LocationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} has been counter offer for postition {1} . please check comment and send onother offer if needed.", ApplicantName, JobTitle);
        }
        public static string OfferRejectByAppicant(string ApplicantName, string JobTitle, string LocationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} has been reject offer for position {1}  for location {2} .", ApplicantName, JobTitle, LocationName);
        }
        public static string SelectedAsInterviewer( string JobTitle)
        {
            return string.Format(CultureInfo.InvariantCulture, "You got selected as a interview for {0} .", JobTitle);
        }
        public static string InterviewAcceptByApplicant(string JobTitle,string Status)
        {
            return string.Format(CultureInfo.InvariantCulture, "Applicant Has selected {1} for interview for{0} .", JobTitle, Status);
        }
        public static string InterviewDenyByApplicant(string JobTitle)
        {
            return string.Format(CultureInfo.InvariantCulture, "Candidate Has been rejected for this {0} .", JobTitle);
        }
        public static string AssessmentReject(string JobTitle)
        {
            return string.Format(CultureInfo.InvariantCulture, "Candidate Has been rejected for this {0} .", JobTitle);
        }
        public static string AssessmentClear(string JobTitle)
        {
            return string.Format(CultureInfo.InvariantCulture, "Candidate Has been selected for this {0} .", JobTitle);
        }
        public static string OnboardingComplete(string ApplicantName)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} has been completed onboarding process please schedule the orientation.", ApplicantName);
        }
        public static string EvaluationStartManager(string EmployeeName, string Assessment)
        {
            return string.Format(CultureInfo.InvariantCulture, " The employee {0} has been completed their self assessment {1}, please complete evaluation for this employee.", EmployeeName, Assessment);
        }
        public static string EvaluationCompleteByManager(string EmployeeName, string ManagerName, string Assessment)
        {
            return string.Format(CultureInfo.InvariantCulture, " The Manager {0} has been completed the evaluation {1} for employee {2}.", EmployeeName, Assessment, ManagerName);
        }
    }
}
