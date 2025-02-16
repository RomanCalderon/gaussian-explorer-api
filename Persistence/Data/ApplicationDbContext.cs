using Domain.Posts;
using Domain.Splats;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; } = default!;
    public DbSet<Splat> Splats { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>(posts =>
        {
            posts.ToTable("Posts");
            posts.HasKey(p => p.Id).HasName("PK_Post");

            posts.Property(p => p.Id)
                .HasColumnName("Id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd();

            posts.Property(p => p.UserId)
                .HasColumnName("UserId")
                .HasColumnType("int")
                .IsRequired();

            posts.Property(p => p.Title)
                .HasColumnName("Title")
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            posts.Property(p => p.Body)
                .HasColumnName("Body")
                .HasColumnType("nvarchar(1000)")
                .IsRequired();
        });

        modelBuilder.Entity<Splat>(splats =>
        {
            splats.ToTable("Splats");
            splats.HasKey(s => s.Id).HasName("PK_Splat");

            splats.Property(s => s.Id)
                .HasColumnName("Id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd();

            splats.Property(s => s.UserId)
                .HasColumnName("UserId")
                .HasColumnType("int")
                .IsRequired();

            splats.Property(s => s.Title)
                .HasColumnName("Title")
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            splats.Property(s => s.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar(1000)");

            splats.Property(s => s.Url)
                .HasColumnName("Url")
                .HasColumnType("nvarchar(2000)")
                .IsRequired();

            splats.Property(s => s.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("datetime")
                .IsRequired();

            splats.Property(s => s.ViewInfo)
                .HasColumnName("ViewInfo")
                .HasColumnType("nvarchar(2000)");
        });
    }
}
