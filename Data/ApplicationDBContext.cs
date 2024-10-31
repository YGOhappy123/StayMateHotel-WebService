using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<AddOn> AddOns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().Property(acc => acc.Role).HasConversion<string>();
            modelBuilder.Entity<Admin>().Property(ad => ad.Gender).HasConversion<string>();
            modelBuilder.Entity<Room>().Property(rm => rm.Status).HasConversion<string>();
        }
    }
}
