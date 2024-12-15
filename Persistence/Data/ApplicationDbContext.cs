using GaussianExplorer.Domain.Posts;
using Microsoft.EntityFrameworkCore;

namespace GaussianExplorer.Persistence.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; } = default!;

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
    }
}
