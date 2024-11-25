using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace WhoKnows_backend.Models;

public partial class WhoknowsContext : DbContext
{
    private readonly IConfiguration Configuration;
    public WhoknowsContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public WhoknowsContext(DbContextOptions<WhoknowsContext> options, IConfiguration configuration)
        : base(options)
    {
        Configuration = configuration;
    }

    public virtual DbSet<Page> Pages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(
                Configuration.GetConnectionString("ConnectionStrings:DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 32))
            );
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Page>(entity =>
        {
            // Set Id as the primary key and ensure it's auto-incremented
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            // Drop the unique constraint on Title if it was set previously
            entity.HasIndex(e => e.Title, "title").IsUnique(false);

            entity.ToTable("pages");

            // Define unique index for the Url column (if needed)
            entity.HasIndex(e => e.Url, "url_idx").IsUnique();

            // Map columns to table fields
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");

            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");

            // Define Id as the primary key and set it to auto-increment (ValueGeneratedOnAdd)
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();  // Ensure auto-increment for new records

            entity.Property(e => e.Language)
                .HasDefaultValueSql("'en'") // Set default value for Language if not provided
                .HasColumnType("enum('en','da')")
                .HasColumnName("language");

            entity.Property(e => e.LastUpdated)
                .HasColumnType("timestamp")
                .HasColumnName("last_updated");

            entity.Property(e => e.Url).HasColumnName("url");
        });


        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.Username, "username").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Username).HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
