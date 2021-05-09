using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ShopDAL
{
    public class ShopContext : DbContext
    {
        #region Fields & properties
        public string ConnectionString { get; set; }
        public bool EnableSensitiveDataLogging { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrdersProducts { get; set; }

        #endregion

        #region Constructors
        
        public ShopContext():this("Server=(local);database=ShopDB;trusted_connection=true;") { }  //with default
        public ShopContext(string connectionString) 
        {
            ConnectionString = connectionString;
        }
        public ShopContext(DbContextOptions<ShopContext> options) : base(options) { }
        #endregion

        #region Configuration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (string.IsNullOrEmpty(ConnectionString))
                optionsBuilder.UseSqlServer();
            else
                optionsBuilder.UseSqlServer(ConnectionString);
            //
            if (EnableSensitiveDataLogging)
                optionsBuilder.LogTo((m)=>Debug.WriteLine(m)).EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(e =>
            {
                e.Property("UserID").ValueGeneratedNever();
                e.Property("Name").IsRequired().HasMaxLength(16);
                e.Property("Email").HasColumnType("varchar(256)");
                e.HasIndex("Email").IsUnique();
                e.Property("PhoneNumber").HasColumnType("varchar(16)");
                e.HasIndex("Name").IsUnique();
            });
            modelBuilder.Entity<Product>(e =>
            {
                e.Property("ProductID").ValueGeneratedNever();
                e.Property("Name").IsRequired().HasMaxLength(16);
                e.HasIndex("Name").IsUnique();
            });
         
            modelBuilder.Entity<Order>(e =>
            {
                e.Property("OrderID").ValueGeneratedNever();
                e.Property("Created").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            });
            modelBuilder.Entity<OrderProduct>(e =>
            {
                e.HasKey(op => new { op.OrderID,op.ProductID });
            });
        }
        #endregion
        public List<Order> GetOrdersThatHaveProducts()
        {
            return (from oc in OrdersProducts
                    join o in Orders on oc.OrderID equals o.OrderID
                    join p in Products on oc.ProductID equals p.ProductID
                    join c in Users on o.UserID equals c.UserID
                    select o).ToList();
        }

        public List<OrderSummery> GetOrdersSummery()
        {
            return (from c in Users
                       join ov in (from o in Orders
                                   join op in OrdersProducts on o.OrderID equals op.OrderID
                                   join p in Products on op.ProductID equals p.ProductID
                                   group new { o, p, op } by new { o.UserID, op.OrderID } into ogs
                                   select new
                                   {
                                       UserID = ogs.Key.UserID,
                                       OrderID = ogs.Key.OrderID,
                                       Value = ogs.Sum(og => og.op.ProductCount * og.p.Value)
                                   }) on c.UserID equals ov.UserID
                       select new OrderSummery()
                        {
                            UserID = c.UserID,
                            Name = c.Name,
                            OrderID = ov.OrderID,
                            Value = ov.Value
                        }).ToList();
        }

        public List<Order> GetOrdersThatDoNotHaveProducts()
        {
            return null;
        }
    }

    
}
