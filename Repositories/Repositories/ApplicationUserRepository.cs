﻿using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using System.Linq;

namespace Repositories.Repositories
{
    public class ApplicationUserRepository : BaseRepository<ApplicationUser>, IApplicationUserRepository
    {
        public IEnumerable<ApplicationUser> GetUsersByName(string userName)
        {
            return Rc.Users.Where(au => au.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }
    }
}