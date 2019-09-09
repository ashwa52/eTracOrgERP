using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public class ClientManager : IClientManager
    {
        UserRegistration ObjManagerUser;
        UserRepository ObjUserRepository;
        GlobalCodesRepository objGlobalCodesRepository;
        AddressManager objAddressManager = new AddressManager();
        ICommonMethodAdmin _ICommonMethodAdmin = new CommonMethodAdmin();
        ICommonMethod _ICommonMethod = new CommonMethodManager();
        IManageManager _IManageManager = new ManageManager();
        EmailLogRepository objEmailLogRepository = new EmailLogRepository();
        workorderEMSEntities _db = new workorderEMSEntities();
        //CommonMethodAdmin ObjCommonMethodAdmin = new CommonMethodAdmin();
        //Result result;
        //nagendra nov 13 2014
        public Result SaveClient(UserModel objUserModel, out long qrcId, bool IsManagerRegistration, DARModel objDARModel)
        {
            try
            {
                qrcId = 0;
                //QRCName = "Manager"; SpecialNotes = "Manager added successfully";
                ObjUserRepository = new UserRepository();
                ObjManagerUser = new UserRegistration();
                objGlobalCodesRepository = new GlobalCodesRepository();
                if (CheckDuplicateUser(objUserModel.UserEmail.Trim(), objUserModel.UserId, out qrcId, objUserModel.AlternateEmail, objUserModel.EmployeeID))
                {
                    objUserModel.IsEmailVerify = true;
                    objUserModel.IsLoginActive = true;
                    objUserModel.SubscriptionEmail = objUserModel.UserEmail;

                    if (objUserModel.Gender != null)
                    {
                        objUserModel.Gender = objUserModel.Gender == 1 ? objGlobalCodesRepository.GetSingleOrDefault(g => g.CodeName == "Male").GlobalCodeId : objGlobalCodesRepository.GetSingleOrDefault(g => g.CodeName == "Female").GlobalCodeId;
                    }

                    using (TransactionScope TransScope = new TransactionScope())
                    {
                        if (objUserModel.UserId == 0)
                        {
                            ObjManagerUser.ProfileImage = objUserModel.ProfileImageFile;
                            AutoMapper.Mapper.CreateMap<UserModel, UserRegistration>();
                            ObjManagerUser = AutoMapper.Mapper.Map(objUserModel, ObjManagerUser);
                            ObjManagerUser.ProfileImage = objUserModel.ProfileImageFile;
                            //if (ObjManagerUser.ProfileImage != null)
                            //    ObjManagerUser.ProfileImage = (!string.IsNullOrEmpty(objUserModel.ProfileImage.FileName) ? objUserModel.ProfileImage.FileName : "no-profile-pic.jpg");

                            ObjUserRepository.Add(ObjManagerUser);

                            //ObjUserRepository.SaveChanges();
                            //if (ObjManagerUser != null)
                            //{
                            //    //ObjManagerUser.Password = (string.IsNullOrEmpty(ObjManagerUser.Password)) ? Cryptography.GetDecryptedData(0.ToString(), true) : ObjManagerUser.Password;
                            //    if (_ICommonMethod.GenerateQRCode(QRCName, ManageManagerMODULE, null, null, QRCDefaultSizeID, QRCTYPEID, SpecialNotes, objUserModel.CreatedBy, out QRCID))
                            //    { ObjUserRepository.Add(ObjManagerUser); }
                            //}


                            if (ObjManagerUser.UserId > 0)
                            {
                                objUserModel.Address.UserId = ObjManagerUser.UserId;

                                objAddressManager.SaveAddress(objUserModel.Address);
                                if (objUserModel.UserType == Convert.ToInt64(UserType.GlobalAdmin, CultureInfo.InvariantCulture)
                                    || objUserModel.UserType == Convert.ToInt64(UserType.ITAdministrator, CultureInfo.InvariantCulture)
                                    || objUserModel.UserType == Convert.ToInt64(UserType.Administrator, CultureInfo.InvariantCulture)
                                    //Commented by Bhushan on 30/05/2016 for no need to check manager bcoz in below method(objBB.AssignRoleAndPermission) code is doing same 
                                    // || objUserModel.UserType == Convert.ToInt64(UserType.Manager, CultureInfo.InvariantCulture)
                                    )
                                { _ICommonMethodAdmin.AssignLocationToAdminUser(objUserModel.Location, ObjManagerUser.UserId); }

                                Result objResult = _ICommonMethod.SaveDAR(objDARModel);

                                TransScope.Complete();
                                return Result.Completed;
                            }
                            else
                            {
                                return Result.Failed;
                            }
                        }
                        else
                        {
                            UpdateUser(objUserModel, out qrcId, IsManagerRegistration);
                            objUserModel.Address.UserId = objUserModel.UserId;
                            objAddressManager.SaveAddress(objUserModel.Address);
                            Result objResult = _ICommonMethod.SaveDAR(objDARModel);
                            TransScope.Complete();
                            return Result.UpdatedSuccessfully;
                        }
                    }
                }
                else
                { return Result.DuplicateRecord; }


            }
            catch (Exception)
            { throw; }
        }


        public Result SaveClient(UserModel objUserModel, out long qrcId, bool IsManagerRegistration, DARModel objDARModel, long locationId, long createdBy, string action)
        {
            try
            {
                qrcId = 0;
                //QRCName = "Manager"; SpecialNotes = "Manager added successfully";
                ObjUserRepository = new UserRepository();
                ObjManagerUser = new UserRegistration();
                objGlobalCodesRepository = new GlobalCodesRepository();
                if (CheckDuplicateUser(objUserModel.UserEmail.Trim(), objUserModel.UserId, out qrcId, objUserModel.AlternateEmail, objUserModel.EmployeeID))
                {
                    objUserModel.IsEmailVerify = true;
                    objUserModel.IsLoginActive = true;
                    objUserModel.SubscriptionEmail = objUserModel.UserEmail;

                    using (TransactionScope TransScope = new TransactionScope())
                    {
                        if (objUserModel.UserId == 0)
                        {
                            AutoMapper.Mapper.CreateMap<UserModel, UserRegistration>();
                            string DOB = objUserModel.DOB;
                            objUserModel.DOB = null;
                            ObjManagerUser = AutoMapper.Mapper.Map(objUserModel, ObjManagerUser);
                            ObjManagerUser.ProfileImage = (objUserModel.ProfileImage != null) ? objUserModel.ProfileImageFile : "no-profile-pic.jpg";
                            if (!string.IsNullOrEmpty(DOB))
                                ObjManagerUser.DOB = Convert.ToDateTime(DOB, CultureInfo.InvariantCulture);
                            ObjManagerUser.IsEmailVerify = false; //added by vijay sahu 12 june 2015 , by default it should not be true
                            ObjManagerUser.IsLoginActive = false;
                            ObjManagerUser.UserSignature = objUserModel.SignatureImageBase;
                            ObjUserRepository.Add(ObjManagerUser);

                            if (ObjManagerUser.UserId > 0)
                            {
                                objUserModel.Address.UserId = ObjManagerUser.UserId;
                                objAddressManager.SaveAddress(objUserModel.Address);
                                if (objUserModel.UserType == Convert.ToInt64(UserType.GlobalAdmin, CultureInfo.InvariantCulture)
                                    || objUserModel.UserType == Convert.ToInt64(UserType.ITAdministrator, CultureInfo.InvariantCulture)
                                    || objUserModel.UserType == Convert.ToInt64(UserType.Administrator, CultureInfo.InvariantCulture)
                                    //Commented by Bhushan on 30/05/2016 for no need to check manager bcoz objBB.AssignRoleAndPermission code is doing same 
                                    // || objUserModel.UserType == Convert.ToInt6 4(UserType.Manager, CultureInfo.InvariantCulture)
                                    )
                                { _ICommonMethodAdmin.AssignLocationToAdminUser(locationId, ObjManagerUser.UserId); }

                                Result objResult = _ICommonMethod.SaveDAR(objDARModel);
                                ////this block is used for assigning roles to the users.. 
                                {
                                    WorkOrderEMS.BusinessLogic.Managers.Common_B objBB = new WorkOrderEMS.BusinessLogic.Managers.Common_B();
                                    byte a = objBB.AssignRoleAndPermission(ObjManagerUser.UserId, objUserModel.UserType, locationId, createdBy, "");
                                }

                                //COMMENTED BY BHUSHAN DOD FOR MULTIPLE ENTRIES INSERTED IN ADMINLOCATIONMAPPING
                                //if (objUserModel.UserType == 6) // only for admin user mapping with location. 
                                //{
                                //    _ICommonMethodAdmin.AssignLocationToAdminUser(locationId, ObjManagerUser.UserId);
                                //}


                                //////////////////if (objUserModel.UserType == 3) // only for admin user mapping with location. 
                                //////////////////{

                                //////////////////    using (workorderEMSEntities Context = new workorderEMSEntities())
                                //////////////////    {
                                //////////////////        EmployeeLocationMappingRepository objmapping = new EmployeeLocationMappingRepository();

                                //////////////////        objmapping.DeleteAll(x => x.EmployeeUserId == objUserModel.UserId && x.LocationId == locationId);

                                //////////////////        EmployeeLocationMapping MapEntity = new EmployeeLocationMapping();
                                //////////////////        MapEntity.LocationId = locationId;
                                //////////////////        MapEntity.EmployeeUserId = objUserModel.UserId;
                                //////////////////        MapEntity.ModifiedBy = createdBy;
                                //////////////////        MapEntity.CreatedBy = createdBy;
                                //////////////////        MapEntity.CreatedOn = DateTime.Now;
                                //////////////////        objmapping.Add(MapEntity);

                                //////////////////        objmapping.SaveChanges();
                                //////////////////    }
                                //////////////////}
                                //_ICommonMethod.GetAdminByIdCode

                                objUserModel.UserId = ObjManagerUser.UserId;
                                TransScope.Complete();
                                return Result.Completed;
                            }
                            else
                            {
                                return Result.Failed;
                            }
                        }
                        else
                        {
                            UpdateUser(objUserModel, out qrcId, IsManagerRegistration);
                            objUserModel.Address.UserId = objUserModel.UserId;
                            objAddressManager.SaveAddress(objUserModel.Address);
                            Result objResult = _ICommonMethod.SaveDAR(objDARModel);
                            TransScope.Complete();
                            return Result.UpdatedSuccessfully;
                        }
                    }
                }
                else
                { return Result.DuplicateRecord; }


            }
            catch (Exception)
            { throw; }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To save client
        /// Created Date : 07-May-2019
        /// </summary>
        /// <param name="objUserModel"></param>
        /// <param name="qrcId"></param>
        /// <param name="IsManagerRegistration"></param>
        /// <param name="objDARModel"></param>
        /// <param name="locationId"></param>
        /// <param name="createdBy"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Result SaveNewClient(UserModel objUserModel, out long qrcId, bool IsManagerRegistration, DARModel objDARModel, long locationId, long createdBy, string action)
        {
            try
            {
                qrcId = 0;
                ObjUserRepository = new UserRepository();
                ObjManagerUser = new UserRegistration();
                objGlobalCodesRepository = new GlobalCodesRepository();
                if (objUserModel.UserId == 0)
                   // if (CheckDuplicateUser(objUserModel.UserEmail.Trim(), objUserModel.UserId, out qrcId, objUserModel.AlternateEmail, objUserModel.EmployeeID))
                {
                    objUserModel.IsEmailVerify = true;
                    objUserModel.IsLoginActive = true;
                    objUserModel.SubscriptionEmail = objUserModel.UserEmail;
                    using (TransactionScope TransScope = new TransactionScope())
                    {
                        if (objUserModel.UserId == 0)
                        {
                            AutoMapper.Mapper.CreateMap<UserModel, UserRegistration>();
                            string DOB = objUserModel.DOB;
                            objUserModel.DOB = null;
                            ObjManagerUser = AutoMapper.Mapper.Map(objUserModel, ObjManagerUser);
                            ObjManagerUser.ProfileImage = (objUserModel.ProfileImage != null) ? objUserModel.ProfileImageFile : "no-profile-pic.jpg";
                            if (!string.IsNullOrEmpty(DOB))
                            ObjManagerUser.DOB = Convert.ToDateTime(DOB, CultureInfo.InvariantCulture);
                            ObjManagerUser.IsEmailVerify = false; //added by vijay sahu 12 june 2015 , by default it should not be true
                            ObjManagerUser.IsLoginActive = false;
                            ObjManagerUser.UserSignature = objUserModel.SignatureImageBase;                            
                            ObjUserRepository.Add(ObjManagerUser);
                            
                            #region LocationClientMappingRepository
                            LocationClientMappingRepository ObjLocationClientMappingRepository;
                            if (ObjManagerUser.UserId > 0)
                            {
                                //Adding Customer to Company table
                                long VendorId = 5; // Vendor Type 
                                long CompanyType = 4; // Client
                                var saveToCompany = _db.spSetCompanyUserId("I",null, objUserModel.CompanyName,
                                    objUserModel.CompanyName, VendorId, CompanyType,null,"Y", ObjManagerUser.UserId);
                                var getCompanyId = saveToCompany.FirstOrDefault();
                                long? CompanyId = getCompanyId;
                                var _vendorManager = new VendorManagementManager();
                                var quickBook = _vendorManager.SaveQuickBookId(objUserModel.QBKId, CompanyId);
                                LocationClientMapping _LocationClientMapping = new LocationClientMapping();
                                 long verificationClientId = 0;
                                if (objUserModel.SelectedLocation != null)
                                {
                                    string[] Location = objUserModel.SelectedLocation.Split(',');
                                    for (int i = 0; i < Location.Length; i++)
                                    {
                                        long locId = Convert.ToInt64(Location[i]);
                                        var saveMultipleLocaation = _db.spSetLocationCompanyMapping("I",null,
                                            locId, CompanyId, objUserModel.CreatedBy,null,"Y");
                                    }
                                }
                            }
                            #endregion ManagerLocationMappingRepository

                            if (ObjManagerUser.UserId > 0)
                            {
                                objUserModel.Address.UserId = ObjManagerUser.UserId;
                                objUserModel.Address.CreatedBy = objUserModel.CreatedBy;
                                objAddressManager.SaveAddress(objUserModel.Address);
                                if (objUserModel.UserType == Convert.ToInt64(UserType.GlobalAdmin, CultureInfo.InvariantCulture)
                                    || objUserModel.UserType == Convert.ToInt64(UserType.ITAdministrator, CultureInfo.InvariantCulture)
                                    || objUserModel.UserType == Convert.ToInt64(UserType.Administrator, CultureInfo.InvariantCulture)
                                    //Commented by Bhushan on 30/05/2016 for no need to check manager bcoz objBB.AssignRoleAndPermission code is doing same 
                                    // || objUserModel.UserType == Convert.ToInt6 4(UserType.Manager, CultureInfo.InvariantCulture)
                                    )
                                { _ICommonMethodAdmin.AssignLocationToAdminUser(locationId, ObjManagerUser.UserId); }

                                Result objResult = _ICommonMethod.SaveDAR(objDARModel);
                                ////this block is used for assigning roles to the users.. 
                                {
                                    WorkOrderEMS.BusinessLogic.Managers.Common_B objBB = new WorkOrderEMS.BusinessLogic.Managers.Common_B();
                                    byte a = objBB.AssignRoleAndPermission(ObjManagerUser.UserId, objUserModel.UserType, locationId, createdBy, "");
                                }                                
                                objUserModel.UserId = ObjManagerUser.UserId;
                                TransScope.Complete();
                                return Result.Completed;
                            }
                            else
                            {
                                return Result.Failed;
                            }
                        }
                        else
                        {
                            UpdateUser(objUserModel, out qrcId, IsManagerRegistration);
                            objUserModel.Address.UserId = objUserModel.UserId;
                            objAddressManager.SaveAddress(objUserModel.Address);
                            Result objResult = _ICommonMethod.SaveDAR(objDARModel);
                            TransScope.Complete();
                            return Result.UpdatedSuccessfully;
                        }
                    }
                }
                else
                {

                    UpdateUser(objUserModel, out qrcId, IsManagerRegistration);
                    objUserModel.Address.UserId = objUserModel.UserId;
                    objAddressManager.SaveAddress(objUserModel.Address);
                    Result objResult = _ICommonMethod.SaveDAR(objDARModel);
                    return Result.UpdatedSuccessfully;
                }


            }
            catch (Exception)
            { throw; }
        }


        public Result SaveClientNewUserRegistrationforAll(UserModel objUserModel, out long qrcId, bool IsManagerRegistration, DARModel objDARModel, long locationId, long createdBy, string action)
        {
            try
            {
                qrcId = 0;

                ObjUserRepository = new UserRepository();
                ObjManagerUser = new UserRegistration();
                objGlobalCodesRepository = new GlobalCodesRepository();
                if (CheckDuplicateUser(objUserModel.UserEmail.Trim(), objUserModel.UserId, out qrcId, objUserModel.AlternateEmail, objUserModel.EmployeeID))
                {
                    objUserModel.IsEmailVerify = true;
                    objUserModel.IsLoginActive = true;
                    objUserModel.SubscriptionEmail = objUserModel.UserEmail;

                    using (TransactionScope TransScope = new TransactionScope())
                    {
                        if (objUserModel.UserId == 0)
                        {
                            objUserModel.FirstName = objUserModel.FirstName.ToTitleCase();
                            objUserModel.LastName = objUserModel.LastName.ToTitleCase();

                            AutoMapper.Mapper.CreateMap<UserModel, UserRegistration>();
                            string DOB = objUserModel.DOB;
                            objUserModel.DOB = null;
                            ObjManagerUser = AutoMapper.Mapper.Map(objUserModel, ObjManagerUser);
                            ObjManagerUser.ProfileImage = (objUserModel.ProfileImage != null) ? objUserModel.ProfileImageFile : "no-profile-pic.jpg";
                            if (!string.IsNullOrEmpty(DOB))
                                ObjManagerUser.DOB = Convert.ToDateTime(DOB, CultureInfo.InvariantCulture);
                            ObjManagerUser.IsEmailVerify = false; //added by vijay sahu 12 june 2015 , by default it should not be true
                            ObjManagerUser.IsLoginActive = false;
                            //Added By Bhushan Dod on 04/Sep/2016 for User type of user.
                            ObjManagerUser.UserType = objUserModel.SelectedUserType;
                            ObjManagerUser.ManagerForUser = objUserModel.SelectManager;
                            ObjManagerUser.UserSignature = objUserModel.SignatureImageBase;
                            ObjUserRepository.Add(ObjManagerUser);

                            if (ObjManagerUser.UserId > 0)
                            {
                                objUserModel.Address.UserId = ObjManagerUser.UserId;
                                objAddressManager.SaveAddress(objUserModel.Address);
                                if (objUserModel.SelectedUserType == Convert.ToInt64(UserType.GlobalAdmin, CultureInfo.InvariantCulture)
                                    || objUserModel.SelectedUserType == Convert.ToInt64(UserType.ITAdministrator, CultureInfo.InvariantCulture)
                                    || objUserModel.SelectedUserType == Convert.ToInt64(UserType.Administrator, CultureInfo.InvariantCulture)
                                    //Commented by Bhushan on 30/05/2016 for no need to check manager bcoz objBB.AssignRoleAndPermission code is doing same 
                                    // || objUserModel.UserType == Convert.ToInt64(UserType.Manager, CultureInfo.InvariantCulture)
                                    )
                                {
                                    _ICommonMethodAdmin.AssignLocationToAdminUser(locationId, ObjManagerUser.UserId);
                                }
                                else if (objUserModel.SelectedUserType == Convert.ToInt64(UserType.Manager, CultureInfo.InvariantCulture))
                                {
                                    _ICommonMethodAdmin.AssignLocationToManagerUser(locationId, ObjManagerUser.UserId);
                                }
                                else if (objUserModel.SelectedUserType == Convert.ToInt64(UserType.Employee, CultureInfo.InvariantCulture))
                                {
                                    _ICommonMethodAdmin.AssignLocationToEmployeeUser(locationId, ObjManagerUser.UserId);
                                }
                                Result objResult = _ICommonMethod.SaveDAR(objDARModel);
                                ////this block is used for assigning roles to the users.. 
                                {
                                    WorkOrderEMS.BusinessLogic.Managers.Common_B objBB = new WorkOrderEMS.BusinessLogic.Managers.Common_B();
                                    byte a = objBB.AssignRoleAndPermission(ObjManagerUser.UserId, objUserModel.UserType, locationId, createdBy, "");
                                }
                                if (objUserModel.SelectedServicesID != "" && objUserModel.SelectedServicesID != null && objUserModel.SelectedServicesID.Trim() != "")
                                {
                                    GlobalAdminManager ObjGlobalAdminManager = new GlobalAdminManager();

                                    var userServicesID = objUserModel.SelectedServicesID.Split(',');
                                    if (userServicesID != null && userServicesID.Length > 0)
                                    {
                                        foreach (var service in userServicesID)
                                        {
                                            if (service != null && !string.IsNullOrEmpty(service) && Convert.ToInt64(service, CultureInfo.InvariantCulture) > 0)
                                            {
                                                long WidgetId = Convert.ToInt64(service, CultureInfo.InvariantCulture);
                                                var IsInserted = CommonMethodManager.AddPermissionDetail(WidgetId, ObjManagerUser.UserId, objUserModel.SelectedLocationId.Value);
                                            }

                                        }
                                        //Added By Bhushan Dod on 07/06/2016 for bydefault setting when location created according to loc services.
                                        ObjGlobalAdminManager.SaveByDefaultWidgetSetting(locationId, objUserModel.SelectedServicesID, ObjManagerUser.UserId);
                                    }
                                }



                                //COMMENTED BY BHUSHAN DOD FOR MULTIPLE ENTRIES INSERTED IN ADMINLOCATIONMAPPING
                                //if (objUserModel.UserType == 6) // only for admin user mapping with location. 
                                //{
                                //    _ICommonMethodAdmin.AssignLocationToAdminUser(locationId, ObjManagerUser.UserId);
                                //}


                                //////////////////if (objUserModel.UserType == 3) // only for admin user mapping with location. 
                                //////////////////{

                                //////////////////    using (workorderEMSEntities Context = new workorderEMSEntities())
                                //////////////////    {
                                //////////////////        EmployeeLocationMappingRepository objmapping = new EmployeeLocationMappingRepository();

                                //////////////////        objmapping.DeleteAll(x => x.EmployeeUserId == objUserModel.UserId && x.LocationId == locationId);

                                //////////////////        EmployeeLocationMapping MapEntity = new EmployeeLocationMapping();
                                //////////////////        MapEntity.LocationId = locationId;
                                //////////////////        MapEntity.EmployeeUserId = objUserModel.UserId;
                                //////////////////        MapEntity.ModifiedBy = createdBy;
                                //////////////////        MapEntity.CreatedBy = createdBy;
                                //////////////////        MapEntity.CreatedOn = DateTime.Now;
                                //////////////////        objmapping.Add(MapEntity);

                                //////////////////        objmapping.SaveChanges();
                                //////////////////    }
                                //////////////////}
                                //_ICommonMethod.GetAdminByIdCode

                                objUserModel.UserId = ObjManagerUser.UserId;
                                TransScope.Complete();
                                return Result.Completed;
                            }
                            else
                            {
                                return Result.Failed;
                            }
                        }
                        else
                        {
                            UpdateUser(objUserModel, out qrcId, IsManagerRegistration);
                            objUserModel.Address.UserId = objUserModel.UserId;
                            objAddressManager.SaveAddress(objUserModel.Address);
                            Result objResult = _ICommonMethod.SaveDAR(objDARModel);
                            TransScope.Complete();
                            return Result.UpdatedSuccessfully;
                        }
                    }
                }
                else
                { return Result.DuplicateRecord; }
            }
            catch (Exception)
            { throw; }
        }

        public Result SaveNewUserRegistration(UserModel objUserModel, out long qrcId, bool IsManagerRegistration, long createdBy, string action)
        {
            try
            {
                qrcId = 0;

                ObjUserRepository = new UserRepository();
                ObjManagerUser = new UserRegistration();
                objGlobalCodesRepository = new GlobalCodesRepository();
                if (CheckDuplicateUser(objUserModel.UserEmail.Trim(), objUserModel.UserId, out qrcId, objUserModel.AlternateEmail, objUserModel.EmployeeID))
                {
                    objUserModel.SubscriptionEmail = objUserModel.UserEmail;

                    using (TransactionScope TransScope = new TransactionScope())
                    {
                        if (objUserModel.UserId == 0)
                        {
                            objUserModel.FirstName = objUserModel.FirstName.ToTitleCase();
                            objUserModel.LastName = objUserModel.LastName.ToTitleCase();
                            AutoMapper.Mapper.CreateMap<UserModel, UserRegistration>();
                            string DOB = objUserModel.DOB;
                            objUserModel.DOB = null;
                            ObjManagerUser = AutoMapper.Mapper.Map(objUserModel, ObjManagerUser);
                            ObjManagerUser.ProfileImage = (objUserModel.ProfileImage != null) ? objUserModel.ProfileImageFile : "no-profile-pic.jpg";
                            if (!string.IsNullOrEmpty(DOB))
                                ObjManagerUser.DOB = Convert.ToDateTime(DOB, CultureInfo.InvariantCulture);
                            ObjManagerUser.IsEmailVerify = false; //added by vijay sahu 12 june 2015 , by default it should not be true
                            ObjManagerUser.IsLoginActive = false;
                            //Added By Bhushan Dod on 04/Sep/2016 for User type of user.
                            ObjManagerUser.UserType = objUserModel.SelectedUserType;

                            ObjUserRepository.Add(ObjManagerUser);

                            if (ObjManagerUser.UserId > 0)
                            {
                                objUserModel.Address.UserId = ObjManagerUser.UserId;
                                objAddressManager.SaveAddress(objUserModel.Address);

                                //////this block is used for assigning roles to the users.. 
                                //{
                                //    WorkOrderEMS.BusinessLogic.Managers.Common_B objBB = new WorkOrderEMS.BusinessLogic.Managers.Common_B();
                                //    byte a = objBB.AssignRoleAndPermission(ObjManagerUser.UserId, objUserModel.UserType, locationId, createdBy, "");
                                //}                       
                                objUserModel.UserId = ObjManagerUser.UserId;
                                TransScope.Complete();
                                return Result.Completed;
                            }
                            else
                            {
                                return Result.Failed;
                            }
                        }
                        else
                        {
                            UpdateUser(objUserModel, out qrcId, IsManagerRegistration);
                            objUserModel.Address.UserId = objUserModel.UserId;
                            objAddressManager.SaveAddress(objUserModel.Address);
                            TransScope.Complete();
                            return Result.UpdatedSuccessfully;
                        }
                    }
                }
                else
                { return Result.DuplicateRecord; }
            }
            catch (Exception)
            { throw; }
        }

        public Result SaveAdminNewUserRegistration(UserModel objUserModel)
        {
            try
            {
                long qrcId = 0;
                ObjUserRepository = new UserRepository();
                ObjManagerUser = new UserRegistration();
                objGlobalCodesRepository = new GlobalCodesRepository();
                if (CheckDuplicateUser(objUserModel.UserEmail.Trim(), objUserModel.UserId, out qrcId, objUserModel.AlternateEmail, objUserModel.EmployeeID))
                {
                    objUserModel.IsEmailVerify = true;
                    objUserModel.IsLoginActive = true;
                    objUserModel.SubscriptionEmail = objUserModel.UserEmail;
                    using (TransactionScope TransScope = new TransactionScope())
                    {
                        if (objUserModel.UserId == 0)
                        {
                            objUserModel.FirstName = objUserModel.FirstName.ToTitleCase();
                            objUserModel.LastName = objUserModel.LastName.ToTitleCase();
                            AutoMapper.Mapper.CreateMap<UserModel, UserRegistration>();
                            string DOB = objUserModel.DOB;
                            objUserModel.DOB = null;
                            ObjManagerUser = AutoMapper.Mapper.Map(objUserModel, ObjManagerUser);
                            ObjManagerUser.ProfileImage = (objUserModel.ProfileImage != null) ? objUserModel.ProfileImageFile : "no-profile-pic.jpg";
                            if (!string.IsNullOrEmpty(DOB))
                                ObjManagerUser.DOB = Convert.ToDateTime(DOB, CultureInfo.InvariantCulture);
                            //ObjManagerUser.IsEmailVerify = false; //added by vijay sahu 12 june 2015 , by default it should not be true
                            //ObjManagerUser.IsLoginActive = false;
                            //Added By Bhushan Dod on 04/Sep/2016 for User type of user.
                            ObjManagerUser.UserType = objUserModel.SelectedUserType;

                            ObjUserRepository.Add(ObjManagerUser);

                            if (ObjManagerUser.UserId > 0)
                            {
                                objUserModel.Address.UserId = ObjManagerUser.UserId;
                                objAddressManager.SaveAddress(objUserModel.Address);
                                TransScope.Complete();
                                return Result.Completed;
                            }
                            else
                            {
                                return Result.Failed;
                            }
                        }
                        else { return Result.Failed; }
                    }
                }
                else
                { return Result.DuplicateRecord; }
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>
        /// To Check Duplicate User At time Registration
        /// </summary>
        /// <ModifiedBy>Manoj Jaswal</ModifiedBy>
        /// <ModifiedDate>2015-03-13</ModifiedDate>
        /// <param name="userEmail"></param>
        /// <param name="userId"></param>
        /// <param name="qrcId"></param>
        /// <param name="UserName"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>

        public bool CheckDuplicateUser(string userEmail, long userId, out long qrcId, string UserName, string EmployeeID)
        {
            try
            {
                dynamic data = null;
                qrcId = 0;
                if (userId > 0)
                {
                    data = ObjUserRepository.GetAll(u => u.UserEmail == userEmail.Trim() && u.IsDeleted == false && u.UserId != userId);
                }
                else
                {
                    data = ObjUserRepository.GetAll(u => (u.UserEmail == userEmail.Trim() || u.AlternateEmail == UserName.Trim() || u.EmployeeID == EmployeeID.Trim()) && u.IsDeleted == false);
                }

                if (data.Count > 0)
                {
                    // qrcId = (data[0].QRCID.HasValue) ? (data[0].QRCID.Value) : 0;
                    return false;
                }
                return true;
            }
            catch (Exception)
            { throw; }
        }

        public void UpdateUser(UserModel objUserModel, out long qrcId, bool isManagerRegistration)
        {
            try
            {
                ObjUserRepository = new UserRepository();
                var data = ObjUserRepository.GetSingleOrDefault(u => u.UserId == objUserModel.UserId && u.IsDeleted == false);
                qrcId = (data.QRCID.HasValue) ? data.QRCID.Value : 0;
                if (data != null)
                {
                    data.FirstName = objUserModel.FirstName.ToTitleCase();
                    data.LastName = objUserModel.LastName.ToTitleCase();
                    //data.DOB = Convert.ToDateTime(objUserModel.DOB);
                    data.BloodGroup = objUserModel.BloodGroup;
                    //data.AlternateEmail = objUserModel.AlternateEmail;
                    data.Gender = objUserModel.Gender;
                    if (isManagerRegistration)
                        if (objUserModel.Password != null)
                        {
                            data.Password = objUserModel.Password;
                        }
                    //data.IsEmailVerify = objUserModel.IsEmailVerify;
                    //data.IsLoginActive = objUserModel.IsLoginActive;
                    data.Gender = objUserModel.Gender;
                    data.UserEmail = objUserModel.UserEmail;
                    //data.AlternateEmail = objUserModel.AlternateEmail;
                    data.SubscriptionEmail = objUserModel.UserEmail;
                    data.ProjectID = objUserModel.ProjectID;
                    data.FirstName = objUserModel.FirstName.ToTitleCase(); ;
                    data.LastName = objUserModel.LastName.ToTitleCase(); ;
                    data.Gender = objUserModel.Gender;
                    if (string.IsNullOrEmpty(objUserModel.DOB))
                    {
                        data.DOB = null;
                    }
                    else
                    { data.DOB = Convert.ToDateTime(objUserModel.DOB); }
                    data.TimeZoneId = objUserModel.TimeZoneId;
                    data.VendorID = objUserModel.VendorID;
                    data.BloodGroup = objUserModel.BloodGroup;
                    if (objUserModel.HiringDate != null)
                    {
                        data.HiringDate = objUserModel.HiringDate.Value.ToClientTimeZoneinDateTime();
                    }
                    data.EmployeeCategory = objUserModel.EmployeeCategory;
                    data.QRCID = objUserModel.QRCID;
                    //data.EmployeeID = objUserModel.EmployeeID;
                    data.JobTitle = objUserModel.JobTitle;
                    data.JobTitleOther = objUserModel.JobTitleOther;
                    if (objUserModel.ProfileImageFile != null)
                    {
                        data.ProfileImage = objUserModel.ProfileImageFile;
                    }
                    if(objUserModel.SignatureImageBase != null && objUserModel.SignatureImageBase != "")
                    {
                        data.UserSignature = objUserModel.SignatureImageBase;
                    }
                    if(objUserModel.PaymentMode > 0 && objUserModel.PaymentTerm > 0)
                    {
                        data.PaymentTerm = objUserModel.PaymentTerm;
                        data.PaymentMode = objUserModel.PaymentMode;
                    }
                    ObjUserRepository.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserModel GetClientById(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch)
        {
            ObjUserRepository = new UserRepository();
            try
            {
                ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
                return ObjUserRepository.GetUserById(userId, operationName, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, paramTotalRecords);

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>Add new employee 
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>UpdateTaskStatus</CreatedFor>
        /// <CreatedOn>Jan-16-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceWorkStatusModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> AddNewClient(UserModel ObjUserModel)
        {
            ObjUserRepository = new UserRepository();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (ObjUserModel.ServiceAuthKey != null && ObjUserModel.UserId != null)
                {
                    var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == ObjUserModel.ServiceAuthKey && x.IsDeleted == false);

                    if (authuser != null && authuser.UserId > 0)
                    {
                        bool result = ObjUserRepository.SaveNewClientRegistration(ObjUserModel);
                        if (result != null || result == true)
                        {
                            ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                            ObjServiceResponseModel.Message = CommonMessage.SaveSuccessMessage();
                        }
                        else
                        {
                            ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                            ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                        }
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.InvalidSessionResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    }
                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception)
            { throw; }

            return ObjServiceResponseModel;
        }

        /// <summary>GetAllClientsDetail
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>12-Feb-2015</CreatedOn>
        /// <CreatedFor>To get the Client details for android</CreatedFor>
        /// </summary>
        /// <returns>lstClients</returns>
        public ServiceResponseModel<List<ServiceUserModel>> GetAllClientList(ServiceUserModel objUserModel)
        {
            ObjUserRepository = new UserRepository();
            ServiceResponseModel<List<ServiceUserModel>> lstClients = new ServiceResponseModel<List<ServiceUserModel>>();
            try
            {
                if (objUserModel.ServiceAuthKey != null && objUserModel.UserId != null)
                {
                    var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == objUserModel.ServiceAuthKey && x.IsDeleted == false);

                    if (authuser != null && authuser.UserId > 0)
                    {
                        var result = ObjUserRepository.GetAllClientsDetail();
                        if (result != null || result.Count > 0)
                        {
                            lstClients.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                            lstClients.Data = result;
                            lstClients.Message = CommonMessage.Successful();
                        }
                        else
                        {
                            lstClients.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                            lstClients.Message = CommonMessage.WrongParameterMessage();
                        }
                    }
                    else
                    {
                        lstClients.Response = Convert.ToInt32(ServiceResponse.InvalidSessionResponse, CultureInfo.CurrentCulture);
                        lstClients.Message = CommonMessage.InvalidUser();
                    }
                }
                else
                {

                    lstClients.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    lstClients.Message = CommonMessage.WrongParameterMessage();
                }
                return lstClients;
            }
            catch (Exception)
            { throw; }

        }

        /// <summary>
        /// TO CANCEL WORKORDER CREATED BY CLIENT 
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>4-14-2015</CreatedDate>
        /// <param name="WorOrderID"></param>
        /// <param name="iUserId"></param>
        /// <returns></returns>
        public Result CancelWorkOrderByEmployee(long WorOrderID, long iUserId)
        {
            try
            {
                using (TransactionScope TransScope = new TransactionScope())
                {
                    WorkRequestAssignmentRepository ObjWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
                    WorkRequestAssignment obj_WorkRequestAssignment = ObjWorkRequestAssignmentRepository.GetAll(x => x.WorkRequestAssignmentID == WorOrderID && x.IsDeleted == false).FirstOrDefault();
                    if (obj_WorkRequestAssignment != null)
                    {
                        obj_WorkRequestAssignment.IsDeleted = true;
                        obj_WorkRequestAssignment.DeletedBy = iUserId;
                        obj_WorkRequestAssignment.ModifiedDate = DateTime.UtcNow;
                        ObjWorkRequestAssignmentRepository.SaveChanges();
                        TransScope.Complete();
                        return Result.Delete;
                    }
                    else { TransScope.Dispose(); return Result.DoesNotExist; }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// TO GET CLIENT DETAIL BY LOCATION
        /// </summary>
        /// <CreatedBy>Manoj jaswal</CreatedBy>
        /// <CreatedDate>17 April 2015</CreatedDate>
        /// <param name="LocationID"></param>
        /// <returns></returns>
        public UserModelList GetClientByLocation(long LocationID)
        {
            try
            {
                LocationClientMappingRepository obj_LocationClientMappingRepository = new LocationClientMappingRepository();
                UserRegistration Client = obj_LocationClientMappingRepository.GetSingleOrDefault(x => x.LocationId == LocationID && x.IsDeleted == false).UserRegistration;
                UserModelList obj_UserModel = new UserModelList();
                obj_UserModel.UserId = Client.UserId;

                obj_UserModel.UserEmail = Client.UserEmail;
                obj_UserModel.Name = Client.FirstName + " " + Client.LastName;
                obj_UserModel.JobTitle = Client.JobTitle;
                if (Client.DOB != null)
                {
                    obj_UserModel.DOB = Client.DOB.Value.ToClientTimeZoneinDateTime();
                }
                if (Client.HiringDate != null)
                {
                    obj_UserModel.HiringDate = Client.HiringDate.Value.ToClientTimeZoneinDateTime();
                }

                obj_UserModel.RoleId = Client.UserType;

                return obj_UserModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 10-May-2019
        /// Created For : To get all user list of a client type.
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <returns></returns>

        public UserModelDetails GetClientList(long? UserId, long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            try
            {
                long Client = Convert.ToInt64(UserType.Client);
                var objDetails = new UserModelDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var Results = new List<UserModel>();
                if (LocationId > 0)
                {
                    Results = _db.UserRegistrations.Where(x => x.UserType == Client && x.IsEmailVerify == true && x.IsDeleted == false)  // .CompanyFacilityMappings.Where(x => x.CFM_CMP_Id == VendorId)
                   .Select(a => new UserModel()
                   {
                       UserName = a.FirstName + " " + a.LastName,
                       UserTypeView = a.UserType == Client ? "Client" : "Other",
                       UserId = a.UserId,
                       ProfileImageFile = a.ProfileImage,
                       UserEmail = a.UserEmail
                   }).OrderByDescending(x => x.UserId).ToList();
                }

                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                objDetails.pageindex = pageindex;
                objDetails.total = totalPages;
                objDetails.records = totRecords;
                objDetails.rows = Results.ToList();
                return objDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public CompanyListDetails GetAllCompanyDataList(long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of all company.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 14-May-2019
        /// Created For : To get all company details by UserId.
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public VendorSetupManagementModel GetCompanyDetails(long UserId)
        {
            var details = new VendorSetupManagementModel();
            try
            {
                if (UserId > 0)
                {
                    details = _db.Companies.Where(x => x.CMP_UserId == UserId).Select(a => new VendorSetupManagementModel()
                    {
                        CompanyId = a.CMP_Id,
                        CompanyNameLegal = a.CMP_NameLegal
                    }).FirstOrDefault();
                }
                else
                {
                    details = null;
                }
                return details;
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public VendorSetupManagementModel GetCompanyDetails()", "Exception While Getting details of company.", null);
                throw;
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 14-May-2019
        /// Created For : To get all locatio of customer.
        /// </summary>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        public List<LocationDataModel> GetLocationIds(long? VendorId)
        {
            var details = new List<LocationDataModel>();
            try
            {
                if (VendorId > 0)
                {
                    details = _db.LocationCompanyMappings.Where(x => x.LCM_CMP_Id == VendorId).Select(a => new LocationDataModel()
                    {
                        LocationId = a.LCM_LocationId
                       // LocationName = a.LocationMaster.LocationName
                    }).ToList();
                }
                else
                {
                    details = null;
                }
                return details;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public VendorSetupManagementModel GetCompanyDetails()", "Exception While Getting details of company.", null);
                throw;
            }
        }
    }
}
