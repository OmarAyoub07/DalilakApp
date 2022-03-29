﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
            uri = new Uri("http://api.dalilak.pro/Login/user_?phone="+phone);

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
            
            uri = new Uri("http://api.dalilak.pro/Query/PlaceImage_?place_id="+id);

            response = await client.GetAsync(uri);

            uri = null;

            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsStringAsync().Result.ToString().Split(',')[1];
            else
                return null;
        }
        /* Functaion to get RNG */
        public async  Task<string> getRNG()
        {
            uri = new Uri("http://api.dalilak.pro/System/RNG_");
            response = await client.GetAsync(uri);
            uri = null ;
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
            }else
                return null;



        }
        
    }
}
