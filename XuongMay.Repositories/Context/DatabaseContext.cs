using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using XuongMay.Contract.Repositories.Entity;

namespace XuongMay.Repositories
{
    public partial class DatabaseContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
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
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("MyCnn"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountId).HasName("PK_Accounts");
                entity.Property(e => e.AccountId).HasDefaultValueSql("NEWID()");

                // Configuring the relationship for OrderAccounts (Orders created by the Account)
                entity.HasMany(a => a.OrderAccounts)
                      .WithOne(o => o.Account)
                      .HasForeignKey(o => o.AccountId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_Orders_Account");

                // Configuring the relationship for OrderAssignedAccounts (Orders assigned to the Account)
                entity.HasMany(a => a.OrderAssignedAccounts)
                      .WithOne(o => o.AssignedAccount)
                      .HasForeignKey(o => o.AssignedAccountId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_Orders_AssignedAccount");
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

            OnModelCreatingPartial(modelBuilder);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
