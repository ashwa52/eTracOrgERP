using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.NewAdminModel
{
   
        public class Stage
        {
            public int position { get; set; }
            public string name { get; set; }
            public int id { get; set; }
            public string category { get; set; }
        }

        public class Admin
        {
            public string timezone { get; set; }
            public string role_name { get; set; }
            public int role_id { get; set; }
            public List<string> role_abilities { get; set; }
            public string photo_thumb_url { get; set; }
            public string photo_normal_url { get; set; }
            public string last_name { get; set; }
            public int id { get; set; }
            public string first_name { get; set; }
            public string email { get; set; }
        }

        public class Follower
        {
            public string timezone { get; set; }
            public string role_name { get; set; }
            public int role_id { get; set; }
            public List<string> role_abilities { get; set; }
            public string photo_thumb_url { get; set; }
            public string photo_normal_url { get; set; }
            public string last_name { get; set; }
            public int id { get; set; }
            public string first_name { get; set; }
            public string email { get; set; }
        }

        public class Offer
        {
            public int candidates_count { get; set; }
            public bool pipeline { get; set; }
            public bool email_confirmation { get; set; }
            public object employment_type { get; set; }
            public int max_hours { get; set; }
            public string email_confirmation_body { get; set; }
            public object country_code { get; set; }
            public bool followed { get; set; }
            public bool has_additional_info { get; set; }
            public int attachments_count { get; set; }
            public string status { get; set; }
            public string options_cv { get; set; }
            public int notes_count { get; set; }
            public string slug { get; set; }
            public string department { get; set; }
            public List<object> fields { get; set; }
            public string kind { get; set; }
            public object state_code { get; set; }
            public DateTime updated_at { get; set; }
            public int position { get; set; }
            public int min_hours { get; set; }
            public bool remote { get; set; }
            public string url { get; set; }
            public object requirements { get; set; }
            public DateTime created_at { get; set; }
            public int id { get; set; }
            public string options_photo { get; set; }
            public object published_at { get; set; }
            public List<Stage> stages { get; set; }
            public string location { get; set; }
            public string options_phone { get; set; }
            public List<Admin> admins { get; set; }
            public object experience { get; set; }
            public string options_cover_letter { get; set; }
            public object city { get; set; }
            public object postal_code { get; set; }
            public int qualified_candidates_count { get; set; }
            public string adminapp_url { get; set; }
            public object education { get; set; }
            public string description { get; set; }
            public string mailbox_email { get; set; }
            public object fieldset_id { get; set; }
            public int disqualified_candidates_count { get; set; }
            public string careers_url { get; set; }
            public string email_confirmation_subject { get; set; }
            public List<object> offer_tags { get; set; }
            public List<string> visibility_options { get; set; }
            public object category { get; set; }
            public string title { get; set; }
            public List<Follower> followers { get; set; }
        }

        public class RootObjectData
        {
            public Offer offer { get; set; }
        }
    
}

