using IdentityModel.Client;
using InformationManagementMVC.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using ZendeskApi_v2.Models.Articles;

namespace InformationManagementMVC.Controllers
{
    public class LoginController : Controller
    {
        //private object token;
        public ActionResult LoginPage()       
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginPage(RegisterUser reg)
        {
            IEnumerable<RegisterUser> RegUsr = null;
            string baseAddress = "http://localhost:62829";
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
               {
                   {"grant_type", "password"},
                   {"username", reg.UserName},
                   {"password", reg.PassWord}
               };
                int regid =Convert.ToInt32(TempData["Regid"]);
                var tokenResponse = client.PostAsync(baseAddress + "/token", new FormUrlEncodedContent(form)).Result;
                var token = tokenResponse.Content.ReadAsAsync<Token>().Result;
                using (HttpClient httpClient1 = new HttpClient())
                {
                    httpClient1.BaseAddress = new Uri(baseAddress);
                    httpClient1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.AccessToken);
                    HttpResponseMessage response= httpClient1.GetAsync("api/Login/LoginAllUsers").Result;
                    var responseTask = httpClient1.GetAsync("api/Login/LoginAllUsers");
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<RegisterUser>>();
                        readTask.Wait();

                        RegUsr = readTask.Result;
                        System.Console.WriteLine("Success");
                        string message = response.Content.ReadAsStringAsync().Result;
                        var json1 = JsonConvert.DeserializeObject(message);
                        Session["AccessToken"] = token.AccessToken;
                        if (Session["AccessToken"] != null)
                        {
                            double uid = 0;
                            foreach (var u in RegUsr)
                            {
                                uid = u.Id;
                                Session["UserID"] = u.Id;
                            }
                            return RedirectToAction("Index", "CallList", new { UserID = Session["UserID"] });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Please login with correct UserName and Password");
                    }
                }
                //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                return View(reg);
            }
        }
        public ActionResult Logout()
        {
            Session["AccessToken"] = null;
            Session["UserID"] = null;
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}