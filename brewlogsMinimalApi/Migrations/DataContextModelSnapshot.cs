﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using brewlogsMinimalApi.Data;

#nullable disable

namespace brewlogsMinimalApi.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("brewlogsMinimalApi.Model.Brewlog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BrewRatio")
                        .HasColumnType("int");

                    b.Property<string>("BrewerUsed")
                        .HasColumnType("longtext");

                    b.Property<string>("CoffeeName")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Dose")
                        .HasColumnType("int");

                    b.Property<string>("Grind")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Roast")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Brewlog");
                });
#pragma warning restore 612, 618
        }
    }
}
