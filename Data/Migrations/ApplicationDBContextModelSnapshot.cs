﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using server.Data;

#nullable disable

namespace server.Data.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    partial class ApplicationDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("server.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("server.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique()
                        .HasFilter("[AccountId] IS NOT NULL");

                    b.HasIndex("CreatedById");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("server.Models.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CheckInTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CheckOutTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GuestId")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("GuestId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("server.Models.BookingRoom", b =>
                {
                    b.Property<int?>("BookingId")
                        .HasColumnType("int");

                    b.Property<int?>("RoomId")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfGuests")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("BookingId", "RoomId");

                    b.HasIndex("RoomId");

                    b.ToTable("BookingRooms");
                });

            modelBuilder.Entity("server.Models.BookingService", b =>
                {
                    b.Property<int?>("BookingId")
                        .HasColumnType("int");

                    b.Property<int?>("ServiceId")
                        .HasColumnType("int");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("BookingId", "ServiceId");

                    b.HasIndex("ServiceId");

                    b.ToTable("BookingServices");
                });

            modelBuilder.Entity("server.Models.Feature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Features");
                });

            modelBuilder.Entity("server.Models.Floor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int");

                    b.Property<string>("FloorNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Floors");
                });

            modelBuilder.Entity("server.Models.Guest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique()
                        .HasFilter("[AccountId] IS NOT NULL");

                    b.ToTable("Guests");
                });

            modelBuilder.Entity("server.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("BookingId")
                        .HasColumnType("int");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PaymentTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("server.Models.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int");

                    b.Property<int?>("FloorId")
                        .HasColumnType("int");

                    b.Property<int?>("RoomClassId")
                        .HasColumnType("int");

                    b.Property<string>("RoomNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("FloorId");

                    b.HasIndex("RoomClassId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("server.Models.RoomClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("BasePrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("RoomClasses");
                });

            modelBuilder.Entity("server.Models.RoomClassFeature", b =>
                {
                    b.Property<int?>("RoomClassId")
                        .HasColumnType("int");

                    b.Property<int?>("FeatureId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("RoomClassId", "FeatureId");

                    b.HasIndex("FeatureId");

                    b.ToTable("RoomClassFeatures");
                });

            modelBuilder.Entity("server.Models.RoomImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RoomId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomImages");
                });

            modelBuilder.Entity("server.Models.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("server.Models.Admin", b =>
                {
                    b.HasOne("server.Models.Account", "Account")
                        .WithOne("Admin")
                        .HasForeignKey("server.Models.Admin", "AccountId");

                    b.HasOne("server.Models.Admin", "CreatedBy")
                        .WithMany("CreatedAdmins")
                        .HasForeignKey("CreatedById");

                    b.Navigation("Account");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("server.Models.Booking", b =>
                {
                    b.HasOne("server.Models.Guest", "Guest")
                        .WithMany("Bookings")
                        .HasForeignKey("GuestId");

                    b.Navigation("Guest");
                });

            modelBuilder.Entity("server.Models.BookingRoom", b =>
                {
                    b.HasOne("server.Models.Booking", "Booking")
                        .WithMany("BookingRooms")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("server.Models.Room", "Room")
                        .WithMany("BookingRooms")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("server.Models.BookingService", b =>
                {
                    b.HasOne("server.Models.Booking", "Booking")
                        .WithMany("BookingServices")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("server.Models.Service", "Service")
                        .WithMany("BookingServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("server.Models.Feature", b =>
                {
                    b.HasOne("server.Models.Admin", "CreatedBy")
                        .WithMany("CreatedFeatures")
                        .HasForeignKey("CreatedById");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("server.Models.Floor", b =>
                {
                    b.HasOne("server.Models.Admin", "CreatedBy")
                        .WithMany("CreatedFloors")
                        .HasForeignKey("CreatedById");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("server.Models.Guest", b =>
                {
                    b.HasOne("server.Models.Account", "Account")
                        .WithOne("Guest")
                        .HasForeignKey("server.Models.Guest", "AccountId");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("server.Models.Payment", b =>
                {
                    b.HasOne("server.Models.Booking", "Booking")
                        .WithMany("Payments")
                        .HasForeignKey("BookingId");

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("server.Models.Room", b =>
                {
                    b.HasOne("server.Models.Admin", "CreatedBy")
                        .WithMany("CreatedRooms")
                        .HasForeignKey("CreatedById");

                    b.HasOne("server.Models.Floor", "Floor")
                        .WithMany("Rooms")
                        .HasForeignKey("FloorId");

                    b.HasOne("server.Models.RoomClass", "RoomClass")
                        .WithMany("Rooms")
                        .HasForeignKey("RoomClassId");

                    b.Navigation("CreatedBy");

                    b.Navigation("Floor");

                    b.Navigation("RoomClass");
                });

            modelBuilder.Entity("server.Models.RoomClass", b =>
                {
                    b.HasOne("server.Models.Admin", "CreatedBy")
                        .WithMany("CreatedRoomClasses")
                        .HasForeignKey("CreatedById");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("server.Models.RoomClassFeature", b =>
                {
                    b.HasOne("server.Models.Feature", "Feature")
                        .WithMany("RoomClassFeatures")
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("server.Models.RoomClass", "RoomClass")
                        .WithMany("RoomClassFeatures")
                        .HasForeignKey("RoomClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Feature");

                    b.Navigation("RoomClass");
                });

            modelBuilder.Entity("server.Models.RoomImage", b =>
                {
                    b.HasOne("server.Models.Room", "Room")
                        .WithMany("Images")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Room");
                });

            modelBuilder.Entity("server.Models.Service", b =>
                {
                    b.HasOne("server.Models.Admin", "CreatedBy")
                        .WithMany("CreatedServices")
                        .HasForeignKey("CreatedById");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("server.Models.Account", b =>
                {
                    b.Navigation("Admin");

                    b.Navigation("Guest");
                });

            modelBuilder.Entity("server.Models.Admin", b =>
                {
                    b.Navigation("CreatedAdmins");

                    b.Navigation("CreatedFeatures");

                    b.Navigation("CreatedFloors");

                    b.Navigation("CreatedRoomClasses");

                    b.Navigation("CreatedRooms");

                    b.Navigation("CreatedServices");
                });

            modelBuilder.Entity("server.Models.Booking", b =>
                {
                    b.Navigation("BookingRooms");

                    b.Navigation("BookingServices");

                    b.Navigation("Payments");
                });

            modelBuilder.Entity("server.Models.Feature", b =>
                {
                    b.Navigation("RoomClassFeatures");
                });

            modelBuilder.Entity("server.Models.Floor", b =>
                {
                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("server.Models.Guest", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("server.Models.Room", b =>
                {
                    b.Navigation("BookingRooms");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("server.Models.RoomClass", b =>
                {
                    b.Navigation("RoomClassFeatures");

                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("server.Models.Service", b =>
                {
                    b.Navigation("BookingServices");
                });
#pragma warning restore 612, 618
        }
    }
}
