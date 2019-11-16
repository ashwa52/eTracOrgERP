
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Exception_B;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.BusinessLogic.Interfaces.eCounting;
using WorkOrderEMS.BusinessLogic.Interfaces.eFleet;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.BusinessLogic.Managers.eFleet;
using WorkOrderEMS.Controllers.QuickBookData;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Infrastructure;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;
using WorkOrderEMS.Models.eMaintenance_M;
using WorkOrderEMS.Models.ServiceModel;
using WorkOrderEMS.Models.ServiceModel.ApiModel;

namespace WorkOrderEMS.Controllers.Services
{
    public class ServiceApiController : ApiController
    {
        private readonly IeFleetFuelingManager _IFuelingManager;
        private readonly IEfleetPM _IEfleetPM;
        private readonly IEfleetVehicle _IEfleetVehicle;
        private readonly IDARManager _IDARManager;
        private readonly IEfleetVehicleIncidentReport _IEfleetVehicleIncidentReport;
        private readonly IEfleetMaintenance _IEfleetMaintenance;
        private readonly IPassengerTracking _IPassengerTracking;
        private readonly IHoursOfServices _IHoursOfServices;
        private readonly IBillDataManager _IBillDataManager;
        private readonly IMiscellaneousManager _IMiscellaneousManager;
        private readonly ICommonMethod _ICommonMethod;
        private readonly IFillableFormManager _IFillableFormManager;
        private readonly INotification _INotification;

        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string EmeregencyImagePath = ConfigurationManager.AppSettings["POEmeregencyImage"];
        private string MiscellaneousImagePath = ConfigurationManager.AppSettings["MiscellaneousImage"];
        //private readonly IEfleetPM _IEfleetPM;
        public ServiceApiController()
        {
        }
        public ServiceApiController(IEfleetPM _IEfleetPM, IeFleetFuelingManager _IFuelingManager, IEfleetVehicle _IEfleetVehicle, IDARManager _IDARManager, IEfleetVehicleIncidentReport _IEfleetVehicleIncidentReport, IEfleetMaintenance _IEfleetMaintenance, IPassengerTracking _IPassengerTracking, IHoursOfServices _IHoursOfServices, IBillDataManager _IBillDataManager, IMiscellaneousManager _IMiscellaneousManager, ICommonMethod _ICommonMethod, IFillableFormManager _IFillableFormManager, INotification _INotification)
        {
            this._IFuelingManager = _IFuelingManager;
            this._IEfleetPM = _IEfleetPM;
            this._IEfleetVehicle = _IEfleetVehicle;
            this._IDARManager = _IDARManager;
            this._IEfleetVehicleIncidentReport = _IEfleetVehicleIncidentReport;
            this._IEfleetMaintenance = _IEfleetMaintenance;
            this._IPassengerTracking = _IPassengerTracking;
            this._IHoursOfServices = _IHoursOfServices;
            this._IBillDataManager = _IBillDataManager;
            this._IMiscellaneousManager = _IMiscellaneousManager;
            this._ICommonMethod = _ICommonMethod;
            this._IFillableFormManager = _IFillableFormManager;
            this._INotification = _INotification;
        }
        // GET: api/ServiceApi
        public IHttpActionResult Get()
        {
            return Ok("Hello");
        }

