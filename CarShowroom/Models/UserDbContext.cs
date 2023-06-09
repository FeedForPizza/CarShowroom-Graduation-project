﻿using CarShowroom.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarShowroom.Data
{
    public class UserDbContext: IdentityDbContext<ApplicationUser>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
        {
        }
    }
   
}
