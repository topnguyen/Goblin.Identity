﻿// <auto-generated />
using System;
using Goblin.Identity.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Goblin.Identity.Repository.Migrations
{
    [DbContext(typeof(GoblinDbContext))]
    [Migration("20200701085845_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Goblin.Identity.Contract.Repository.Models.UserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<long?>("DeletedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EmailConfirmToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("EmailConfirmTokenExpireTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("EmailConfirmedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("FacebookId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GithubId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("LastUpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("PasswordLastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("RevokeTokenGeneratedBeforeTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("SetPasswordToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("SetPasswordTokenExpireTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("SkypeId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("WebsiteUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedTime");

                    b.HasIndex("DeletedTime");

                    b.HasIndex("Email");

                    b.HasIndex("Id");

                    b.HasIndex("LastUpdatedTime");

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasFilter("[UserName] IS NOT NULL");

                    b.ToTable("User");
                });
#pragma warning restore 612, 618
        }
    }
}