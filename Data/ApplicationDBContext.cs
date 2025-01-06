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

        // Auth Related Tables
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Admin> Admins { get; set; }

        // Hotel Facilities Related Tables
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<RoomClass> RoomClasses { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Service> Services { get; set; }

        // Booking Related Tables
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }

        // Many-To-Many Join Tables
        public DbSet<BookingRoom> BookingRooms { get; set; }
        public DbSet<BookingService> BookingServices { get; set; }
        public DbSet<RoomClassFeature> RoomClassFeatures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().Property(acc => acc.Role).HasConversion<string>();
            modelBuilder.Entity<Room>().Property(rm => rm.Status).HasConversion<string>();
            modelBuilder.Entity<Booking>().Property(bk => bk.Status).HasConversion<string>();
            modelBuilder.Entity<BookingService>().Property(bks => bks.Status).HasConversion<string>();
            modelBuilder.Entity<Payment>().Property(pm => pm.Method).HasConversion<string>();

            modelBuilder.Entity<BookingRoom>().HasKey(br => new { br.BookingId, br.RoomId });
            modelBuilder.Entity<BookingRoom>().HasOne(br => br.Booking).WithMany(bk => bk.BookingRooms).HasForeignKey(br => br.BookingId);
            modelBuilder.Entity<BookingRoom>().HasOne(br => br.Room).WithMany(rm => rm.BookingRooms).HasForeignKey(br => br.RoomId);

            modelBuilder.Entity<BookingService>().HasKey(bs => new { bs.BookingId, bs.ServiceId });
            modelBuilder
                .Entity<BookingService>()
                .HasOne(bs => bs.Booking)
                .WithMany(bk => bk.BookingServices)
                .HasForeignKey(bs => bs.BookingId);
            modelBuilder
                .Entity<BookingService>()
                .HasOne(bs => bs.Service)
                .WithMany(sv => sv.BookingServices)
                .HasForeignKey(bs => bs.ServiceId);

            modelBuilder.Entity<RoomClassFeature>().HasKey(rcf => new { rcf.RoomClassId, rcf.FeatureId });
            modelBuilder
                .Entity<RoomClassFeature>()
                .HasOne(rcf => rcf.RoomClass)
                .WithMany(rc => rc.RoomClassFeatures)
                .HasForeignKey(rcf => rcf.RoomClassId);
            modelBuilder
                .Entity<RoomClassFeature>()
                .HasOne(rcf => rcf.Feature)
                .WithMany(ft => ft.RoomClassFeatures)
                .HasForeignKey(rcf => rcf.FeatureId);

            modelBuilder
                .Entity<Room>()
                .HasMany(rm => rm.Images)
                .WithOne(ri => ri.Room)
                .HasForeignKey(ri => ri.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
