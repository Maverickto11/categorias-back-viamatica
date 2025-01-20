﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using categorias_back_viamatica.Data;

#nullable disable

namespace categorias_back_viamatica.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("categorias_back_viamatica.Models.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImagenUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categorias");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ImagenUrl = "url_de_imagen_deportes",
                            Nombre = "Deportes"
                        },
                        new
                        {
                            Id = 2,
                            ImagenUrl = "url_de_imagen_artes",
                            Nombre = "Artes"
                        },
                        new
                        {
                            Id = 3,
                            ImagenUrl = "url_de_imagen_tecnologia",
                            Nombre = "Tecnología"
                        },
                        new
                        {
                            Id = 4,
                            ImagenUrl = "url_de_imagen_videojuegos",
                            Nombre = "Videojuegos"
                        },
                        new
                        {
                            Id = 5,
                            ImagenUrl = "url_de_imagen_ciencia",
                            Nombre = "Ciencia"
                        },
                        new
                        {
                            Id = 6,
                            ImagenUrl = "url_de_imagen_musica",
                            Nombre = "Música"
                        },
                        new
                        {
                            Id = 7,
                            ImagenUrl = "url_de_imagen_cine",
                            Nombre = "Cine"
                        },
                        new
                        {
                            Id = 8,
                            ImagenUrl = "url_de_imagen_viajes",
                            Nombre = "Viajes"
                        });
                });

            modelBuilder.Entity("categorias_back_viamatica.Models.Comentario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Contenido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PublicacionId")
                        .HasColumnType("int");

                    b.Property<int?>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PublicacionId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Comentarios");
                });

            modelBuilder.Entity("categorias_back_viamatica.Models.Publicacion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<string>("Contenido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Publicaciones");
                });

            modelBuilder.Entity("categorias_back_viamatica.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Contraseña")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("categorias_back_viamatica.Models.Comentario", b =>
                {
                    b.HasOne("categorias_back_viamatica.Models.Publicacion", "Publicacion")
                        .WithMany()
                        .HasForeignKey("PublicacionId");

                    b.HasOne("categorias_back_viamatica.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId");

                    b.Navigation("Publicacion");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("categorias_back_viamatica.Models.Publicacion", b =>
                {
                    b.HasOne("categorias_back_viamatica.Models.Categoria", "Categoria")
                        .WithMany("Publicaciones")
                        .HasForeignKey("CategoriaId");

                    b.HasOne("categorias_back_viamatica.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId");

                    b.Navigation("Categoria");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("categorias_back_viamatica.Models.Categoria", b =>
                {
                    b.Navigation("Publicaciones");
                });
#pragma warning restore 612, 618
        }
    }
}