        /// <summary>Get eFleetVehicleID Details 
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>GeteFleetVehicleIDdetails</CreatedFor>
        /// <CreatedOn>Sept-06-2017</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceEfleetModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GeteFleetVehicleIDdetails(VehicleScanModel objServiceVehicleModel)
        {
            // var ObjeFleetCommonManager = new eFleetCommonManager();
            ServiceResponseModel<VehicleScanModel> objServiceResponseModel = new ServiceResponseModel<VehicleScanModel>();
            VehicleScanModel ObjVehicleScanModel = new VehicleScanModel();
            try
            {
                if (objServiceVehicleModel.ServiceAuthKey != null && objServiceVehicleModel.LocationID > 0 && objServiceVehicleModel.QRCodeID != null)
                {

                    objServiceResponseModel = _IFuelingManager.GeteFleetVehicleDetailsByID(objServiceVehicleModel);
                }
                else
                {
                    objServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    objServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception ex)
            {
                objServiceResponseModel.Message = ex.Message;
                objServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                objServiceResponseModel.Data = null;
            }

            return Ok(objServiceResponseModel);
        }

        [HttpPost]
        public IHttpActionResult SaveeFleetDamageTireInspection(eFleetDamageTireModel objeFleetDamageTireModel)
        {
            // var ObjVehicleManager = new VehicleManager();
            var ObjServiceResponseModel = new ServiceResponseModel<UpdateEfleetInspectionTypeXML>();
            try
            {
                if (objeFleetDamageTireModel != null && objeFleetDamageTireModel.ServiceAuthKey != null && objeFleetDamageTireModel.UserId > 0)
                {
                    var ObjRespnse = _IEfleetVehicle.SaveeFleetDamageTireInspectionDetails(objeFleetDamageTireModel);
                    ObjRespnse.Data.DamageTireDetails = null;
                    ObjRespnse.Data.InteriorMileageDriverDetails = null;
                    ObjRespnse.Data.EngineExteriorDetails = null;
                    ObjRespnse.Data.EmergencyAccessoriesDetails = null;
                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult SaveeFleetInteriorMileageInspection(eFleetInteriorMileageDriverModel objeFleetInteriorMileageModel)
        {
            //var ObjVehicleManager = new VehicleManager();
            var ObjServiceResponseModel = new ServiceResponseModel<UpdateEfleetInspectionTypeXML>();
            try
            {
                if (objeFleetInteriorMileageModel != null && objeFleetInteriorMileageModel.ServiceAuthKey != null && objeFleetInteriorMileageModel.UserId > 0)
                {
                    var ObjRespnse = _IEfleetVehicle.SaveeFleetInteriorMileageInspectionDetails(objeFleetInteriorMileageModel);
                    ObjRespnse.Data.DamageTireDetails = null;
                    ObjRespnse.Data.InteriorMileageDriverDetails = null;
                    ObjRespnse.Data.EngineExteriorDetails = null;
                    ObjRespnse.Data.EmergencyAccessoriesDetails = null;
                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult SaveeFleetEngineExteriorInspection(eFleetEngineExteriorModel objeFleetEngineExteriorModel)
        {
            // var ObjVehicleManager = new VehicleManager();
            var ObjServiceResponseModel = new ServiceResponseModel<UpdateEfleetInspectionTypeXML>();
            try
            {
                if (objeFleetEngineExteriorModel != null && objeFleetEngineExteriorModel.ServiceAuthKey != null && objeFleetEngineExteriorModel.UserId > 0)
                {
                    var ObjRespnse = _IEfleetVehicle.SaveeFleetEngineExteriorInspectionDetails(objeFleetEngineExteriorModel);
                    if (ObjRespnse.Data != null)
                    {
                        ObjRespnse.Data.DamageTireDetails = null;
                        ObjRespnse.Data.InteriorMileageDriverDetails = null;
                        ObjRespnse.Data.EngineExteriorDetails = null;
                        ObjRespnse.Data.EmergencyAccessoriesDetails = null;
                    }

                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult SaveeFleetEmergencyAccessoriesInspection(eFleetEmergencyAccessoriesModel objeFleetEmergencyAccessoriesModel)
        {
            //var ObjVehicleManager = new VehicleManager();
            var ObjServiceResponseModel = new ServiceResponseModel<UpdateEfleetInspectionTypeXML>();
            try
            {
                if (objeFleetEmergencyAccessoriesModel != null && objeFleetEmergencyAccessoriesModel.ServiceAuthKey != null && objeFleetEmergencyAccessoriesModel.UserId > 0)
                {
                    var ObjRespnse = _IEfleetVehicle.SaveeFleetEmergencyAccessoriesInspectionDetails(objeFleetEmergencyAccessoriesModel);
                    ObjRespnse.Data.DamageTireDetails = null;
                    ObjRespnse.Data.InteriorMileageDriverDetails = null;
                    ObjRespnse.Data.EngineExteriorDetails = null;
                    ObjRespnse.Data.EmergencyAccessoriesDetails = null;
                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult ChangeInspectionCheckOutInStatus(ChangeInspectionStatusModel objChangeInspectionStatusModel)
        {
            // var ObjVehicleManager = new VehicleManager();
            var ObjServiceResponseModel = new ServiceResponseModel<UpdateEfleetInspectionTypeXML>();
            try
            {
                if (objChangeInspectionStatusModel != null && objChangeInspectionStatusModel.ServiceAuthKey != null && objChangeInspectionStatusModel.UserId > 0)
                {
                    var ObjRespnse = _IEfleetVehicle.ChangingStatusOfInsection(objChangeInspectionStatusModel);
                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult SaveeFleetPreventativeMaintenance(eFleetPreventaticeMaintenanceModel objeFleetPreventaticeMaintenanceModel)
        {
            // var ObjPreventativeMaintenaceManager = new PreventativeMaintenaceManager();
            var ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (objeFleetPreventaticeMaintenanceModel != null && objeFleetPreventaticeMaintenanceModel.ServiceAuthKey != null && objeFleetPreventaticeMaintenanceModel.UserId > 0)
                {
                    var ObjRespnse = _IEfleetPM.InsertPreventativeMaintenance(objeFleetPreventaticeMaintenanceModel);
                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        //[TokenAuthorizeFilter]
        [HttpPost]
        public IHttpActionResult GetMeterMilesHoursValueList(MeterModel objMeterModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<eFleetMeterModel>>();
            try
            {
                if (objMeterModel != null && objMeterModel.ServiceAuthKey != null && objMeterModel.UserId > 0)
                {
                    // var ObjPreventativeMaintenaceManager = new PreventativeMaintenaceManager();
                    var meterList = _IEfleetPM.GetAllMilesValue();
                    if (meterList.Count > 0)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = meterList;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;

                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult SaveeFleetFueling(eFleetFuelingModelForService objeFleetFuelingModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                // var objeFleetCommonManager = new eFleetCommonManager();
                if (objeFleetFuelingModel != null && objeFleetFuelingModel.ServiceAuthKey != null && objeFleetFuelingModel.UserId > 0)
                {
                    var ObjRespnse = _IFuelingManager.InserteFleetFueling(objeFleetFuelingModel);
                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.InnerException.ToString();
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;

                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult StartFuelingTimer(ServiceDARModel objeFleetDARModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<long>();
            try
            {
                //var objDarManager = new DARManager();
                if (objeFleetDARModel != null && objeFleetDARModel.ServiceAuthKey != null && objeFleetDARModel.UserId > 0)
                {
                    long DARId = _IDARManager.SaveeFleetDAR(objeFleetDARModel);
                    if (DARId > 0)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.InvariantCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = DARId;
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                //ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult SaveeFleetIncident(eFleetIncidentModel objeFleetIncidentModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                // var objVehicleIncidentManager = new VehicleIncidentManager();
                if (objeFleetIncidentModel != null && objeFleetIncidentModel.ServiceAuthKey != null && objeFleetIncidentModel.UserId > 0)
                {

                    var ObjRespnse = _IEfleetVehicleIncidentReport.InsertVehicleIncident(objeFleetIncidentModel);
                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;

                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetAllVehicleList(ServiceBaseModel obj)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<VehicleDetailsModel>>();
            try
            {
                if (obj != null && obj.ServiceAuthKey != null && obj.UserId > 0 && obj.LocationID > 0)
                {
                    //  var ObjVehicleManager = new VehicleManager();
                    var vehicleList = _IEfleetVehicle.GetAllVehicleListDetails(obj);
                    if (vehicleList.Count > 0)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = vehicleList;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;

                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult SaveeFleetMaintenance(eFleetMaintenanceModelForApiService objeeFleetMaintenanceModel)
        {
            //var ObjMaintenanceManager = new MaintenanceManager();
            var ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (objeeFleetMaintenanceModel != null && objeeFleetMaintenanceModel.ServiceAuthKey != null && objeeFleetMaintenanceModel.UserId > 0)
                {
                    var ObjRespnse = _IEfleetMaintenance.InsertMaintenance(objeeFleetMaintenanceModel);
                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetAllPendingPreventativeMaintenanceList(eFleetIncidentModelPMPending obj)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<PendingPM>>();
            try
            {
                if (obj != null && obj.ServiceAuthKey != null && obj.UserId > 0 && obj.LocationID > 0 && obj.VehicleID > 0)
                {
                    //var ObjPreventativeMaintenaceManager = new PreventativeMaintenaceManager();
                    var PMList = _IEfleetPM.GetAllPendingPMReminderDescription(obj.LocationID, obj.VehicleID);
                    if (PMList.Count > 0)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = PMList;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;

                return Ok(ObjServiceResponseModel);
            }
        }
        /// <summary>
        /// Created By : bhushan dod
        /// Creaed for : to multiupload images.
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult UploadFiles()
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<string>>();
            try
            {
                var uploadFolderPath = HostingEnvironment.MapPath("~/Content/eFleetDocs/VehicleIncident/");

                //#region CleaningUpPreviousFiles.InDevelopmentOnly
                //DirectoryInfo directoryInfo = new DirectoryInfo(uploadFolderPath);
                //foreach (FileInfo fileInfo in directoryInfo.GetFiles())
                //	fileInfo.Delete();
                //#endregion

                if (Request.Content.IsMimeMultipartContent())
                {
                    var streamProvider = new WithExtensionMultipartFormDataStreamProvider(uploadFolderPath);
                    var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith<List<string>>(t =>
                    {
                        if (t.IsFaulted || t.IsCanceled)
                        {
                            throw new HttpResponseException(HttpStatusCode.InternalServerError);
                        }

                        var fileInfo = streamProvider.FileData.Select(i =>
                        {
                            var info = new FileInfo(i.LocalFileName);
                            return info.Name;
                        });
                        return fileInfo.ToList();
                    });
                    ObjServiceResponseModel.Data = task.Result.ToList();
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.InvariantCulture);
                    ObjServiceResponseModel.Message = CommonMessage.Successful();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidEntry();
                    ObjServiceResponseModel.Data = null;
                }
                //return Ok(ObjServiceResponseModel);
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                // return Ok(ObjServiceResponseModel);
            }
            return Ok(ObjServiceResponseModel);
        }

        [HttpPost]
        public IHttpActionResult GetAlleFleetPassengerTrackingRouteList(eFleetPassengerTrackingRouteServiceModel obj)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<eFleetPassengerTrackingRouteModel>>();
            try
            {
                if (obj != null && obj.ServiceAuthKey != null && obj.UserId > 0 && obj.ServiceType > 0)
                {
                    //var ObjPassengerTrackingManager = new PassengerTrackingManager();
                    var routeList = _IPassengerTracking.GetAllPassengerTrackingRouteDetails(obj);
                    if (routeList.Count > 0)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = routeList;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;

                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult SaveeFleetPassengerTrackingCount(eFleetPassengerTrackingCountModelForService obj)
        {
            //  var ObjPassengerTrackingManager = new PassengerTrackingManager();
            var ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (obj != null && obj.ServiceAuthKey != null && obj.UserId > 0)
                {
                    var ObjRespnse = _IPassengerTracking.InsertPassengerTrackingCount(obj);
                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : Oct-25-2017
        /// Created for: Saving Hours of services
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveeFleetHourseOfServices(HoursOfServicesModel obj)
        {
            // var ObjHoursOfServicesManager = new HoursOfServicesManager();
            var ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (obj != null && obj.ServiceAuthKey != null && obj.UserId > 0)
                {

                    var ObjRespnse = _IHoursOfServices.InsertHoursOfServices(obj);
                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        // Created By : Ashwajit Bansod
        // Created Date : 25 July 2018
        // Created For : To start manager app services 
        #region Manager App Service
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 25 july 2018
        /// Created For : to checking for validate login of manager user.
        /// </summary>
        /// <param name="objLogIn"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ValidateLoginForManager(eTracLoginModel objLogIn)
        {
            //ServiceResponseModel<eTracLoginModel> serviceresponse = new ServiceResponseModel<eTracLoginModel>();
            var ObjServiceResponseModel = new ServiceResponseModel<eTracLoginModel>();
            LoginManager _ILogin = new LoginManager();
            try
            {
                if (objLogIn != null && objLogIn.UserName != null && objLogIn.Password != null)
                {
                    var result = _ILogin.AuthenticateUser(objLogIn);
                    // This condition for invalid user
                    // Added By Bhushan Dod on Jan 12 2015
                    ObjServiceResponseModel.Message = (result != null && !string.IsNullOrEmpty(result.ResponseMessage)) ? result.ResponseMessage : CommonMessage.DoesNotExistsRecordMessage();
                    ObjServiceResponseModel.Response = (result != null) ? result.Response : 0;
                    ObjServiceResponseModel.Data = result;
                    //var DeviceId = "cNIqoC3sxb8:APA91bGk018nm8kqylw_y20tyYt2U7owYNXoDBOq50ugthDf2s1dujq4J1EckZrj4_g2DzVGEspe2yB2omFATRieb1qjqEfIixOSj4d6J3LJjSGwy8kE-gs67OatEjiHzqQMWyPYQ1xb_89PcwI5brv3yjhpzuNjXQ";
                    //EmailHelper objEmailHelper = new EmailHelper();
                    //objEmailHelper.MailType = "EMAINTENANCE";
                    //objEmailHelper.LocationID = result.LocationID;
                    //objEmailHelper.LocationName = result.LocationNames;
                    //objEmailHelper.UserName = result.UserName;                  
                    //if (objLogIn.DeviceId != null)
                    //{
                    //    PushNotificationFCM.FCMAndroid("Facility request submitted at ", objLogIn.DeviceId, objEmailHelper);
                    //}
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : July 26 2018
        /// Created For : To check for manager idle status
        /// </summary>
        /// <param name="objLogIn"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult LoginLogForManager(eTracLoginModel objLogIn)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<eTracLoginModel>();
            LoginManager _ILogin = new LoginManager();
            try
            {
                if (objLogIn != null && objLogIn.UserId != null && objLogIn.LocationID != null && objLogIn.UserRoleId != null)
                {
                    eTracLoginModel result = _ILogin.InsertLoginLog(objLogIn);
                    // This condition for invalid user
                    // Added By Bhushan Dod on Jan 12 2015
                    ObjServiceResponseModel.Message = (result != null && !string.IsNullOrEmpty(result.ResponseMessage)) ? result.ResponseMessage : CommonMessage.DoesNotExistsRecordMessage();
                    ObjServiceResponseModel.Response = (result != null) ? result.Response : 0;
                    ObjServiceResponseModel.Data = result;
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : Aug-10-2018
        /// Created For : To fetching the Unassigned Work Order List 
        /// </summary>
        /// <param name="objManagerAppModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UnAssignedWorkOrderForManager(ManagerAppModel objManagerAppModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<UnAssignedWorkOrderModel>>();
            ManagerAppeMaintenanceManager _IManagerAppeMaintenanceManager = new ManagerAppeMaintenanceManager();
            try
            {
                if (objManagerAppModel != null && objManagerAppModel.ServiceAuthKey != null && objManagerAppModel.LocationId != null)
                {
                    var result = _IManagerAppeMaintenanceManager.UnassignedWorkOrderList(objManagerAppModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = result;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : Aug-13-2018
        /// Created For : to fetch list of employee for assigning work order
        /// </summary>
        /// <param name="objManagerAppModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ListOfEmployeeToAssignedForManager(ManagerAppModel objManagerAppModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<EmployeeListModel>>();
            ManagerAppeMaintenanceManager _IManagerAppeMaintenanceManager = new ManagerAppeMaintenanceManager();
            try
            {
                if (objManagerAppModel != null && objManagerAppModel.ServiceAuthKey != null && objManagerAppModel.LocationId != null)
                {
                    var result = _IManagerAppeMaintenanceManager.EmployeeList(objManagerAppModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = result;
                        return Ok(ObjServiceResponseModel);
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : Aug-17-2018
        /// Created For : To Assign Employee to WO
        /// </summary>
        /// <param name="objManagerAppModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AssignedEmployeeToWorkOrder(ManagerAppModel objManagerAppModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<string>();
            ManagerAppeMaintenanceManager _IManagerAppeMaintenanceManager = new ManagerAppeMaintenanceManager();
            try
            {
                if (objManagerAppModel != null && objManagerAppModel.ServiceAuthKey != null
                    && objManagerAppModel.LocationId != null
                    && objManagerAppModel.UserId > 0
                    && objManagerAppModel.WorkRequestCodeId != null)
                {
                    var result = _IManagerAppeMaintenanceManager.AssignedEmployee(objManagerAppModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = result.Response;
                        ObjServiceResponseModel.Message = result.Message;
                        return Ok(ObjServiceResponseModel);
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : Aug-17-2018
        /// Created For : Dashboard count of unassignedWO,Continues WO, DAR
        /// </summary>
        /// <param name="objServiceDashboardModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DashboardCountForManager(ManagerAppModel objManagerAppModel)
        {
            CommonMethodManager ObjCommonMethodManager = new CommonMethodManager();
            var _IManagerAppeMaintenanceManager = new ManagerAppeMaintenanceManager();
            var objServiceResponseModel = new ServiceResponseModel<DashboardCountModel>();
            try
            {

                if (objManagerAppModel != null && objManagerAppModel.ServiceAuthKey != null && objManagerAppModel.UserId != null && objManagerAppModel.LocationId != null)
                {
                    objServiceResponseModel = _IManagerAppeMaintenanceManager.GetCountOfDashboardForManager(objManagerAppModel);
                }
                else
                {
                    objServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    objServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                objServiceResponseModel.Message = ex.Message;
                objServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                objServiceResponseModel.Data = null;
            }

            return Ok(objServiceResponseModel);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : Aug-27-2018
        /// Created For: To fetc list of all continues request .
        /// </summary>
        /// <param name="objManagerAppModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ContinuesWorkOrderListForManager(ManagerAppModel objManagerAppModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<UnAssignedWorkOrderModel>>();
            ManagerAppeMaintenanceManager _IManagerAppeMaintenanceManager = new ManagerAppeMaintenanceManager();
            try
            {
                if (objManagerAppModel != null && objManagerAppModel.ServiceAuthKey != null && objManagerAppModel.LocationId != null)
                {
                    var result = _IManagerAppeMaintenanceManager.AllContinuesWorkOrderList(objManagerAppModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = result;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashutosh Dwivedi
        /// Created Date : 15-Oct-2018
        /// Created for: To Fetch PO type list 
        /// </summary>
        /// <param name="objServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult POTypeListManager(ServiceBaseModel objServiceBaseModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<POTypeServiceModel>();
            var _POTypeDetailsManager = new POTypeDetailsManager();
            try
            {
                if (objServiceBaseModel != null && objServiceBaseModel.ServiceAuthKey != null
                    && objServiceBaseModel.LocationID > 0 && objServiceBaseModel.UserId > 0)
                {
                    var result = _POTypeDetailsManager.AllPOTypeList(objServiceBaseModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = result;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashutosh Dwivedi
        /// Created Date : Oct-16-2018
        /// Created For : To fetch vendor list as per LocationId for Manager
        /// </summary>
        /// <param name="objServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult VendorListByLocationIdManager(ServiceBaseModel objServiceBaseModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<VendorTypeListServiceModel>>();
            var _POTypeDetailsManager = new POTypeDetailsManager();
            try
            {
                if (objServiceBaseModel != null && objServiceBaseModel.ServiceAuthKey != null
                    && objServiceBaseModel.LocationID > 0 && objServiceBaseModel.UserId > 0)
                {
                    var result = _POTypeDetailsManager.GetCompany_VendorListByLocationId(objServiceBaseModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = result;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By :Ashutosh Dwivedi
        /// Created For : To fetch the list of Company Facility as per vendor Id for Manager
        /// Created Date: Oct-16-2018
        /// </summary>
        /// <param name="objServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CompanyFacilityListByVendorIdManager(POCommonServiceModel objPOCommonServiceModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<CompanyFacilityListServiceModel>();
            var _POTypeDetailsManager = new POTypeDetailsManager();
            try
            {
                if (objPOCommonServiceModel != null
                    && objPOCommonServiceModel.LocationId > 0 && objPOCommonServiceModel.UserId > 0)
                {
                    var result = _POTypeDetailsManager.GetCompanyFacilityByVendoeId(objPOCommonServiceModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = result;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashutosh Dwivedi
        /// Created Date : Oct-16-2018
        /// Created For : To get List of all question
        /// </summary>
        /// <param name="objPOCommonServiceModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult EmeregencyPOQuestonManager(POCommonServiceModel objPOCommonServiceModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<QuestionsEmergencyPO>>();
            var _POTypeDetailsManager = new POTypeDetailsManager();
            try
            {
                if (objPOCommonServiceModel != null
                    && objPOCommonServiceModel.LocationId > 0 && objPOCommonServiceModel.UserId > 0)
                {
                    var result = _POTypeDetailsManager.GetQuestionList(objPOCommonServiceModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = result.Response;
                        ObjServiceResponseModel.Message = result.Message;
                        ObjServiceResponseModel.Data = result.Data;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By Ashwajit Bansod
        /// Created Date : 05-Oct-2018
        /// Created For  : To get All info of PO by POId
        /// </summary>
        /// <param name="objPOCommonServiceModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetAllPODataDetailsByPONumberManager(POCommonServiceModel objPOCommonServiceModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<PODetailsServiceModel>();
            var _POTypeDetailsManager = new POTypeDetailsManager();
            try
            {
                if (objPOCommonServiceModel != null
                    && objPOCommonServiceModel.LocationId > 0 && objPOCommonServiceModel.POId > 0)
                {
                    var result = _POTypeDetailsManager.GetAllPODetailsByIdForMobile(objPOCommonServiceModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = result.Response;
                        ObjServiceResponseModel.Message = result.Message;
                        ObjServiceResponseModel.Data = result.Data;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 26-Sept-2018
        /// Created For: To dave all PO and Grid into database all merge in on model
        /// </summary>
        /// <param name="objSavingServicePOModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SavePODataManager(SavingServicePOModel objSavingServicePOModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<QuestionsEmergencyPO>>();
            var GridData = new List<GridDataPO>();
            var QuestionData = new List<QuestionAnswerModel>();
            var PODetailData = new POTypeDataModel();
            var _POTypeDetailsManager = new POTypeDetailsManager();
            try
            {
                if (objSavingServicePOModel != null && objSavingServicePOModel.CompanyFacility.Count > 0
                      && objSavingServicePOModel.UserId > 0)
                {
                    GridData = objSavingServicePOModel.CompanyFacility;
                    QuestionData = objSavingServicePOModel.QusetionAnswer;
                    //To save objSavingServicePOModel data to PODetailData as we use this model in business logic and 
                    // not need to add new logic for same saving of PO 
                    PODetailData.PointOfContactAddress = objSavingServicePOModel.Address;
                    PODetailData.AllocateToLocation = objSavingServicePOModel.AllocateToLocation;
                    PODetailData.Amount = objSavingServicePOModel.Amount;
                    PODetailData.BillDate = objSavingServicePOModel.BillDate;
                    PODetailData.Vendor = objSavingServicePOModel.VendorId;
                    PODetailData.Comment = objSavingServicePOModel.Comment;
                    PODetailData.DeliveryDate = objSavingServicePOModel.DeliveryDate;
                    PODetailData.InvoicingFrequency = objSavingServicePOModel.InvoicingFrequency;
                    PODetailData.IssueDate = objSavingServicePOModel.IssueDate;
                    PODetailData.IsVendorRegister = objSavingServicePOModel.IsVendorRegister;
                    PODetailData.Location = objSavingServicePOModel.LocationId;
                    PODetailData.PointOfContactName = objSavingServicePOModel.PointOfContact;
                    PODetailData.PONumber = objSavingServicePOModel.PONumber; PODetailData.POType = objSavingServicePOModel.POType;
                    PODetailData.UserId = objSavingServicePOModel.UserId;
                    PODetailData.POStatus = objSavingServicePOModel.POStatus;
                    bool IsManager = true;
                    var result = _POTypeDetailsManager.SavePODetails(PODetailData, GridData, QuestionData, IsManager); 
                    if (result == true)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.POSaved(); ;
                        ObjServiceResponseModel.Data = null;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashutosh Dwivedi
        /// Created Date : 23-OCT-2018
        /// Created for to get waiting list of PO for Manager
        /// </summary>
        /// <param name="objServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult PODetailsListManagerWaiting(ServiceBaseModel objServiceBaseModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<POListModel>>();
            var _POTypeDetailsManager = new POTypeDetailsManager();
            try
            {
                if (objServiceBaseModel != null && objServiceBaseModel.ServiceAuthKey != null
                    && objServiceBaseModel.LocationID > 0 && objServiceBaseModel.UserId > 0)
                {
                    var result = _POTypeDetailsManager.WaitingPODetailList(objServiceBaseModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = result;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By :  Ashutosh Dwivedi
        /// Created Date : 23-OCT-2018
        /// Created For : To Approve PO By Manager
        /// </summary>
        /// <param name="objServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult PODetailsApproval(ServiceBaseModel objServiceBaseModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<ServiceBaseModel>();
            var _POTypeDetailsManager = new POTypeDetailsManager();
            var objPOApproveRejectModel = new POApproveRejectModel();
            var objListData = new POListModel();
            try
            {
                if (objServiceBaseModel != null && objServiceBaseModel.ServiceAuthKey != null
                    && objServiceBaseModel.LocationID > 0 && objServiceBaseModel.UserId > 0)
                {
                    objPOApproveRejectModel.POModifiedId = objServiceBaseModel.POId;
                    objListData.Total = objServiceBaseModel.Total;
                    objPOApproveRejectModel.POId = objServiceBaseModel.LogPOId;
                    objListData.CompanyName = objServiceBaseModel.CompanyName;
                    objPOApproveRejectModel.UserId = objServiceBaseModel.UserId;
                    var result = _POTypeDetailsManager.ApprovePOByPOId(objPOApproveRejectModel, objListData);//PODetailsApproval(objServiceBaseModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = null;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 25-OCT-2018
        /// Created For : To save bill to Manager
        /// </summary>
        /// <param name="objBillDataServiceModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveBillDataManager(BillDataServiceModel objBillDataServiceModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<BillDataServiceModel>();
            try
            {
                if (objBillDataServiceModel != null && objBillDataServiceModel.VendorId > 0
                      && objBillDataServiceModel.LocationId > 0 && objBillDataServiceModel.UserId > 0)
                {
                    var result = _IBillDataManager.SaveBillDetails(objBillDataServiceModel);
                    if (result == true)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.BillSaved(); ;
                        ObjServiceResponseModel.Data = null;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 26-OCT-2018
        /// Created For : To get Location which is assign to manager.
        /// </summary>
        /// <param name="ObjServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetLocationAssignToUserForManager(ServiceBaseModel ObjServiceBaseModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<LocationServiceModel>>();
            try
            {
                if (ObjServiceBaseModel != null && ObjServiceBaseModel.ServiceAuthKey != null)
                {
                    bool isManager = true;
                    var result = _IMiscellaneousManager.GetLocationAssignedListByUserId(ObjServiceBaseModel, isManager);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = result.Response;
                        ObjServiceResponseModel.Message = result.Message;
                        ObjServiceResponseModel.Data = result.Data;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By :  Ashwajit Bansod
        /// Crated Date :  26-OCT-2018
        /// Created For : To get all miscellaneous list as per locatio Id.
        /// </summary>
        /// <param name="ObjServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMiscellaneousListForManager(ServiceBaseModel ObjServiceBaseModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<MiscellaneousModel>>();
            try
            {
                if (ObjServiceBaseModel != null && ObjServiceBaseModel.ServiceAuthKey != null && ObjServiceBaseModel.LocationID > 0)
                {
                    var result = _IMiscellaneousManager.GetMiscellaneousList(ObjServiceBaseModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = result.Response;
                        ObjServiceResponseModel.Message = result.Message;
                        ObjServiceResponseModel.Data = result.Data;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By: Ashwajit Bansod
        /// Created Date : 26-OCT-2018
        /// Created For : To get Miscellaneous number for Manager.
        /// </summary>
        /// <param name="ObjServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMiscellaneousNumberForManager(ServiceBaseModel ObjServiceBaseModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<miscellaneousNumberModel>();
            try
            {
                if (ObjServiceBaseModel != null && ObjServiceBaseModel.ServiceAuthKey != null)
                {
                    var result = _IMiscellaneousManager.GetMiscellaneousNumberData(ObjServiceBaseModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = result.Response;
                        ObjServiceResponseModel.Message = result.Message;
                        ObjServiceResponseModel.Data = result.Data;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 26-OCT-2018
        /// Created For : To save all miscellaneous data 
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveMiscellaneousDataForManager(MiscellaneousDetails Obj)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<MiscellaneousDetails>();
            try
            {
                if (Obj.MiscellaneousDetailsmodel.Count > 0)
                {
                    var result = _IMiscellaneousManager.SaveMiscellaneous(Obj);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = result.Response;
                        ObjServiceResponseModel.Message = result.Message;
                        ObjServiceResponseModel.Data = result.Data;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 26-OCT-2018
        /// Created For : To get Location List
        /// </summary>
        /// <param name="ObjServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetLocationList(ServiceBaseModel ObjServiceBaseModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<LocationServiceModel>>();
            try
            {
                if (ObjServiceBaseModel != null && ObjServiceBaseModel.ServiceAuthKey != null)
                {
                    var result = _IMiscellaneousManager.GetLocationList(ObjServiceBaseModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = result.Response;
                        ObjServiceResponseModel.Message = result.Message;
                        ObjServiceResponseModel.Data = result.Data;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : Nov-13-2018
        /// Created for : To get list of all miscellaneous list for manager app
        /// </summary>
        /// <param name="ObjServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetAllListMiscellaneousForManager(MiscellaneousServiceModel ObjServiceBaseModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<MiscellaneousListModel>>();
            try
            {
                if (ObjServiceBaseModel != null && ObjServiceBaseModel.ServiceAuthKey != null && ObjServiceBaseModel.LocationId > 0)
                {
                    var result = _IMiscellaneousManager.GetAllMiscellaneousListForManager(ObjServiceBaseModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = result.Response;
                        ObjServiceResponseModel.Message = result.Message;
                        ObjServiceResponseModel.Data = result.Data;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 13-Nov-2018
        /// Created For : To Get sub list of miscellaneous by misc id.
        /// </summary>
        /// <param name="ObjServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetAllListChildMiscellaneousByMiscIdForManager(MiscellaneousServiceModel ObjServiceBaseModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<MiscellaneousListModel>>();
            try
            {
                if (ObjServiceBaseModel != null && ObjServiceBaseModel.ServiceAuthKey != null && ObjServiceBaseModel.LocationId > 0)
                {
                    var result = _IMiscellaneousManager.GetAllChildeMiscellaneousListForManagerByMiscId(ObjServiceBaseModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = result.Response;
                        ObjServiceResponseModel.Message = result.Message;
                        ObjServiceResponseModel.Data = result.Data;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By:Ashwajit Bansod
        /// Created Date : 13 - Nov -2018
        /// Created For : To approve miscellaneous list.
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ApproveMiscellaneousForManager(MiscellaneousList Obj)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<MiscellaneousList>>();
            workorderEMSEntities _workorderems = new workorderEMSEntities();
            string UserName = "";
            try
            {
                long QuikBookMiscId = 0;
                if (Obj.ChildMiscellaneousList.Count > 0 && Obj.UserId > 0)
                {
                    var result = _IMiscellaneousManager.ApproveMiscellaneous(Obj.ChildMiscellaneousList, Obj.UserName, Obj.UserId,Obj.LocationId, QuikBookMiscId, 10019);
                    //passing hardcoded for only elite parking service vendor
                    if (result  == true)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.ErrorMiscellaneous();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }
        #endregion Manager App Service

        #region PO
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-Sep-2018
        /// Created for: To Fetch PO type list 
        /// </summary>
        /// <param name="objServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult POTypeList(ServiceBaseModel objServiceBaseModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<POTypeServiceModel>();
            var _POTypeDetailsManager = new POTypeDetailsManager();
            try
            {
                if (objServiceBaseModel != null && objServiceBaseModel.ServiceAuthKey != null
                    && objServiceBaseModel.LocationID > 0 && objServiceBaseModel.UserId > 0)
                {
                    var result = _POTypeDetailsManager.AllPOTypeList(objServiceBaseModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = result;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : Sept-17-2018
        /// Created For : To fetch vendor list as per LocationId
        /// </summary>
        /// <param name="objServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult VendorListByLocationId(ServiceBaseModel objServiceBaseModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<VendorTypeListServiceModel>>();
            var _POTypeDetailsManager = new POTypeDetailsManager();
            try
            {
                if (objServiceBaseModel != null && objServiceBaseModel.ServiceAuthKey != null
                    && objServiceBaseModel.LocationID > 0 && objServiceBaseModel.UserId > 0)
                {
                    var result = _POTypeDetailsManager.GetCompany_VendorListByLocationId(objServiceBaseModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = result;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To fetch the list of Company Facility as per vendor Id
        /// Created Date: Sept-19-2018
        /// </summary>
        /// <param name="objServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CompanyFacilityListByVendorId(POCommonServiceModel objPOCommonServiceModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<CompanyFacilityListServiceModel>();
            var _POTypeDetailsManager = new POTypeDetailsManager();
            try
            {
                if (objPOCommonServiceModel != null
                    && objPOCommonServiceModel.LocationId > 0 && objPOCommonServiceModel.UserId > 0)
                {
                    var result = _POTypeDetailsManager.GetCompanyFacilityByVendoeId(objPOCommonServiceModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = result;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : Sept-24-2018
        /// Created For : To get List of all question
        /// </summary>
        /// <param name="objPOCommonServiceModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult EmeregencyPOQueston(POCommonServiceModel objPOCommonServiceModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<QuestionsEmergencyPO>>();
            var _POTypeDetailsManager = new POTypeDetailsManager();
            try
            {
                if (objPOCommonServiceModel != null
                    && objPOCommonServiceModel.LocationId > 0 && objPOCommonServiceModel.UserId > 0)
                {
                    var result = _POTypeDetailsManager.GetQuestionList(objPOCommonServiceModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = result.Response;
                        ObjServiceResponseModel.Message = result.Message;
                        ObjServiceResponseModel.Data = result.Data;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 26-Sept-2018
        /// Created For: To dave all PO and Grid into database all merge in on model
        /// </summary>
        /// <param name="objSavingServicePOModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SavePOData(SavingServicePOModel objSavingServicePOModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<QuestionsEmergencyPO>>();
            var GridData = new List<GridDataPO>();
            var QuestionData = new List<QuestionAnswerModel>();
            var PODetailData = new POTypeDataModel();
            var _POTypeDetailsManager = new POTypeDetailsManager();
            try
            {
                if (objSavingServicePOModel != null 
                      && objSavingServicePOModel.UserId > 0)
                {
                    GridData = objSavingServicePOModel.CompanyFacility;
                    QuestionData = objSavingServicePOModel.QusetionAnswer;
                    //To save objSavingServicePOModel data to PODetailData as we use this model in business logic and 
                    // not need to add new logic for same saving of PO 
                    PODetailData.PointOfContactAddress = objSavingServicePOModel.Address;
                    PODetailData.AllocateToLocation = objSavingServicePOModel.AllocateToLocation;
                    PODetailData.Amount = objSavingServicePOModel.Amount;
                    PODetailData.BillDate = objSavingServicePOModel.BillDate;
                    PODetailData.Vendor = objSavingServicePOModel.VendorId;
                    PODetailData.Comment = objSavingServicePOModel.Comment;
                    PODetailData.DeliveryDate = objSavingServicePOModel.DeliveryDate;
                    PODetailData.InvoicingFrequency = objSavingServicePOModel.InvoicingFrequency;
                    PODetailData.IssueDate = objSavingServicePOModel.IssueDate;
                    PODetailData.IsVendorRegister = objSavingServicePOModel.IsVendorRegister;
                    PODetailData.Location = objSavingServicePOModel.LocationId;
                    PODetailData.PointOfContactName = objSavingServicePOModel.PointOfContact;
                    PODetailData.PONumber = objSavingServicePOModel.PONumber; PODetailData.POType = objSavingServicePOModel.POType;
                    PODetailData.UserId = objSavingServicePOModel.UserId;
                    PODetailData.Total = objSavingServicePOModel.Total;
                    PODetailData.VendorName = objSavingServicePOModel.VendorName;
                    PODetailData.POStatus = objSavingServicePOModel.POStatus;
                    bool IsManager = false;
                    var result = _POTypeDetailsManager.SavePODetails(PODetailData, GridData, QuestionData, IsManager); ;
                    if (result == true)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.POSaved(); ;
                        ObjServiceResponseModel.Data = null;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }



        /// <summary>
        /// Created By  : Ashwajit bansod
        /// Created Date : 04-Oct-2018
        /// Created For : To get All PO list by PO Number
        /// </summary>
        /// <param name="objPOCommonServiceModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetAllPOList(POCommonServiceModel objPOCommonServiceModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<POListModel>>();
            var _POTypeDetailsManager = new POTypeDetailsManager();
            try
            {
                if (objPOCommonServiceModel != null
                    && objPOCommonServiceModel.LocationId > 0 && objPOCommonServiceModel.UserId > 0)
                {
                    var result = _POTypeDetailsManager.GetAllPOListForMobile(objPOCommonServiceModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = result.Response;
                        ObjServiceResponseModel.Message = result.Message;
                        ObjServiceResponseModel.Data = result.Data;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By Ashwajit Bansod
        /// Created Date : 05-Oct-2018
        /// Created For  : To get All info of PO by POId
        /// </summary>
        /// <param name="objPOCommonServiceModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetAllPODataDetailsByPONumber(POCommonServiceModel objPOCommonServiceModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<PODetailsServiceModel>();
            var _POTypeDetailsManager = new POTypeDetailsManager();
            try
            {
                if (objPOCommonServiceModel != null
                    && objPOCommonServiceModel.LocationId > 0 && objPOCommonServiceModel.POId > 0)
                {
                    var result = _POTypeDetailsManager.GetAllPODetailsByIdForMobile(objPOCommonServiceModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = result.Response;
                        ObjServiceResponseModel.Message = result.Message;
                        ObjServiceResponseModel.Data = result.Data;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }


        #endregion PO

        #region Bill
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 12-OCT-2018
        /// Crerated For : To save Bill against PO
        /// </summary>
        /// <param name="objBillDataServiceModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveBillData(BillDataServiceModel objBillDataServiceModel)
        {
            var _vendorManager = new VendorManagementManager();
            var ObjServiceResponseModel = new ServiceResponseModel<BillDataServiceModel>();
            try
            {
                var resultBill = new Intuit.Ipp.Data.Bill();
                if (objBillDataServiceModel != null && objBillDataServiceModel.VendorId > 0
                      && objBillDataServiceModel.LocationId > 0 && objBillDataServiceModel.UserId > 0)
                {
                    if (objBillDataServiceModel.CheckedPOId != null && objBillDataServiceModel.CheckedPOId != "")
                    {
                        string[] CheckedId = objBillDataServiceModel.CheckedPOId.Split(',');
                        string[] UnitPrice = objBillDataServiceModel.UnitPrice.Split(',');
                        string[] CostCodeList = objBillDataServiceModel.CostCode.Split(',');                        
                    }

                    if (objBillDataServiceModel.PODId > 0)
                    {
                        if (CallbackController.RealMId != null)
                        {
                            string realmId = CallbackController.RealMId.ToString();
                            try
                            {
                                string AccessToken = CallbackController.AccessToken.ToString();
                                var principal = User as ClaimsPrincipal;
                                OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(AccessToken);

                                // Create a ServiceContext with Auth tokens and realmId
                                ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                                serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
                                DataService commonServiceQBO = new DataService(serviceContext);
                                // Create a QuickBooks QueryService using ServiceContext
                                QueryService<Vendor> querySvc = new QueryService<Vendor>(serviceContext);
                                List<Vendor> vendorList = querySvc.ExecuteIdsQuery("SELECT * FROM Vendor MaxResults 1000").ToList();

                                QueryService<Account> querySvcAccount = new QueryService<Account>(serviceContext);
                                List<Account> accountData = querySvcAccount.ExecuteIdsQuery("SELECT * FROM Account MaxResults 1000").ToList();
                                var VendorDetails = _vendorManager.GetCompanyQuickBookId(objBillDataServiceModel.VendorId);
                                var reference = new ReferenceType();
                                var accountRef = new AccountBasedExpenseLineDetail();
                                var bill = new Intuit.Ipp.Data.Bill();
                                if (VendorDetails > 0)
                                {
                                    var vendorData = vendorList.Where(x => x.Id == VendorDetails.ToString()).FirstOrDefault();                                   
                                    //Vendor Reference                                   
                                    bill.VendorRef = new ReferenceType()
                                    {
                                        name = vendorData.DisplayName,
                                        Value = vendorData.Id
                                    };
                                }
                                //End Vendor Reference
                                var metaData = new ModificationMetaData();
                                metaData.CreateTime = Convert.ToDateTime(objBillDataServiceModel.InvoiceDate);
                                bill.MetaData = metaData;
                                //End Time

                                bill.APAccountRef = new ReferenceType()
                                {
                                    name = "Accounts Payable (A/P)",
                                    Value = "33"
                                };
                                //reference.name = dataget.Name;
                                //reference.Value = dataget.Id;
                                var LocationName = _IBillDataManager.GetLocationDataByLocId(objBillDataServiceModel.LocationId);
                                bill.DepartmentRef = new ReferenceType()
                                {
                                    name = LocationName.LocationName,
                                    Value = LocationName.QBK_Id.ToString()
                                };
                                Line line = new Line();
                                List<Line> lineList = new List<Line>();                               
                                int i = 1;
                                long CostCodeIdData = 0;                     
                                string[] ActiveId = objBillDataServiceModel.IsActive.Split(',');
                                var length = ActiveId.Length;
                                Line[] lstData = new Line[length];
                                if (ActiveId != null)
                                {
                                    foreach (var item in ActiveId)
                                    {

                                        var dataFacility = _IBillDataManager.GetFacilityDataByFacilityId(item);

                                        long CostCodeId = Convert.ToInt64(dataFacility.CostCode);
                                        var costCodeName = _IBillDataManager.GetCostCodeData(CostCodeId);
                                        var dataget = accountData.Where(x => x.Name == costCodeName.Description).FirstOrDefault();
                                        accountRef.AccountRef = new ReferenceType()
                                        {
                                            name = dataget.Name,
                                            Value = dataget.Id
                                        };

                                        line.AnyIntuitObject = accountRef;
                                        line.DetailType = LineDetailTypeEnum.AccountBasedExpenseLineDetail;
                                        line.DetailTypeSpecified = true;
                                        //line.LineNum = i.ToString();
                                        decimal? cal = dataFacility.UnitPrice * dataFacility.Quantity;
                                        line.Amount = Convert.ToDecimal(cal);
                                        line.AmountSpecified = true;
                                        line.LineNum = i.ToString();
                                        i++;
                                        lineList.Add(line);
                                        line = new Line();
                                        // reference = new ReferenceType();
                                        accountRef = new AccountBasedExpenseLineDetail();
                                        // bill.Line = lineList.ToArray();
                                    }
                                }
                                bill.Line = lineList.ToArray();
                                resultBill = commonServiceQBO.Add(bill) as Intuit.Ipp.Data.Bill;
                            }
                            catch (Exception ex)
                            {
                                ObjServiceResponseModel.Message = ex.Message;
                                ObjServiceResponseModel.Response = -1;
                                ObjServiceResponseModel.Data = null;
                            }

                        }
                    }
                    else
                    {
                        if (objBillDataServiceModel.BillNumber > 0)
                        {
                            if (CallbackController.RealMId != null)
                            {
                                string realmId = CallbackController.RealMId.ToString();
                                try
                                {
                                    string AccessToken = CallbackController.AccessToken.ToString();
                                    var principal = User as ClaimsPrincipal;
                                    OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(AccessToken);

                                    // Create a ServiceContext with Auth tokens and realmId
                                    ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                                    serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
                                    DataService commonServiceQBO = new DataService(serviceContext);
                                    // Create a QuickBooks QueryService using ServiceContext
                                    QueryService<Vendor> querySvc = new QueryService<Vendor>(serviceContext);
                                    List<Vendor> vendorList = querySvc.ExecuteIdsQuery("SELECT * FROM Vendor MaxResults 1000").ToList();

                                    QueryService<Account> querySvcAccount = new QueryService<Account>(serviceContext);
                                    List<Account> accountData = querySvcAccount.ExecuteIdsQuery("SELECT * FROM Account MaxResults 1000").ToList();
                                    var reference = new ReferenceType();
                                    var accountRef = new AccountBasedExpenseLineDetail();
                                    var bill = new Intuit.Ipp.Data.Bill();
                                    if (objBillDataServiceModel.VendorId > 0)
                                    {
                                        var VendorDetails = _vendorManager.GetCompanyQuickBookId(objBillDataServiceModel.VendorId);
                                        if (VendorDetails > 0)
                                        {
                                            var vendorData = vendorList.Where(x => x.Id == VendorDetails.ToString()).FirstOrDefault();
                                            //Vendor Reference                                   
                                            bill.VendorRef = new ReferenceType()
                                            {
                                                name = vendorData.DisplayName,
                                                Value = vendorData.Id
                                            };
                                        }
                                    }
                                    //End Vendor Reference
                                    var metaData = new ModificationMetaData();
                                    metaData.CreateTime = Convert.ToDateTime(objBillDataServiceModel.InvoiceDate);
                                    bill.MetaData = metaData;
                                    //End Time

                                    bill.APAccountRef = new ReferenceType()
                                    {
                                        name = "Accounts Payable (A/P)",
                                        Value = "33"
                                    };
                                    //reference.name = dataget.Name;
                                    //reference.Value = dataget.Id;
                                    var LocationName = _IBillDataManager.GetLocationDataByLocId(objBillDataServiceModel.LocationId);
                                    bill.DepartmentRef = new ReferenceType()
                                    {
                                        name = LocationName.LocationName,
                                        Value = LocationName.QBK_Id.ToString()
                                    };
                                    Line line = new Line();
                                    List<Line> lineList = new List<Line>();
                                    int i = 1;
                                    long CostCodeIdData = 0;
                                    string[] ActiveId = objBillDataServiceModel.IsActive.Split(',');
                                    var length = ActiveId.Length;
                                    Line[] lstData = new Line[length];
                                    if (objBillDataServiceModel.FacilityListForManualBill != null && objBillDataServiceModel.FacilityListForManualBill.Count() > 0)
                                    {
                                        foreach (var item in objBillDataServiceModel.FacilityListForManualBill)
                                        {
                                            var dataFacility = _IBillDataManager.GetFacilityDataByFacilityId(item.COM_FacilityId.ToString());
                                            long CostCodeId = Convert.ToInt64(dataFacility.CostCode);
                                            var costCodeName = _IBillDataManager.GetCostCodeData(CostCodeId);
                                            var dataget = accountData.Where(x => x.Name == costCodeName.Description).FirstOrDefault();
                                            accountRef.AccountRef = new ReferenceType()
                                            {
                                                name = dataget.Name,
                                                Value = dataget.Id
                                            };

                                            line.AnyIntuitObject = accountRef;
                                            line.DetailType = LineDetailTypeEnum.AccountBasedExpenseLineDetail;
                                            line.DetailTypeSpecified = true;
                                            //line.LineNum = i.ToString();
                                            decimal? cal = dataFacility.UnitPrice * dataFacility.Quantity;
                                            line.Amount = Convert.ToDecimal(cal);
                                            line.AmountSpecified = true;
                                            line.LineNum = i.ToString();
                                            i++;
                                            lineList.Add(line);
                                            line = new Line();
                                            // reference = new ReferenceType();
                                            accountRef = new AccountBasedExpenseLineDetail();
                                            // bill.Line = lineList.ToArray();
                                        }
                                    }
                                    else
                                    {
                                        var dataget = accountData.Where(x => x.Name == "Other Expenses").FirstOrDefault();
                                        accountRef.AccountRef = new ReferenceType()
                                        {
                                            name = dataget.Name,
                                            Value = dataget.Id
                                        };

                                        line.AnyIntuitObject = accountRef;
                                        line.DetailType = LineDetailTypeEnum.AccountBasedExpenseLineDetail;
                                        line.DetailTypeSpecified = true;
                                        line.Amount = objBillDataServiceModel.InvoiceAmount;// Convert.ToDecimal(cal);
                                        line.AmountSpecified = true;
                                        line.LineNum = i.ToString();
                                        i++;
                                        lineList.Add(line);
                                        line = new Line();
                                    }
                                    bill.Line = lineList.ToArray();
                                    resultBill = commonServiceQBO.Add(bill) as Intuit.Ipp.Data.Bill;
                                }
                                catch (Exception ex)
                                {
                                    ObjServiceResponseModel.Message = ex.Message;
                                    ObjServiceResponseModel.Response = -1;
                                    ObjServiceResponseModel.Data = null;
                                }

                            }
                        }
                    }
                    objBillDataServiceModel.QuickBookBillId = Convert.ToInt64(resultBill.Id);
                    var result = _IBillDataManager.SaveBillDetails(objBillDataServiceModel);
                    if (result == true)
                    {                        
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.BillSaved(); ;
                        ObjServiceResponseModel.Data = null;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To get Pre bill Number
        /// Created Date : 20-Dec-2018
        /// </summary>
        /// <param name="ObjServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetPreBillNumber(ServiceBaseModel ObjServiceBaseModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<BillNumberData>();
            try
            {
                if (ObjServiceBaseModel != null && ObjServiceBaseModel.ServiceAuthKey != null)
                {
                    var result = _IBillDataManager.GetPreBillNumberData(ObjServiceBaseModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = result.Response;
                        ObjServiceResponseModel.Message = result.Message;
                        ObjServiceResponseModel.Data = result.Data;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }
        #endregion Bill

        #region Miscellineous
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 16-OCT-2018
        /// Created For : To get List location assigned to user. 
        /// </summary>
        /// <param name="ObjServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetLocationAssignToUser(ServiceBaseModel ObjServiceBaseModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<LocationServiceModel>>();
            try
            {
                if (ObjServiceBaseModel != null && ObjServiceBaseModel.ServiceAuthKey != null)
                {
                    bool isManager = false;
                    var result = _IMiscellaneousManager.GetLocationAssignedListByUserId(ObjServiceBaseModel, isManager);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = result.Response;
                        ObjServiceResponseModel.Message = result.Message;
                        ObjServiceResponseModel.Data = result.Data;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 16-OCT-2018
        /// Created For : To get List location assigned to user. 
        /// </summary>
        /// <param name="ObjServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMiscellaneousList(ServiceBaseModel ObjServiceBaseModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<MiscellaneousModel>>();
            try
            {
                if (ObjServiceBaseModel != null && ObjServiceBaseModel.ServiceAuthKey != null && ObjServiceBaseModel.LocationID > 0)
                {
                    var result = _IMiscellaneousManager.GetMiscellaneousList(ObjServiceBaseModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = result.Response;
                        ObjServiceResponseModel.Message = result.Message;
                        ObjServiceResponseModel.Data = result.Data;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }


        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-OCT-2018
        /// Created For : To get miscellaneous number.
        /// </summary>
        /// <param name="ObjServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMiscellaneousNumber(ServiceBaseModel ObjServiceBaseModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<miscellaneousNumberModel>();
            try
            {
                if (ObjServiceBaseModel != null && ObjServiceBaseModel.ServiceAuthKey != null)
                {
                    var result = _IMiscellaneousManager.GetMiscellaneousNumberData(ObjServiceBaseModel);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = result.Response;
                        ObjServiceResponseModel.Message = result.Message;
                        ObjServiceResponseModel.Data = result.Data;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-OCT-2018
        /// Created For : To save Miscellaneous data
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveMiscellaneousData(MiscellaneousDetails Obj)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<MiscellaneousDetails>();
            try
            {
                if (Obj.MiscellaneousDetailsmodel.Count > 0)
                {
                    var result = _IMiscellaneousManager.SaveMiscellaneous(Obj);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = result.Response;
                        ObjServiceResponseModel.Message = result.Message;
                        ObjServiceResponseModel.Data = result.Data;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }
        #endregion Miscellineous

        #region Image Upload Manager
        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date: 23-OCT-2018
        /// Created for :  To save Images from manager app// we using Imageupload wcf service for employee app and this api for
        /// Manager app because andriod developer have different ids for both app dont use same service.
        /// </summary>
        /// <param name="objServiceImageUpload"></param>
        /// <returns></returns>
        public IHttpActionResult ImageUploadForManager(ServiceImageUpload objServiceImageUpload)
        {
            var serviceresponse = new ServiceResponseModel<ServiceImageUpload>();
            ServiceImageUpload imgupload = new ServiceImageUpload();
            try
            {
                if (objServiceImageUpload != null
                    && objServiceImageUpload.Image != null
                    && objServiceImageUpload.UserId > 0
                    && objServiceImageUpload.Image.Trim() != ""
                    && objServiceImageUpload.ImageModuleName != null)
                {
                    string ImagePath = string.Empty;
                    string ImageUniqueName = string.Empty;
                    string ImageURL = string.Empty;
                    string url = "";
                    //Added By Bhushan Dod On 17-06-2015 for save image
                    #region For EmeregencyImage
                    if (objServiceImageUpload.ImageModuleName == "EmeregencyImage")
                    {
                        ImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["POEmeregencyImage"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmm") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId + "POEmeregencyImage";                        
                        url = HostingPrefix + EmeregencyImagePath.Replace("~", "") + ImageUniqueName+".jpg";
                    }
                    #endregion For EmeregencyImage

                    if (objServiceImageUpload.ImageModuleName == "BillImage")
                    {
                        ImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["BillImage"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmmsstt") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId + "BillImage";
                    }

                    if (objServiceImageUpload.ImageModuleName == "MiscellaneousImage")
                    {
                        ImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MiscellaneousImage"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmmsstt") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId + "MiscellaneousImage";
                        url = HostingPrefix + MiscellaneousImagePath.Replace("~", "") + ImageUniqueName + ".jpg";
                    }
                    ImageURL = ImageUniqueName + ".jpg";
                    // Code for to get path of root directory and attach path of directory to store image
                    //string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                    //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + WorkOrderImagePath;
                    if (!Directory.Exists(ImagePath))
                    {
                        Directory.CreateDirectory(ImagePath);
                    }
                     
                    var ImageLocation = ImagePath + ImageURL;
                    //Save the image to directory
                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(objServiceImageUpload.Image)))
                    {
                        using (Bitmap bm2 = new Bitmap(ms))
                        {
                            //bm2.Save("SavingPath" + "ImageName.jpg");
                            bm2.Save(ImageLocation);
                            imgupload.Image = ImageURL;
                            imgupload.ImageUrl = ImageLocation;
                        }
                    }
                    if (objServiceImageUpload.ImageModuleName == "EmeregencyImage" || objServiceImageUpload.ImageModuleName == "MiscellaneousImage")
                    {
                        imgupload.ImageUrl = url;
                    }
                    serviceresponse.Message = (imgupload.ImageUrl != "" && !string.IsNullOrEmpty(imgupload.ImageUrl)) ? CommonMessage.Successful() : CommonMessage.DoesNotExistsRecordMessage();
                    serviceresponse.Response = (imgupload.ImageUrl != "" && !string.IsNullOrEmpty(imgupload.ImageUrl)) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Data = imgupload;
                }
                else
                {
                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }
            return Ok(serviceresponse);
        }
        #endregion Image Upload Manager

        #region Email
        [HttpPost]
        public IHttpActionResult SendMailToAddNewBudget(AddBudgetMailModel Obj)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<AddBudgetMailModel>();
            var _POTypeDetails = new POTypeDetailsManager();
            try
            {
                if (Obj != null && Obj.CostCode > 0 && Obj.Amount > 0 && Obj.LocationId > 0 &&
                    Obj.UserId > 0 && Obj.VendorId > 0)
                {
                    var result = _POTypeDetails.SendMailToManagerForBudget(Obj.UserId, Obj.Amount, Obj.CostCode, Obj.LocationId, Obj.VendorId);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = result;
                        ObjServiceResponseModel.Data = null;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date  : 18-April-2019
        /// Created For : Send email and push to manager if checkout already checked by anyone 
        /// </summary>
        /// <param name="objServiceQrcModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CheckoutEmail(ServiceQrcModel objServiceQrcModel)
        {
            var ObjQRCSetupManager = new QRCSetupManager();
            var ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (objServiceQrcModel != null && objServiceQrcModel.ServiceAuthKey != null && objServiceQrcModel.LocationId > 0)
                {
                    bool result = ObjQRCSetupManager.SendCheckoutDetailsToManager(objServiceQrcModel);
                    ObjServiceResponseModel.Message = (result == true) ? CommonMessage.Successful() : CommonMessage.InvalidEntry();
                    ObjServiceResponseModel.Response = (result == true) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }

            return Ok(ObjServiceResponseModel);
        }
        #endregion Email

        #region New Employee App
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date: 15-Jan-2019
        /// Created For : To validate employee manager login
        /// </summary>
        /// <param name="objLogIn"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ValidateLoginForEmployee(eTracLoginModel objLogIn)
        {
            //ServiceResponseModel<eTracLoginModel> serviceresponse = new ServiceResponseModel<eTracLoginModel>();
            var ObjServiceResponseModel = new ServiceResponseModel<eTracLoginModel>();
            LoginManager _ILogin = new LoginManager();
            //long userType = 0;
            try
            {
                if (objLogIn != null && objLogIn.UserName != null && objLogIn.Password != null)
                {
                    //if (objLogIn.UserType > 0)
                    //{
                    //    userType = objLogIn.UserType;
                    //}
                    var result = _ILogin.AuthenticateUser(objLogIn);
                    //if (result.UserRoleId == userType)
                    if(result != null)
                    {
                        // This condition for invalid user
                        // Added By Bhushan Dod on Jan 12 2015
                        ObjServiceResponseModel.Message = (result != null && !string.IsNullOrEmpty(result.ResponseMessage)) ? result.ResponseMessage : CommonMessage.DoesNotExistsRecordMessage();
                        ObjServiceResponseModel.Response = (result != null) ? result.Response : 0;
                        ObjServiceResponseModel.Data = result;
                        return Ok(ObjServiceResponseModel);
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                        return Ok(ObjServiceResponseModel);
                    }
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 16-Jan-2019
        /// Created For : To forgot password of employee
        /// </summary>
        /// <param name="objETracLoginModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ForgotPasswordForEmployee(eTracLoginModel objETracLoginModel)
        {
            var serviceresponse = new ServiceResponseModel<string>();
            LoginManager _ILogin = new LoginManager();
            bool status = false;
            //status
            string message = "";
            try
            {
                if (objETracLoginModel != null && objETracLoginModel.RecoveryEmail != null && !string.IsNullOrEmpty(objETracLoginModel.RecoveryEmail))
                {
                    //status = _ILogin.RecoveryEmailPassword(eTracLogin, out message, out recoveryPassword);
                    status = _ILogin.RecoveryEmailPassword(objETracLoginModel, out message);
                    if (status) //ViewBag.ForgotPWDModalflag = true;
                    {
                        message = CommonMessage.RecoveryPasswordSent(objETracLoginModel.RecoveryEmail);
                        serviceresponse.Message = message;
                        serviceresponse.Response = (status != null) ? Convert.ToInt32(status) : Convert.ToInt32(ServiceResponse.FailedResponse);
                    }
                    else
                    {
                        serviceresponse.Message = CommonMessage.InvalidUser();
                        serviceresponse.Response = Convert.ToInt32(ServiceResponse.InvalidSessionResponse, CultureInfo.CurrentCulture);
                        serviceresponse.Data = null;
                    }
                }
                else
                {

                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
                return Ok(serviceresponse);
            }
            return Ok(serviceresponse);
        }

       
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-Jan-2019
        /// Created For : To logout employee manager by service auth key
        /// </summary>
        /// <param name="objLogOut"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult LogoutEmployeeManager(eTracLoginModel objLogOut)
        {
            var serviceresponse = new ServiceResponseModel<string>();
            LoginManager _ILogin = new LoginManager();
            try
            {
                if (objLogOut != null && objLogOut.ServiceAuthKey != null)
                {

                    eTracLoginModel result = _ILogin.Logout(objLogOut);

                    // This condition for invalid user
                    // Added By Bhushan Dod on Jan 12 2015                
                    serviceresponse.Message = (result != null && !string.IsNullOrEmpty(result.ResponseMessage)) ? result.ResponseMessage : CommonMessage.DoesNotExistsRecordMessage();
                    serviceresponse.Response = (result != null) ? result.Response : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Data = (result != null && !string.IsNullOrEmpty(result.ServiceAuthKey)) ? result.ServiceAuthKey : null;
                }
                else
                {

                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
                return Ok(serviceresponse);
            }
            return Ok(serviceresponse);
        }

        #region Work Request Assignment
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-Jan-2019
        /// Created For : To get  employee all task list 
        /// </summary>
        /// <param name="objEmpManagerAppModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetEmployeeTaskList(EmployeeManagerModel objEmpManagerAppModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<UnAssignedWorkOrderModel>>();
            var _INewEmployeeAppManager = new NewEmployeeAppManager();
            try
            {
                if (objEmpManagerAppModel != null && objEmpManagerAppModel.ServiceAuthKey != null &&
                    objEmpManagerAppModel.LocationId != null)
                {
                    var result = _INewEmployeeAppManager.TaskListForEmployee(objEmpManagerAppModel);
                    if (result.Count() >  0)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = result;                
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Dete : 13-June-2019
        /// Created For : To get all employee facility list showing to all employee.
        /// </summary>
        /// <param name="objEmpManagerAppModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetEmployeeFacilityTaskList(EmployeeManagerModel objEmpManagerAppModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<UnAssignedWorkOrderModel>>();
            var _INewEmployeeAppManager = new NewEmployeeAppManager();
            try
            {
                if (objEmpManagerAppModel != null && objEmpManagerAppModel.ServiceAuthKey != null &&
                    objEmpManagerAppModel.LocationId > 0)
                {
                    var result = _INewEmployeeAppManager.FacilityTaskListForEmployee(objEmpManagerAppModel);
                    if (result.Count() > 0)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = result;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }



        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 17-Jan-2019
        /// Created For : To save WO 
        /// </summary>
        /// <param name="objServiceWorkAssignmentModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveWorkOrderForEmployeeManager(ServiceWorkAssignmentModel objServiceWorkAssignmentModel)
        {
            var  objServiceResponse = new ServiceResponseModel<ServiceWorkAssignmentModel>();
            WorkRequestManager objWorkRequestManager = new WorkRequestManager();
            try
            {
                if (objServiceWorkAssignmentModel != null && objServiceWorkAssignmentModel.ServiceAuthKey != null && objServiceWorkAssignmentModel.RequestBy > 0)
                {

                    //Added By Ashwajit Bansod On 17-Jan-2019 for client want to add image in work order request
                    string WorkOrderImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["WorkRequestAssignmentPath"].ToString());
                    string ImageUniqueName = string.Empty;
                    string ImageURL = string.Empty;
                    if (objServiceWorkAssignmentModel.WorkRequestImage != null && objServiceWorkAssignmentModel.WorkRequestImage.Trim() != "")
                    {
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmm") + "EMSWorkOrder" + "_" + objServiceWorkAssignmentModel.RequestBy;
                        ImageURL = ImageUniqueName + ".jpg";

                        // Code for to get path of root directory and attach path of directory to store image
                        //string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                        //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + System.Configuration.ConfigurationManager.AppSettings.GetValues("WorkRequestAssignmentPath")[0];

                        if (!Directory.Exists(WorkOrderImagePath))
                        {
                            Directory.CreateDirectory(WorkOrderImagePath);
                        }
                        var ImageLocation = WorkOrderImagePath + ImageURL;
                        using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(objServiceWorkAssignmentModel.WorkRequestImage)))
                        {
                            using (Bitmap bm2 = new Bitmap(ms))
                            {
                                bm2.Save(ImageLocation);
                                objServiceWorkAssignmentModel.WorkRequestImage = ImageURL;

                            }
                        }
                    }
                    ServiceWorkAssignmentModel result = objWorkRequestManager.SaveWorkOrderRequest(objServiceWorkAssignmentModel);
                    objServiceResponse.Message = (result != null && !string.IsNullOrEmpty(result.ResponseMessage)) ? result.ResponseMessage : CommonMessage.DoesNotExistsRecordMessage();
                    objServiceResponse.Response = (result != null) ? result.Response : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    objServiceResponse.Data = result;
                }
                else
                {
                    objServiceResponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    objServiceResponse.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                objServiceResponse.Message = ex.Message;
                objServiceResponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                objServiceResponse.Data = null;
                return Ok(objServiceResponse);
            }

            return Ok(objServiceResponse);
        }        

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-Jan-2019
        /// Created For : To Update statis of task
        /// </summary>
        /// <param name="objWorkStatusModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateTaskStatusOfWOForEmployeeManager(ServiceWorkStatusModel objWorkStatusModel)
        {
            WorkRequestManager ObjWorkRequestManager = new WorkRequestManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                // var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == ObjWorkStatusModel.ServiceAuthKey && x.UserId == ObjWorkStatusModel.UserId);
                if (objWorkStatusModel.ServiceAuthKey != null && objWorkStatusModel.UserId > 0 && objWorkStatusModel.LocationID > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjWorkRequestManager.UpdateTaskStatus(objWorkStatusModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }

            return Ok(ObjServiceResponseModel);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : Aug-27-2018
        /// Created For: To fetc list of all continues request .
        /// </summary>
        /// <param name="objManagerAppModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ContinuesWorkOrderList(ManagerAppModel objManagerAppModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<UnAssignedWorkOrderModel>>();
            var _IManagerAppeMaintenanceManager = new ManagerAppeMaintenanceManager();
            try
            {
                if (objManagerAppModel != null && objManagerAppModel.ServiceAuthKey != null && objManagerAppModel.LocationId != null)
                {
                    var result = _IManagerAppeMaintenanceManager.AllContinuesWorkOrderList(objManagerAppModel);
                    if (result.Count() > 0)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = result;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 11-Feb-2019
        /// Created For  : To get all in progress continues task list for employee.
        /// </summary>
        /// <param name="objServiceDurationModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult EmployeeContinuousTaskList(ServiceDurationModel objServiceDurationModel)
        {
            var serviceresponse = new ServiceResponseModel<List<ServiceWorkAssignmentModel>>();
            LoginManager objLoginManager = new LoginManager();
            try
            {
                if (objServiceDurationModel != null && objServiceDurationModel.ServiceAuthKey != null && objServiceDurationModel.UserId > 0 && objServiceDurationModel.LocationId > 0)
                {
                    List<ServiceWorkAssignmentModel> result = objLoginManager.GetContinuousTaskListByEmployeeId(objServiceDurationModel.ServiceAuthKey, objServiceDurationModel.UserId, objServiceDurationModel.LocationId, objServiceDurationModel.TimeZoneName).ToList();
                    serviceresponse.Message = (result != null && result.Count > 0) ? CommonMessage.Successful() : CommonMessage.NoRecordMessage();
                    serviceresponse.Response = (result != null && result.Count > 0) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                    //serviceresponse.Data = result;                
                    serviceresponse.Data = result;
                }
                else
                {

                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                    serviceresponse.Data = null;
                }

            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
                return Ok(serviceresponse);
            }

            return Ok(serviceresponse);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 07-June-2019
        /// Created For : To get Continue WO Arrival notification as pe time
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CRAlertEmployee(ServiceWorkStatusModel obj)
        {
            WorkRequestManager objWorkRequestManager = new WorkRequestManager();
            var ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (obj.LocationID > 0 && obj.UserId > 0)
                {
                    bool result = objWorkRequestManager.CREmployeeAlert(obj.LocationID, obj.UserId, obj.TimeZoneName, obj.TimeZoneOffset, obj.IsTimeZoneinDaylight);

                    ObjServiceResponseModel.Message = (result == true) ? CommonMessage.Successful() : CommonMessage.NoRecordMessage();
                    ObjServiceResponseModel.Response = (result == true) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }

            return Ok(ObjServiceResponseModel);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To pause and resume the employee task
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult WorkOrderPauseResumeForEmployee(ServiceWorkOrderAcceptanceModel obj)
        {
            var objWorkRequestManager = new WorkRequestManager();
            var ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (obj.LocationID > 0 && obj.UserId > 0)
                {
                    bool result = objWorkRequestManager.WorkOrderPauseResume(obj);

                    ObjServiceResponseModel.Message = (result == true) ? CommonMessage.Successful() : CommonMessage.NoRecordMessage();
                    ObjServiceResponseModel.Response = (result == true) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);

                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.InnerException.ToString();
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }

            return Ok(ObjServiceResponseModel);
        }


        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 28-Jan-2019
        /// Created For : To accept urgent work request
        /// </summary>
        /// <param name="objServiceDARModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AcceptUrgentWorkRequest_EMP_MAN(ServiceDARModel objServiceDARModel)
        {
            ServiceResponseModel<ServiceWorkAssignmentModel> serviceresponse = new ServiceResponseModel<ServiceWorkAssignmentModel>();
            WorkRequestManager objWorkRequestManager = new WorkRequestManager();
            try
            {
                if (objServiceDARModel != null &&
                    objServiceDARModel.ServiceAuthKey != null &&
                    objServiceDARModel.UserName != null &&
                    objServiceDARModel.UserId > 0
                    )
                {
                    ServiceWorkAssignmentModel result = objWorkRequestManager.UrgentWOAccpetedByEmployee(objServiceDARModel);

                    //serviceresponse.Message = (result != null && !string.IsNullOrEmpty(result.ResponseMessage)) ? result.ResponseMessage : CommonMessage.DoesNotExistsRecordMessage();
                    //serviceresponse.Response = (result != null) ? result.Response : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = result.ResponseMessage;
                    serviceresponse.Response = result.Response;
                    serviceresponse.Data = result;
                }
                else
                {
                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
                return Ok(serviceresponse);
            }

            return Ok(serviceresponse);
        }

        #region Facility

        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 28-Jan-2019
        /// Created For : To Save Image of Facility request and convert it to PDF.
        /// </summary>
        /// <param name="objServiceImageUpload"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult FacilitySignatureUpload_EMP_MAN(ServiceImageUpload objServiceImageUpload)
        {
            var serviceresponse = new ServiceResponseModel<ServiceImageUpload>();
            var imgupload = new ServiceImageUpload();
            var objWorkRequestManager = new WorkRequestManager();
            var objData = new WorkRequestAssignment_M();
            var objM = new GlobalAdminManager();
            var _NewEmployeeAppManager = new NewEmployeeAppManager();
            bool st;
            try
            {
                if (objServiceImageUpload != null
                    && objServiceImageUpload.Image != null
                    && objServiceImageUpload.UserId > 0
                    && objServiceImageUpload.Image.Trim() != ""
                    && objServiceImageUpload.ImageModuleName != null
                    && objServiceImageUpload.ImageEmp != null
                    && objServiceImageUpload.ImageEmp.Trim() != ""
                    && objServiceImageUpload.ImageModuleNameEmp != null)
                {
                    string WorkOrderImagePath = string.Empty;
                    string ImageUniqueName = string.Empty;
                    string ImageURL = string.Empty;
                    string ImageUniqueNameEmp = string.Empty;
                    string ImageURLEmp = string.Empty;

                    //Added By Bhushan Dod On 17-04-2015 for For Facilty Request Disclaimer Signature
                    #region For Facilty Request Disclaimer Signature
                    if (objServiceImageUpload.ImageModuleName == "FacilityRequestSign")
                    {
                        WorkOrderImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["FRSignature"].ToString());
                        ImageUniqueName = objServiceImageUpload.WorkAssignmentId + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "_" + objServiceImageUpload.UserId;
                    }
                    #endregion For Facilty Request Disclaimer Signature

                    //Added By Bhushan Dod On 25-07-2015 for For Facilty Request Employee or Manager Signature
                    #region For Facilty Request Employee or Manager Signature
                    if (objServiceImageUpload.ImageModuleNameEmp == "FacilitySignEmp")
                    {
                        WorkOrderImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["FRSignature"].ToString());
                        ImageUniqueNameEmp = objServiceImageUpload.UserId + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "_" + objServiceImageUpload.WorkAssignmentId;
                    }
                    #endregion For Facilty Request Employee or Manager Signature
                    ImageURL = ImageUniqueName + ".jpg";
                    ImageURLEmp = ImageUniqueNameEmp + ".jpg";

                    // Code for to get path of root directory and attach path of directory to store image
                    //string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                    //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + WorkOrderImagePath;
                    if (!Directory.Exists(WorkOrderImagePath))
                    {
                        Directory.CreateDirectory(WorkOrderImagePath);
                    }
                    var ImageLocation = WorkOrderImagePath + ImageURL;
                    var ImageLocationEmp = WorkOrderImagePath + ImageURLEmp;

                    //Save the image to directory
                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(objServiceImageUpload.Image)))
                    {
                        using (Bitmap bm2 = new Bitmap(ms))
                        {
                            bm2.Save(ImageLocation);
                            imgupload.Image = ImageURL;
                            imgupload.ImageUrl = ImageLocation;
                        }
                    }
                    //Save the image to directory
                    using (MemoryStream ms1 = new MemoryStream(Convert.FromBase64String(objServiceImageUpload.ImageEmp)))
                    {
                        using (Bitmap bm21 = new Bitmap(ms1))
                        {
                            bm21.Save(ImageLocationEmp);

                            imgupload.ImageEmp = ImageURLEmp;
                            imgupload.ImageUrl = ImageLocationEmp;
                        }
                    }

                    //objData.StartTime, objData.EndTime, objData.AssignedToName);

                    // htmlData = htmlData.Replace("\r\n", "").Replace("\0", "");
                    var EncryptWorkId = Cryptography.GetEncryptedData(objServiceImageUpload.WorkAssignmentId.ToString(), true);
                    EncryptWorkId = EncryptWorkId.Replace(' ', '+').Replace('/', '@');//Here @ char replace due to '/' encrypt id it break the URL to open file
                    string filename = HttpContext.Current.Server.MapPath("~/Content/eMaintenance/DisclaimerDownload/" + EncryptWorkId + ".pdf");
                    // HttpContext.Current.Server.MapPath("~") + "/Content/eMaintenance/DisclaimerDownload/" + EncryptWorkId + ".pdf";
                    string waterMarkDisclaimerFilename = "eTrac" + EncryptWorkId + ".pdf";
                    st = objWorkRequestManager.WorkFrSignature(objServiceImageUpload.WorkAssignmentId, imgupload.Image, imgupload.ImageEmp, waterMarkDisclaimerFilename, "", "");
                    //Commented due to watermark. earlier we saved real file name but now we save watermark conversion file name. Because while adding watermark we need to delete disclaimer form and add dummy file with watermark.
                    //st = objWorkRequestManager.WorkFrSignature(objServiceImageUpload.WorkAssignmentId, imgupload.Image, imgupload.ImageEmp, EncryptWorkId + ".pdf", "");

                    if (st)
                    {
                        objData = objM.GetDataForRendringHTML(objServiceImageUpload.WorkAssignmentId);

                        //var data = _NewEmployeeAppManager.SaveData(objData, imgupload, objServiceImageUpload);
                        var htmlData = TemplateDesigner.eMaintenanceTemplate(objData.LicensePlateNo, objData.CustomerName, objData.Address, objData.CustomerContact,
                                                                            imgupload.Image, imgupload.ImageEmp, objData.CurrentLocation, objData.VehicleMake,
                                                                            objData.VehicleYear, objData.DriverLicenseNo, objData.VehicleModel, objData.FacilityRequestName,
                                                                            objServiceImageUpload.TimeZoneName, objServiceImageUpload.TimeZoneOffset, objServiceImageUpload.IsTimeZoneinDaylight);
                        //----------------------------
                        Document doc = new Document(PageSize.A4, 30f, 30f, 40f, 30f);
                        iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(filename, FileMode.CreateNew));
                        doc.Open();
                        try
                        {
                            //Logo
                            string imageURL = HttpContext.Current.Server.MapPath("~/Images/logo-etrac.png");
                            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                            jpg.Alignment = 3;
                            jpg.SpacingBefore = 30f;
                            jpg.ScaleToFit(100f, 80f);
                            jpg.SpacingBefore = 10f;
                            jpg.SpacingAfter = 1f;
                            doc.Add(jpg);

                            foreach (IElement element in HTMLWorker.ParseToList(
                            new StringReader(htmlData), null))
                            {
                                doc.Add(element);
                            }

                            PdfPTable table = new PdfPTable(2);

                            table.WidthPercentage = 96;
                            BaseFont bf = BaseFont.CreateFont(
                                        BaseFont.TIMES_ROMAN,
                                        BaseFont.CP1252,
                                        BaseFont.EMBEDDED);
                            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 11);

                            PdfPCell cell = new PdfPCell(new Phrase("Manager or Employee Name: " + objData.AssignedFirstName + ' ' + objData.AssignedLastName, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Drivers Licence Number: " + objData.DriverLicenseNo, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Customer Name: " + objData.CustomerName, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Customer Address: " + objData.Address, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Customer Telephone: " + objData.CustomerContact, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            imageURL = HttpContext.Current.Server.MapPath("~/Content/Images/FRSignature/" + imgupload.Image);
                            jpg = iTextSharp.text.Image.GetInstance(imageURL);
                            jpg.ScaleToFit(140f, 120f);
                            jpg.SpacingBefore = 10f;
                            jpg.SpacingAfter = 1f;

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                            cell.AddElement(jpg);
                            table.AddCell(cell);

                            imageURL = HttpContext.Current.Server.MapPath("~/Content/Images/FRSignature/" + imgupload.ImageEmp);
                            jpg = iTextSharp.text.Image.GetInstance(imageURL);
                            jpg.ScaleToFit(140f, 120f);
                            jpg.SpacingBefore = 10f;
                            jpg.SpacingAfter = 1f;

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                            cell.AddElement(jpg);
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(objData.CustomerName, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(objData.AssignedFirstName + ' ' + objData.AssignedLastName, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Signature of Customer", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Signature of Manager or Employee", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            doc.Add(table);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            doc.Close();
                            // Watermark code            
                            string watermarkLoc = System.Web.HttpContext.Current.Server.MapPath("~/Images/eTrac380-light.png");
                            PdfReader pdfReader = new PdfReader(filename);
                            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(filename.Replace(EncryptWorkId + ".pdf", waterMarkDisclaimerFilename), FileMode.Create));

                            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(watermarkLoc);
                            var pageSize = pdfReader.GetPageSizeWithRotation(1);

                            var x = pageSize.Width / 2 - img.ScaledWidth / 2;
                            var y = pageSize.Height / 2 - img.ScaledHeight / 2;
                            img.SetAbsolutePosition(x, y);
                            //img.ScaleToFit(100f,120f);
                            PdfContentByte waterMark;
                            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                            {
                                waterMark = pdfStamper.GetUnderContent(page);
                                waterMark.AddImage(img);
                            }
                            pdfStamper.FormFlattening = true;
                            pdfStamper.Close();
                            pdfReader.Close();
                            //delete old file. No more need of that file.
                            System.IO.File.Delete(filename);

                        }
                    }
                    serviceresponse.Message = (imgupload.ImageUrl != "" && !string.IsNullOrEmpty(imgupload.ImageUrl) && st == true) ? CommonMessage.Successful() : CommonMessage.FailureMessage();
                    serviceresponse.Response = (imgupload.ImageUrl != "" && !string.IsNullOrEmpty(imgupload.ImageUrl) && st == true) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Data = imgupload;
                }
                else if (objServiceImageUpload != null &&
                    objServiceImageUpload.IsDecline == true &&
                    objServiceImageUpload.UserId > 0 &&
                    objServiceImageUpload.WorkAssignmentId > 0)
                {
                    bool status = objWorkRequestManager.WorkFrIsDecline(objServiceImageUpload.WorkAssignmentId, objServiceImageUpload.UserId, objServiceImageUpload.IsDecline);
                    serviceresponse.Message = (status == true) ? CommonMessage.Successful() : CommonMessage.FailureMessage();
                    serviceresponse.Response = (status == true) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                }
                else
                {
                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                Exception_B.exceptionHandel_Runtime(ex, "public IHttpActionResult FacilitySignatureUpload_EMP_MAN(ServiceImageUpload objServiceImageUpload)", "objServiceImageUpload.UserId", objServiceImageUpload.UserId);
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
                return Ok(serviceresponse);
            }
            return Ok(serviceresponse);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 28-Jan-2019
        /// Created For : To send feedback mail to employee.
        /// </summary>
        /// <param name="objServiceFedbackModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult FeedbackSurvey_EMP_MAN(ServiceFedbackModel objServiceFedbackModel)
        {
            ServiceResponseModel<string> serviceresponse = new ServiceResponseModel<string>();
            WorkRequestManager objWorkRequestManager = new WorkRequestManager();
            try
            {
                if (objServiceFedbackModel != null && objServiceFedbackModel.Email != null && objServiceFedbackModel.UserId > 0)
                {
                    bool st = objWorkRequestManager.FeedbackEmailToEmployee(objServiceFedbackModel);
                    serviceresponse.Message = (st == true) ? CommonMessage.Successful() : CommonMessage.DoesNotExistsRecordMessage();
                    serviceresponse.Response = (st == true) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                }
                else
                {
                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
                return Ok(serviceresponse);
            }
            return Ok(serviceresponse);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 28-Jan-2019
        /// Created For : To start and stop request timer.
        /// </summary>
        /// <param name="objServiceWorkStatusModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult FacilityRequestTimer_EMP_MAN(ServiceWorkStatusModel objServiceWorkStatusModel)
        {
            ServiceResponseModel<string> serviceresponse = new ServiceResponseModel<string>();

            var objWork = new WorkRequestAssignmentModel();
            WorkRequestManager objWorkRequestManager = new WorkRequestManager();
            try
            {
                if (objServiceWorkStatusModel != null && objServiceWorkStatusModel.WorkRequestAssignmentID != null && objServiceWorkStatusModel.WorkRequestAssignmentID > 0)
                {
                    bool st = objWorkRequestManager.GetFacilityRequestByID(objServiceWorkStatusModel.WorkRequestAssignmentID, objServiceWorkStatusModel.LocationID, objServiceWorkStatusModel.UserId);
                    serviceresponse.Message = (st == true) ? CommonMessage.Successful() : CommonMessage.DoesNotExistsRecordMessage();
                    serviceresponse.Response = (st == true) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                }
                else
                {
                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
                return Ok(serviceresponse);
            }
            return Ok(serviceresponse);
        }
        #endregion Faciliity

        #endregion Work Request Assigment

        #region QRC Details
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-Jan-2019
        /// Created For : To get QRC details by QRCId
        /// </summary>
        /// <param name="objServiceQrcModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetQrcIdDetailsForEmployeeManager(ServiceQrcModel objServiceQrcModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<QRCModel> objServiceResponseModel = new ServiceResponseModel<QRCModel>();
            QRCModel ObjQRCModel = new QRCModel();
            try
            {

                if (objServiceQrcModel != null && objServiceQrcModel.ServiceAuthKey != null
                    && objServiceQrcModel.LocationId > 0 && objServiceQrcModel.QrcId > 0)
                {
                    objServiceResponseModel = ObjQRCSetupManager.GetQRCDetailsByID(objServiceQrcModel);
                }
                else
                {
                    objServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    objServiceResponseModel.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                objServiceResponseModel.Message = ex.Message;
                objServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                objServiceResponseModel.Data = null;
                return Ok(objServiceResponseModel);
            }
            return Ok(objServiceResponseModel);
        }

        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created For : To Save QRC Bathroom request 
        /// Created Date : 04-Jan-2019
        /// </summary>
        /// <param name="objServiceQrcBathroomModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult QrcBathroomRequestDetails_EMP_MAN(ServiceQrcBathroomModel objServiceQrcBathroomModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (objServiceQrcBathroomModel != null && objServiceQrcBathroomModel.ServiceAuthKey != null && objServiceQrcBathroomModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcBathroomRequestDetails(objServiceQrcBathroomModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }

            return Ok(ObjServiceResponseModel);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To save QRC bus station request
        /// Created Date : 04-Jan-2019
        /// </summary>
        /// <param name="objServiceQrcBusStationModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult QrcBusStationRequestDetails_EMP_MAN(ServiceQrcBusStationModel objServiceQrcBusStationModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (objServiceQrcBusStationModel != null && objServiceQrcBusStationModel.ServiceAuthKey != null && objServiceQrcBusStationModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcBusStationRequestDetails(objServiceQrcBusStationModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }

            return Ok(ObjServiceResponseModel);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 04-Jan-2019
        /// Created For : To save Cell Phone Qrc Request 
        /// </summary>
        /// <param name="objServiceQrcCellphoneModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult QrcCellphoneRequestDetails_EMP_MAN(ServiceQrcCellphoneModel objServiceQrcCellphoneModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (objServiceQrcCellphoneModel != null && objServiceQrcCellphoneModel.ServiceAuthKey != null && objServiceQrcCellphoneModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcCellPhoneRequestDetails(objServiceQrcCellphoneModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }

            return Ok(ObjServiceResponseModel);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 04-Jan-2019
        /// Created For : To save Elevator QRC request
        /// </summary>
        /// <param name="objServiceQrcElevatorModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult QrcElevatorRequestDetails_EMP_MAN(ServiceQrcElevatorModel objServiceQrcElevatorModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (objServiceQrcElevatorModel != null && objServiceQrcElevatorModel.ServiceAuthKey != null && objServiceQrcElevatorModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcElevatorRequestDetails(objServiceQrcElevatorModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
            return Ok(ObjServiceResponseModel);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 04-Jan-2019
        /// Created For : To save Emergency Phone contact QRC Request
        /// </summary>
        /// <param name="objServiceQrcPhoneSystemModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult QrcPhoneSystemRequestDetails_EMP_MAN(ServiceQrcPhoneSystemModel objServiceQrcPhoneSystemModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (objServiceQrcPhoneSystemModel != null && objServiceQrcPhoneSystemModel.ServiceAuthKey != null && objServiceQrcPhoneSystemModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcPhoneSystemsRequestDetails(objServiceQrcPhoneSystemModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }

            return Ok(ObjServiceResponseModel);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 04-Jan-2019
        /// Created For : To save Equipment QRC request 
        /// </summary>
        /// <param name="objServiceQrcEquipmentModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult QrcEquipmentRequestDetails_EMP_MAN(ServiceQrcEquipmentModel objServiceQrcEquipmentModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (objServiceQrcEquipmentModel != null && objServiceQrcEquipmentModel.ServiceAuthKey != null && objServiceQrcEquipmentModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcEquipmentRequestDetails(objServiceQrcEquipmentModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }

            return Ok(ObjServiceResponseModel);
        }

        /// <summary>
        /// Created By:Ashwajit Bansod
        /// Created Date : 04-Jan-2019
        /// Created For : To save escalator qrc details
        /// </summary>
        /// <param name="objServiceQrcEscalatorsModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult QrcEscalatorsRequestDetails_EMP_MAN(ServiceQrcEscalatorsModel objServiceQrcEscalatorsModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (objServiceQrcEscalatorsModel != null && objServiceQrcEscalatorsModel.ServiceAuthKey != null && objServiceQrcEscalatorsModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcEscalatorsRequestDetails(objServiceQrcEscalatorsModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }

            return Ok(ObjServiceResponseModel);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 04-jan-2019
        /// Created For : To save Gate arm QRC request 
        /// </summary>
        /// <param name="objServiceQrcGateArmModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult QrcGateArmRequestDetails_EMP_MAN(ServiceQrcGateArmModel objServiceQrcGateArmModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (objServiceQrcGateArmModel != null && objServiceQrcGateArmModel.ServiceAuthKey != null && objServiceQrcGateArmModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcGateArmRequestDetails(objServiceQrcGateArmModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
            return Ok(ObjServiceResponseModel);
        }

        /// <summary>
        /// Created BY : Ashwajit Bansod
        /// Created Date : 04-Jan-2019
        /// Created For : To save Moving walk way QRC request
        /// </summary>
        /// <param name="objServiceQrcMovingWalkwayModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult QrcMovingWalkwayRequestDetails_EMP_MAN(ServiceQrcMovingWalkwayModel objServiceQrcMovingWalkwayModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (objServiceQrcMovingWalkwayModel != null && objServiceQrcMovingWalkwayModel.ServiceAuthKey != null && objServiceQrcMovingWalkwayModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcMovingWalkwayRequestDetails(objServiceQrcMovingWalkwayModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
            return Ok(ObjServiceResponseModel);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 04-Jan-2019
        /// Created For : To save Parking QRC request details
        /// </summary>
        /// <param name="objServiceQrcParkingModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult QrcParkingRequestDetails_EMP_MAN(ServiceQrcParkingModel objServiceQrcParkingModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (objServiceQrcParkingModel != null && objServiceQrcParkingModel.ServiceAuthKey != null && objServiceQrcParkingModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcParkingRequestDetails(objServiceQrcParkingModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
            return Ok(ObjServiceResponseModel);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 04-Jan-2019
        /// Created For : 
        /// </summary>
        /// <param name="objServiceQrcShuttleBusModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult QrcShuttleRequestDetails_EMP_MAN(ServiceQrcShuttleBusModel objServiceQrcShuttleBusModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (objServiceQrcShuttleBusModel != null && objServiceQrcShuttleBusModel.ServiceAuthKey != null && objServiceQrcShuttleBusModel.UserId > 0)
                {
                    // Code for to get path of root directory and attach path of directory to store image
                    //string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                    //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + System.Configuration.ConfigurationManager.AppSettings.GetValues("QRCVehiclePath")[0];
                    //if (objServiceQrcVehicleModel.CheckingOut.IsDamage==true)
                    //{ 
                    //     objServiceQrcVehicleModel.CheckingOut.CroppedPicture = RootDirectory + objServiceQrcVehicleModel.CheckingOut.CroppedPicture;
                    //     objServiceQrcVehicleModel.CheckingOut.CapturedImage = RootDirectory + objServiceQrcVehicleModel.CheckingOut.CapturedImage;
                    //}
                    //if (objServiceQrcVehicleModel.VehicleCheck.IsDamage == true)
                    //{
                    //    objServiceQrcVehicleModel.VehicleCheck.CroppedPicture = RootDirectory + objServiceQrcVehicleModel.VehicleCheck.CroppedPicture;
                    //    objServiceQrcVehicleModel.VehicleCheck.CapturedImage = RootDirectory + objServiceQrcVehicleModel.VehicleCheck.CapturedImage;
                    //}

                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcShuttleRequestDetails(objServiceQrcShuttleBusModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
            return Ok(ObjServiceResponseModel);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 04-Jan-2019
        /// Created For : To save Ticket Spitter QRC request details
        /// </summary>
        /// <param name="objServiceQrcTicketSplitterModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult QrcTicketSplitterRequestDetails_EMP_MAN(ServiceQrcTicketSplitterModel objServiceQrcTicketSplitterModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (objServiceQrcTicketSplitterModel != null && objServiceQrcTicketSplitterModel.ServiceAuthKey != null && objServiceQrcTicketSplitterModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcTicketSpitterRequestDetails(objServiceQrcTicketSplitterModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
            return Ok(ObjServiceResponseModel);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 04-Jan-2019
        /// Created For : To save Trash can QRC request details
        /// </summary>
        /// <param name="objServiceQrcTrashcanModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult QrcTrashCanRequestDetails_EMP_MAN(ServiceQrcTrashcanModel objServiceQrcTrashcanModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (objServiceQrcTrashcanModel != null && objServiceQrcTrashcanModel.ServiceAuthKey != null && objServiceQrcTrashcanModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcTrashCanRequestDetails(objServiceQrcTrashcanModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
            return Ok(ObjServiceResponseModel);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To save Vehicle QRC request details
        /// Created Date : 04-Jan-2019
        /// </summary>
        /// <param name="objServiceQrcVehicleModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult QrcVehicleRequestDetails_EMP_MAN(ServiceQrcVehicleModel objServiceQrcVehicleModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (objServiceQrcVehicleModel != null && objServiceQrcVehicleModel.ServiceAuthKey != null && objServiceQrcVehicleModel.UserId > 0)
                {
                    // Code for to get path of root directory and attach path of directory to store image
                    //string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                    //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + System.Configuration.ConfigurationManager.AppSettings.GetValues("QRCVehiclePath")[0];
                    //if (objServiceQrcVehicleModel.CheckingOut.IsDamage==true)
                    //{ 
                    //     objServiceQrcVehicleModel.CheckingOut.CroppedPicture = RootDirectory + objServiceQrcVehicleModel.CheckingOut.CroppedPicture;
                    //     objServiceQrcVehicleModel.CheckingOut.CapturedImage = RootDirectory + objServiceQrcVehicleModel.CheckingOut.CapturedImage;
                    //}
                    //if (objServiceQrcVehicleModel.VehicleCheck.IsDamage == true)
                    //{
                    //    objServiceQrcVehicleModel.VehicleCheck.CroppedPicture = RootDirectory + objServiceQrcVehicleModel.VehicleCheck.CroppedPicture;
                    //    objServiceQrcVehicleModel.VehicleCheck.CapturedImage = RootDirectory + objServiceQrcVehicleModel.VehicleCheck.CapturedImage;
                    //}

                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcVehicleRequestDetails(objServiceQrcVehicleModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }

            return Ok(ObjServiceResponseModel);
        }

        #endregion QRC Details

        #region DAR
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 28-Jan-2019
        /// Created For : To save all dar details
        /// </summary>
        /// <param name="objServiceDARModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveDarDetailsFor_Emp_Man(ServiceDARModel objServiceDARModel)
        {
            var serviceresponse = new ServiceResponseModel<ServiceDARModel>();
            DARManager objDARManager = new DARManager();
            try
            {
                if (objServiceDARModel != null &&
                    objServiceDARModel.ServiceAuthKey != null &&
                    objServiceDARModel.UserName != null &&
                    objServiceDARModel.UserId > 0
                    )
                {
                    ServiceDARModel result = objDARManager.SaveDARDetails(objServiceDARModel);
                    serviceresponse.Message = (result != null && !string.IsNullOrEmpty(result.ResponseMessage)) ? result.ResponseMessage : CommonMessage.DoesNotExistsRecordMessage();
                    serviceresponse.Response = (result != null) ? result.Response : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Data = result;
                }
                else
                {
                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
                return Ok(serviceresponse);
            }

            return Ok(serviceresponse);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For :To Insert Jump start and GT tracker.
        /// Created Date : 03-Mar-2019
        /// </summary>
        /// <param name="objServiceDARModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult InsertDarDetailsTrackingForEmployee(ServiceDARModel objServiceDARModel)
        {
            var serviceresponse = new ServiceResponseModel<ServiceDARModel>();
            DARManager objDARManager = new DARManager();
            try
            {
                if (objServiceDARModel != null &&
                    objServiceDARModel.ServiceAuthKey != null &&
                    objServiceDARModel.UserName != null &&
                    objServiceDARModel.UserId > 0 &&
                    objServiceDARModel.StartTime != null
                    )
                {
                    ServiceDARModel result = objDARManager.SaveDARDetailsForTracking(objServiceDARModel);
                    serviceresponse.Message = (result != null && !string.IsNullOrEmpty(result.ResponseMessage)) ? result.ResponseMessage : CommonMessage.DoesNotExistsRecordMessage();
                    serviceresponse.Response = (result != null) ? result.Response : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Data = result;
                }
                else
                {
                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
                return Ok(serviceresponse);
            }
            return Ok(serviceresponse);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 03-Mar-2019
        /// Created For : Update the status of DAR jump start
        /// For Update Jump Start and GT Tracker
        /// </summary>
        /// <param name="objServiceDARModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateDarTaskStatusForEmployee(ServiceDARModel objServiceDARModel)
        {
            var ObjDARManager = new DARManager();
            var ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                // var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == ObjWorkStatusModel.ServiceAuthKey && x.UserId == ObjWorkStatusModel.UserId);
                if (objServiceDARModel.ServiceAuthKey != null && objServiceDARModel.UserId > 0)
                {
                    var ObjRespnse = ObjDARManager.UpdateDarTaskStatus(objServiceDARModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }

            return Ok(ObjServiceResponseModel);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 27-03-2019
        /// Created For : To save Desclaimer for data 
        /// </summary>
        /// <param name="objServiceDisclaimerModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DarDisclaimerFormFor_Emp_Man(ServiceDisclaimerModel objServiceDisclaimerModel)
        {
            var serviceresponse = new ServiceResponseModel<long>();
            var objServiceDARModel = new ServiceDARModel();
            var objDARManager = new DARManager();
            // WorkRequestAssignment_M objData = new WorkRequestAssignment_M();
            var objM = new GlobalAdminManager();
            try
            {
                if (objServiceDisclaimerModel != null
                    && objServiceDisclaimerModel.ImageCust != null
                    && objServiceDisclaimerModel.UserId > 0
                    && objServiceDisclaimerModel.ImageCust.Trim() != ""
                    && objServiceDisclaimerModel.ImageModuleNameCust != null
                    && objServiceDisclaimerModel.ImageEmp != null
                    && objServiceDisclaimerModel.ImageEmp.Trim() != ""
                    && objServiceDisclaimerModel.ImageModuleNameEmp != null)
                {
                    string DARImagePath = string.Empty;
                    string ImageUniqueNameCust = string.Empty;
                    string ImageURL = string.Empty;
                    string ImageUniqueNameEmp = string.Empty;
                    string ImageURLEmp = string.Empty;

                    //Added By Bhushan Dod On 25-05-2017 for DAR Employee Disclaimer Signature
                    #region For DAR Employee Disclaimer Signature
                    if (objServiceDisclaimerModel.ImageModuleNameEmp == "DarDisclaimerEmpSign")
                    {
                        DARImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DARSignature"].ToString());
                        ImageUniqueNameEmp = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + objServiceDisclaimerModel.UserId;
                    }
                    #endregion For DAR Employee Disclaimer Signature

                    //Added By Bhushan Dod On 25-07-2015 for DAR Customer Disclaimer Signature
                    #region For DAR Customer Disclaimer Signature
                    if (objServiceDisclaimerModel.ImageModuleNameCust == "DarDisclaimerCustomerSign")
                    {
                        DARImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DARSignature"].ToString());
                        ImageUniqueNameCust = objServiceDisclaimerModel.UserId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    }
                    #endregion For DAR Customer Disclaimer Signature
                    ImageURL = ImageUniqueNameCust + ".jpg";
                    ImageURLEmp = ImageUniqueNameEmp + ".jpg";

                    // Code for to get path of root directory and attach path of directory to store image
                    //string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                    //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + WorkOrderImagePath;
                    if (!Directory.Exists(DARImagePath))
                    {
                        Directory.CreateDirectory(DARImagePath);
                    }
                    var ImageLocationCust = DARImagePath + ImageURL;
                    var ImageLocationEmp = DARImagePath + ImageURLEmp;

                    //Save the image to directory
                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(objServiceDisclaimerModel.ImageCust)))
                    {
                        using (Bitmap bm2 = new Bitmap(ms))
                        {
                            bm2.Save(ImageLocationCust);
                            objServiceDisclaimerModel.ImageCust = ImageURL;
                            objServiceDisclaimerModel.ImageUrlCust = ImageLocationCust;
                        }
                    }
                    //Save the image to directory
                    using (MemoryStream ms1 = new MemoryStream(Convert.FromBase64String(objServiceDisclaimerModel.ImageEmp)))
                    {
                        using (Bitmap bm21 = new Bitmap(ms1))
                        {
                            bm21.Save(ImageLocationEmp);

                            objServiceDisclaimerModel.ImageEmp = ImageURLEmp;
                            objServiceDisclaimerModel.ImageUrlEmp = ImageLocationEmp;
                        }
                    }

                    string filename = HttpContext.Current.Server.MapPath("~/Content/DARDisclaimer/" + ImageUniqueNameCust + ".pdf");
                    objServiceDisclaimerModel.DisclaimerFormFile = "eTrac" + ImageUniqueNameCust + ".pdf";
                    var st = objDARManager.SaveDisclaimerDARDetails(objServiceDisclaimerModel);
                    //Commented due to watermark. earlier we saved real file name but now we save watermark conversion file name. Because while adding watermark we need to delete disclaimer form and add dummy file with watermark.
                    //st = objWorkRequestManager.WorkFrSignature(objServiceImageUpload.WorkAssignmentId, imgupload.Image, imgupload.ImageEmp, EncryptWorkId + ".pdf", "");

                    if (st.DARId > 0)
                    {
                        //if (objData.StartTime != null && objData.EndTime != null)
                        //{
                        //    imgupload.StartTime = objData.StartTime.Value.ToMobileClientTimeZone(true);
                        //    imgupload.EndTime = objData.EndTime.Value.ToMobileClientTimeZone(true);

                        //    TimeSpan ts = objData.EndTime.Value - objData.StartTime.Value;

                        //    imgupload.TotalTime = ts.Days + "Days:" + ts.Hours + "Hours:" + ts.Minutes + "Minutes";
                        //}
                        var htmlData = TemplateDesigner.eMaintenanceTemplate(objServiceDisclaimerModel.LicensePlateNo, objServiceDisclaimerModel.CustomerName, objServiceDisclaimerModel.Address, objServiceDisclaimerModel.CustomerContact,
                                                                            objServiceDisclaimerModel.ImageCust, objServiceDisclaimerModel.ImageEmp, objServiceDisclaimerModel.CurrentLocation, objServiceDisclaimerModel.VehicleMake,
                                                                            objServiceDisclaimerModel.VehicleYear, objServiceDisclaimerModel.DriverLicenseNo, objServiceDisclaimerModel.VehicleModel, objServiceDisclaimerModel.FacilityRequestName,
                                                                            objServiceDisclaimerModel.TimeZoneName, objServiceDisclaimerModel.TimeZoneOffset, objServiceDisclaimerModel.IsTimeZoneinDaylight);
                        //----------------------------
                        Document doc = new Document(PageSize.A4, 30f, 30f, 40f, 30f);
                        iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(filename, FileMode.CreateNew));
                        doc.Open();
                        try
                        {
                            //var content = writer.DirectContent;
                            //var pageBorderRect = new iTextSharp.text.Rectangle(doc.PageSize);
                            //pageBorderRect.Left += doc.LeftMargin;
                            //pageBorderRect.Right -= doc.RightMargin;
                            //pageBorderRect.Top -= doc.TopMargin;
                            //pageBorderRect.Bottom += doc.BottomMargin;
                            //content.SetColorStroke(BaseColor.BLACK);
                            //content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
                            //content.Stroke();

                            //Logo
                            string imageURL = HttpContext.Current.Server.MapPath("~/Images/logo-etrac.png");
                            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                            jpg.Alignment = 3;
                            jpg.SpacingBefore = 30f;
                            jpg.ScaleToFit(100f, 80f);
                            jpg.SpacingBefore = 10f;
                            jpg.SpacingAfter = 1f;
                            doc.Add(jpg);

                            foreach (IElement element in HTMLWorker.ParseToList(
                            new StringReader(htmlData), null))
                            {
                                doc.Add(element);
                            }

                            PdfPTable table = new PdfPTable(2);

                            table.WidthPercentage = 96;
                            BaseFont bf = BaseFont.CreateFont(
                                        BaseFont.TIMES_ROMAN,
                                        BaseFont.CP1252,
                                        BaseFont.EMBEDDED);
                            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 11);

                            //PdfPCell cell = new PdfPCell(new Phrase("Start Time: " + imgupload.StartTime, font));
                            //cell.Colspan = 1;
                            //cell.Border = 0;
                            //cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            //table.AddCell(cell);

                            //cell = new PdfPCell(new Phrase("End Time: " + imgupload.EndTime, font));
                            //cell.Colspan = 1;
                            //cell.Border = 0;
                            //cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            //table.AddCell(cell);

                            PdfPCell cell = new PdfPCell(new Phrase("Manager or Employee Name: " + objServiceDisclaimerModel.UserName, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Drivers Licence Number: " + objServiceDisclaimerModel.DriverLicenseNo, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Customer Name: " + objServiceDisclaimerModel.CustomerName, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Customer Address: " + objServiceDisclaimerModel.Address, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Customer Telephone: " + objServiceDisclaimerModel.CustomerContact, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            imageURL = HttpContext.Current.Server.MapPath("~/Content/Images/DARSignature/" + objServiceDisclaimerModel.ImageCust);
                            jpg = iTextSharp.text.Image.GetInstance(imageURL);
                            jpg.ScaleToFit(140f, 120f);
                            jpg.SpacingBefore = 10f;
                            jpg.SpacingAfter = 1f;

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                            cell.AddElement(jpg);
                            table.AddCell(cell);

                            imageURL = HttpContext.Current.Server.MapPath("~/Content/Images/DARSignature/" + objServiceDisclaimerModel.ImageEmp);
                            jpg = iTextSharp.text.Image.GetInstance(imageURL);
                            jpg.ScaleToFit(140f, 120f);
                            jpg.SpacingBefore = 10f;
                            jpg.SpacingAfter = 1f;

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                            cell.AddElement(jpg);
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(objServiceDisclaimerModel.CustomerName, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(objServiceDisclaimerModel.UserName, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Signature of Customer", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Signature of Manager or Employee", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            doc.Add(table);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            doc.Close();
                            // Watermark code            
                            string watermarkLoc = System.Web.HttpContext.Current.Server.MapPath("~/Images/eTrac380-light.png");
                            PdfReader pdfReader = new PdfReader(filename);
                            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(filename.Replace(ImageUniqueNameCust + ".pdf", objServiceDisclaimerModel.DisclaimerFormFile), FileMode.Create));

                            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(watermarkLoc);
                            var pageSize = pdfReader.GetPageSizeWithRotation(1);

                            var x = pageSize.Width / 2 - img.ScaledWidth / 2;
                            var y = pageSize.Height / 2 - img.ScaledHeight / 2;
                            img.SetAbsolutePosition(x, y);
                            //img.ScaleToFit(100f,120f);
                            PdfContentByte waterMark;
                            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                            {
                                waterMark = pdfStamper.GetUnderContent(page);
                                waterMark.AddImage(img);
                            }
                            pdfStamper.FormFlattening = true;
                            pdfStamper.Close();
                            pdfReader.Close();
                            //delete old file. No more need of that file.
                            System.IO.File.Delete(filename);
                        }
                    }
                    serviceresponse.Message = (objServiceDisclaimerModel.ImageUrlCust != "" && !string.IsNullOrEmpty(objServiceDisclaimerModel.ImageUrlCust) && st.DARId > 0) ? CommonMessage.Successful() : CommonMessage.FailureMessage();
                    serviceresponse.Response = (objServiceDisclaimerModel.ImageUrlCust != "" && !string.IsNullOrEmpty(objServiceDisclaimerModel.ImageUrlCust) && st.DARId > 0) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Data = objServiceDisclaimerModel.DARId;
                }
                else
                {
                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                Exception_B.exceptionHandel_Runtime(ex, "ServiceResponseModel<ServiceImageUpload> FacilitySignatureUpload(ServiceImageUpload objServiceImageUpload)", "objServiceImageUpload.UserId", objServiceDisclaimerModel.UserId);
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = 0;
            }
            return Ok(serviceresponse);
        }

        /// <summary>Update the status of disclaimer end time
        /// <CreatedFor>For Update disclaimer end time status</CreatedFor>
        /// <CreatedBy>Ashwajit Bansod</CreatedBy>
        /// <CreatedOn>Mar-27-2019</CreatedOn>
        /// </summary>
        /// <param name="objServiceDARModel"></param>
        /// <returns></returns> 
        [HttpPost]
        public IHttpActionResult UpdateDisclaimerEndTimeStatusFor_Emp_Man(ServiceDARModel objServiceDARModel)
        {
            var ObjDARManager = new DARManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                // var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == ObjWorkStatusModel.ServiceAuthKey && x.UserId == ObjWorkStatusModel.UserId);
                if (objServiceDARModel.LocationId > 0 && objServiceDARModel.ServiceAuthKey != null && objServiceDARModel.UserId > 0 && objServiceDARModel.DARId > 0)
                {
                    var result = ObjDARManager.UpdateEndTimeDAR(objServiceDARModel);

                    ObjServiceResponseModel.Response = result.Response;
                    ObjServiceResponseModel.Message = result.ResponseMessage;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }
            return Ok(ObjServiceResponseModel);
        }
        #endregion DAR

        #region Image Upload Employee Manager
        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date: 23-OCT-2018
        /// Created for :  To save Images from manager app// we using Imageupload wcf service for employee app and this api for
        /// Manager app because andriod developer have different ids for both app dont use same service.
        /// </summary>
        /// <param name="objServiceImageUpload"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ImageUploadForEMP_MAN(ServiceImageUpload objServiceImageUpload)
        {
            var serviceresponse = new ServiceResponseModel<ServiceImageUpload>();
            ServiceImageUpload imgupload = new ServiceImageUpload();
            try
            {
                if (objServiceImageUpload != null
                    && objServiceImageUpload.Image != null
                    && objServiceImageUpload.UserId > 0
                    && objServiceImageUpload.Image.Trim() != ""
                    && objServiceImageUpload.ImageModuleName != null)
                {
                    string ImagePath = string.Empty;
                    string ImageUniqueName = string.Empty;
                    string ImageURL = string.Empty;
                    string url = "";
                    //Added By Bhushan Dod On 17-06-2015 for save image
                    #region For QRCParkingFacility
                    if (objServiceImageUpload.ImageModuleName == "QRCParkingFacility")
                    {
                        ImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["QRCParkingFacilityPath"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmm") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId;
                    }
                    #endregion For QRCParkingFacility

                    //Added By Bhushan Dod On 20-02-2015 for save image
                    #region For QRCVehicleDamage
                    if (objServiceImageUpload.ImageModuleName == "QRCVehicleDamage")
                    {
                        ImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["QRCVehiclePath"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmmsstt") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId;
                    }
                    #endregion For QRCVehicleDamage

                    //Added By Bhushan Dod On 07-04-2015 for QRCVehicleEnforcement
                    #region For QRCVehicleEnforcement
                    if (objServiceImageUpload.ImageModuleName == "QRCVehicleEnforcement")
                    {
                        ImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["QRCVehicleEnforcementPath"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmm") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId;
                    }
                    #endregion For QRCVehicleEnforcement

                    //Added By Bhushan Dod On 17-04-2015 for DarImage
                    #region For DarImage
                    if (objServiceImageUpload.ImageModuleName == "DarImage")
                    {
                        ImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DarImagePath"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmm") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId;
                    }
                    #endregion For DarImage

                    //Added By Bhushan Dod On 17-04-2015 for For Facilty Request Disclaimer Signature
                    #region For Facilty Request Disclaimer Signature
                    if (objServiceImageUpload.ImageModuleName == "FacilityRequestSign")
                    {
                        ImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["FRSignature"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmm") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId;
                    }
                    #endregion For Facilty Request Disclaimer Signature

                    //Added By Bhushan Dod On 17-06-2015 for save image
                    #region For DAR Image
                    if (objServiceImageUpload.ImageModuleName == "DARImage")
                    {
                        ImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DarImagePath"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmm") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId;
                    }
                    #endregion For DAR Image

                    //Added By Bhushan Dod On 17-06-2015 for save image
                    #region For Rules Violation
                    if (objServiceImageUpload.ImageModuleName == "RulesViolation")
                    {
                        ImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["RulesViolationPath"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmm") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId;
                    }
                    #endregion For Rules Violation

                    //Added By Bhushan Dod On 08-05-2017 for saving image of Shuttle bus.
                    #region For QRC Shuttle Bus
                    if (objServiceImageUpload.ImageModuleName == "QRCShuttleBusDamage")
                    {
                        ImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["QRCVehiclePath"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmmsstt") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId + "Shuttle";
                    }
                    #endregion QRC Shuttle Bus

                    //Added By Bhushan Dod On 26-11-2017 for Save eFleet Fueling.
                    #region For Save eFleet Fueling
                    if (objServiceImageUpload.ImageModuleName == "eFleetFueling")
                    {
                        ImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["eFleetFueling"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmmsstt") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId + "eFleetFueling";
                    }
                    #endregion Save eFleet Fueling

                    //Added By Bhushan Dod On 26-11-2017 for Save eFleet Fueling.
                    #region For Inspection Doc
                    if (objServiceImageUpload.ImageModuleName == "InspectionDoc")
                    {
                        ImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["InspectionDocPath"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmmsstt") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId + "InspectionDoc";
                    }
                    #endregion Inspection Doc

                    //Added By Bhushan Dod On 28-11-2017 for Save eFleet Maintenance.
                    #region For Inspection Doc
                    if (objServiceImageUpload.ImageModuleName == "eFleetMaintenance")
                    {
                        ImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AttachmentOfMaintenance"].ToString() + "AttachmentMaintenance/");
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmmsstt") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId + "InspectionDoc";
                    }
                    #endregion Inspection Doc

                    //Added By Bhushan Dod On 29-11-2017 for Save eFleet Incident Image.
                    #region For Incident Image
                    if (objServiceImageUpload.ImageModuleName == "IncidentImage")
                    {
                        ImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["IncidentImage"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmmsstt") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId + "IncidentImage";
                    }
                    #endregion Incident Image
                    #region For DAR Image
                    if (objServiceImageUpload.ImageModuleName == "DARImage")
                    {
                        ImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DarImagePath"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmm") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId;
                    }
                    #endregion For DAR Image
                    #region For EmeregencyImage
                    if (objServiceImageUpload.ImageModuleName == "EmeregencyImage")
                    {
                        ImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["POEmeregencyImage"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmm") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId + "POEmeregencyImage";
                        url = HostingPrefix + EmeregencyImagePath.Replace("~", "") + ImageUniqueName + ".jpg";
                    }
                    #endregion For EmeregencyImage

                    if (objServiceImageUpload.ImageModuleName == "BillImage")
                    {
                        ImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["BillImage"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmmsstt") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId + "BillImage";
                    }

                    if (objServiceImageUpload.ImageModuleName == "MiscellaneousImage")
                    {
                        ImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MiscellaneousImage"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmmsstt") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId + "MiscellaneousImage";
                        url = HostingPrefix + MiscellaneousImagePath.Replace("~", "") + ImageUniqueName + ".jpg";
                    }
                    ImageURL = ImageUniqueName + ".jpg";
                    // Code for to get path of root directory and attach path of directory to store image
                    //string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                    //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + WorkOrderImagePath;
                    if (!Directory.Exists(ImagePath))
                    {
                        Directory.CreateDirectory(ImagePath);
                    }

                    var ImageLocation = ImagePath + ImageURL;
                    //Save the image to directory
                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(objServiceImageUpload.Image)))
                    {
                        using (Bitmap bm2 = new Bitmap(ms))
                        {
                            //bm2.Save("SavingPath" + "ImageName.jpg");
                            bm2.Save(ImageLocation);
                            imgupload.Image = ImageURL;
                            imgupload.ImageUrl = ImageLocation;
                        }
                    }
                    if (objServiceImageUpload.ImageModuleName == "EmeregencyImage" || objServiceImageUpload.ImageModuleName == "MiscellaneousImage")
                    {
                        imgupload.ImageUrl = url;
                    }
                    serviceresponse.Message = (imgupload.ImageUrl != "" && !string.IsNullOrEmpty(imgupload.ImageUrl)) ? CommonMessage.Successful() : CommonMessage.DoesNotExistsRecordMessage();
                    serviceresponse.Response = (imgupload.ImageUrl != "" && !string.IsNullOrEmpty(imgupload.ImageUrl)) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Data = imgupload;
                }
                else
                {
                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }
            return Ok(serviceresponse);
        }
        #endregion Image Upload Employee Manager

        #region Notification
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created date : 22-May-2019
        /// Created For : To make otification readable.
        /// </summary>
        /// <param name="ObjServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult NotificationRead(NotificationDetailModel ObjServiceBaseModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (ObjServiceBaseModel != null && ObjServiceBaseModel.UserId > 0)
                {
                    ObjServiceBaseModel.ApproveStatus = true;
                    var result = _ICommonMethod.UpdateNotificationDetail(ObjServiceBaseModel);
                    if (result == true)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.FailureMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 22-May-2019
        /// Created For : To get list of unseen notification
        /// </summary>
        /// <param name="ObjServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult NotificationUnseenList(NotificationDetailModel ObjServiceBaseModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<EmailHelper>>();
            try
            {
                if (ObjServiceBaseModel != null && ObjServiceBaseModel.UserId > 0)
                {
                    var result = _ICommonMethod.GetUnseenList(ObjServiceBaseModel);
                    if (result.Count() > 0)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = result;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                        ObjServiceResponseModel.Data = result;
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 13-Nov-2019
        /// Created For: To get WO Unseen WO list
        /// </summary>
        /// <param name="ObjServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult EmaintenanceUnseenList(NotificationDetailModel ObjServiceBaseModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<EmailHelper>>();
            try
            {
                if (ObjServiceBaseModel != null && ObjServiceBaseModel.UserId > 0)
                {
                    var result = _INotification.GetEmaintanaceUnseenList(ObjServiceBaseModel);
                    if (result.Count() > 0)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = result;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                        ObjServiceResponseModel.Data = result;
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }



        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 05-July-2019
        /// Created For : To get PO list created by user and self approve list bind in one
        /// </summary>
        /// <param name="ObjServiceBaseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult POListByUserId(eTracLoginModel obj)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<POListSelfServiceModel>>();
            try
            {
                if (obj != null && obj.ServiceAuthKey != null)
                {
                    var result = _ICommonMethod.GetPOList(obj);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = result.Response;
                        ObjServiceResponseModel.Message = result.Message;
                        ObjServiceResponseModel.Data = result.Data;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                        ObjServiceResponseModel.Data = null;
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }
        #endregion Notification

        #region ePeople
        [HttpPost]
        public IHttpActionResult SendUserInfo(eTracLoginModel obj)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<POListSelfServiceModel>>();
            try
            {
                if (obj != null && obj.ServiceAuthKey != null)
                {
                    var result = _ICommonMethod.GetPOList(obj);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = result.Response;
                        ObjServiceResponseModel.Message = result.Message;
                        ObjServiceResponseModel.Data = result.Data;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                        ObjServiceResponseModel.Data = null;
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created For : To get All forms details by Form name
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetFormInfo(eTracLoginModel obj)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<CommonFormModel>();
            var model = new CommonFormModel();
            try
            {
                if (obj != null && obj.ServiceAuthKey != null && obj.FormName != null && obj.UserId > 0)
                {
                    model.FormName = obj.FormName;
                    model.ServiceAuthKey = obj.ServiceAuthKey;
                    model.UserId = obj.UserId;
                    var result = _IFillableFormManager.GetFormDetails(model);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = result;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                        ObjServiceResponseModel.Data = null;
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 24-Oct-2019
        /// Created For : To get File type list 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetFileListWithFileType(eTracLoginModel obj)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<FormTypeListModel>>();
            var model = new CommonFormModel();
            try
            {
                if (obj != null && obj.ServiceAuthKey != null  && obj.UserId > 0)
                {
                    model.FormName = obj.FormName;
                    model.ServiceAuthKey = obj.ServiceAuthKey;
                    model.UserId = obj.UserId;
                    var result = _IFillableFormManager.GetFileList(obj);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = result;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                        ObjServiceResponseModel.Data = null;
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Creted By : Ashwajit Bansod
        /// Created For : To save files
        /// Created Date : -2-Nov-2019
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveForm(CommonFormModel obj)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<string>();
            var model = new CommonFormModel();
            try
            {
                if (obj != null && obj.ServiceAuthKey != null && obj.UserId > 0)
                {
                    model.FormName = obj.FormName;
                    model.ServiceAuthKey = obj.ServiceAuthKey;
                    model.UserId = obj.UserId;
                    var result = _IFillableFormManager.SaveFileList(obj);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        //ObjServiceResponseModel.Data = result;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                        ObjServiceResponseModel.Data = null;
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = -1;
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }
        #endregion ePeople
        #endregion New Employee App
    }
}