using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace NPlatform.EFRepository.Models
{
    public partial class nframeContext : DbContext
    {
        public nframeContext()
        {
        }

        public nframeContext(DbContextOptions<nframeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Testuser> Testusers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("Server=localhost;Uid=root;Pwd=ydl825913;Database=nframe;SslMode=none");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Testuser>(entity =>
            {
                entity.ToTable("testuser");

                entity.Property(e => e.Id)
                    .HasMaxLength(200)
                    .HasColumnName("id")
                    .HasComment("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .HasColumnName("address")
                    .HasComment("地址");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password")
                    .HasComment("密码");

                entity.Property(e => e.UserName)
                    .HasMaxLength(255)
                    .HasColumnName("userName")
                    .HasComment("用户名");

                entity.Property(e => e.Work)
                    .HasMaxLength(255)
                    .HasColumnName("work")
                    .HasComment("工作");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
