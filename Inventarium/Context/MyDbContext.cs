using System;
using System.Collections.Generic;
using Inventarium.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventarium.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Component> Components { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Equipment> Equipment { get; set; }

    public virtual DbSet<Movement> Movements { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=CompReestrDB;Username=postgres;Password=2804");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.ComponentId).HasName("components_pkey");

            entity.ToTable("components");

            entity.HasIndex(e => e.EquipmentId, "IX_components_equipment_id");

            entity.Property(e => e.ComponentId).HasColumnName("component_id");
            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.SerialNumber)
                .HasMaxLength(150)
                .HasColumnName("serial_number");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");

            entity.HasOne(d => d.Equipment).WithMany(p => p.Components)
                .HasForeignKey(d => d.EquipmentId)
                .HasConstraintName("components_equipment_id_fkey");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("employees_pkey");

            entity.ToTable("employees");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.DateOfTermination).HasColumnName("date_of_termination");
            entity.Property(e => e.FirstName)
                .HasMaxLength(150)
                .HasColumnName("first_name");
            entity.Property(e => e.HireDate).HasColumnName("hire_date");
            entity.Property(e => e.LastName)
                .HasMaxLength(150)
                .HasColumnName("last_name");
            entity.Property(e => e.Position)
                .HasMaxLength(150)
                .HasColumnName("position");
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.EquipmentId).HasName("equipment_pkey");

            entity.ToTable("equipment");

            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.Ipadres)
                .HasMaxLength(150)
                .HasColumnName("ipadres");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.SerialNumber)
                .HasMaxLength(150)
                .HasColumnName("serial_number");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Movement>(entity =>
        {
            entity.HasKey(e => e.MovementId).HasName("movements_pkey");

            entity.ToTable("movements");

            entity.HasIndex(e => e.EmployeeId, "IX_movements_employee_id");

            entity.HasIndex(e => e.EquipmentId, "IX_movements_equipment_id");

            entity.HasIndex(e => e.SourceRoomId, "IX_movements_source_room_id");

            entity.HasIndex(e => e.TargetRoomId, "IX_movements_target_room_id");

            entity.Property(e => e.MovementId).HasColumnName("movement_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.MovementDate).HasColumnName("movement_date");
            entity.Property(e => e.SourceRoomId).HasColumnName("source_room_id");
            entity.Property(e => e.TargetRoomId).HasColumnName("target_room_id");

            entity.HasOne(d => d.Employee).WithMany(p => p.Movements)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("movements_employee_id_fkey");

            entity.HasOne(d => d.Equipment).WithMany(p => p.Movements)
                .HasForeignKey(d => d.EquipmentId)
                .HasConstraintName("movements_equipment_id_fkey");

            entity.HasOne(d => d.SourceRoom).WithMany(p => p.MovementSourceRooms)
                .HasForeignKey(d => d.SourceRoomId)
                .HasConstraintName("movements_source_room_id_fkey");

            entity.HasOne(d => d.TargetRoom).WithMany(p => p.MovementTargetRooms)
                .HasForeignKey(d => d.TargetRoomId)
                .HasConstraintName("movements_target_room_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.RoleName, "roles_role_name_key").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("rooms_pkey");

            entity.ToTable("rooms");

            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.RoomNumber).HasColumnName("room_number");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.RoleId, "IX_users_role_id");

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("users_role_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
