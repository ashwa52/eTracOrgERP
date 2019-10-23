using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces.eFleet;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel.ApiModel;

namespace WorkOrderEMS.BusinessLogic.Managers.eFleet
{
    public class HoursOfServicesManager : IHoursOfServices
    {
        /// <summary>
        /// Created By: Ashwajit Bansod
        /// Created Date : Oct-25-2017
        /// Created for : Saving Hours of services
        /// </summary>
        /// <param name="objModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> InsertHoursOfServices(HoursOfServicesModel objModel)
        {
            var objReturnModel = new ServiceResponseModel<string>();
            try
            {               
                var objHoursOfServicesRepository = new HoursOfServicesRepository();
                TimeSpan AddHour = TimeSpan.Zero;
                TimeSpan EightHour = TimeSpan.Zero;
                TimeSpan eightHourDiff = TimeSpan.Zero;
                TimeSpan ts = TimeSpan.Zero;
                TimeSpan PrevEightHour = TimeSpan.Zero;
                TimeSpan prevEightHourDiff = TimeSpan.Zero;
                DateTime lastReocrds = DateTime.UtcNow.AddHours(-30);
                //ts = Convert.ToDateTime(objModel.EndTime) - Convert.ToDateTime(objModel.StartTime);
                //ts.Duration();
                if (objModel.StartDate != objModel.EndDate)
                {
                    ts = Convert.ToDateTime(objModel.StartTime) - Convert.ToDateTime(objModel.EndTime);
                    var convertString = ts.ToString().Replace("-", "");
                    ts = Convert.ToDateTime(convertString).TimeOfDay;
                }
                else
                {
                    
                    ts = Convert.ToDateTime(objModel.EndTime) - Convert.ToDateTime(objModel.StartTime);
                    var convertString = ts.ToString().Replace("-", "");
                    ts = Convert.ToDateTime(convertString).TimeOfDay;
                }
                var lastRecord = objHoursOfServicesRepository.GetAll(pm => pm.IsDeleted == false
                                                                        && pm.CreatedDate >= lastReocrds
                                                                        && pm.CreatedBy == objModel.UserId).OrderByDescending(p => p.HoursID).FirstOrDefault();
                HoursOfService Obj = new HoursOfService();
                //AutoMapper.Mapper.CreateMap<HoursOfServicesModel, HoursOfService>();
                //Obj = AutoMapper.Mapper.Map(objModel, Obj);
                Obj.StartTime = Convert.ToDateTime(objModel.StartTime).TimeOfDay;
                Obj.EndTime = Convert.ToDateTime(objModel.EndTime).TimeOfDay;
                Obj.StartDate = Convert.ToDateTime(objModel.StartDate).Date;
                Obj.EndDate = Convert.ToDateTime(objModel.EndDate).Date;
                Obj.CreatedBy = objModel.UserId;
                Obj.CreatedDate = DateTime.UtcNow;
                Obj.IsDeleted = false;
                Obj.LocationID = objModel.LocationID;
                objHoursOfServicesRepository.Add(Obj);
                if (Obj.HoursID > 0)
                {
                    if (ts.TotalHours >= 10)
                    {
                        objReturnModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.InvariantCulture);
                        objReturnModel.Message = CommonMessage.HOSEmployeeNotAbleToWork();
                    }
                    else
                    {
                        var Data = objHoursOfServicesRepository.GetAll(pm => pm.IsDeleted == false
                                                                        && pm.CreatedDate >= lastReocrds 
                                                                        && pm.CreatedBy == objModel.UserId).ToList();
                        foreach (var item in Data)
                        {
                            int i = 1;
                            
                            if (Data.Count == i + 1)
                            {
                                PrevEightHour = prevEightHourDiff - item.StartTime;
                                i++;
                            }
                            prevEightHourDiff = item.EndTime;
                            if (PrevEightHour.TotalHours <= 8)
                            {
                                TimeSpan prevTimeDiff = item.EndTime - item.StartTime;
                                AddHour += prevTimeDiff;
                            }
                            else
                            {
                                AddHour = TimeSpan.Zero;
                                TimeSpan prevTimeDiff = item.EndTime - item.StartTime;
                                AddHour += prevTimeDiff;
                            }
                        }
                        //For Calculating 8 Hour Difference of Last record in databas with the start time
                        if (lastRecord != null)
                        {
                            eightHourDiff = Convert.ToDateTime(objModel.StartTime).TimeOfDay - lastRecord.EndTime;
                            var convertString = eightHourDiff.ToString().Replace("-", "");
                            eightHourDiff = Convert.ToDateTime(convertString).TimeOfDay;
                            EightHour = eightHourDiff;
                        }
                        if (EightHour.TotalHours >= 8)
                        {
                            objReturnModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.InvariantCulture);
                            objReturnModel.Message = CommonMessage.Successful();
                        }
                        else if (AddHour.TotalHours >= 10)
                        {
                            objReturnModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.InvariantCulture);
                            objReturnModel.Message = CommonMessage.HOSEmployeeNotAbleToWork();
                        }
                        else
                        {
                           objReturnModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.InvariantCulture);
                           objReturnModel.Message = CommonMessage.Successful();
                        }                        
                    }
                }
                else
                {
                    objReturnModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.InvariantCulture);
                    objReturnModel.Message = CommonMessage.NoRecordMessage();
                }                                
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceResponseModel<string> InsertHoursOfServices(HoursOfServicesModel objModel)", "Exception while insert Hours Of Services", objModel);
                objReturnModel.Message = ex.Message;
                objReturnModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                objReturnModel.Data = null;
            }
            return objReturnModel;
        }
    }
}
