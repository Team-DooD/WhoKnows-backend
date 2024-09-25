using Microsoft.EntityFrameworkCore;

namespace WhoKnows_backend.Entities
{
    public partial class WhoknowsContext : DbContext
    {
        public WhoknowsContext()
        {
        }

        public WhoknowsContext(DbContextOptions<WhoknowsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Page> Pages { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(
                    "server=40.87.131.145;port=3307;user=root;password=MySQL1234;database=whoKnowsDB;Pooling=true;",
                    new MySqlServerVersion(new Version(8, 0, 32)) // Replace with your MySQL version
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Page>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("pages");

                entity.HasIndex(e => e.Url, "url_UNIQUE").IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Content)
                    .HasMaxLength(255)
                    .HasColumnName("content");
                entity.Property(e => e.Language)
                    .HasMaxLength(45)
                    .HasDefaultValueSql("'en'")
                    .HasColumnName("language");
                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");
                entity.Property(e => e.Url).HasColumnName("url");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("users");

                entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

                entity.Property(e => e.Id)
                    .HasMaxLength(45)
                    .HasColumnName("id");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Password)
                    .HasMaxLength(45)
                    .HasColumnName("password");
                entity.Property(e => e.Username).HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
