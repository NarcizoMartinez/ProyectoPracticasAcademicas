using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace SistemaPracticasAcademicas.Models
{
    public partial class sistema_academicoContext : DbContext
    {
        public sistema_academicoContext()
        {
        }

        public sistema_academicoContext(DbContextOptions<sistema_academicoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Asignatura> Asignaturas { get; set; }
        public virtual DbSet<Division> Divisions { get; set; }
        public virtual DbSet<Practica> Practicas { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("latin1");

            modelBuilder.Entity<Asignatura>(entity =>
            {
                entity.ToTable("asignaturas");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Clave)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsFixedLength(true);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Division>(entity =>
            {
                entity.ToTable("division");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.HasIndex(e => e.IdJefe, "FK_IdJefe_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Clave)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsFixedLength(true);

                entity.Property(e => e.IdJefe).HasColumnType("int(11)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.IdJefeNavigation)
                    .WithMany(p => p.Divisions)
                    .HasForeignKey(d => d.IdJefe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IdJefe");
            });

            modelBuilder.Entity<Practica>(entity =>
            {
                entity.ToTable("practicas");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.HasIndex(e => e.IdAsignatura, "FK_IdAsignatura_idx");

                entity.HasIndex(e => e.IdUsuario, "FK_IdUsuario_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Activo).HasColumnType("bit(1)");

                entity.Property(e => e.IdAsignatura).HasColumnType("int(11)");

                entity.Property(e => e.IdUsuario).HasColumnType("int(11)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.NombreUnidad)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NumUnidad).HasColumnType("int(11)");

                entity.Property(e => e.Objetivo)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Periodo)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Planteamiento)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Resultado)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Tema)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdAsignaturaNavigation)
                    .WithMany(p => p.Practicas)
                    .HasForeignKey(d => d.IdAsignatura)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IdAsignatura");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Practicas)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IdUsuario");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Rol)
                    .IsRequired()
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.HasIndex(e => e.IdDivision, "FK_IdDivision_idx");

                entity.HasIndex(e => e.IdRol, "FK_IdRol_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Contrasena)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.CorreoElectronico)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.IdDivision).HasColumnType("int(11)");

                entity.Property(e => e.IdRol).HasColumnType("int(11)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.NumeroControl)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.HasOne(d => d.IdDivisionNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdDivision)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IdDivision");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IdRol");
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
