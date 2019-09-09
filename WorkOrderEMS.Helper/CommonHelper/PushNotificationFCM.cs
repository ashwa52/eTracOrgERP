using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace WorkOrderEMS.Helper
{
    public class PushNotificationFCM
    {
            //public static string FCMAndroid(string alertmessage, string deviceId, object Data)
            //{

            //    string SERVER_API_KEY = "AIzaSyCdJnLkfUWJ9mT6LSMbWZ5fhSaKzyhwklk";
            //    var SENDER_ID = "182928006939";
            //    string regIds = string.Join("\",\"", deviceId);
            //    //NotificationMessage nm = new NotificationMessage();
            //    //nm.Message = alertmessage;
            //    //nm.Data = Data;
            //    var value = new JavaScriptSerializer().Serialize(Data);
            //    WebRequest tRequest;
            //    tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            //    tRequest.Method = "post";
            //    tRequest.ContentType = "application/json";
            //    tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_API_KEY));

            //    tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

            //    string postData = "{\"collapse_key\":\"score_update\",\"time_to_live\":108,\"delay_while_idle\":true,\"data\": { \"message\" : " + value + ",\"time\": " + "\"" + System.DateTime.Now.ToString() + "\"},\"registration_ids\":[\"" + regIds + "\"]}";
            //    Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            //    tRequest.ContentLength = byteArray.Length;
            //    Stream dataStream = tRequest.GetRequestStream();
            //    dataStream.Write(byteArray, 0, byteArray.Length);
            //    dataStream.Close();
            //    WebResponse tResponse = tRequest.GetResponse();
            //    dataStream = tResponse.GetResponseStream();
            //    StreamReader tReader = new StreamReader(dataStream);
            //    String sResponseFromServer = tReader.ReadToEnd();
            //    tReader.Close();
            //    dataStream.Close();
            //    tResponse.Close();
            //    return sResponseFromServer;
            //}
          
        public static string FCMAndroid(string alertmessage, string deviceId, object Data)
        {
            string response;
            try
            {
                // From: https://console.firebase.google.com/project/x.y.z/settings/general/android:x.y.z

                // Projekt-ID: x.y.z
                // Web-API-Key: A...Y (39 chars)
                // App-ID: 1:...:android:...

                // From https://console.firebase.google.com/project/x.y.z/settings/
                // cloudmessaging/android:x,y,z
                // Server-Key: AAAA0...    ...._4

                //var applicationID = "AIzaSyD0Syee6jGXHkXy76gpb9xlp_bBw_TuMK0";
                //var SENDER_ID = "182928006939";
                string serverKey = "AAAACoawrgM:APA91bFNYOXFwfA7TDml1gKpg4LQoGNmyu5BytyuyaRnwiDF-_txYKf0AuyrpO7zQsV1ezSGmijWp3czSAeNRvHtPaE6oL9jZDQEPHL7BqgnzpRZV3i7Wnvye_EjLUCfNi-upXxVTkCd";
                string senderId = "45209398787";
                    //Correct Id
                //string serverKey = "AIzaSyD0Syee6jGXHkXy76gpb9xlp_bBw_TuMK0"; // Something very long
                //string senderId = "182928006939";

                // Also something very long, 
                // got from android
                //string deviceId = "//topics/all";             // Use this to notify all devices, 
                // but App must be subscribed to 
                // topic notification
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");

                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = deviceId,
                    //data= Data,
                    //title = alertmessage,
                    //body= Data,
                    //sound = "Enabled",
                    //message = alertmessage,
                    priority = "high",
                    //notification = new
                    //{
                    //    data = Data,
                    //    title = alertmessage,
                    //    sound = "Enabled"
                    //}
                    //body = Data,
                    notification = new
                    {
                        body = Data,
                        title = "Elite Parking Services",                      
                        sound = "Enabled"
                    }
                };


                //string postData = "{collapse_key:score_update","time_to_live:108,delay_while_idle:true,data: { "message" : " + value + ","time": " + """ + System.DateTime.Now.ToString() + ""},"registration_ids":["" + regIds + ""]}";
                //    string postData = "{collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message="
                //        + alertmessage + "&data.time=" + System.DateTime.UtcNow.ToString() + "&data.details=" + Data + "&registration_id=" + deviceId + "}";

                //    Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                //    tRequest.ContentLength = byteArray.Length;

                //    Stream dataStream = tRequest.GetRequestStream();
                //    dataStream.Write(byteArray, 0, byteArray.Length);
                //    dataStream.Close();

                //    WebResponse tResponse = tRequest.GetResponse();

                //    dataStream = tResponse.GetResponseStream();

                //    StreamReader tReader = new StreamReader(dataStream);

                //     response = tReader.ReadToEnd();

                //    tReader.Close();
                //    dataStream.Close();
                //    tResponse.Close();
                //}

                //catch (Exception ex)
                //{
                //    response = ex.Message;
                //}
                //return response;




                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                response = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;





            //if (deviceId != null)
            //{
            //    //if (deviceId.Length > 120)
            //    //{
            //    // your RegistrationID and DeviceId both are same which is received from GCM server.                                                              
            //    var applicationID = "AIzaSyD0Syee6jGXHkXy76gpb9xlp_bBw_TuMK0";
            //    var SENDER_ID = "182928006939";   // SENDER_ID is nothing but your ProjectID (from API Console- google code)//                                         
            //    //var applicationID = "AIzaSyDwUYDu7255IR3266k3AYjWErp4bCeeGl8";// applicationID means google Api key                                                                                                    
            //    //var SENDER_ID = "1064381594331";
            //    WebRequest tRequest;
            //    tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            //    tRequest.Method = "POST";
            //    tRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            //    tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
            //    tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

            //    //string postData = "data.Alert=" + alertmessage + "&data.action-loc-key=" + actionKey + " &registration_id=" + deviceId + "&badge=" + (1).ToString() + "";
            //    var Details = JsonConvert.SerializeObject(Data);

            //    string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message="
            //        + alertmessage + "&data.time=" + System.DateTime.UtcNow.ToString() + "&data.details=" + Details + "&registration_id=" + deviceId + "";
            //    try
            //    {
            //        Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            //        tRequest.ContentLength = byteArray.Length;
            //        Stream dataStream = tRequest.GetRequestStream();
            //        dataStream.Write(byteArray, 0, byteArray.Length);
            //        dataStream.Close();
            //        WebResponse tResponse = tRequest.GetResponse();

            //        dataStream = tResponse.GetResponseStream();
            //        StreamReader tReader = new StreamReader(dataStream);
            //        String sResponseFromServer = tReader.ReadToEnd();   //Get response from GCM server.

            //        tReader.Close();
            //        dataStream.Close();
            //        tResponse.Close();
            //        return "success";
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //    //}
            //    //else
            //    //{
            //    //    return "failed";
            //    //}
            //}
            //else
            //{
            //    return "failed";
            //}
        }
    }    
}
