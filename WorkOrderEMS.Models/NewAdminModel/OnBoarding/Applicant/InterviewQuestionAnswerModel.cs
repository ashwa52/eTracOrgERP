using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.NewAdminModel
{
    public class InterviewQuestionAnswerModel
    {
        public List<ChildrenQuestionModel> ChildrenQuestionModel { get; set; }
        public long MasterId { get; set; }
        public string MasterQuestion { get; set; }
        public string selectAnswer { get; set; }
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string Comment31 { get; set; }
        public string Comment32 { get; set; }
        public string Comment5 { get; set; }
        public long ApplicantId { get; set; }
        public string IsExempt { get; set; }

    }
    public class ChildrenQuestionModel
    {
        public long IQM_Id { get; set; }
        public string IQM_Question { get; set; }
        public int? IQM_ScoreYes { get; set; }
        public int? IQM_ScoreNo { get; set; }
    }
    public class AnswerModel
    {
        public string ApplicantImage { get; set; }
        public string ApplicantName { get; set; }
        public List<ListAnswerModel> ListAnswerModel { get; set; }
    }
    public class ListAnswerModel
    {
        public List<ChildQuestionAnswerModel> ListAnswerMainModel { get; set; }
        public string IQM_Question { get; set; }
        public long IQM_Id { get; set; }
    }
    public class ChildQuestionAnswerModel
    {
        public long? IQM_Id { get; set; }
        public string IQM_Question { get; set; }
        public string IQM_Answer { get; set; }
        public string IQM_Comment { get; set; }
    }
}
