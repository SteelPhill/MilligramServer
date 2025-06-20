﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MilligramServer.Database.Context;

#nullable disable

namespace MilligramServer.Database.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20250607201305_AddFileTable")]
    partial class AddFileTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MilligramServer.Domain.Entities.Chat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("OwnerUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OwnerUserId");

                    b.ToTable("Chats", t =>
                        {
                            t.HasCheckConstraint("CK_Chats_Name", "LEN(Name) > 0");
                        });
                });

            modelBuilder.Entity("MilligramServer.Domain.Entities.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AddedUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("OwnerUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AddedUserId");

                    b.HasIndex("OwnerUserId");

                    b.ToTable("Contacts", t =>
                        {
                            t.HasCheckConstraint("CK_Contacts_Name", "Name IS NULL OR LEN(Name) > 0");
                        });
                });

            modelBuilder.Entity("MilligramServer.Domain.Entities.File", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsImage")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<long>("SizeBytes")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Files", t =>
                        {
                            t.HasCheckConstraint("CK_Files_Extension", "LEN(Extension) > 0");

                            t.HasCheckConstraint("CK_Files_Name", "LEN(Name) > 0");

                            t.HasCheckConstraint("CK_Files_SizeBytes", "SizeBytes > 0 AND SizeBytes <= 10485760");
                        });
                });

            modelBuilder.Entity("MilligramServer.Domain.Entities.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChatId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<Guid?>("FileId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastChangeTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("FileId")
                        .IsUnique()
                        .HasFilter("[FileId] IS NOT NULL");

                    b.HasIndex("UserId");

                    b.ToTable("Messages", t =>
                        {
                            t.HasCheckConstraint("CK_Messages_Text", "Text IS NULL OR LEN(Text) > 0");
                        });
                });

            modelBuilder.Entity("MilligramServer.Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique();

                    b.ToTable("Roles", t =>
                        {
                            t.HasCheckConstraint("CK_Roles_Name", "LEN(Name) > 0");

                            t.HasCheckConstraint("CK_Roles_NormalizedName", "LEN(NormalizedName) > 0");
                        });
                });

            modelBuilder.Entity("MilligramServer.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("MinLength", 8);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique();

                    b.ToTable("Users", t =>
                        {
                            t.HasCheckConstraint("CK_Users_Name", "LEN(Name) > 0");

                            t.HasCheckConstraint("CK_Users_Nickname", "LEN(Nickname) > 0");

                            t.HasCheckConstraint("CK_Users_NormalizedName", "LEN(NormalizedName) > 0");

                            t.HasCheckConstraint("CK_Users_PasswordHash", "LEN(PasswordHash) >= 8");
                        });
                });

            modelBuilder.Entity("MilligramServer.Domain.Entities.UserChat", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChatId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "ChatId");

                    b.HasIndex("ChatId");

                    b.ToTable("UsersChats");
                });

            modelBuilder.Entity("MilligramServer.Domain.Entities.UserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UsersRoles");
                });

            modelBuilder.Entity("MilligramServer.Domain.Entities.Chat", b =>
                {
                    b.HasOne("MilligramServer.Domain.Entities.User", "OwnerUser")
                        .WithMany("ChatsOwner")
                        .HasForeignKey("OwnerUserId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("OwnerUser");
                });

            modelBuilder.Entity("MilligramServer.Domain.Entities.Contact", b =>
                {
                    b.HasOne("MilligramServer.Domain.Entities.User", "AddedUser")
                        .WithMany("AddedUserForContacts")
                        .HasForeignKey("AddedUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MilligramServer.Domain.Entities.User", "OwnerUser")
                        .WithMany("OwnerUserForContacts")
                        .HasForeignKey("OwnerUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("AddedUser");

                    b.Navigation("OwnerUser");
                });

            modelBuilder.Entity("MilligramServer.Domain.Entities.Message", b =>
                {
                    b.HasOne("MilligramServer.Domain.Entities.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MilligramServer.Domain.Entities.File", "File")
                        .WithOne("Message")
                        .HasForeignKey("MilligramServer.Domain.Entities.Message", "FileId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("MilligramServer.Domain.Entities.User", "User")
                        .WithMany("Messages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("File");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MilligramServer.Domain.Entities.UserChat", b =>
                {
                    b.HasOne("MilligramServer.Domain.Entities.Chat", "Chat")
                        .WithMany("UsersChats")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MilligramServer.Domain.Entities.User", "User")
                        .WithMany("UsersChats")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MilligramServer.Domain.Entities.UserRole", b =>
                {
                    b.HasOne("MilligramServer.Domain.Entities.Role", "Role")
                        .WithMany("UsersRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MilligramServer.Domain.Entities.User", "User")
                        .WithMany("UsersRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MilligramServer.Domain.Entities.Chat", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("UsersChats");
                });

            modelBuilder.Entity("MilligramServer.Domain.Entities.File", b =>
                {
                    b.Navigation("Message")
                        .IsRequired();
                });

            modelBuilder.Entity("MilligramServer.Domain.Entities.Role", b =>
                {
                    b.Navigation("UsersRoles");
                });

            modelBuilder.Entity("MilligramServer.Domain.Entities.User", b =>
                {
                    b.Navigation("AddedUserForContacts");

                    b.Navigation("ChatsOwner");

                    b.Navigation("Messages");

                    b.Navigation("OwnerUserForContacts");

                    b.Navigation("UsersChats");

                    b.Navigation("UsersRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
