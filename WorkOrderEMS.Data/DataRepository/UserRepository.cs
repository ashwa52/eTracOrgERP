﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.NewAdminModel;
using WorkOrderEMS.Models.UserModels;


namespace WorkOrderEMS.Data
{
    public class UserRepository : BaseRepository<UserRegistration>, IUserRepository
    {
        workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();

        private readonly string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private readonly string ProfilePicPath = System.Configuration.ConfigurationManager.AppSettings["ProfilePicPath"];
        private readonly string ConstantImages = ConfigurationManager.AppSettings["ConstantImages"];

        public UserModel GetUserById(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter paramTotalRecords)
        {
            UserModel objUserModel = new UserModel();
            var data = _workorderEMSEntities.SP_GetUser(userId, operationName, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, paramTotalRecords);
            foreach (var item in data)
            {
                objUserModel.UserId = item.UserId;
                objUserModel.UserType = item.UserType;
                objUserModel.FirstName = item.FirstName;
                objUserModel.LastName = item.LastName;
                objUserModel.UserEmail = item.UserEmail;
                objUserModel.AlternateEmail = item.AlternateEmail;
                objUserModel.DOB = item.DOB.ToString("MM/dd/yy");
                objUserModel.BloodGroup = item.BloodGroup;
                //objUserModel.Gender = item.GenderName != "" ? (item.GenderName == "Male" ? 1 : 2) : 0; // previously written.
                objUserModel.Gender = item.GenderName != "" ? (item.GenderName == "Male" ? 9 : 10) : 9; // added by vijay sahu .
                objUserModel.myProfileImage = item.ProfileImage;
                objUserModel.Password = item.Password;
                objUserModel.Address = new AddressModel();
                objUserModel.Address.AddressMasterId = Convert.ToInt32(item.AddressMasterId);
                objUserModel.Address.Address1 = item.Address1;
                objUserModel.Address.Address2 = item.Address2;
                objUserModel.Address.CountryId = item.CountryId;
                objUserModel.Address.StateId = item.StateId;
                objUserModel.Address.City = item.City;
                objUserModel.Address.ZipCode = item.ZipCode;
                objUserModel.Address.Mobile = item.Mobile;
                objUserModel.Address.Phone = item.Phone;
                objUserModel.EmployeeID = item.EmployeeID;
                objUserModel.JobTitle = item.JobTitle;
                objUserModel.JobTitleOther = item.JobTitleOther;
                objUserModel.SignatureImageBase = item.UserSignature;
                objUserModel.PaymentMode = item.PaymentMode;
                objUserModel.PaymentTerm = item.PaymentTerm;
            }
            return objUserModel;
        }

