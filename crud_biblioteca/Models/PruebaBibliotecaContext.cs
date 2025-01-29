using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace crud_biblioteca.Models;

public partial class PruebaBibliotecaContext : DbContext
{
    public PruebaBibliotecaContext()
    {
    }
    private readonly IConfiguration _configuration;
    public PruebaBibliotecaContext(DbContextOptions<PruebaBibliotecaContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Libro> Libros { get; set; }

    public virtual DbSet<Prestamo> Prestamos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            //var connectionString = _configuration.GetConnectionString("DefaultConnection");
            //optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Libro>(entity =>
        {
            entity.HasKey(e => e.IdLibros).HasName("PK__Libros__153221F3BA354CCE");

            entity.HasIndex(e => e.Isbn, "UQ__Libros__447D36EA2C76A7DC").IsUnique();

            entity.Property(e => e.Autor).HasMaxLength(150);
            entity.Property(e => e.Disponibilidad).HasDefaultValueSql("((1))");
            entity.Property(e => e.Isbn)
                .HasMaxLength(50)
                .HasColumnName("ISBN");
            entity.Property(e => e.Titulo).HasMaxLength(255);
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.HasKey(e => e.IdPrestamos).HasName("PK__Prestamo__C9249103B1AC4F57");

            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValueSql("('Prestado')");
            entity.Property(e => e.FechaDevolucion).HasColumnType("date");
            entity.Property(e => e.FechaPrestamo)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdLibrosNavigation).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.IdLibros)
                .HasConstraintName("FK_Prestamos_Libros");

            entity.HasOne(d => d.IdUsuariosNavigation).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.IdUsuarios)
                .HasConstraintName("FK_Prestamos_Usuarios");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuarios).HasName("PK__Usuarios__EAEBAC8FE4DFBD6D");

            entity.HasIndex(e => e.Correo, "UQ__Usuarios__60695A193D1FB680").IsUnique();

            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
