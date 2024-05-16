
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EA.Domain.FILE.Models;

namespace EA.Infra.FILE.Mappings
{
    public class AccessPermissionMap : IEntityTypeConfiguration<AccessPermission>
    {
        public void Configure(EntityTypeBuilder<AccessPermission> entity)
        {
            entity.ToTable("AccessPermission", "file");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Item).WithMany(p => p.AccessPermissions)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_AccessPermission_Item");
        }
    }
    public class ActivityMap : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> entity)
        {
            entity.ToTable("Activity", "file");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Body).HasMaxLength(500);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Item).WithMany(p => p.Activities)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Activity_Item");
        }
    }
    public class ItemMap : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> entity)
        {
            entity.ToTable("Item", "file");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Cdn).HasMaxLength(255);
            entity.Property(e => e.Content).HasColumnType("ntext");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.LocalPath).HasMaxLength(255);
            entity.Property(e => e.MimeType).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Product)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Tenant)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Workspace).HasMaxLength(255);

            entity.HasOne(d => d.ProductNavigation).WithMany(p => p.Items)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.Product)
                .HasConstraintName("FK_Item_Product");
        }
    }
    public class OpenActivityMap : IEntityTypeConfiguration<OpenActivity>
    {
        public void Configure(EntityTypeBuilder<OpenActivity> entity)
        {
            entity.ToTable("OpenActivity", "file");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.OpenDate).HasColumnType("datetime");

            entity.HasOne(d => d.Item).WithMany(p => p.OpenActivities)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OpenActivity_Item");
        }
    }
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity.ToTable("Product", "file");

            entity.HasIndex(e => e.Code, "IX_Product").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(255);
        }
    }
}
