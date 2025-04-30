using Microsoft.EntityFrameworkCore;
using SystemProject_Hotel_Management_.Entities;
using SystemProject_Hotel_Management_.Models;

namespace SystemProject_Hotel_Management_
{
    public class BiteHubDbContext : DbContext
    {
        public BiteHubDbContext(DbContextOptions<BiteHubDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
       
        public DbSet<Table> Tables { get; set; }
        public DbSet<Reservation1> Reservations1 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Reservation1>()
           .HasOne(r => r.Table) // Each Reservation has one Table
           .WithMany(t => t.Reservations1) // Each Table can have many Reservations
           .HasForeignKey(r => r.TableId) // Foreign key in Reservation
           .OnDelete(DeleteBehavior.Cascade); // If a Table is deleted, related Reservations are also deleted

            // Configuring the relationship between User and Reservation
            modelBuilder.Entity<Reservation1>()
                .HasOne(r => r.User) // Each Reservation is made by one User
                .WithMany(u => u.Reservations1) // Each User can have many Reservations
                .HasForeignKey(r => r.UserId) // Foreign key in Reservation
                .OnDelete(DeleteBehavior.Cascade); // If a User is deleted, related Reservations are also deleted

            // Configure any additional properties or indexes if needed
            modelBuilder.Entity<Table>()
                .HasIndex(t => t.TableNumber) // Unique index on TableNumber
                .IsUnique();


            // Configure the foreign key relationship between Reservation and User
           
            //// Configure the relationship between Order and User (1-to-many)
            //modelBuilder.Entity<Order>()
            //    .HasOne(o => o.User)
            //    .WithMany(u => u.Orders) // A User can have multiple Orders
            //    .HasForeignKey(o => o.UserId); // Foreign Key

            //// Configure the relationship between OrderItem and Order (many-to-1)
            //modelBuilder.Entity<OrderItem>()
            //    .HasOne(oi => oi.Order)
            //    .WithMany(o => o.OrderItems) // An Order can have multiple OrderItems
            //    .HasForeignKey(oi => oi.OrderID); // Foreign Key

            // Configure the relationship between OrderItem and FoodItem (many-to-1)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.FoodItem)
                .WithMany() // A FoodItem can appear in multiple OrderItems
                .HasForeignKey(oi => oi.FoodItemID); // Foreign Key

            //// Configure the relationship between Cart and User (1-to-1 or 1-to-many)
            //modelBuilder.Entity<Cart>()
            //    .HasOne(c => c.User)
            //    .WithMany() // A User can have one or many Carts
            //    .HasForeignKey(c => c.UserId); // Foreign Key (optional, if a User can only have one Cart)

            //// Configure the relationship between CartItem and Cart (many-to-1)
            //modelBuilder.Entity<CartItem>()
            //    .HasOne(ci => ci.Cart)
            //    .WithMany(c => c.CartItems) // A Cart can have multiple CartItems
            //    .HasForeignKey(ci => ci.CartId); // Foreign Key

            // Configure the relationship between CartItem and FoodItem (many-to-1)
            //modelBuilder.Entity<CartItem>()
            //    .HasOne(ci => ci.FoodItem)
            //    .WithMany() // A FoodItem can appear in multiple CartItems
            //    .HasForeignKey(ci => ci.FoodItemId); // Foreign Key
            modelBuilder.Entity<Order>()
       .HasMany(o => o.OrderItems)
       .WithOne(oi => oi.Order)
       .HasForeignKey(oi => oi.OrderID);

            modelBuilder.Entity<Cart>()
           .HasMany(c => c.CartItems)
           .WithOne(ci => ci.Cart)
           .HasForeignKey(ci => ci.CartID);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.FoodItem)
                .WithMany()
                .HasForeignKey(ci => ci.FoodItemID);
            // Configure the relationship between FoodItem and Category (many-to-1)
            modelBuilder.Entity<FoodItem>()
                .HasOne(f => f.Category)
                .WithMany() // A Category can have multiple FoodItems
                .HasForeignKey(f => f.CategoryID); // Foreign Key
        }


    }
}
