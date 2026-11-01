using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RegistroLibros.Data;

public partial class BibliotecaBdContext : DbContext
{
    public BibliotecaBdContext()
    {
    }

    public BibliotecaBdContext(DbContextOptions<BibliotecaBdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Devolucione> Devoluciones { get; set; }

    public virtual DbSet<Entrada> Entradas { get; set; }

    public virtual DbSet<EntradasDetalle> EntradasDetalles { get; set; }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<Libro> Libros { get; set; }

    public virtual DbSet<Prestamo> Prestamos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:ap1-sql.database.windows.net,1433;Initial Catalog=BibliotecaBd;Persist Security Info=False;User ID=adminGestion;Password=Cabrera006;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=120;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Devolucione>(entity =>
        {
            entity.HasKey(e => e.DevolucionId);

            entity.HasIndex(e => e.PrestamoId, "IX_Devoluciones_PrestamoId");

            entity.HasOne(d => d.Prestamo).WithMany(p => p.Devoluciones).HasForeignKey(d => d.PrestamoId);
        });

        modelBuilder.Entity<EntradasDetalle>(entity =>
        {
            entity.ToTable("EntradasDetalle");

            entity.HasIndex(e => e.EntradaId, "IX_EntradasDetalle_EntradaId");

            entity.HasIndex(e => e.LibroId, "IX_EntradasDetalle_LibroId");

            entity.HasOne(d => d.Entrada).WithMany(p => p.EntradasDetalles).HasForeignKey(d => d.EntradaId);

            entity.HasOne(d => d.Libro).WithMany(p => p.EntradasDetalles).HasForeignKey(d => d.LibroId);
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.HasIndex(e => e.EstudianteId, "IX_Prestamos_EstudianteId");

            entity.HasIndex(e => e.LibroId, "IX_Prestamos_LibroId");

            entity.HasOne(d => d.Estudiante).WithMany(p => p.Prestamos).HasForeignKey(d => d.EstudianteId);

            entity.HasOne(d => d.Libro).WithMany(p => p.Prestamos).HasForeignKey(d => d.LibroId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
