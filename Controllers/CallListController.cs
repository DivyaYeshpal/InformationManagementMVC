using InformationManagementMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace InformationManagementMVC.Controllers
{
    public class CallListController : Controller
    {

        // GET: CallList
        public ActionResult Index(int? UserId,int? page)
        {
            int pageSize = 10;
            page = page.HasValue ? Convert.ToInt32(page) : 1;
            IEnumerable<CallList> cll1 = null;
            string baseAddress = "http://localhost:62829";
            string AccessToken = "";
            double UserID=0;
            if (Session["AccessToken"] != null)
            {
                AccessToken =  HttpContext.Session["AccessToken"].ToString();
            }
            else
            {
                return RedirectToAction("LoginPage", "Login");
            }
            
            if (Session["UserID"] != null)
            {
                UserID = double.Parse(HttpContext.Session["UserID"].ToString());
            }
            else
            {
                return RedirectToAction("LoginPage", "Login");
            }
            using (HttpClient httpClient1 = new HttpClient())
            {
                httpClient1.BaseAddress = new Uri(baseAddress);
                httpClient1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", AccessToken);
                HttpResponseMessage response = httpClient1.GetAsync("api/CallList/GetCallList?UserID=" + UserId.ToString()).Result;
                var responseTask = httpClient1.GetAsync("api/CallList/GetCallList?UserID=" + UserId.ToString());
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<CallList>>();
                    readTask.Wait();
                    cll1 = readTask.Result;
                    foreach (var u in cll1)
                    {
                        u.UserID = UserID;
                        u.accessToken = AccessToken;
                    }
                    return View(cll1.ToList().ToPagedList(page??1,pageSize));
                }
                else
                {
                    cll1 = null;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View(cll1.ToList().ToPagedList(page ?? 1, pageSize));
                }
            }
        }
        public ActionResult create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult create(CallList cll)
        {
            string baseAddress = "http://localhost:62829";
            string AccessToken = HttpContext.Session["AccessToken"].ToString();
            double UserID = Convert.ToDouble(HttpContext.Session["UserID"].ToString());
            cll.UserID = UserID;
            using (HttpClient httpClient1 = new HttpClient())
            {
                httpClient1.BaseAddress = new Uri(baseAddress);
                httpClient1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", AccessToken);
                var postTask = httpClient1.PostAsJsonAsync<CallList>("api/CallList/SaveCallList", cll);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Saved Successfully");
                    TempData["success"] = "Saved Successfully";
                    return RedirectToAction("Index", "CallList", new { UserID = UserID });
                }
                else
                {
                    if (result.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        ModelState.AddModelError(string.Empty, "CallList already exist");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                    return View(cll);
                }
            }
        }
        [HttpGet]
        public ActionResult Edit(double Id)
        {
            CallList cll = null;
            string baseAddress = "http://localhost:62829";
            string AccessToken = HttpContext.Session["AccessToken"].ToString();
            double UserID = Convert.ToDouble(HttpContext.Session["UserID"].ToString());
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:62829/api/");
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", AccessToken);
                HttpResponseMessage response = client.GetAsync("api/CallList/GetCalllistByid?Id=" + Id.ToString()).Result;
                var responseTask = client.GetAsync("api/CallList/GetCalllistByid?Id=" + Id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<CallList>();
                    readTask.Wait();
                    cll = readTask.Result;
                    cll.UserID = UserID;
                    cll.accessToken = AccessToken;
                }
                else
                {
                    if(result.StatusCode==System.Net.HttpStatusCode.Forbidden)
                    {
                        ModelState.AddModelError("Error", "Edit is possible only for SuperAdmin and Admin");
                        TempData["error"] = "Edit is possible only for SuperAdmin and Admin";
                        return RedirectToAction("Index", "CallList", new { UserID = UserID });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                    }
                }
            }
            return View(cll);
        }

        [HttpPost]
        public ActionResult Edit(CallList cll)
        {
            string baseAddress = "http://localhost:62829";
            string AccessToken = HttpContext.Session["AccessToken"].ToString();
            double UserID = Convert.ToDouble(HttpContext.Session["UserID"].ToString());
            cll.UserID = UserID;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", AccessToken);
                var putTask = client.PutAsJsonAsync<CallList>("api/CallList/EditCallList", cll);
                putTask.Wait();
                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Updated Successfully");
                    TempData["success"] = "Updated Successfully";
                    return RedirectToAction("Index", "CallList", new { UserID = UserID });
                }
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                return View(cll);
            }
        }
    }
}
