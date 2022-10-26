﻿// <auto-generated />
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FridgeProject.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20221025154552_InitialData")]
    partial class InitialData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Entities.Models.Fridge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdFridge")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdModel")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdModel");

                    b.ToTable("Fridges");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IdModel = 1,
                            Name = "Fridge1",
                            OwnerName = "Ivan"
                        },
                        new
                        {
                            Id = 2,
                            IdModel = 1,
                            Name = "Fridge2",
                            OwnerName = "Andrey"
                        },
                        new
                        {
                            Id = 3,
                            IdModel = 2,
                            Name = "Fridge3",
                            OwnerName = "Dima"
                        },
                        new
                        {
                            Id = 4,
                            IdModel = 3,
                            Name = "Fridge4",
                            OwnerName = "Vova"
                        },
                        new
                        {
                            Id = 5,
                            IdModel = 3,
                            Name = "Fridge5",
                            OwnerName = "Egor"
                        });
                });

            modelBuilder.Entity("Entities.Models.FridgeProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdFridgeProduct")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdFridge")
                        .HasColumnType("int");

                    b.Property<int>("IdProduct")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("FridgeProducts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IdFridge = 1,
                            IdProduct = 1,
                            Quantity = 1
                        },
                        new
                        {
                            Id = 2,
                            IdFridge = 1,
                            IdProduct = 2,
                            Quantity = 2
                        },
                        new
                        {
                            Id = 3,
                            IdFridge = 1,
                            IdProduct = 3,
                            Quantity = 3
                        },
                        new
                        {
                            Id = 4,
                            IdFridge = 1,
                            IdProduct = 4,
                            Quantity = 4
                        },
                        new
                        {
                            Id = 5,
                            IdFridge = 1,
                            IdProduct = 5,
                            Quantity = 5
                        },
                        new
                        {
                            Id = 6,
                            IdFridge = 2,
                            IdProduct = 2,
                            Quantity = 2
                        },
                        new
                        {
                            Id = 7,
                            IdFridge = 2,
                            IdProduct = 3,
                            Quantity = 3
                        },
                        new
                        {
                            Id = 8,
                            IdFridge = 2,
                            IdProduct = 4,
                            Quantity = 4
                        },
                        new
                        {
                            Id = 9,
                            IdFridge = 2,
                            IdProduct = 5,
                            Quantity = 5
                        },
                        new
                        {
                            Id = 10,
                            IdFridge = 3,
                            IdProduct = 3,
                            Quantity = 5
                        },
                        new
                        {
                            Id = 11,
                            IdFridge = 3,
                            IdProduct = 4,
                            Quantity = 5
                        },
                        new
                        {
                            Id = 12,
                            IdFridge = 3,
                            IdProduct = 5,
                            Quantity = 5
                        },
                        new
                        {
                            Id = 13,
                            IdFridge = 4,
                            IdProduct = 4,
                            Quantity = 4
                        },
                        new
                        {
                            Id = 14,
                            IdFridge = 4,
                            IdProduct = 5,
                            Quantity = 4
                        });
                });

            modelBuilder.Entity("Entities.Models.Model", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdModel")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Models");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Atlant",
                            Year = 2001
                        },
                        new
                        {
                            Id = 2,
                            Name = "Vestfrost",
                            Year = 2002
                        },
                        new
                        {
                            Id = 3,
                            Name = "Mitsubishi",
                            Year = 2003
                        },
                        new
                        {
                            Id = 4,
                            Name = "Bosch",
                            Year = 2004
                        },
                        new
                        {
                            Id = 5,
                            Name = "Samsung",
                            Year = 2005
                        });
                });

            modelBuilder.Entity("Entities.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdProduct")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DefaultQuantity")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DefaultQuantity = 1,
                            Name = "Milk"
                        },
                        new
                        {
                            Id = 2,
                            DefaultQuantity = 2,
                            Name = "Bacon"
                        },
                        new
                        {
                            Id = 3,
                            DefaultQuantity = 3,
                            Name = "Beans"
                        },
                        new
                        {
                            Id = 4,
                            DefaultQuantity = 4,
                            Name = "Carrot"
                        },
                        new
                        {
                            Id = 5,
                            DefaultQuantity = 5,
                            Name = "Apple"
                        });
                });

            modelBuilder.Entity("Entities.Models.Fridge", b =>
                {
                    b.HasOne("Entities.Models.Model", "Model")
                        .WithMany()
                        .HasForeignKey("IdModel")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Model");
                });
#pragma warning restore 612, 618
        }
    }
}
