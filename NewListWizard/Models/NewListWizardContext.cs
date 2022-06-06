using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NewListWizard.Models
{
    public partial class NewListWizardContext : DbContext
    {
        //public NewListWizardContext()
        //{
        //}

        public NewListWizardContext(DbContextOptions<NewListWizardContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CsvContent> CsvContents { get; set; } = null!;
        public virtual DbSet<UserInfo> UserInfos { get; set; } = null!;
        public virtual DbSet<WizardList> WizardLists { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source= YSALOKHE-LAP-05\\MSSQLSERVER01 ;Initial Catalog=NewListWizard;Integrated Security=SSPI");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CsvContent>(entity =>
            {
                entity.HasKey(e => e.CsvId)
                    .HasName("PK__CsvConte__AA1473CD575E0E3F");

                entity.ToTable("CsvContent");

                entity.Property(e => e.CsvId).HasColumnName("csvId");

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("companyName");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("lastName");

                entity.Property(e => e.ListId).HasColumnName("listId");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.HasOne(d => d.List)
                    .WithMany(p => p.CsvContents)
                    .HasForeignKey(d => d.ListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CsvConten__listI__3C69FB99");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserInfo__CB9A1CFFDCBC1E82");

                entity.ToTable("UserInfo");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("companyName");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FailedAttempts).HasColumnName("failedAttempts");

                entity.Property(e => e.IsLockedOut).HasColumnName("isLockedOut");

                entity.Property(e => e.IsRememberMe).HasColumnName("isRememberMe");

                entity.Property(e => e.LastLoggedIn)
                    .HasColumnType("date")
                    .HasColumnName("lastLoggedIn");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("phoneNumber");
            });

            modelBuilder.Entity<WizardList>(entity =>
            {
                entity.HasKey(e => e.ListId)
                    .HasName("PK__WizardLi__7D4CA77BE4FB053D");

                entity.ToTable("WizardList");

                entity.Property(e => e.ListId).HasColumnName("listId");

                entity.Property(e => e.AssignedTo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("assignedTo");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("createdDate");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.ListName)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("listName");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("date")
                    .HasColumnName("modifiedDate");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WizardLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__WizardLis__userI__38996AB5");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
