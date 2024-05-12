﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TgStorage;

#nullable disable

namespace TgStorage.Migrations
{
    [DbContext(typeof(TgEfContext))]
    partial class TgEfContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("TgStorage.Domain.Apps.TgEfAppEntity", b =>
                {
                    b.Property<Guid>("Uid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("CHAR(36)")
                        .HasColumnName("UID");

                    b.Property<Guid>("ApiHash")
                        .IsConcurrencyToken()
                        .HasColumnType("CHAR(36)")
                        .HasColumnName("API_HASH");

                    b.Property<int>("ApiId")
                        .IsConcurrencyToken()
                        .HasColumnType("INT")
                        .HasColumnName("API_ID");

                    b.Property<string>("PhoneNumber")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("NVARCHAR(16)")
                        .HasColumnName("PHONE_NUMBER");

                    b.Property<Guid?>("ProxyUid")
                        .IsConcurrencyToken()
                        .HasColumnType("CHAR(36)")
                        .HasColumnName("PROXY_UID");

                    b.HasKey("Uid");

                    b.HasIndex("ApiHash")
                        .IsUnique();

                    b.HasIndex("ApiId");

                    b.HasIndex("PhoneNumber");

                    b.HasIndex("ProxyUid");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.ToTable("APPS");
                });

            modelBuilder.Entity("TgStorage.Domain.Documents.TgEfDocumentEntity", b =>
                {
                    b.Property<Guid>("Uid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("CHAR(36)")
                        .HasColumnName("UID");

                    b.Property<long>("AccessHash")
                        .IsConcurrencyToken()
                        .HasColumnType("INT(20)")
                        .HasColumnName("ACCESS_HASH");

                    b.Property<string>("FileName")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("NVARCHAR(100)")
                        .HasColumnName("FILE_NAME");

                    b.Property<long>("FileSize")
                        .IsConcurrencyToken()
                        .HasColumnType("INT(20)")
                        .HasColumnName("FILE_SIZE");

                    b.Property<long>("Id")
                        .IsConcurrencyToken()
                        .HasColumnType("INT(20)")
                        .HasColumnName("ID");

                    b.Property<long>("MessageId")
                        .IsConcurrencyToken()
                        .HasColumnType("INT(20)")
                        .HasColumnName("MESSAGE_ID");

                    b.Property<long?>("SourceId")
                        .IsConcurrencyToken()
                        .HasColumnType("INT(20)")
                        .HasColumnName("SOURCE_ID");

                    b.HasKey("Uid");

                    b.HasIndex("AccessHash");

                    b.HasIndex("FileName");

                    b.HasIndex("FileSize");

                    b.HasIndex("Id");

                    b.HasIndex("MessageId");

                    b.HasIndex("SourceId");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.ToTable("DOCUMENTS");
                });

            modelBuilder.Entity("TgStorage.Domain.Filters.TgEfFilterEntity", b =>
                {
                    b.Property<Guid>("Uid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("CHAR(36)")
                        .HasColumnName("UID");

                    b.Property<int>("FilterType")
                        .IsConcurrencyToken()
                        .HasColumnType("INT")
                        .HasColumnName("FILTER_TYPE");

                    b.Property<bool>("IsEnabled")
                        .IsConcurrencyToken()
                        .HasColumnType("BIT")
                        .HasColumnName("IS_ENABLED");

                    b.Property<string>("Mask")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("NVARCHAR(128)")
                        .HasColumnName("MASK");

                    b.Property<string>("Name")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("NVARCHAR(128)")
                        .HasColumnName("NAME");

                    b.Property<long>("Size")
                        .IsConcurrencyToken()
                        .HasColumnType("INT(20)")
                        .HasColumnName("SIZE");

                    b.Property<int>("SizeType")
                        .IsConcurrencyToken()
                        .HasColumnType("INT")
                        .HasColumnName("SIZE_TYPE");

                    b.HasKey("Uid");

                    b.HasIndex("FilterType");

                    b.HasIndex("IsEnabled");

                    b.HasIndex("Mask");

                    b.HasIndex("Name");

                    b.HasIndex("Size");

                    b.HasIndex("SizeType");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.ToTable("FILTERS");
                });

            modelBuilder.Entity("TgStorage.Domain.Messages.TgEfMessageEntity", b =>
                {
                    b.Property<Guid>("Uid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("CHAR(36)")
                        .HasColumnName("UID");

                    b.Property<DateTime>("DtCreated")
                        .IsConcurrencyToken()
                        .HasColumnType("DATETIME")
                        .HasColumnName("DT_CREATED");

                    b.Property<long>("Id")
                        .IsConcurrencyToken()
                        .HasColumnType("INT(20)")
                        .HasColumnName("ID");

                    b.Property<string>("Message")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("NVARCHAR(100)")
                        .HasColumnName("MESSAGE");

                    b.Property<long>("Size")
                        .IsConcurrencyToken()
                        .HasColumnType("INT(20)")
                        .HasColumnName("SIZE");

                    b.Property<long?>("SourceId")
                        .IsConcurrencyToken()
                        .HasColumnType("INT(20)")
                        .HasColumnName("SOURCE_ID");

                    b.Property<int>("Type")
                        .IsConcurrencyToken()
                        .HasColumnType("INT")
                        .HasColumnName("TYPE");

                    b.HasKey("Uid");

                    b.HasIndex("DtCreated");

                    b.HasIndex("Id");

                    b.HasIndex("Message");

                    b.HasIndex("Size");

                    b.HasIndex("SourceId");

                    b.HasIndex("Type");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.ToTable("MESSAGES");
                });

            modelBuilder.Entity("TgStorage.Domain.Proxies.TgEfProxyEntity", b =>
                {
                    b.Property<Guid>("Uid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("CHAR(36)")
                        .HasColumnName("UID");

                    b.Property<string>("HostName")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("INT")
                        .HasColumnName("HOST_NAME");

                    b.Property<string>("Password")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("NVARCHAR(128)")
                        .HasColumnName("PASSWORD");

                    b.Property<ushort>("Port")
                        .IsConcurrencyToken()
                        .HasColumnType("INT(5)")
                        .HasColumnName("PORT");

                    b.Property<string>("Secret")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("NVARCHAR(128)")
                        .HasColumnName("SECRET");

                    b.Property<int>("Type")
                        .IsConcurrencyToken()
                        .HasColumnType("INT")
                        .HasColumnName("TYPE");

                    b.Property<string>("UserName")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("NVARCHAR(128)")
                        .HasColumnName("USER_NAME");

                    b.HasKey("Uid");

                    b.HasIndex("HostName");

                    b.HasIndex("Password");

                    b.HasIndex("Port");

                    b.HasIndex("Secret");

                    b.HasIndex("Type");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.HasIndex("UserName");

                    b.ToTable("PROXIES");
                });

            modelBuilder.Entity("TgStorage.Domain.Sources.TgEfSourceEntity", b =>
                {
                    b.Property<Guid>("Uid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("CHAR(36)")
                        .HasColumnName("UID");

                    b.Property<string>("About")
                        .IsConcurrencyToken()
                        .HasColumnType("NVARCHAR(1024)")
                        .HasColumnName("ABOUT");

                    b.Property<int>("Count")
                        .IsConcurrencyToken()
                        .HasColumnType("INT")
                        .HasColumnName("COUNT");

                    b.Property<string>("Directory")
                        .IsConcurrencyToken()
                        .HasMaxLength(1024)
                        .HasColumnType("NVARCHAR(256)")
                        .HasColumnName("DIRECTORY");

                    b.Property<DateTime>("DtChanged")
                        .IsConcurrencyToken()
                        .HasColumnType("DATETIME")
                        .HasColumnName("DT_CHANGED");

                    b.Property<int>("FirstId")
                        .IsConcurrencyToken()
                        .HasColumnType("INT")
                        .HasColumnName("FIRST_ID");

                    b.Property<long>("Id")
                        .IsConcurrencyToken()
                        .HasColumnType("INT(20)")
                        .HasColumnName("ID");

                    b.Property<bool>("IsAutoUpdate")
                        .IsConcurrencyToken()
                        .HasColumnType("BIT")
                        .HasColumnName("IS_AUTO_UPDATE");

                    b.Property<string>("Title")
                        .IsConcurrencyToken()
                        .HasMaxLength(1024)
                        .HasColumnType("NVARCHAR(256)")
                        .HasColumnName("TITLE");

                    b.Property<string>("UserName")
                        .IsConcurrencyToken()
                        .HasMaxLength(256)
                        .HasColumnType("NVARCHAR(128)")
                        .HasColumnName("USER_NAME");

                    b.HasKey("Uid");

                    b.HasIndex("Count");

                    b.HasIndex("Directory");

                    b.HasIndex("DtChanged");

                    b.HasIndex("FirstId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("IsAutoUpdate");

                    b.HasIndex("Title");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.HasIndex("UserName");

                    b.ToTable("SOURCES");
                });

            modelBuilder.Entity("TgStorage.Domain.Versions.TgEfVersionEntity", b =>
                {
                    b.Property<Guid>("Uid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("CHAR(36)")
                        .HasColumnName("UID");

                    b.Property<string>("Description")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("NVARCHAR(128)")
                        .HasColumnName("DESCRIPTION");

                    b.Property<short>("Version")
                        .IsConcurrencyToken()
                        .HasMaxLength(4)
                        .HasColumnType("SMALLINT")
                        .HasColumnName("VERSION");

                    b.HasKey("Uid");

                    b.HasIndex("Description");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.HasIndex("Version")
                        .IsUnique();

                    b.ToTable("VERSIONS");
                });

            modelBuilder.Entity("TgStorage.Domain.Apps.TgEfAppEntity", b =>
                {
                    b.HasOne("TgStorage.Domain.Proxies.TgEfProxyEntity", "Proxy")
                        .WithMany("Apps")
                        .HasForeignKey("ProxyUid");

                    b.Navigation("Proxy");
                });

            modelBuilder.Entity("TgStorage.Domain.Documents.TgEfDocumentEntity", b =>
                {
                    b.HasOne("TgStorage.Domain.Sources.TgEfSourceEntity", "Source")
                        .WithMany("Documents")
                        .HasForeignKey("SourceId")
                        .HasPrincipalKey("Id");

                    b.Navigation("Source");
                });

            modelBuilder.Entity("TgStorage.Domain.Messages.TgEfMessageEntity", b =>
                {
                    b.HasOne("TgStorage.Domain.Sources.TgEfSourceEntity", "Source")
                        .WithMany("Messages")
                        .HasForeignKey("SourceId")
                        .HasPrincipalKey("Id");

                    b.Navigation("Source");
                });

            modelBuilder.Entity("TgStorage.Domain.Proxies.TgEfProxyEntity", b =>
                {
                    b.Navigation("Apps");
                });

            modelBuilder.Entity("TgStorage.Domain.Sources.TgEfSourceEntity", b =>
                {
                    b.Navigation("Documents");

                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
