using System;
using System.Collections.Generic;
using FilmsDomain.Model;
using Microsoft.EntityFrameworkCore;

namespace FilmsInfrastructure;

public partial class DbfilmsContext : DbContext
{
    public DbfilmsContext()
    {
    }

    public DbfilmsContext(DbContextOptions<DbfilmsContext> options)
        : base(options) //Виклик конструктора з цим параметром (options) з базового класу - DbContext 
    {
    }

    public virtual DbSet<Actor> Actors { get; set; }

    public virtual DbSet<ActorsFilm> ActorsFilms { get; set; }

    public virtual DbSet<CountriesFilm> CountriesFilms { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Director> Directors { get; set; }

    public virtual DbSet<Film> Films { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Preorder> Preorders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-9FE8NR6\\SQLEXPRESS; Database=DBFilms; Trusted_Connection=True; TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actor>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(40);
        });

        modelBuilder.Entity<ActorsFilm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ActorsFilms_1");

            entity.Property(e => e.Role).HasMaxLength(30);

            entity.HasOne(d => d.Actor).WithMany(p => p.ActorsFilms)
                .HasForeignKey(d => d.ActorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACTORS_FILMS_ACTORS");

            entity.HasOne(d => d.Film).WithMany(p => p.ActorsFilms)
                .HasForeignKey(d => d.FilmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACTORS_FILMS_FILMS");
        });

        modelBuilder.Entity<CountriesFilm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CountriesFilms_1");

            entity.HasOne(d => d.Country).WithMany(p => p.CountriesFilms)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COUNTRIES_FILMS_COUNTRIES");

            entity.HasOne(d => d.Film).WithMany(p => p.CountriesFilms)
                .HasForeignKey(d => d.FilmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COUNTRIES_FILMS_FILMS");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(30);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(40);
        });

        modelBuilder.Entity<Director>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(40);
        });

        modelBuilder.Entity<Film>(entity =>
        {
            //entity.Property(e => e.BoxOffice).HasMaxLength(40);
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.TrailerLink).HasMaxLength(60);

            entity.HasOne(d => d.Director).WithMany(p => p.Films)
                .HasForeignKey(d => d.DirectorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FILMS_DIC_DIRECTORS");

            entity.HasOne(d => d.Genre).WithMany(p => p.Films)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FILMS_DIC_GENRES");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<Preorder>(entity =>
        {
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Customer).WithMany(p => p.Preorders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PREORDERS_CUSTOMERS");

            entity.HasOne(d => d.Film).WithMany(p => p.Preorders)
                .HasForeignKey(d => d.FilmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PREORDERS_FILMS");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
