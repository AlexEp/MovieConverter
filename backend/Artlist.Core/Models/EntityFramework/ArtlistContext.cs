using System;
using Artlist.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Artlist.Core.Models.EntityFramework
{
    public partial class ArtlistContext : DbContext
    {
        public ArtlistContext()
        {
        }

        public ArtlistContext(DbContextOptions<ArtlistContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ConvertedFile> ConvertedFiles { get; set; }
        public virtual DbSet<Thumbnail> Thumbnails { get; set; }
        public virtual DbSet<UploadedFile> UploadedFiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConvertedFile>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Codec)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.SourceFilesId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.SourceFiles)
                    .WithMany(p => p.ConvertedFiles)
                    .HasForeignKey(d => d.SourceFilesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConvertedFile_SourceFiles");
            });

            modelBuilder.Entity<Thumbnail>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Filename)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.SourceFileId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.SourceFiles)
                    .WithMany(p => p.Thumbnails)
                    .HasForeignKey(d => d.SourceFileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Thumbnail_UploadedFile");
            });

            modelBuilder.Entity<UploadedFile>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Filename)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Hashed).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
