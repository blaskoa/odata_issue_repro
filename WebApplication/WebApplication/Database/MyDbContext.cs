using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication.Models;

namespace WebApplication.Database
{
    public class MyDbContext : DbContext
    {
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Child> Children { get; set; }

        public MyDbContext(DbContextOptions opts) : base(opts)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("server=localhost;Initial catalog=Poc;User ID=sa;Password=Test1234")
                .LogTo(Console.WriteLine);
            optionsBuilder.ConfigureWarnings(warn =>
                warn.Ignore(
                    Microsoft.EntityFrameworkCore.Diagnostics.CoreEventId.DetachedLazyLoadingWarning,
                    Microsoft.EntityFrameworkCore.Diagnostics.CoreEventId.LazyLoadOnDisposedContextWarning)
            ).LogTo(Console.WriteLine);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies();
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ParentMapping());
            modelBuilder.ApplyConfiguration(new ChildMapping());
        }
    }

    public class ChildMapping : IEntityTypeConfiguration<Child>
    {
        public void Configure(EntityTypeBuilder<Child> builder)
        {
            builder.ToTable("Children", "core");
            builder.HasKey(e => e.InternalId);

            builder.Property(e => e.InternalId)
                .HasColumnName("Id")
                .ValueGeneratedNever();

            builder.HasOne(e => e.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(e => e.ParentId);
        }
    }

    public class ParentMapping : IEntityTypeConfiguration<Parent>
    {
        public void Configure(EntityTypeBuilder<Parent> builder)
        {
            builder.ToTable("Parents", "core");
            builder.HasKey(e => e.InternalId);

            builder.Property(e => e.InternalId)
                .HasColumnName("Id")
                .ValueGeneratedNever();
        }
    }
}