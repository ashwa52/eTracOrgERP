using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.NewAdminModel.RecruiteeModels.Candidate
{
    public class Reference
    {
        public string type { get; set; }
        public string title { get; set; }
        public string status { get; set; }
        public string slug { get; set; }
        public int position { get; set; }
        public string location { get; set; }
        public string lang_code { get; set; }
        public string kind { get; set; }
        public int id { get; set; }
        public DateTime? updated_at { get; set; }
        public string name { get; set; }
        public bool? locked { get; set; }
        public DateTime? created_at { get; set; }
        public string category { get; set; }
        public List<object> action_templates { get; set; }
    }

    public class OpenQuestionAnswer
    {
        public string question { get; set; }
        public string kind { get; set; }
        public int id { get; set; }
        public string content { get; set; }
    }

    public class OpenQuestionAnswer2
    {
        public string question { get; set; }
        public string kind { get; set; }
        public int id { get; set; }
        public string content { get; set; }
    }

    public class Offer
    {
        public string title { get; set; }
        public string status { get; set; }
        public int id { get; set; }
    }

    public class GroupedOpenQuestionAnswer
    {
        public List<OpenQuestionAnswer2> open_question_answers { get; set; }
        public Offer offer { get; set; }
        public DateTime created_at { get; set; }
    }

    public class Ratings
    {
    }

    public class Placement
    {
        public DateTime updated_at { get; set; }
        public int stage_id { get; set; }
        public int position { get; set; }
        public int offer_id { get; set; }
        public object language { get; set; }
        public int id { get; set; }
        public DateTime created_at { get; set; }
        public int candidate_id { get; set; }
    }

    public class Candidate
    {
        public List<object> referral_referrers_ids { get; set; }
        public List<object> custom_fields { get; set; }
        public string referrer { get; set; }
        public int id { get; set; }
        public int mailbox_messages_count { get; set; }
        public List<string> phones { get; set; }
        public object gdpr_expires_at { get; set; }
        public string photo_url { get; set; }
        public int attachments_count { get; set; }
        public bool upcoming_event { get; set; }
        public List<string> emails { get; set; }
        public List<OpenQuestionAnswer> open_question_answers { get; set; }
        public DateTime last_activity_at { get; set; }
        public bool pending_request_link { get; set; }
        public bool viewed { get; set; }
        public int ratings_count { get; set; }
        public List<GroupedOpenQuestionAnswer> grouped_open_question_answers { get; set; }
        public List<object> admin_ids { get; set; }
        public int tasks_count { get; set; }
        public object admin_id { get; set; }
        public object positive_ratings { get; set; }
        public bool my_pending_result_request { get; set; }
        public object cover_letter { get; set; }
        public object sourcing_origin { get; set; }
        public int notes_count { get; set; }
        public bool unread_notifications { get; set; }
        public string photo_thumb_url { get; set; }
        public string source { get; set; }
        public DateTime updated_at { get; set; }
        public List<object> tags { get; set; }
        public string cv_processing_status { get; set; }
        public string name { get; set; }
        public string adminapp_url { get; set; }
        public bool in_active_share { get; set; }
        public List<object> fields { get; set; }
        public object last_message_at { get; set; }
        public List<object> links { get; set; }
        public string cv_original_url { get; set; }
        public Ratings ratings { get; set; }
        //public double ratings { get; set; }
        public object sourcing_data { get; set; }
        public double rating { get; set; }
        public object my_last_rating { get; set; }
        public DateTime created_at { get; set; }
        public bool my_upcoming_event { get; set; }
        public List<string> sources { get; set; }
        public bool pending_result_request { get; set; }
        public object online_data { get; set; }
        public bool followed { get; set; }
        public List<Placement> placements { get; set; }
        public string cv_url { get; set; }
        public List<object> social_links { get; set; }
        public List<int> duplicates { get; set; }
    }

    public class PerCandidateData
    {
        public List<Reference> references { get; set; }
        public Candidate candidate { get; set; }
    }
}
