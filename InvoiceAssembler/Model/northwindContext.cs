using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace InvoiceAssembler
{
    public partial class northwindContext : DbContext
    {
        public northwindContext()
        {
        }

        public northwindContext(DbContextOptions<northwindContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<CustomerCustomerDemo> CustomerCustomerDemo { get; set; }
        public virtual DbSet<CustomerDemographics> CustomerDemographics { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<EmployeeTerritories> EmployeeTerritories { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Shippers> Shippers { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<Territories> Territories { get; set; }
        public virtual DbSet<UsStates> UsStates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5433;user id=postgres;Password=masterkey;Database=northwind");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("pk_categories");

                entity.ToTable("categories");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("category_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnName("category_name")
                    .HasMaxLength(15);

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Picture).HasColumnName("picture");
            });

            modelBuilder.Entity<CustomerCustomerDemo>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.CustomerTypeId })
                    .HasName("pk_customer_customer_demo");

                entity.ToTable("customer_customer_demo");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasColumnType("char");

                entity.Property(e => e.CustomerTypeId)
                    .HasColumnName("customer_type_id")
                    .HasColumnType("char");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerCustomerDemo)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_customer_customer_demo_customers");

                entity.HasOne(d => d.CustomerType)
                    .WithMany(p => p.CustomerCustomerDemo)
                    .HasForeignKey(d => d.CustomerTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_customer_customer_demo_customer_demographics");
            });

            modelBuilder.Entity<CustomerDemographics>(entity =>
            {
                entity.HasKey(e => e.CustomerTypeId)
                    .HasName("pk_customer_demographics");

                entity.ToTable("customer_demographics");

                entity.Property(e => e.CustomerTypeId)
                    .HasColumnName("customer_type_id")
                    .HasColumnType("char");

                entity.Property(e => e.CustomerDesc).HasColumnName("customer_desc");
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasKey(e => e.CustomerId)
                    .HasName("pk_customers");

                entity.ToTable("customers");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasColumnType("char");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(60);

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(15);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasColumnName("company_name")
                    .HasMaxLength(40);

                entity.Property(e => e.ContactName)
                    .HasColumnName("contact_name")
                    .HasMaxLength(30);

                entity.Property(e => e.ContactTitle)
                    .HasColumnName("contact_title")
                    .HasMaxLength(30);

                entity.Property(e => e.Country)
                    .HasColumnName("country")
                    .HasMaxLength(15);

                entity.Property(e => e.Fax)
                    .HasColumnName("fax")
                    .HasMaxLength(24);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(24);

                entity.Property(e => e.PostalCode)
                    .HasColumnName("postal_code")
                    .HasMaxLength(10);

                entity.Property(e => e.Region)
                    .HasColumnName("region")
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<EmployeeTerritories>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.TerritoryId })
                    .HasName("pk_employee_territories");

                entity.ToTable("employee_territories");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.TerritoryId)
                    .HasColumnName("territory_id")
                    .HasMaxLength(20);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeTerritories)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_employee_territories_employees");

                entity.HasOne(d => d.Territory)
                    .WithMany(p => p.EmployeeTerritories)
                    .HasForeignKey(d => d.TerritoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_employee_territories_territories");
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .HasName("pk_employees");

                entity.ToTable("employees");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("employee_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(60);

                entity.Property(e => e.BirthDate)
                    .HasColumnName("birth_date")
                    .HasColumnType("date");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(15);

                entity.Property(e => e.Country)
                    .HasColumnName("country")
                    .HasMaxLength(15);

                entity.Property(e => e.Extension)
                    .HasColumnName("extension")
                    .HasMaxLength(4);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(10);

                entity.Property(e => e.HireDate)
                    .HasColumnName("hire_date")
                    .HasColumnType("date");

                entity.Property(e => e.HomePhone)
                    .HasColumnName("home_phone")
                    .HasMaxLength(24);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(20);

                entity.Property(e => e.Notes).HasColumnName("notes");

                entity.Property(e => e.Photo).HasColumnName("photo");

                entity.Property(e => e.PhotoPath)
                    .HasColumnName("photo_path")
                    .HasMaxLength(255);

                entity.Property(e => e.PostalCode)
                    .HasColumnName("postal_code")
                    .HasMaxLength(10);

                entity.Property(e => e.Region)
                    .HasColumnName("region")
                    .HasMaxLength(15);

                entity.Property(e => e.ReportsTo).HasColumnName("reports_to");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(30);

                entity.Property(e => e.TitleOfCourtesy)
                    .HasColumnName("title_of_courtesy")
                    .HasMaxLength(25);

                entity.HasOne(d => d.ReportsToNavigation)
                    .WithMany(p => p.InverseReportsToNavigation)
                    .HasForeignKey(d => d.ReportsTo)
                    .HasConstraintName("fk_employees_employees");
            });

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId })
                    .HasName("pk_order_details");

                entity.ToTable("order_details");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Discount).HasColumnName("discount");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.UnitPrice).HasColumnName("unit_price");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_order_details_orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_order_details_products");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("pk_orders");

                entity.ToTable("orders");

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasColumnType("char");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.Freight).HasColumnName("freight");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("order_date")
                    .HasColumnType("date");

                entity.Property(e => e.RequiredDate)
                    .HasColumnName("required_date")
                    .HasColumnType("date");

                entity.Property(e => e.ShipAddress)
                    .HasColumnName("ship_address")
                    .HasMaxLength(60);

                entity.Property(e => e.ShipCity)
                    .HasColumnName("ship_city")
                    .HasMaxLength(15);

                entity.Property(e => e.ShipCountry)
                    .HasColumnName("ship_country")
                    .HasMaxLength(15);

                entity.Property(e => e.ShipName)
                    .HasColumnName("ship_name")
                    .HasMaxLength(40);

                entity.Property(e => e.ShipPostalCode)
                    .HasColumnName("ship_postal_code")
                    .HasMaxLength(10);

                entity.Property(e => e.ShipRegion)
                    .HasColumnName("ship_region")
                    .HasMaxLength(15);

                entity.Property(e => e.ShipVia).HasColumnName("ship_via");

                entity.Property(e => e.ShippedDate)
                    .HasColumnName("shipped_date")
                    .HasColumnType("date");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("fk_orders_customers");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("fk_orders_employees");

                entity.HasOne(d => d.ShipViaNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ShipVia)
                    .HasConstraintName("fk_orders_shippers");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("pk_products");

                entity.ToTable("products");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Discontinued).HasColumnName("discontinued");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnName("product_name")
                    .HasMaxLength(40);

                entity.Property(e => e.QuantityPerUnit)
                    .HasColumnName("quantity_per_unit")
                    .HasMaxLength(20);

                entity.Property(e => e.ReorderLevel).HasColumnName("reorder_level");

                entity.Property(e => e.SupplierId).HasColumnName("supplier_id");

                entity.Property(e => e.UnitPrice).HasColumnName("unit_price");

                entity.Property(e => e.UnitsInStock).HasColumnName("units_in_stock");

                entity.Property(e => e.UnitsOnOrder).HasColumnName("units_on_order");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("fk_products_categories");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("fk_products_suppliers");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("region");

                entity.Property(e => e.RegionId)
                    .HasColumnName("region_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.RegionDescription)
                    .IsRequired()
                    .HasColumnName("region_description")
                    .HasColumnType("char");
            });

            modelBuilder.Entity<Shippers>(entity =>
            {
                entity.HasKey(e => e.ShipperId)
                    .HasName("pk_shippers");

                entity.ToTable("shippers");

                entity.Property(e => e.ShipperId)
                    .HasColumnName("shipper_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasColumnName("company_name")
                    .HasMaxLength(40);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(24);
            });

            modelBuilder.Entity<Suppliers>(entity =>
            {
                entity.HasKey(e => e.SupplierId)
                    .HasName("pk_suppliers");

                entity.ToTable("suppliers");

                entity.Property(e => e.SupplierId)
                    .HasColumnName("supplier_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(60);

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(15);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasColumnName("company_name")
                    .HasMaxLength(40);

                entity.Property(e => e.ContactName)
                    .HasColumnName("contact_name")
                    .HasMaxLength(30);

                entity.Property(e => e.ContactTitle)
                    .HasColumnName("contact_title")
                    .HasMaxLength(30);

                entity.Property(e => e.Country)
                    .HasColumnName("country")
                    .HasMaxLength(15);

                entity.Property(e => e.Fax)
                    .HasColumnName("fax")
                    .HasMaxLength(24);

                entity.Property(e => e.Homepage).HasColumnName("homepage");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(24);

                entity.Property(e => e.PostalCode)
                    .HasColumnName("postal_code")
                    .HasMaxLength(10);

                entity.Property(e => e.Region)
                    .HasColumnName("region")
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<Territories>(entity =>
            {
                entity.HasKey(e => e.TerritoryId)
                    .HasName("pk_territories");

                entity.ToTable("territories");

                entity.Property(e => e.TerritoryId)
                    .HasColumnName("territory_id")
                    .HasMaxLength(20);

                entity.Property(e => e.RegionId).HasColumnName("region_id");

                entity.Property(e => e.TerritoryDescription)
                    .IsRequired()
                    .HasColumnName("territory_description")
                    .HasColumnType("char");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Territories)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_territories_region");
            });

            modelBuilder.Entity<UsStates>(entity =>
            {
                entity.HasKey(e => e.StateId)
                    .HasName("pk_usstates");

                entity.ToTable("us_states");

                entity.Property(e => e.StateId)
                    .HasColumnName("state_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.StateAbbr)
                    .HasColumnName("state_abbr")
                    .HasMaxLength(2);

                entity.Property(e => e.StateName)
                    .HasColumnName("state_name")
                    .HasMaxLength(100);

                entity.Property(e => e.StateRegion)
                    .HasColumnName("state_region")
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
