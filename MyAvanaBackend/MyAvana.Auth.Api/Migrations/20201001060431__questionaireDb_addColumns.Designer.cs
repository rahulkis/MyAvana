﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyAvana.DAL.Auth;

namespace MyAvana.Auth.Api.Migrations
{
    [DbContext(typeof(AvanaContext))]
    [Migration("20201001060431__questionaireDb_addColumns")]
    partial class _questionaireDb_addColumns
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("MyAvana.Models.Entities.Answer", b =>
                {
                    b.Property<int>("AnswerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<bool?>("IsActive");

                    b.Property<string>("Option1");

                    b.Property<string>("Option2");

                    b.Property<string>("Option3");

                    b.Property<string>("Option4");

                    b.Property<int>("QuestionId");

                    b.HasKey("AnswerId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("MyAvana.Models.Entities.BlogArticle", b =>
                {
                    b.Property<int>("BlogArticleId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<string>("Details");

                    b.Property<string>("HeadLine");

                    b.Property<string>("ImageUrl");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Url");

                    b.HasKey("BlogArticleId");

                    b.ToTable("BlogArticles");
                });

            modelBuilder.Entity("MyAvana.Models.Entities.IngedientsEntity", b =>
                {
                    b.Property<int>("IngedientsEntityId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Challenges");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<string>("Description");

                    b.Property<string>("Image");

                    b.Property<string>("ImageUrl");

                    b.Property<bool?>("IsActive");

                    b.Property<string>("Name");

                    b.Property<string>("Type");

                    b.HasKey("IngedientsEntityId");

                    b.ToTable("IngedientsEntities");
                });

            modelBuilder.Entity("MyAvana.Models.Entities.MediaLinkEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<string>("Description");

                    b.Property<string>("Header");

                    b.Property<string>("ImageLink");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsFeatured");

                    b.Property<string>("Title");

                    b.Property<string>("VideoId");

                    b.HasKey("Id");

                    b.ToTable("MediaLinkEntities");
                });

            modelBuilder.Entity("MyAvana.Models.Entities.ProductType", b =>
                {
                    b.Property<Guid>("ProductTypeId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<bool?>("IsActive");

                    b.Property<string>("ProductName");

                    b.HasKey("ProductTypeId");

                    b.ToTable("ProductTypes");
                });

            modelBuilder.Entity("MyAvana.Models.Entities.Questionaire", b =>
                {
                    b.Property<int>("QuestionaireId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AnswerId");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<bool?>("IsActive");

                    b.Property<int>("QuestionId");

                    b.Property<string>("UserId");

                    b.HasKey("QuestionaireId");

                    b.HasIndex("AnswerId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Questionaires");
                });

            modelBuilder.Entity("MyAvana.Models.Entities.Questions", b =>
                {
                    b.Property<int>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<bool?>("IsActive");

                    b.Property<bool?>("IsTagged");

                    b.HasKey("QuestionId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("MyAvana.Models.Entities.RegimenSteps", b =>
                {
                    b.Property<int>("RegimenStepsId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Step1Instruction");

                    b.Property<string>("Step1Photo");

                    b.Property<string>("Step2Instruction");

                    b.Property<string>("Step2Photo");

                    b.Property<string>("Step3Instruction");

                    b.Property<string>("Step3Photo");

                    b.Property<string>("Step4Instruction");

                    b.Property<string>("Step4Photo");

                    b.Property<string>("Step5Instruction");

                    b.Property<string>("Step5Photo");

                    b.HasKey("RegimenStepsId");

                    b.ToTable("RegimenSteps");
                });

            modelBuilder.Entity("MyAvana.Models.Entities.Regimens", b =>
                {
                    b.Property<int>("RegimensId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int>("RegimenStepsId");

                    b.Property<int>("Step1");

                    b.Property<int>("Step2");

                    b.Property<int>("Step3");

                    b.Property<int>("Step4");

                    b.Property<int>("Step5");

                    b.HasKey("RegimensId");

                    b.HasIndex("RegimenStepsId");

                    b.ToTable("Regimens");
                });

            modelBuilder.Entity("MyAvana.Models.Entities.UsersTicketsEntity", b =>
                {
                    b.Property<long>("TicketId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedAt");

                    b.Property<string>("Description");

                    b.Property<string>("Priority");

                    b.Property<string>("Status");

                    b.Property<string>("Subject");

                    b.Property<string>("UserId");

                    b.HasKey("TicketId");

                    b.ToTable("UsersTicketsEntities");
                });

            modelBuilder.Entity("MyAvana.Models.Entities.WebLogin", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<bool?>("IsActive");

                    b.Property<string>("Password");

                    b.Property<string>("UserEmail");

                    b.HasKey("UserId");

                    b.ToTable("WebLogins");
                });

            modelBuilder.Entity("MyAvanaApi.Models.Entities.CodeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<string>("Code");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<bool>("IsActive");

                    b.Property<int>("OpCode");

                    b.HasKey("Id");

                    b.ToTable("CodeEntities");
                });

            modelBuilder.Entity("MyAvanaApi.Models.Entities.EmailTemplate", b =>
                {
                    b.Property<string>("TemplateCode")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.Property<bool>("EnableSSL");

                    b.Property<string>("HostName");

                    b.Property<int>("HostPort");

                    b.Property<string>("SMTPPassword");

                    b.Property<string>("SMTPUsername");

                    b.Property<string>("SenderEmail");

                    b.Property<string>("SenderName");

                    b.Property<string>("Subject");

                    b.Property<string>("TemplateName");

                    b.Property<string>("TemplateType");

                    b.Property<int>("TimeOut");

                    b.HasKey("TemplateCode");

                    b.ToTable("EmailTemplates");
                });

            modelBuilder.Entity("MyAvanaApi.Models.Entities.GenericSetting", b =>
                {
                    b.Property<Guid>("SettingID");

                    b.Property<string>("AdminAccountId");

                    b.Property<string>("SettingName");

                    b.Property<string>("SubSettingName");

                    b.Property<int>("DefalutInteger1");

                    b.Property<int>("DefalutInteger2");

                    b.Property<int>("DefalutInteger3");

                    b.Property<int>("DefalutInteger4");

                    b.Property<int>("DefalutInteger5");

                    b.Property<bool>("DefaultBool1");

                    b.Property<bool>("DefaultBool2");

                    b.Property<bool>("DefaultBool3");

                    b.Property<bool>("DefaultBool4");

                    b.Property<bool>("DefaultBool5");

                    b.Property<DateTime?>("DefaultDateTime1");

                    b.Property<DateTime?>("DefaultDateTime2");

                    b.Property<DateTime?>("DefaultDateTime3");

                    b.Property<DateTime?>("DefaultDateTime4");

                    b.Property<DateTime?>("DefaultDateTime5");

                    b.Property<decimal>("DefaultDecimal1");

                    b.Property<decimal>("DefaultDecimal2");

                    b.Property<decimal>("DefaultDecimal3");

                    b.Property<decimal>("DefaultDecimal4");

                    b.Property<decimal>("DefaultDecimal5");

                    b.Property<string>("DefaultTextMax");

                    b.Property<string>("DefaultTextMax1");

                    b.Property<string>("DefaultTextMax2");

                    b.Property<string>("DefaultTextMax3");

                    b.Property<string>("DefaultTextMax4");

                    b.Property<string>("DefaultTextValue100_1")
                        .HasMaxLength(100);

                    b.Property<string>("DefaultTextValue100_2")
                        .HasMaxLength(100);

                    b.Property<string>("DefaultTextValue20_1")
                        .HasMaxLength(20);

                    b.Property<string>("DefaultTextValue20_2")
                        .HasMaxLength(20);

                    b.Property<string>("DefaultTextValue250_1")
                        .HasMaxLength(250);

                    b.Property<string>("DefaultTextValue250_2")
                        .HasMaxLength(250);

                    b.Property<string>("DefaultTextValue50_1")
                        .HasMaxLength(50);

                    b.Property<string>("DefaultTextValue50_2")
                        .HasMaxLength(50);

                    b.HasKey("SettingID", "AdminAccountId", "SettingName", "SubSettingName");

                    b.HasAlternateKey("AdminAccountId", "SettingID", "SettingName", "SubSettingName");

                    b.ToTable("GenericSettings");
                });

            modelBuilder.Entity("MyAvanaApi.Models.Entities.PaymentEntity", b =>
                {
                    b.Property<Guid>("PaymentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CCNumber");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("EmailAddress");

                    b.Property<string>("PaymentAmount");

                    b.Property<string>("ProviderId");

                    b.Property<string>("ProviderName");

                    b.Property<string>("SubscriptionId");

                    b.HasKey("PaymentId");

                    b.ToTable("PaymentEntities");
                });

            modelBuilder.Entity("MyAvanaApi.Models.Entities.ProductEntity", b =>
                {
                    b.Property<Guid>("guid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActualName");

                    b.Property<string>("BrandName");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<string>("ImageName");

                    b.Property<string>("Ingredients");

                    b.Property<bool>("IsActive");

                    b.Property<string>("ProductDetails");

                    b.Property<string>("ProductLink");

                    b.Property<string>("ProductName");

                    b.Property<Guid?>("ProductTypeId");

                    b.Property<string>("TypeFor");

                    b.HasKey("guid");

                    b.HasIndex("ProductTypeId");

                    b.ToTable("ProductEntities");
                });

            modelBuilder.Entity("MyAvanaApi.Models.Entities.PromoCode", b =>
                {
                    b.Property<string>("Code")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<DateTime?>("ExpireDate");

                    b.HasKey("Code");

                    b.ToTable("PromoCodes");
                });

            modelBuilder.Entity("MyAvanaApi.Models.Entities.SubscriptionsEntity", b =>
                {
                    b.Property<string>("StripePlanId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<double>("Amount");

                    b.Property<string>("Details");

                    b.Property<string>("PlanName");

                    b.Property<double>("Validity");

                    b.HasKey("StripePlanId");

                    b.ToTable("SubscriptionsEntities");
                });

            modelBuilder.Entity("MyAvanaApi.Models.Entities.UserEmails", b =>
                {
                    b.Property<Guid>("UserEmailId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("MobileNumber");

                    b.HasKey("UserEmailId");

                    b.ToTable("UserEmails");
                });

            modelBuilder.Entity("MyAvanaApi.Models.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("AccountNo");

                    b.Property<bool>("Active");

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Country");

                    b.Property<int>("CountryCode");

                    b.Property<DateTimeOffset?>("CreatedAt");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("HubSpotContactId");

                    b.Property<bool>("IsBlocked");

                    b.Property<DateTimeOffset?>("LastModifiedAt");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<bool>("LoginAlert");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("State");

                    b.Property<string>("StripeCustomerId");

                    b.Property<bool>("Subscribe");

                    b.Property<long>("TicketUserId");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<bool>("TwoFactorTrans");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<string>("ZipCode");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("MyAvanaApi.Models.Entities.UserHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("AccessTime");

                    b.Property<string>("Email");

                    b.Property<string>("Mobile");

                    b.Property<string>("UsedCode");

                    b.HasKey("Id");

                    b.ToTable("UserHistories");
                });

            modelBuilder.Entity("MyAvanaApi.Models.Entities.UserRoleEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("MyAvanaApi.Models.Entities.UsersCode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Code");

                    b.Property<string>("Email");

                    b.HasKey("Id");

                    b.ToTable("Codes");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("MyAvanaApi.Models.Entities.UserRoleEntity")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("MyAvanaApi.Models.Entities.UserEntity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("MyAvanaApi.Models.Entities.UserEntity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("MyAvanaApi.Models.Entities.UserRoleEntity")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MyAvanaApi.Models.Entities.UserEntity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("MyAvanaApi.Models.Entities.UserEntity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MyAvana.Models.Entities.Answer", b =>
                {
                    b.HasOne("MyAvana.Models.Entities.Questions", "Questions")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MyAvana.Models.Entities.Questionaire", b =>
                {
                    b.HasOne("MyAvana.Models.Entities.Answer", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MyAvana.Models.Entities.Questions", "Questions")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MyAvana.Models.Entities.Regimens", b =>
                {
                    b.HasOne("MyAvana.Models.Entities.RegimenSteps", "RegimenSteps")
                        .WithMany()
                        .HasForeignKey("RegimenStepsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MyAvanaApi.Models.Entities.ProductEntity", b =>
                {
                    b.HasOne("MyAvana.Models.Entities.ProductType", "ProductTypes")
                        .WithMany()
                        .HasForeignKey("ProductTypeId");
                });
#pragma warning restore 612, 618
        }
    }
}
