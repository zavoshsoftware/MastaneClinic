﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Models
{
   public class DatabaseContext:DbContext
    {
        static DatabaseContext()
        {
            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Migrations.Configuration>());
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogGroup> BlogGroups { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<VisitRequest> VisitRequests { get; set; }
        public DbSet<Expert> Experts { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<TextItem> TextItems { get; set; }
        public DbSet<TextItemType> TextItemTypes { get; set; }
        public DbSet<ContactUsForm> ContactUsForms { get; set; } 
        public DbSet<ServiceBlog> ServiceBlogs { get; set; } 
    }
}
