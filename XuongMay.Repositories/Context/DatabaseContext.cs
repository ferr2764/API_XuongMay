using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using XuongMay.Contract.Repositories.Entity;
using Microsoft.EntityFrameworkCore;

namespace XuongMay.Repositories
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=115.73.218.193,1433;Database=XuongMay;User Id=user;Password=12345;Encrypt=false;TrustServerCertificate=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountId).HasName("PK_Accounts");
                entity.Property(e => e.AccountId).HasDefaultValueSql("NEWID()");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId).HasName("PK_Categories");
                entity.Property(e => e.CategoryId).HasDefaultValueSql("NEWID()");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId).HasName("PK_Orders");
                entity.Property(e => e.OrderId).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.CreatedDate).HasColumnType("DATETIME");
                entity.Property(e => e.FinishDate).HasColumnType("DATETIME");
                entity.Property(e => e.Deadline).HasColumnType("DATETIME");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.DetailId).HasName("PK_OrderDetails");
                entity.Property(e => e.DetailId).HasDefaultValueSql("NEWID()");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId).HasName("PK_Products");
                entity.Property(e => e.ProductId).HasDefaultValueSql("NEWID()");
            });

        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}