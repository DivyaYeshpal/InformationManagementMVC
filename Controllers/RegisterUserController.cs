using InformationManagementMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace InformationManagementMVC.Controllers
{
    public class RegisterUserController : Controller
    {
        // GET: RegisterUser
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult create(RegisterUser reg)
        {
            string baseAddress = "http://localhost:62829";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                var postTask = client.PostAsJsonAsync<RegisterUser>("api/RegisterUser/EditRegList", reg);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    TempData["success"] = "Registered Successfully, Please enter your UserName and Password for login";
                    TempData["Regid"] = reg._id;
                    return RedirectToAction("LoginPage", "Login", new { _id = reg._id });
                }
                else
                {
                    if (result.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        ModelState.AddModelError(string.Empty, "User already exist");
                        return View(reg);
                    }
                }
                }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(reg);
        }
    }
}