using System;
using System.Collections.Generic;
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
                return response.Content.ReadAsStringAsync().Result.ToString().Split(',')[1];
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
    }
}
