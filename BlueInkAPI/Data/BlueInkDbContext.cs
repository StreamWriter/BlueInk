using BlueInk.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueInk.API.Data
{
    public class BlueInkDbContext : DbContext
    {
        public BlueInkDbContext(DbContextOptions<BlueInkDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectType> ProjectTypes { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }
        public DbSet<OwnerData> OwnerData { get; set; }

    }
}
