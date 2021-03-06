﻿using System;
using System.Net.Http;
using System.Web.Mvc;
using Presentation.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Presentation.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Home()
        {
            if (Session["userToken"] == null)
                return RedirectToAction("Login", "Login");

            HttpClient client = MVCUtils.GetClient(Session["userToken"].ToString());

            ApplicationUserViewModel appUser = JsonConvert.DeserializeObject<ApplicationUserViewModel>(client.GetStringAsync("api/ApplicationUser/GetLoggedUser").Result);
            return View(appUser);
        }

        public ActionResult ListUsers(int offset, int quantity)
        {
            HttpClient client = MVCUtils.GetClient("");

            if (offset < 0)
                offset = 0;

            TempData["offset"] = offset;

            IEnumerable<ApplicationUserViewModel> appUsers = JsonConvert.DeserializeObject<IEnumerable<ApplicationUserViewModel>>(client.GetStringAsync($"api/ApplicationUser/GetUsersByRange?offset={offset}&quantity={quantity}").Result);

            if (appUsers.Count() == 0)
                TempData["errorMsg"] = "Fim da lista de usuários";

            return View(appUsers);
        }

        [HttpPost]
        public ActionResult ListFilteredUsers(string pSearch)
        {
            HttpClient client = MVCUtils.GetClient("");
            IEnumerable<ApplicationUserViewModel> appUsers = JsonConvert.DeserializeObject<IEnumerable<ApplicationUserViewModel>>(client.GetStringAsync($"api/ApplicationUser/GetUsersByName?userName={pSearch}").Result);

            return View(appUsers);
        }


        public ActionResult AcceptFriendshipRequest(string toUserId, string notificationId, string notificationMessage)
        {
            if (Session["userToken"] == null)
                return RedirectToAction("Login", "Login");

            HttpClient client = MVCUtils.GetClient(Session["userToken"].ToString());

            JObject requestBody = new JObject();
            requestBody["toUserId"] = toUserId;
            requestBody["notificationId"] = notificationId;

            //"..aceitou seu pedido de amizade"
            if (notificationMessage.Contains("amizade"))
            {
                HttpResponseMessage httpMessage = client.PostAsJsonAsync("api/Notification/DeleteNotification", requestBody).Result;
                return Redirect("Home");
            }

            var j = client.PostAsJsonAsync("api/ApplicationUser/AcceptUserFriendship", requestBody).Result;
            
            return Redirect("Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUserPost(PostViewModel pvm)
        {
            if (pvm.Content == null)
            {
                TempData["IsPostEmpty"] = true;
                return RedirectToAction("Home", "User");
            }
            pvm.PostTime = DateTime.Now;
            HttpClient client = MVCUtils.GetClient(Session["userToken"].ToString());
            var POSTResult = client.PostAsJsonAsync("api/ApplicationUser/CreateUserPost", pvm).Result;
            return RedirectToAction("Home", "User");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}