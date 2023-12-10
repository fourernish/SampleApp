using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SampleApp.Models
{
    public partial class SampleAppContext : DbContext
    {
        public SampleAppContext()
        {
        }

        public SampleAppContext(DbContextOptions<SampleAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Micropost> Microposts { get; set; } = null!;
        public virtual DbSet<Relation> Relations { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SampleApp;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Micropost>(entity =>
            {
                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Microposts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Microposts_ToUsers");
            });

            modelBuilder.Entity<Relation>(entity =>
            {
                entity.HasIndex(e => new { e.FollowerId, e.FollowedId }, "UniqPairFollowedFollower")
                    .IsUnique();

                entity.HasOne(d => d.Followed)
                    .WithMany(p => p.RelationFolloweds)
                    .HasForeignKey(d => d.FollowedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Followed");

                entity.HasOne(d => d.Follower)
                    .WithMany(p => p.RelationFollowers)
                    .HasForeignKey(d => d.FollowerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Follower");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordConfirmation)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Password_Confirmation");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
