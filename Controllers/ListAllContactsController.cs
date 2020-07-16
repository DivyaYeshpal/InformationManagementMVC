
using InformationManagementMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace InformationManagementMVC.Controllers
{
    public class ListAllContactsController : Controller
    {
        // GET: ListAllContacts
        public  ActionResult Index(int? pageIndex)
        {
            int pageSize = 10;
            pageIndex = pageIndex.HasValue ? Convert.ToInt32(pageIndex) : 1;
            IList<CallList> cll1 = null;
            string baseAddress = "http://localhost:62829";
            string AccessToken = HttpContext.Session["AccessToken"].ToString();
            double UserID = double.Parse(HttpContext.Session["UserID"].ToString());
            using (HttpClient httpClient1 = new HttpClient())
            {
                httpClient1.BaseAddress = new Uri(baseAddress);
                httpClient1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", AccessToken);
                var responseTask = httpClient1.GetAsync("api/ListAllContacts/GetCallList?pageNumber=" + pageIndex.ToString());
                // New code:
                if (responseTask.Result.IsSuccessStatusCode)
                {

                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<CallList>>();
                        cll1 = readTask.Result;
                        return View(cll1.ToPagedList(pageIndex??1,pageSize));
                    }
                    else
                    {
                        cll1 = null;
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                        return View(cll1.ToPagedList(pageIndex??1,pageSize));
                    }
                }
            }
            return View(cll1);
        }
    }
}