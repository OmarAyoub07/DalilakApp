using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DalilakAPI.Models;
using Newtonsoft.Json;

namespace DalilakApp.Services

{
    public class DalilakapiService
    {
        private HttpClient client = new HttpClient();
        private HttpResponseMessage response;
        private Uri uri = null;

        /* Log in function */
        public async Task<string> login(string phone)
        {
            uri = new Uri("http://api.dalilak.pro/Login/user_?phone=" + phone);

            response = await client.PostAsync(uri, null);

            uri = null;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result.ToString();
            }
            return null;
        }

        public async Task<string> image(string id)
        {

            uri = new Uri("http://api.dalilak.pro/Query/PlaceImage_?place_id=" + id);

            response = await client.GetAsync(uri);

            uri = null;

            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsStringAsync().Result.ToString();
            else
                return null;
        }

        public async Task<List<string>> adsImage(string id)
        {

            uri = new Uri("http://api.dalilak.pro/Query/Ads_?city_id=" + id);

            response = await client.GetAsync(uri);

            uri = null;

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<string>>(response.Content.ReadAsStringAsync().Result);
                //return response.Content.ReadAsStringAsync().Result.ToString();
            }
            else
                return null;
        }

        /* Functaion to get RNG */
        public async Task<string> getRNG()
        {
            uri = new Uri("http://api.dalilak.pro/System/RNG_");
            response = await client.GetAsync(uri);
            uri = null;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsStringAsync().Result;
            else
                return null;

        }
        /* function to get user */
        public async Task<User> getUser(string id)
        {
            uri = new Uri("http://api.dalilak.pro/Query/User_?id=" + id);
            response = await client.GetAsync(uri);
            uri = null;

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<User>>(response.Content.ReadAsStringAsync().Result)[0];
            }
            else
                return null;
        }

        /* post user to database */
        public async Task<bool> postUser(string name, string phone, string email)
        {
            uri = new Uri("http://api.dalilak.pro/Insert/NewUser_?name=" + name + "&phone=" + phone + "&email=" + email);
            response = await client.PostAsync(uri, null);
            uri = null;

            if (response.IsSuccessStatusCode)
            {
                return Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
            }
            else
                return false;
        }
        /* Function to get profail image for user */
        public async Task<string> getProfileImage(string id)
        {

            uri = new Uri("http://api.dalilak.pro/Query/ProfileImage_?id=" + id);

            response = await client.GetAsync(uri);

            uri = null;

            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsStringAsync().Result.ToString();
            else
                return null;
        }
        /* get user record */
        public async Task<History> getRecord(string id)
        {
            uri = new Uri("http://api.dalilak.pro/query/UserHistory_?id=" + id);
            response = await client.GetAsync(uri);
            uri = null;

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<History>>(response.Content.ReadAsStringAsync().Result)[0];
            }
            else
                return null;

        }

        /* edit user to database */
        public async Task<bool> UpdateUser(string id, string name, int age, string info, string cityName)
        {
            string tempURI = "http://api.dalilak.pro/Insert/UpdateUser_?id=" + id;

            if (name != "")
                tempURI += "&name=" + name;
            if (age != 0)
                tempURI += "&age=" + age;
            if (info != "")
                tempURI += "&info=" + info;
            if (cityName != "")
                tempURI += "&cityName=" + cityName;

            uri = new Uri(tempURI);
            response = await client.PostAsync(uri, null);

            uri = null;

            if (response.IsSuccessStatusCode)
            {
                return Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
            }
            else
                return false;
        }

        public async Task<List<Place>> GetPlaces(string Cityid, string placeType)
        {
            uri = new Uri("http://api.dalilak.pro/Query/Places_?city_id=" + Cityid+"&place_type="+placeType);

            response = await client.GetAsync(uri);

            uri = null;

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<List<Place>>(response.Content.ReadAsStringAsync().Result);
            else
                return null;
        }

        /* Get Information of one place*/
        public async Task<Place> GetPlace(string place_id)
        {
            uri = new Uri("http://api.dalilak.pro/Query/Place_?palce_id=" + place_id);

            response = await client.GetAsync(uri);

            uri = null;

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<List<Place>>(response.Content.ReadAsStringAsync().Result)[0];
            else
                return null;
        }
        public async Task<List<Reviewer>> GetComments(string place_id)
        {
            uri = new Uri("http://api.dalilak.pro/Query/PlaceComments_?place_id=" + place_id);

            response = await client.GetAsync(uri);

            uri = null;

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<List<Reviewer>>(response.Content.ReadAsStringAsync().Result);
            else
                return null;
        }


        /* Post CV to Admin */
        public async Task<bool> PostCV(string user_id, byte[] file)
        {
            uri = new Uri("http://api.dalilak.pro/Insert/Request_?user_id="+user_id);

            var stringPayload = JsonConvert.SerializeObject(file); // File is the body of the request
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            response = await client.PostAsync(uri, content);

            uri = null;

            if (response.IsSuccessStatusCode)
                return Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
            else
                return false;
        }

        public async Task<bool> PostPlace(string user_id, string[] body, string operation)
        {
            operation = "New Add - "+operation;
            uri = new Uri("http://api.dalilak.pro/Insert/Modification_?user_id="+user_id+"&operation="+operation);

            var stringPayload = JsonConvert.SerializeObject(body); // File is the body of the request
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            response = await client.PostAsync(uri, content);

            uri = null;

            if (response.IsSuccessStatusCode)
                return Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
            else
                return false;
        }

        public async Task<bool> PostProfileImg(string user_id, byte[] img)
        {
            uri = new Uri("http://api.dalilak.pro/Insert/UpdateProfile_?user_id="+user_id);

            var stringPayload = JsonConvert.SerializeObject(img); // File is the body of the request
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            response = await client.PostAsync(uri, content);

            uri = null;

            if (response.IsSuccessStatusCode)
                return Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
            else
                return false;
        }

        public async Task<List<Schedules>> GetScheduals(string user_id)
        {
            uri = new Uri("http://api.dalilak.pro/Query/ListOfSchdls_?user_id=" + user_id);

            response = await client.GetAsync(uri);

            uri = null;

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<List<Schedules>>(response.Content.ReadAsStringAsync().Result);
            else
                return null;
        }

        public async Task<bool> PostSchedules(string user_id, Schedules schedule)
        {
            uri = new Uri("http://api.dalilak.pro/Insert/Schedule_?user_id="+user_id);

            var stringPayload = JsonConvert.SerializeObject(schedule); // File is the body of the request
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            response = await client.PostAsync(uri, content);

            uri = null;

            if (response.IsSuccessStatusCode)
                return Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
            else
                return false;
        }

        public async Task<List<Schedules>> GenerateSchdl(string user_id, string city_id, string fromDate, string toDate, float VisitsRate)
        {
            uri = new Uri(
                "http://api.dalilak.pro/Predict/GenerateSchedule_?user_id=" + user_id 
                +"&cityID=" + city_id + "&fromDate="+fromDate+"&toDate="+toDate+"&crowdRate="+VisitsRate);

            response = await client.GetAsync(uri);

            uri = null;

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<List<Schedules>>(response.Content.ReadAsStringAsync().Result);
            else
                return null;
        } 
        public async Task<string> getCityName(string cityID)
        {
            uri = new Uri( "http://api.dalilak.pro/Query/Cities_");

            response = await client.GetAsync(uri);

            uri = null;

            if (response.IsSuccessStatusCode)
            {
                List<City> cities = JsonConvert.DeserializeObject<List<City>>(response.Content.ReadAsStringAsync().Result);
                return cities.Single(C =>C.id==cityID).name;
            }
           
            else
                return null;
        }

        public async Task<bool> PostModification(string user_id, string[] body)
        {
            string operation = "New Add - ";
            uri = new Uri("http://api.dalilak.pro/Insert/Modification_?operation="+operation+"&user_id="+user_id);

            var stringPayload = JsonConvert.SerializeObject(body);
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            response = await client.PostAsync(uri, content);

            uri = null;

            if (response.IsSuccessStatusCode)
                return Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
            else
                return false;
        }
    }
}
