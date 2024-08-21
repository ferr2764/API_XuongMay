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
            => optionsBuilder.UseSqlServer("Server=115.73.218.193,1433;Database=XuongMay;User Id=user;Password=12345;Encrypt=false;TrustServerCertificate=true;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA5A6827A32E2");

                entity.Property(e => e.AccountId).HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(200);
                entity.Property(e => e.Name).HasMaxLength(200);
                entity.Property(e => e.Password).HasMaxLength(200);
                entity.Property(e => e.Role).HasMaxLength(200);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0BAD4F3E2F");

                entity.Property(e => e.CategoryId).HasMaxLength(100);
                entity.Property(e => e.CategoryDescription).HasMaxLength(200);
                entity.Property(e => e.CategoryName).HasMaxLength(200);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF0B5640D8");

                entity.Property(e => e.OrderId).HasMaxLength(100);
                entity.Property(e => e.AccountId).HasMaxLength(100);
                entity.Property(e => e.AssignedAccountId).HasMaxLength(100);
                entity.Property(e => e.Status).HasMaxLength(200);

                entity.HasOne(d => d.Account).WithMany(p => p.OrderAccounts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__AccountI__4BAC3F29");

                entity.HasOne(d => d.AssignedAccount).WithMany(p => p.OrderAssignedAccounts)
                    .HasForeignKey(d => d.AssignedAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__Assigned__4CA06362");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.DetailId).HasName("PK__OrderDet__135C316D4DBC8E31");

                entity.ToTable("OrderDetail");

                entity.Property(e => e.DetailId).HasMaxLength(100);
                entity.Property(e => e.OrderId).HasMaxLength(100);
                entity.Property(e => e.ProductId).HasMaxLength(100);
                entity.Property(e => e.Status).HasMaxLength(200);

                entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderDeta__Order__5441852A");

                entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderDeta__Produ__5535A963");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CD1123D242");

                entity.Property(e => e.ProductId).HasMaxLength(100);
                entity.Property(e => e.CategoryId).HasMaxLength(100);
                entity.Property(e => e.ProductName).HasMaxLength(200);
                entity.Property(e => e.ProductSize).HasMaxLength(200);
                entity.Property(e => e.Status).HasMaxLength(200);

                entity.HasOne(d => d.Category).WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Products__Catego__5165187F");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}