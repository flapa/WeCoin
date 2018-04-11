﻿using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using System.Linq;

namespace Repositories.Repositories
{
    public class ApplicationUserRepository : BaseRepository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUser GetUserById(string id)
        {
            return Rc.Users.SingleOrDefault(u => u.Id.Equals(id));
        }

        //utilizável com uma lista de procura AJAX
        public IEnumerable<ApplicationUser> GetUsersByName(string userName)
        {
            return Rc.Users.Where(u => u.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public void EditUser(ApplicationUser appUser)
        {
            ApplicationUser applicationUser = GetUserById(appUser.Id);
            applicationUser.Name = appUser.Name;
            applicationUser.Email = appUser.Email;
            applicationUser.ImgUrl = appUser.ImgUrl;
            Rc.SaveChanges();
        }

        public void CreateUserPost(string userId, Post post)
        {
            Rc.Users.SingleOrDefault(u => u.Id == userId).Posts.Add(post);
        }
    }
}