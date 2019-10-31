using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class CandidateGetModel
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

        public class Ratings
        {
        }

        public class Language
        {
            public string native_name { get; set; }
            public string name { get; set; }
            public string code { get; set; }
        }

        public class Placement
        {
            public DateTime updated_at { get; set; }
            public int stage_id { get; set; }
            public int position { get; set; }
            public int offer_id { get; set; }
            public Language language { get; set; }
            public int id { get; set; }
            public DateTime created_at { get; set; }
            public int candidate_id { get; set; }
        }

        public class Candidate
        {
            public bool viewed { get; set; }
            public DateTime updated_at { get; set; }
            public bool upcoming_event { get; set; }
            public bool unread_notifications { get; set; }
            public int tasks_count { get; set; }
            public string source { get; set; }
            public string referrer { get; set; }
            public int ratings_count { get; set; }
            public Ratings ratings { get; set; }
            public double rating { get; set; }
            public object positive_ratings { get; set; }
            public List<Placement> placements { get; set; }
            public string photo_thumb_url { get; set; }
            public List<string> phones { get; set; }
            public bool pending_result_request { get; set; }
            public int notes_count { get; set; }
            public string name { get; set; }
            public bool my_upcoming_event { get; set; }
            public bool my_pending_result_request { get; set; }
            public object my_last_rating { get; set; }
            public object last_message_at { get; set; }
            public int id { get; set; }
            public bool followed { get; set; }
            public List<string> emails { get; set; }
            public DateTime created_at { get; set; }
            public string adminapp_url { get; set; }
            public object admin_id { get; set; }
        }

        public class CandidateGet
        {
            public List<Reference> references { get; set; }
            public DateTime generated_at { get; set; }
            public List<Candidate> candidates { get; set; }
        }
    }
}
