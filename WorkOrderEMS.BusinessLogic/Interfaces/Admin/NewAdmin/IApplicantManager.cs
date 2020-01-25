using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface IApplicantManager
    {
        ServiceResponseModel<eTracLoginModel> ValidateApplicant(eTracLoginModel obj);
        ServiceResponseModel<eTracLoginModel> ForgotPassword(eTracLoginModel obj);
        ServiceResponseModel<eTracLoginModel> SetForgotPassword(eTracLoginModel obj);
        ServiceResponseModel<string> SignUpApplicant(eTracLoginModel obj);
        ServiceResponseModel<string> ChangePassword(eTracLoginModel obj);
        ServiceResponseModel<string> CheckLoginId(eTracLoginModel obj);
        bool SaveAssets(AssetsAllocationModel model);
        bool SendOffer(OfferModel model);
        bool SaveApplicantData(CommonApplicantModel Obj);
    }
}
