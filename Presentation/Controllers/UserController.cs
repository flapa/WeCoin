﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web;
using AutoMapper;
using Presentation.ViewModels;
using Newtonsoft.Json;

namespace Presentation.Controllers
{
    public class UserController : Controller
    {
        private const string BASE_API_ADDRESS = "http://localhost:2539";

        public ActionResult Home()
        {
            //if (Session["userToken"] == null)
            //    return RedirectToAction("Login");

            //return View();

            //TODO: a partir da lista de usuários, carregar posts relativos ao usuário logado

            if (Session["userToken"] == null)
                return RedirectToAction("Login", "Login");

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASE_API_ADDRESS);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["userToken"].ToString());
            
            var y = client.GetStringAsync("api/ApplicationUser/GetLoggedUser").Result;

            //IEnumerable<ApplicationUserViewModel> appUsers = JsonConvert.DeserializeObject<IEnumerable<ApplicationUserViewModel>>(client.GetStringAsync("api/ApplicationUser").Result);
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(ApplicationUserViewModel profile, HttpPostedFileBase profilePhoto)
        {
            if (ModelState.IsValid)
            {
                //---- Upload da Foto ----
                profile.ImgUrl = Services.BlobService.GetInstance()
                    .UploadFile("simplesocialnetwork", profile.Id, profilePhoto.InputStream, profilePhoto.ContentType);
                //------------------------
                //HttpClient 
                return RedirectToAction("Index");
            }

            return View(profile);
        }
        [HttpPost]
        public ActionResult CreatePost(PostViewModel pvm)
        {
            pvm.PostTime = DateTime.Now;
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Home", "Home");
            }
            catch
            {
                return View();
            }
        }
    }
}