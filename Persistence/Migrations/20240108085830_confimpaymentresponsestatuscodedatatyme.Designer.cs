﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240108085830_confimpaymentresponsestatuscodedatatyme")]
    partial class confimpaymentresponsestatuscodedatatyme
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.DatabaseC.ChannelCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CodeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ChannelCode");
                });

            modelBuilder.Entity("Domain.DatabaseC.ChannelList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ChannelName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ChannelList");
                });

            modelBuilder.Entity("Domain.DatabaseC.GatewayCheckInfo", b =>
                {
                    b.Property<int>("StanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StanId"));

                    b.Property<DateTime>("ServerReqDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnOrder(2)
                        .HasDefaultValueSql("GetDate()");

                    b.Property<string>("AccountID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ActualAmount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BranchCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChannelCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ChannelReqDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Command")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConversationID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EndMonth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Event")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FinAppResCode")
                        .HasColumnType("int");

                    b.Property<string>("GatewayId")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InitiatorID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InitiatorKYC")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("InquiryMode")
                        .HasColumnType("int");

                    b.Property<int?>("InquiryType")
                        .HasColumnType("int");

                    b.Property<string>("LoginID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartialAmountFlag")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartnerID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PayAmount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PayMode")
                        .HasColumnType("int");

                    b.Property<int?>("PayType")
                        .HasColumnType("int");

                    b.Property<string>("PaymentContractNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequesterMSISDN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResponseDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SpResDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("StartMonth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SysChannelReqDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SysChannelResDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SysSpReqDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SysSpResDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SystemID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Version")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StanId", "ServerReqDate");

                    b.ToTable("GatewayCheckInfo", t =>
                        {
                            t.HasTrigger("Trigger_GatewayCheckInfo_Insert");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("Domain.DatabaseC.GatewayInfo", b =>
                {
                    b.Property<int>("StanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StanId"));

                    b.Property<DateTime>("ServerReqDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnOrder(2)
                        .HasDefaultValueSql("GetDate()");

                    b.Property<string>("AccountID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ActualAmount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BranchCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChannelCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ChannelListId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ChannelReqDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Command")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConversationID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EndMonth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Event")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FinAppResCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GatewayId")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InitiatorID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InitiatorKYC")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("InquiryMode")
                        .HasColumnType("int");

                    b.Property<int?>("InquiryType")
                        .HasColumnType("int");

                    b.Property<string>("LoginID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartialAmountFlag")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartnerID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PayAmount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PayMode")
                        .HasColumnType("int");

                    b.Property<int?>("PayType")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PaymentConfirmReqDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentConfirmResCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("PaymentConfirmResDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentConfirmResDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentConfirmResNote")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentContractNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequesterMSISDN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResponseDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SpResDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("StartMonth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SysChannelReqDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SysChannelResDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SysSpReqDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SysSpResDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SystemID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SystemTrID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("TrxRef")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Version")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StanId", "ServerReqDate");

                    b.HasIndex("ChannelListId");

                    b.ToTable("GatewayInfo", t =>
                        {
                            t.HasTrigger("Trigger_GatewayInfos_Insert");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("Domain.DatabaseC.RoutingServerList", b =>
                {
                    b.Property<int>("RID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RID"));

                    b.Property<string>("BranchCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RID");

                    b.ToTable("RoutingServerList");
                });

            modelBuilder.Entity("Domain.DatabaseC.Stan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CounterDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CounterValue")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("stan", t =>
                        {
                            t.HasTrigger("Trigger_Stan_update");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("Domain.Identity.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<int>("ChannelListID")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("ChannelListID");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Domain.DatabaseC.GatewayInfo", b =>
                {
                    b.HasOne("Domain.DatabaseC.ChannelList", "ChannelList")
                        .WithMany("GatewayInfo")
                        .HasForeignKey("ChannelListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChannelList");
                });

            modelBuilder.Entity("Domain.Identity.AppUser", b =>
                {
                    b.HasOne("Domain.DatabaseC.ChannelList", "ChannelList")
                        .WithMany("AppUser")
                        .HasForeignKey("ChannelListID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChannelList");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Domain.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Domain.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Domain.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.DatabaseC.ChannelList", b =>
                {
                    b.Navigation("AppUser");

                    b.Navigation("GatewayInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
