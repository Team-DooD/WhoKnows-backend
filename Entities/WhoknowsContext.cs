using Microsoft.EntityFrameworkCore;

namespace WhoKnows_backend.Entities
{
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
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("pages");

                // Set up a unique index on the Url property
                entity.HasIndex(e => e.Url, "url_UNIQUE").IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd(); // Ensure Id is auto-generated if it's an identity column

                entity.Property(e => e.Content)
                    .HasMaxLength(65535) // Adjusted max length for content
                    .HasColumnName("content")
                    .IsRequired(); // Make this column required

                entity.Property(e => e.Language)
                    .HasMaxLength(45)
                    .HasDefaultValueSql("'en'")
                    .HasColumnName("language");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title")
                    .IsRequired(); // Make this column required

                // Specify the max length for Url to allow unique indexing
                entity.Property(e => e.Url)
                    .HasMaxLength(255) // Specify max length here
                    .HasColumnName("url")
                    .IsRequired(); // Make sure this column is required
            });


            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("users");

                entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id");
                entity.Property(e => e.Email)
                    .HasColumnName("email");
                entity.Property(e => e.Password)
                    .HasMaxLength(45)
                    .HasColumnName("password");
                entity.Property(e => e.Username)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