        public List<UserModelList> GetAllVerfiedUser(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter paramTotalRecords)
        {
            List<UserModelList> lstVerifiedMnagaer = new List<UserModelList>();
            try
            {
                int ss = _workorderEMSEntities.SP_GetAllVerifiedUser(userId, operationName, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, paramTotalRecords);
                //lstVerifiedMnagaer = _workorderEMSEntities.SP_GetAllVerifiedUser(userId, operationName, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, paramTotalRecords).Select(t =>
                //    new UserModelList()
                //    {
                //        UserId = t.UserId,
                //        ProjectID = t.ProjectID,
                //        ProjectName = t.ProjectName,
                //        UserEmail = t.UserEmail,
                //        DOB = t.DOB,
                //        Name = t.Name,
                //        HiringDate = t.HiringDate,
                //        EmployeeCategoryid = t.EmployeeCategory,
                //        EmployeeProfile = t.EmployeeProfile,
                //        EmployeeID = t.EmployeeID,
                //        UserType = t.UserType
                //    }).ToList();
                return lstVerifiedMnagaer;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>GetAllVerfiedUsers
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedFor>Get All IT Administrator List</CreatedFor>
        /// <CreatedOn>Nov-14-2014</CreatedOn>
        /// <param name="UserID"></param>
        /// <param name="OperationName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<UserModelList> GetAllVerfiedUsers(long? userId, long locationId, string useType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords)
        {
            //totalRecords = 0;
            ObjectParameter totalRecord = new ObjectParameter("TotalRecords", typeof(int));

            List<UserModelList> lstVerifiedMnagaer = new List<UserModelList>();
            try
            {
                lstVerifiedMnagaer = _workorderEMSEntities.SP_GetAllActiveUser(userId, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, locationId, useType, totalRecord).Select(t =>
                    new UserModelList()
                    {
                        UserId = t.UserId,
                        //ProjectID = t.ProjectID,
                        //ProjectName = t.ProjectName,
                        UserEmail = t.UserEmail,
                        DOB = t.DOB,
                        Name = t.Name,
                        HiringDate = t.HiringDate,
                        EmployeeProfile = t.EmployeeProfile,
                        UserType = t.UserType,
                        CodeName = t.CodeName,
                        ProfileImage = t.ProfileImage,
                        QRCID = t.QRCID,
                    }).ToList();
                totalRecords = Convert.ToInt32(totalRecord.Value);
                return lstVerifiedMnagaer;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>GetAllVerfiedUsers
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedFor>Get All IT Administrator List</CreatedFor>
        /// <CreatedOn>Nov-14-2014</CreatedOn>
        /// <param name="UserID"></param>
        /// <param name="OperationName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<UserModelList> GetAllVerfiedUsersForReport(long? userId, long locationId, string useType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords)
        {
            //totalRecords = 0;
            ObjectParameter totalRecord = new ObjectParameter("TotalRecords", typeof(int));

            List<UserModelList> lstVerifiedMnagaer = new List<UserModelList>();
            try
            {
                lstVerifiedMnagaer = _workorderEMSEntities.SP_GetAllActiveUserForReport(userId, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, locationId, useType, totalRecord).Select(t =>
                    new UserModelList()
                    {
                        UserId = t.UserId,
                        //ProjectID = t.ProjectID,
                        //ProjectName = t.ProjectName,
                        UserEmail = t.UserEmail,
                        DOB = t.DOB,
                        Name = t.Name + " (" + t.EmployeeID + ")",
                        HiringDate = t.HiringDate,
                        EmployeeProfile = t.EmployeeProfile,
                        UserType = t.UserType,
                        CodeName = t.CodeName,
                        ProfileImage = t.ProfileImage,
                        QRCID = t.QRCID,

                    }).ToList();
                totalRecords = Convert.ToInt32(totalRecord.Value);
                return lstVerifiedMnagaer;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>GetAllVerfiedUsersinDAR
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedFor>Get all user list for DAR</CreatedFor>
        /// <CreatedOn>June-26-2017</CreatedOn>
        /// <param name="UserID"></param>
        /// <param name="OperationName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<UserModelList> GetAllVerfiedUsersDAROnly(long? userId, long locationId, string useType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords)
        {
            //totalRecords = 0;
            ObjectParameter totalRecord = new ObjectParameter("TotalRecords", typeof(int));

            List<UserModelList> lstVerifiedMnagaer = new List<UserModelList>();
            try
            {
                lstVerifiedMnagaer = _workorderEMSEntities.SP_GetAllActiveUserForDAR(userId, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, locationId, useType, totalRecord).Select(t =>
                    new UserModelList()
                    {
                        UserId = t.UserId,
                        //ProjectID = t.ProjectID,
                        //ProjectName = t.ProjectName,
                        UserEmail = t.UserEmail,
                        DOB = t.DOB,
                        Name = t.Name,
                        HiringDate = t.HiringDate,
                        EmployeeProfile = t.EmployeeProfile,
                        UserType = t.UserType,
                        CodeName = t.CodeName,
                        ProfileImage = t.ProfileImage,
                        QRCID = t.QRCID,
                    }).ToList();
                totalRecords = Convert.ToInt32(totalRecord.Value);
                return lstVerifiedMnagaer;
            }
            catch (Exception)
            { throw; }
        }


        //public List<UserModelList> GetLocationListAdministrator(string LocationId, UserType UserType = UserType.Administrator)
        //{
        //    List<UserModelList> lstVerifiedMnagaer = new List<UserModelList>();
        //    try
        //    {
        //        lstVerifiedMnagaer = _workorderEMSEntities.(UserID, UseType, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, paramTotalRecords).Select(t =>
        //        return lstVerifiedMnagaer;
        //    }
        //    catch (Exception ex)
        //    {throw ex;}
        //        }
        //}

        /// <summary>ListLocationAdministrator
        /// <CreatedFor>ListLocationAdministrator</CreatedFor>
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Dec-10-2014</CreatedOn>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public List<UserModelList> ListLocationAdministrator(long locationId, string UserType = "")
        {
            try
            {
                _workorderEMSEntities = new workorderEMSEntities();
                return _workorderEMSEntities.fnListLocationAdministrator(locationId, Convert.ToInt32(UserType)).Select(t =>
                                    new UserModelList()
                                    {
                                        UserId = t.UserId,
                                        UserEmail = t.UserEmail,
                                        Name = t.Name,
                                        DOB = t.DOB,
                                        JobTitle = t.JobTitle,
                                        ProfileImage = t.ProfileImage,
                                        //UserType = t.UserType
                                    }).ToList();
            }
            catch (Exception)
            { throw; }

        }

        /// <summary>
        /// Created by vijay sahu on 10 march 2015 where We fatching location based on his userTYpe means 
        /// all manager of given locationid, or we can say all admin user of given location.
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        public List<AdminUserForDrop> UnAssignedAdministrationIdRepo(long LocationId, string UserType)
        {

            List<AdminUserForDrop> list = new List<AdminUserForDrop>();
            try
            {
                using (workorderEMSEntities objContext = new workorderEMSEntities())
                {
                    long userTy = Convert.ToInt32(UserType);



                    if (userTy == 6) // 6 means admin user
                    {

                        //list = (from UR in objContext.UserRegistrations
                        //        join ADL in objContext.AdminLocationMappings
                        //            on UR.UserId equals ADL.AdminUserId
                        //        where UR.UserType == userTy
                        //        && ADL.LocationId != LocationId
                        //        && UR.IsLoginActive == true
                        //        && UR.IsEmailVerify == true
                        //        && UR.IsDeleted == false
                        //        && ADL.IsDeleted == false
                        //        select new AdminUserForDrop()
                        //        {
                        //            UserId = UR.UserId,
                        //            Name = UR.FirstName + " " + UR.LastName,
                        //            UserEmail = UR.UserEmail
                        //        }).ToList();


                        list = (from UR in objContext.UserRegistrations

                                where UR.UserType == userTy
                                && UR.UserId != ((from ad in objContext.AdminLocationMappings where ad.LocationId == LocationId && ad.IsDeleted == false select ad.AdminUserId).FirstOrDefault())
                                //&& ADL.LocationId != LocationId
                                && UR.IsLoginActive == true
                                && UR.IsEmailVerify == true
                                && UR.IsDeleted == false
                                //&& ADL.IsDeleted == false
                                select new AdminUserForDrop()
                                {
                                    UserId = UR.UserId,
                                    Name = UR.FirstName + " " + UR.LastName,
                                    UserEmail = UR.UserEmail
                                }).ToList();


                    }
                    else if (userTy == 2)// means manager user.
                    {
                        //list = (from UR in objContext.UserRegistrations
                        //        join ADL in objContext.ManagerLocationMappings
                        //            on UR.UserId equals ADL.ManagerUserId
                        //        where UR.UserType == userTy
                        //        && ADL.LocationId != LocationId
                        //        && UR.IsLoginActive == true
                        //        && UR.IsEmailVerify == true
                        //        && UR.IsDeleted == false
                        //        && ADL.IsDeleted == false
                        //        select new AdminUserForDrop()
                        //        {
                        //            UserId = UR.UserId,
                        //            Name = UR.FirstName + " " + UR.LastName,
                        //            UserEmail = UR.UserEmail
                        //        }).Distinct().ToList();


                        list = (from UR in objContext.UserRegistrations

                                where UR.UserType == userTy
                                && UR.UserId != ((from m in objContext.ManagerLocationMappings where m.LocationId == LocationId && m.IsDeleted == false select m.ManagerUserId).FirstOrDefault())
                                //&& ADL.LocationId != LocationId
                                && UR.IsLoginActive == true
                                    && UR.IsEmailVerify == true
                                && UR.IsDeleted == false

                                select new AdminUserForDrop()
                                {
                                    UserId = UR.UserId,
                                    Name = UR.FirstName + " " + UR.LastName,
                                    UserEmail = UR.UserEmail
                                }).ToList();
                    }
                    else if (userTy == 3)// means Employee user.
                    {

                        list = (from UR in objContext.UserRegistrations

                                where UR.UserType == userTy
                                && UR.UserId != ((from m in objContext.EmployeeLocationMappings where m.LocationId == LocationId && m.IsDeleted == false select m.EmployeeUserId).FirstOrDefault())
                                //&& ADL.LocationId != LocationId
                                && UR.IsLoginActive == true
                                && UR.IsEmailVerify == true
                                && UR.IsDeleted == false
                                //&& ADL.IsDeleted == false
                                select new AdminUserForDrop()
                                {
                                    UserId = UR.UserId,
                                    Name = UR.FirstName + " " + UR.LastName,
                                    UserEmail = UR.UserEmail
                                }).ToList();
                    }

                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return list;
        }

        public List<UserModelList> GetManagerByAdminId(long AdminId)
        {
            List<UserModelList> lstManager = new List<UserModelList>();
            try
            {
                lstManager = (from t in _workorderEMSEntities.UserRegistrations
                              join ae in _workorderEMSEntities.AdminEmployeeMappings on t.UserId equals ae.EmployeeUserId
                              where ae.AdminUserId == AdminId
                              select t).ToList().Select(c => new UserModelList()
                              {
                                  UserId = c.UserId,
                                  Name = c.FirstName + " " + c.LastName
                              }).ToList();
                return lstManager;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public eTracLoginModel GetLocationDetailsByUserID(long userId)
        {

            try
            {
                eTracLoginModel locDetails = (from ur in _workorderEMSEntities.UserRegistrations
                                              join elm in _workorderEMSEntities.EmployeeLocationMappings on ur.UserId equals elm.EmployeeUserId
                                              join lm in _workorderEMSEntities.LocationMasters on elm.LocationId equals lm.LocationId
                                              where ur.UserId == userId && ur.IsDeleted == false
                                              select new eTracLoginModel { LocationID = lm.LocationId, Location = lm.LocationName }).FirstOrDefault();

                return locDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public UserRegistration AuthenticateLogin(eTracLoginModel eTracLogin)
        //{
        //    _workorderEMSEntities = new workorderEMSEntities();
        //    GetSingleOrDefault(x => x.UserEmail == loginViewModel.Email && x.Password == mypassword && x.IsDeleted == false && x.IsEmailVerify == true && x.IsLoginActive == true);
        //    //UserModel User = (from mlm in _workorderEMSEntities.userLocationMappings
        //    //                  join ur in objworkorderEMSEntities.UserRegistrations on mlm.ManagerUserId equals ur.UserId
        //    //                  where mlm.LocationId == locationId
        //    //                  select ur).Select(u => new UserModel()
        //    //                               {
        //    //                                   UserId = u.UserId,
        //    //                                   FirstName = !string.IsNullOrEmpty(u.LastName) ? u.FirstName + ' ' + u.LastName : u.FirstName,
        //    //                                   UserEmail = u.UserEmail,
        //    //                                   UserType = u.UserType
        //    //                               }).ToList();
        //}
        /// <summary>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedDate>12-Feb-2015</CreatedDate>
        /// <Description>From android to save new client registration </Description>
        /// </summary>
        /// <param name="ObjUserModel"></param>
        /// <returns></returns>
        public bool SaveNewClientRegistration(UserModel ObjUserModel)
        {
            bool flag = false;
            UserRegistration ObjUserRegistration = new UserRegistration();
            try
            {
                ObjUserRegistration.UserEmail = ObjUserModel.UserEmail;
                ObjUserRegistration.AlternateEmail = ObjUserModel.AlternateEmail;
                ObjUserRegistration.SubscriptionEmail = ObjUserModel.UserEmail;
                ObjUserRegistration.CreatedBy = ObjUserModel.UserId;
                ObjUserRegistration.CreatedDate = DateTime.UtcNow;
                ObjUserRegistration.DeletedBy = null;
                ObjUserRegistration.DeletedDate = null;
                ObjUserRegistration.IsDeleted = false;
                ObjUserRegistration.UserType = ObjUserModel.UserType;
                ObjUserRegistration.ModifiedBy = null;
                ObjUserRegistration.ModifiedDate = null;
                //ObjUserRegistration.LocationClientMappings = ObjUserModel.Location;
                ObjUserRegistration.FirstName = ObjUserModel.FirstName;
                ObjUserRegistration.LastName = ObjUserModel.LastName;
                ObjUserRegistration.Gender = ObjUserModel.Gender;
                ObjUserRegistration.DOB = Convert.ToDateTime(ObjUserModel.DOB);

                Add(ObjUserRegistration);
                flag = true;
                return flag;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>GetAllClientsDetail
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>12-Feb-2015</CreatedOn>
        /// <CreatedFor>To get the Client details for android</CreatedFor>
        /// </summary>
        /// <returns>lstClients</returns>
        public List<ServiceUserModel> GetAllClientsDetail()
        {
            try
            {
                List<ServiceUserModel> lstClients = (from lcm in _workorderEMSEntities.LocationClientMappings
                                                     join ur in _workorderEMSEntities.UserRegistrations on lcm.ClientUserId equals ur.UserId
                                                     join lm in _workorderEMSEntities.LocationMasters on lcm.LocationId equals lm.LocationId
                                                     //where lcm.LocationId == locationId
                                                     where ur.IsDeleted == false
                                                     select new ServiceUserModel()
                                                     {
                                                         UserId = ur.UserId,
                                                         FirstName = ur.FirstName,
                                                         LastName = ur.LastName,
                                                         UserEmail = ur.UserEmail,
                                                         UserType = ur.UserType,
                                                         Gender = ur.Gender,
                                                         ProfileImageFile = ur.ProfileImage,
                                                         EmployeeID = ur.EmployeeID,
                                                         JobTitle = ur.JobTitle,
                                                         LocationId = lm.LocationId,
                                                         LocationName = lm.LocationName,
                                                         Address1 = lm.Address1,
                                                         Address2 = lm.Address2,
                                                         City = lm.City,
                                                         CountryId = lm.CountryId,
                                                         PhoneNo = lm.PhoneNo,
                                                         Mobile = lm.Mobile,
                                                         ZipCode = lm.ZipCode
                                                     }).ToList();
                return lstClients;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// TO GET EMPLOYEE ASSIGNED WORKREQUEST
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>2015-03-12</CreatedDate>
        /// <param name="LocationId"></param>
        /// <param name="UserID"></param>
        /// <param name="OrderBy"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public List<Proc_GetAllWorkAssignedToEmployee_Result> GetEmployeeAssignedWorkRequest(long LocationId, long UserID, string OrderBy, string columnName)
        {
            return _workorderEMSEntities.Proc_GetAllWorkAssignedToEmployee(LocationId, UserID, OrderBy, columnName).ToList();
        }
        /// <summary>
        /// TO GET NOT ASSIGNED USERS
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>28-03-2015</CreatedDate>
        /// <param name="requestedBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="textSearch"></param>
        /// <param name="userType"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public List<NotAssignedUserModel> GetNotAssignedUsers(long? requestedBy, int? pageIndex, string sortColumnName, string sortOrderBy, int? numberOfRows, string textSearch, string userType, ObjectParameter totalRecords)
        {
            try
            {
                return _workorderEMSEntities.SP_GetAllNotAssignedUsers(requestedBy, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, userType, totalRecords).Select(x => new NotAssignedUserModel()
                {
                    RN = x.RN,
                    CodeName = x.CodeName,
                    GlobalCodeId = x.GlobalCodeId,
                    UserId = x.UserId,
                    UserEmail = x.UserEmail,
                    Name = x.Name,
                    Gender = x.Gender,
                    DOB = x.DOB,
                    ProfileImage = x.ProfileImage,
                    IsLoginActive = x.IsLoginActive,

                }).ToList();

            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created Date:05-Oct-2016
        /// This method call the sp SP_GetAllUnVerifiedUsers to get unverified user list.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="locationId"></param>
        /// <param name="useType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public List<UserModelList> GetUnVerifiedUsers(long? userId, long locationId, string useType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords)
        {
            //totalRecords = 0;
            ObjectParameter totalRecord = new ObjectParameter("TotalRecords", typeof(int));

            List<UserModelList> lstVerifiedMnagaer = new List<UserModelList>();
            try
            {
                lstVerifiedMnagaer = _workorderEMSEntities.SP_GetAllUnVerifiedUsers(userId, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, locationId, useType, totalRecord).Select(t =>
                    new UserModelList()
                    {
                        UserId = t.UserId,
                        //ProjectID = t.ProjectID,
                        //ProjectName = t.ProjectName,
                        UserEmail = t.UserEmail,
                        DOB = t.DOB,
                        Name = t.Name,
                        HiringDate = t.HiringDate,
                        EmployeeProfile = t.EmployeeProfile,
                        UserType = t.UserType,
                        CodeName = t.CodeName,
                        ProfileImage = t.ProfileImage,
                        QRCID = t.QRCID,

                    }).ToList();
                totalRecords = Convert.ToInt32(totalRecord.Value);
                return lstVerifiedMnagaer;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created Date:05-Oct-2016
        /// This method fetch user to edit unverified user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="operationName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <param name="paramTotalRecords"></param>
        /// <returns></returns>
        public UserModel GetUserByIdForUnverifiedUser(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter paramTotalRecords)
        {
            UserModel objUserModel = new UserModel();
            var data = _workorderEMSEntities.SP_GetUnverifiedUser(userId, operationName, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, paramTotalRecords);
            if (data != null)
            {
                foreach (var item in data)
                {
                    objUserModel.UserId = item.UserId;
                    objUserModel.UserType = item.UserType;
                    objUserModel.FirstName = item.FirstName;
                    objUserModel.LastName = item.LastName;
                    objUserModel.UserEmail = item.UserEmail;
                    objUserModel.AlternateEmail = item.AlternateEmail;
                    objUserModel.DOB = item.DOB.ToString("MM/dd/yy");
                    objUserModel.BloodGroup = item.BloodGroup;
                    objUserModel.Gender = item.GenderName != "" ? (item.GenderName == "Male" ? 9 : 10) : 9; // added by vijay sahu .
                    objUserModel.myProfileImage = item.ProfileImage;
                    objUserModel.Password = item.Password;
                    objUserModel.Address = new AddressModel();
                    objUserModel.Address.AddressMasterId = Convert.ToInt32(item.AddressMasterId);
                    objUserModel.Address.Address1 = item.Address1;
                    objUserModel.Address.Address2 = item.Address2;
                    objUserModel.Address.CountryId = item.CountryId;
                    objUserModel.Address.StateId = item.StateId;
                    objUserModel.Address.City = item.City;
                    objUserModel.Address.ZipCode = item.ZipCode;
                    objUserModel.Address.Mobile = item.Mobile;
                    objUserModel.Address.Phone = item.Phone;
                    objUserModel.EmployeeID = item.EmployeeID;
                    objUserModel.JobTitle = item.JobTitle;
                    objUserModel.JobTitleOther = item.JobTitleOther;
                }
            }

            return objUserModel;
        }

        /// <summary>GetListOf306090ForJSGrid
        /// <Modified By>mayur sahu</Modified> 
        /// <CreatedFor>To Get Performance 306090 list</CreatedFor>
        /// <CreatedOn>13-Oct-2019</CreatedOn>
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="OperationName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<PerformanceModel> GetListOf306090ForJSGrid(string userId, long locationId, string useType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords)
        {
            //totalRecords = 0;
            ObjectParameter totalRecord = new ObjectParameter("TotalRecords", typeof(int));

            List<PerformanceModel> ListOf306090Records = new List<PerformanceModel>();
            try
            {
                //lstVerifiedMnagaer = _workorderEMSEntities.spGetAssessmentList306090(userId, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, locationId, useType, totalRecord).Select(t =>
                ListOf306090Records = _workorderEMSEntities.spGetAssessmentList306090(userId).Select(t =>

                new PerformanceModel()
                {
                    EMP_EmployeeID = t.EMP_EmployeeID,
                    EmployeeName = t.EmployeeName,
                    EMP_Photo = t.EMP_Photo,
                    DepartmentName = t.DepartmentName,
                    JBT_JobTitle = t.JBT_JobTitle,
                    LocationName = t.LocationName,
                    EMP_DateOfJoining = t.EMP_DateOfJoining,
                    Assesment = t.Assesment,
                    Status = t.SAM_IsActive,
             



                }).ToList();
                totalRecords = Convert.ToInt32(totalRecord.Value);
                return ListOf306090Records;
            }
            catch (Exception)
            { throw; }
        }

        public List<GWCQUestionModel> GetGWCQuestions(string Id, string AssessmentType)
        {
            try
            {
                List<GWCQUestionModel> QuestionList = new List<GWCQUestionModel>();
                //lstVerifiedMnagaer = _workorderEMSEntities.spGetAssessmentList306090(userId, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, locationId, useType, totalRecord).Select(t =>
                if (AssessmentType == "30" || AssessmentType == "60" || AssessmentType == "90")
                {
                    QuestionList = _workorderEMSEntities.spGetAssessmentQuestion(Id, AssessmentType).Select(t =>

                     new GWCQUestionModel()
                     {
                         AssessmentType = t.ASQ_AssessmentType,
                         Question = t.ASQ_Question,
                         QuestionId = t.ASQ_Id,
                         QuestionType = t.ASQ_QuestionType,
                         EmployeeId = Id,
                         SelfAssessmentId = t.SAM_Id ?? 0,
                         Answer = t.SAM_Answer,
                         SAM_IsActive = t.SAM_IsActive
                         

                     }).ToList();
                }
                else if (AssessmentType == "QC" || AssessmentType == "QM")
                {
                    QuestionList = _workorderEMSEntities.spGetAssessmentQuestionQCQM(Id, AssessmentType).Select(t =>

                     new GWCQUestionModel()
                     {
                         SelfAssessmentId = t.EEL_Id,
                         EmployeeId = Id,
                         SAR_EMP_EmployeeIdManager = t.EEL_EMP_EmployeeIdManager,
                         QuestionType = t.ASQ_QuestionType,
                         QuestionId = t.ASQ_Id,
                         Question = t.ASQ_Question,
                         Answer = t.EEL_AnswerSelf,
                         SAR_AnswerManager = t.EEL_AnswerManager,
                         Comment = t.EEL_Comments,
                         SAM_IsActive = t.EEL_IsActive,
                         EEL_FinencialYear=t.EEL_FinencialYear,
                         EEL_FinQuarter=t.EEL_FinQuarter,

                     }).ToList();
                }
                else
                {

                    QuestionList = _workorderEMSEntities.spGetAssessmentQuestion316191(Id, AssessmentType).Select(t =>

        new GWCQUestionModel()
        {
                 SAR_Id=t.SAR_Id,
                SAR_EMP_EmployeeId=t.SAR_EMP_EmployeeId,
                SAR_EMP_EmployeeIdManager=t.SAR_EMP_EmployeeIdManager,
                  SAR_QuestionType=t.SAR_QuestionType,
                  ASQ_Id=t.ASQ_Id,
                  ASQ_Question=t.ASQ_Question,
                  SAR_AnswerSelf=t.SAR_AnswerSelf,
                  SAR_AnswerManager=t.SAR_AnswerManager,
                  SAR_Comments=t.SAR_Comments,
            SAR_IsActive=t.SAR_IsActive
        }).ToList();
                }
                return QuestionList;


            }
            catch (Exception)
            { throw; }
        }

        public bool saveSelfAssessment(List<GWCQUestionModel> data, string action)
        {
            try
            {
                string EmployeeId = string.Empty;
                string AssessmentType = string.Empty;

                if (data.Count() > 0)
                {
                    foreach (var i in data)
                    {
                        EmployeeId = i.EmployeeId;
                        AssessmentType = i.AssessmentType;
                        _workorderEMSEntities.spSetSelfAssessment306090((i.SAM_IsActive == null || i.SAM_IsActive == "" || i.SAM_IsActive != "Y") ? "I" : "U", i.EmployeeId, i.QuestionId, i.SelfAssessmentId, i.Answer == "Y" ? "Y" : i.Answer == "N" ? "N" : i.Answer == "S" ? "S" : null, action == "S" ? "S" : "Y");
                    }

                    if (action == "S")
                    {
                        _workorderEMSEntities.spSetSelfAssessment306090Submmit(EmployeeId, AssessmentType);

                    }

                }

                return true;

            }
            catch (Exception)
            { throw; }
        }

        public bool saveEvaluation(List<GWCQUestionModel> data, string action)
        {
            try
            {
                if (data.Count() > 0)
                {
                    foreach (var i in data)
                    {
                        _workorderEMSEntities.spSetReview306090("U", i.SAR_EMP_EmployeeId, i.ASQ_Id, i.SAR_Id, i.SAR_AnswerManager == "Y" ? "Y" : i.SAR_AnswerManager == "N" ? "N" : i.SAR_AnswerManager == "S" ? "S" : null, i.SAR_Comments, action == "S" ? "S" : "Y");
                    }
                }

                return true;

            }
            catch (Exception)
            { throw; }
        }

        /// <summary>GetListOf306090ForJSGrid
        /// <Modified By>mayur sahu</Modified> 
        /// <CreatedFor>To Get Performance 306090 list</CreatedFor>
        /// <CreatedOn>13-Oct-2019</CreatedOn>
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="OperationName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<PerformanceModel> GetListOfExpectationsForJSGrid(string userId, long locationId, string useType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords)
        {
            //totalRecords = 0;
            ObjectParameter totalRecord = new ObjectParameter("TotalRecords", typeof(int));

            List<PerformanceModel> ListOf306090Records = new List<PerformanceModel>();
            try
            {
                //lstVerifiedMnagaer = _workorderEMSEntities.spGetAssessmentList306090(userId, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, locationId, useType, totalRecord).Select(t =>
                ListOf306090Records = _workorderEMSEntities.spGetAssessmentList(userId).Select(t =>

                new PerformanceModel()
                {
                    EMP_EmployeeID = t.EMP_EmployeeID,
                    EmployeeName = t.EmployeeName,
                    EMP_Photo = t.EMP_Photo,
                    DepartmentName = t.DepartmentName,
                    JBT_JobTitle = t.JBT_JobTitle,
                    LocationName = t.LocationName,
                    EMP_DateOfJoining = t.EMP_DateOfJoining,
                    Expectation = t.Expectation,
                    Status = t.EEL_IsActive,
                    VST_Level=t.VST_Level,
                    FinYear=t.FinYear.Value,
                    AssessmentType=t.AssessmentType



                }).ToList();
                totalRecords = Convert.ToInt32(totalRecord.Value);
                return ListOf306090Records;
            }
            catch (Exception)
            { throw; }
        }
        public bool saveExpectations(List<GWCQUestionModel> data, string action)
        {
            try
            {
                if (data.Count() > 0)
                {
                    foreach (var i in data)
                    {
                        _workorderEMSEntities.spSetSelfAssessmentQuarterly((i.EEL_IsActive == null || i.EEL_IsActive == "" || i.EEL_IsActive != "Y") ? "I" : "U", i.EEL_EMP_EmployeeId,i.EEL_EMP_EmployeeIdManager,i.QuestionType, i.ASQ_Id, i.EEL_Id,i.EEL_FinencialYear,i.EEL_FinQuarter, i.EEL_AnswerSelf == "Y" ? "Y" : i.EEL_AnswerSelf == "N" ? "N" : i.EEL_AnswerSelf == "S" ? "S" : null, action == "S" ? "S" : "Y");
                    }
                }

                return true;

            }
            catch (Exception)
            { throw; }
        }
    }
}
