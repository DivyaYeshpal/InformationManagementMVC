using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;

namespace InformationManagementMVC
{
    public class TokenGeneration
    {
        public string TokenVal()
        {
            string baseAddress = "http://localhost:62829/api/Login";
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
               {
                   {"grant_type", "password"},
                   {"username", "divya"},
                   {"password", "Advaith"},
               };
                var tokenResponse = client.PostAsync(baseAddress + "/oauth/token", new FormUrlEncodedContent(form)).Result;
                var token = tokenResponse.Content.ReadAsAsync<Token>().Result;
                //var token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                //if (string.IsNullOrEmpty(token.Error))
                //{
                //    Console.WriteLine("Token issued is: {0}", token.AccessToken);
                //}
                //else
                //{
                //    Console.WriteLine("Error : {0}", token.Error);
                //}
                //Console.Read();
                if (string.IsNullOrEmpty(token.AccessToken))
                {
                    return token.AccessToken.ToString();
                }
                else
                {
                    return "0";
                }
            }
        }
    }
}