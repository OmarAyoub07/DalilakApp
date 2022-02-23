using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DalilakApp.Services
{
    public class DalilakapiService
    {
        public async Task<bool> login(string phone)
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri("http://api.dalilak.pro/Login/user_?phone="+phone);

            HttpResponseMessage response = await client.PostAsync(uri, null);
            
           // if (response.IsSuccessStatusCode)
            //{
               return Convert.ToBoolean(response.Content.ReadAsStringAsync().Result.ToString());
            //}
            //return null;
}
    }
}
