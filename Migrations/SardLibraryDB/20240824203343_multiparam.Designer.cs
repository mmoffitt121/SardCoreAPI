﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SardCoreAPI.Database.DBContext;

#nullable disable

namespace SardCoreAPI.Migrations.SardLibraryDB
{
    [DbContext(typeof(SardLibraryDBContext))]
    [Migration("20240824203343_multiparam")]
    partial class multiparam
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("SardCoreAPI.Models.Calendars.CalendarDataAccessWrapper", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int?>("Id"));

                    b.Property<string>("CalendarObject")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("CalendarObject");

                    b.HasKey("Id");

                    b.ToTable("Calendar");
                });

            modelBuilder.Entity("SardCoreAPI.Models.Content.Image", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPoint", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int?>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("DataPoint");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPointLocation", b =>
                {
                    b.Property<int>("DataPointId")
                        .HasColumnType("int");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.HasKey("DataPointId", "LocationId");

                    b.HasIndex("LocationId");

                    b.ToTable("DataPointLocation");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameter", b =>
                {
                    b.Property<int?>("DataPointId")
                        .HasColumnType("int");

                    b.Property<int>("DataPointTypeParameterId")
                        .HasColumnType("int");

                    b.Property<int>("Sequence")
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(34)
                        .HasColumnType("varchar(34)");

                    b.HasKey("DataPointId", "DataPointTypeParameterId", "Sequence");

                    b.HasIndex("DataPointTypeParameterId");

                    b.ToTable("DataPointParameter");

                    b.HasDiscriminator<string>("Discriminator").HasValue("DataPointParameter");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointTypeParameter", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int?>("Id"));

                    b.Property<int>("DataPointTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("DataPointTypeReferenceId")
                        .HasColumnType("int");

                    b.Property<bool?>("IsMultiple")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Sequence")
                        .HasColumnType("int");

                    b.Property<string>("Settings")
                        .HasColumnType("longtext");

                    b.Property<string>("Summary")
                        .HasColumnType("longtext");

                    b.Property<string>("TypeValue")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("DataPointTypeId");

                    b.ToTable("DataPointTypeParameter");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPointType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Settings")
                        .HasColumnType("longtext");

                    b.Property<string>("Summary")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("DataPointType");
                });

            modelBuilder.Entity("SardCoreAPI.Models.Hub.Worlds.World", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Summary")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("Location")
                        .IsUnique();

                    b.ToTable("World");
                });

            modelBuilder.Entity("SardCoreAPI.Models.Map.Location.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("IconSize")
                        .HasColumnType("int");

                    b.Property<string>("IconURL")
                        .HasColumnType("longtext");

                    b.Property<string>("LabelFontColor")
                        .HasColumnType("longtext");

                    b.Property<string>("LabelFontSize")
                        .HasColumnType("longtext");

                    b.Property<double?>("Latitude")
                        .HasColumnType("double");

                    b.Property<int>("LayerId")
                        .HasColumnType("int");

                    b.Property<int?>("LocationTypeId")
                        .HasColumnType("int");

                    b.Property<string>("LocationTypeName")
                        .HasColumnType("longtext");

                    b.Property<double?>("Longitude")
                        .HasColumnType("double");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<bool?>("UsesIcon")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("UsesLabel")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("ZoomProminenceMax")
                        .HasColumnType("int");

                    b.Property<int?>("ZoomProminenceMin")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("SardCoreAPI.Models.Map.LocationType.LocationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("AnyTypeParent")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("IconPath")
                        .HasColumnType("longtext");

                    b.Property<int?>("IconSize")
                        .HasColumnType("int");

                    b.Property<string>("IconURL")
                        .HasColumnType("longtext");

                    b.Property<string>("LabelFontColor")
                        .HasColumnType("longtext");

                    b.Property<string>("LabelFontSize")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("ParentTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Summary")
                        .HasColumnType("longtext");

                    b.Property<bool?>("UsesIcon")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("UsesLabel")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("ZoomProminenceMax")
                        .HasColumnType("int");

                    b.Property<int?>("ZoomProminenceMin")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("LocationType");
                });

            modelBuilder.Entity("SardCoreAPI.Models.Map.Map", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<float?>("DefaultX")
                        .HasColumnType("float");

                    b.Property<float?>("DefaultY")
                        .HasColumnType("float");

                    b.Property<float?>("DefaultZ")
                        .HasColumnType("float");

                    b.Property<string>("IconId")
                        .HasColumnType("longtext");

                    b.Property<bool?>("IsDefault")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("Loops")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("MaxZoom")
                        .HasColumnType("int");

                    b.Property<int?>("MinZoom")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Summary")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Map");
                });

            modelBuilder.Entity("SardCoreAPI.Models.Map.MapLayer.MapLayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("IconURL")
                        .HasColumnType("longtext");

                    b.Property<bool?>("IsBaseLayer")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("IsIconLayer")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("MapId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Summary")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("MapLayer");
                });

            modelBuilder.Entity("SardCoreAPI.Models.Map.MapLayer.PersistentZoomLevel", b =>
                {
                    b.Property<int>("Zoom")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Zoom"));

                    b.Property<int>("MapLayerId")
                        .HasColumnType("int");

                    b.HasKey("Zoom");

                    b.HasIndex("MapLayerId");

                    b.ToTable("PersistentZoomLevel");
                });

            modelBuilder.Entity("SardCoreAPI.Models.Map.MapTile.MapTile", b =>
                {
                    b.Property<int>("X")
                        .HasColumnType("int");

                    b.Property<int>("Y")
                        .HasColumnType("int");

                    b.Property<int>("Z")
                        .HasColumnType("int");

                    b.Property<int>("LayerId")
                        .HasColumnType("int");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.HasKey("X", "Y", "Z", "LayerId");

                    b.ToTable("MapTile");
                });

            modelBuilder.Entity("SardCoreAPI.Models.Map.Region.Region", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int?>("Id"));

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Shape")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("ShowByDefault")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("Region");
                });

            modelBuilder.Entity("SardCoreAPI.Models.Pages.Pages.Page", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PageData")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Page");
                });

            modelBuilder.Entity("SardCoreAPI.Models.Pages.Views.ViewWrapper", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("View")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("View");
                });

            modelBuilder.Entity("SardCoreAPI.Models.Security.LibraryRoles.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("SardCoreAPI.Models.Security.LibraryRoles.RolePermission", b =>
                {
                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Permission")
                        .HasColumnType("varchar(255)");

                    b.HasKey("RoleId", "Permission");

                    b.ToTable("RolePermission");
                });

            modelBuilder.Entity("SardCoreAPI.Models.Security.LibraryRoles.UserRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("SardCoreAPI.Models.Settings.SettingJSON", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Setting")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("SettingJSON");
                });

            modelBuilder.Entity("SardCoreAPI.Models.Units.Measurable", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int?>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Summary")
                        .HasColumnType("longtext");

                    b.Property<int>("UnitType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Measurable");
                });

            modelBuilder.Entity("SardCoreAPI.Models.Units.Unit", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int?>("Id"));

                    b.Property<double?>("AmountPerParent")
                        .HasColumnType("double");

                    b.Property<int>("MeasurableId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Summary")
                        .HasColumnType("longtext");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("MeasurableId");

                    b.ToTable("Unit");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameterBoolean", b =>
                {
                    b.HasBaseType("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameter");

                    b.Property<bool>("BoolValue")
                        .HasColumnType("tinyint(1)");

                    b.HasDiscriminator().HasValue("DataPointParameterBoolean");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameterDataPoint", b =>
                {
                    b.HasBaseType("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameter");

                    b.Property<int>("DataPointValueId")
                        .HasColumnType("int");

                    b.HasIndex("DataPointValueId");

                    b.HasDiscriminator().HasValue("DataPointParameterDataPoint");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameterDocument", b =>
                {
                    b.HasBaseType("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameter");

                    b.Property<string>("DocumentValue")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasDiscriminator().HasValue("DataPointParameterDocument");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameterDouble", b =>
                {
                    b.HasBaseType("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameter");

                    b.Property<double?>("DoubleValue")
                        .HasColumnType("double");

                    b.HasDiscriminator().HasValue("DataPointParameterDouble");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameterInt", b =>
                {
                    b.HasBaseType("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameter");

                    b.Property<long?>("IntValue")
                        .HasColumnType("bigint");

                    b.Property<string>("IntValueString")
                        .HasColumnType("longtext");

                    b.HasDiscriminator().HasValue("DataPointParameterInt");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameterString", b =>
                {
                    b.HasBaseType("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameter");

                    b.Property<string>("StringValue")
                        .IsRequired()
                        .HasColumnType("varchar(1000)");

                    b.HasDiscriminator().HasValue("DataPointParameterString");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameterSummary", b =>
                {
                    b.HasBaseType("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameter");

                    b.Property<string>("SummaryValue")
                        .IsRequired()
                        .HasColumnType("varchar(5000)");

                    b.HasDiscriminator().HasValue("DataPointParameterSummary");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameterTime", b =>
                {
                    b.HasBaseType("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameter");

                    b.Property<long?>("TimeValue")
                        .HasColumnType("bigint");

                    b.HasDiscriminator().HasValue("DataPointParameterTime");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameterUnit", b =>
                {
                    b.HasBaseType("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameter");

                    b.Property<int?>("UnitID")
                        .HasColumnType("int");

                    b.Property<double>("UnitValue")
                        .HasColumnType("double");

                    b.HasIndex("UnitID");

                    b.HasDiscriminator().HasValue("DataPointParameterUnit");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPoint", b =>
                {
                    b.HasOne("SardCoreAPI.Models.DataPoints.DataPointType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Type");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPointLocation", b =>
                {
                    b.HasOne("SardCoreAPI.Models.DataPoints.DataPoint", "DataPoint")
                        .WithMany()
                        .HasForeignKey("DataPointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SardCoreAPI.Models.Map.Location.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataPoint");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameter", b =>
                {
                    b.HasOne("SardCoreAPI.Models.DataPoints.DataPoint", null)
                        .WithMany("Parameters")
                        .HasForeignKey("DataPointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointTypeParameter", "DataPointTypeParameter")
                        .WithMany()
                        .HasForeignKey("DataPointTypeParameterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataPointTypeParameter");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointTypeParameter", b =>
                {
                    b.HasOne("SardCoreAPI.Models.DataPoints.DataPointType", null)
                        .WithMany("TypeParameters")
                        .HasForeignKey("DataPointTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SardCoreAPI.Models.Map.MapLayer.PersistentZoomLevel", b =>
                {
                    b.HasOne("SardCoreAPI.Models.Map.MapLayer.MapLayer", null)
                        .WithMany("PersistentZoomLevels")
                        .HasForeignKey("MapLayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SardCoreAPI.Models.Units.Unit", b =>
                {
                    b.HasOne("SardCoreAPI.Models.Units.Measurable", "Measurable")
                        .WithMany()
                        .HasForeignKey("MeasurableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Measurable");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameterDataPoint", b =>
                {
                    b.HasOne("SardCoreAPI.Models.DataPoints.DataPoint", "DataPointValue")
                        .WithMany()
                        .HasForeignKey("DataPointValueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataPointValue");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPointParameters.DataPointParameterUnit", b =>
                {
                    b.HasOne("SardCoreAPI.Models.Units.Unit", "Unit")
                        .WithMany()
                        .HasForeignKey("UnitID");

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPoint", b =>
                {
                    b.Navigation("Parameters");
                });

            modelBuilder.Entity("SardCoreAPI.Models.DataPoints.DataPointType", b =>
                {
                    b.Navigation("TypeParameters");
                });

            modelBuilder.Entity("SardCoreAPI.Models.Map.MapLayer.MapLayer", b =>
                {
                    b.Navigation("PersistentZoomLevels");
                });
#pragma warning restore 612, 618
        }
    }
}
