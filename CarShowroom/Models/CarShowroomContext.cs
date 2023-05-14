using System;
using System.Collections.Generic;
using CarShowroom.Entities;
using Microsoft.EntityFrameworkCore;


namespace CarShowroom.Data;

public partial class CarShowroomContext : DbContext
{
    
    public CarShowroomContext(DbContextOptions<CarShowroomContext> options)
        : base(options)
    {
    }
    
    public DbSet<Car> Cars { get; set; }
    
    public virtual DbSet<CarExtra> CarExtras { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Extra> Extras { get; set; }

    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderExtra> OrderExtras { get; set; }

    public virtual DbSet<Storage> Storages { get; set; }

    public virtual DbSet<TestDrive> TestDrives { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=DESKTOP-C7ALLTO;Database=CarShowroom;Integrated Security=True");
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.CarId)
                .HasName("PK_CAR")
                .IsClustered(false);

            entity.ToTable("Car", "19118133");

            entity.Property(e => e.CarId)
                .ValueGeneratedNever()
                .HasColumnName("CarID");
            entity.Property(e => e.AverageExpenseCombined)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("AverageExpenseCOMBINED");
            entity.Property(e => e.AverageExpenseOnroad)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("AverageExpenseONROAD");
            entity.Property(e => e.AverageExpenseTown)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("AverageExpenseTOWN");
            entity.Property(e => e.Height).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Hp).HasColumnName("HP");
            entity.Property(e => e.Model).HasColumnType("text");
            entity.Property(e => e.OriginalPrice).HasColumnType("money");
            entity.Property(e => e.PictureUrl)
                .HasColumnType("text")
                .HasColumnName("PictureURL");
            entity.Property(e => e.TypeCompartment).HasColumnType("text");
            entity.Property(e => e.TypeEngine).HasColumnType("text");
            entity.Property(e => e.TypeFuel).HasColumnType("text");
            entity.Property(e => e.Weight).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.YearOfManufacure).HasColumnType("datetime");
        });

        modelBuilder.Entity<CarExtra>(entity =>
        {
            entity.HasKey(e => e.CarExtraId)
                .HasName("PK_CAREXTRA")
                .IsClustered(false);
            entity
                .ToTable("CarExtra", "19118133");

            entity.Property(e => e.CarId).HasColumnName("CarID");
            entity.Property(e => e.ExtraId).HasColumnName("ExtraID");

            entity.HasOne(d => d.Car).WithMany()
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CarExtra_Car");

            entity.HasOne(d => d.Extra).WithMany()
                .HasForeignKey(d => d.ExtraId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CarExtra_Extra");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer", "19118133");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(50);
        });

        modelBuilder.Entity<Extra>(entity =>
        {
            entity.HasKey(e => e.ExtraId)
                .HasName("PK_EXTRA")
                .IsClustered(false);

            entity.ToTable("Extra", "19118133");

            entity.Property(e => e.ExtraId)
                .ValueGeneratedNever()
                .HasColumnName("ExtraID");
            entity.Property(e => e.ExtraName).HasColumnType("text");
            entity.Property(e => e.Price).HasColumnType("money");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId)
                .HasName("PK_ORDER")
                .IsClustered(false);

            entity.ToTable("Order", "19118133");

            entity.Property(e => e.OrderId)
                .HasColumnName("OrderID");
            entity.Property(e => e.CarId).HasColumnName("CarID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.OriginalPrice).HasColumnType("money");
            entity.Property(e => e.TotalSum).HasColumnType("money");

            entity.HasOne(d => d.Car).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK_Order_Car");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Customer");
        });
        modelBuilder.Entity<OrderExtra>(entity =>
        {
            entity.HasKey(e => e.OrderExtraId)
                .HasName("PK_ORDEREXTRA")
                .IsClustered(false);
            entity
                .ToTable("OrderExtra", "19118133");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ExtraId).HasColumnName("ExtraID");

            entity.HasOne(d => d.Order).WithMany()
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderExtra_Order");

            entity.HasOne(d => d.Extra).WithMany()
                .HasForeignKey(d => d.ExtraId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CarExtra_Extra");
        });

        modelBuilder.Entity<Storage>(entity =>
        {
            entity.HasKey(e => e.StorageId)
                .HasName("PK_STORAGE")
                .IsClustered(false);

            entity.ToTable("Storage", "19118133");

            entity.Property(e => e.StorageId)
                .ValueGeneratedNever()
                .HasColumnName("StorageID");
            entity.Property(e => e.YearOfManufacture).HasColumnType("datetime");
        });

        modelBuilder.Entity<TestDrive>(entity =>
        {
            entity.HasKey(e => e.TestDriveId)
                .HasName("PK_TESTDRIVE")
                .IsClustered(false);

            entity.ToTable("TestDrive", "19118133");

            entity.HasIndex(e => e.CarId, "Relationship_3_FK");

            entity.Property(e => e.TestDriveId)
                .ValueGeneratedNever()
                .HasColumnName("TestDriveID");
            entity.Property(e => e.CarId).HasColumnName("CarID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DateOfQuery).HasColumnType("datetime");
            entity.Property(e => e.DateOfTestDrive).HasColumnType("datetime");

            entity.HasOne(d => d.Car).WithMany(p => p.TestDrives)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestDrive_Car");

            entity.HasOne(d => d.Customer).WithMany(p => p.TestDrives)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestDrive_Customer");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
