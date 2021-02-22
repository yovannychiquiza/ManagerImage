using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

using ManagerImage.Models.ManagerImage;

namespace ManagerImage.Data
{
  public partial class ManagerImageContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public ManagerImageContext(DbContextOptions<ManagerImageContext> options):base(options)
    {
    }

    public ManagerImageContext()
    {
    }

    partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<ManagerImage.Models.ManagerImage.BatchTask>()
              .Property(p => p.LastEjecution)
              .HasColumnType("datetime");

            modelBuilder.Entity<ManagerImage.Models.ManagerImage.BatchTask>()
              .Property(p => p.CreationDate)
              .HasColumnType("datetime");


            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.DateModified)
                    .HasColumnName("Date_Modified")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateUpload).HasColumnType("datetime");

                entity.Property(e => e.PalletNo).HasMaxLength(50);

                entity.Property(e => e.S3url)
                    .HasColumnName("S3URL")
                    .HasMaxLength(150);

                entity.Property(e => e.Sno)
                    .HasColumnName("SNo")
                    .HasMaxLength(50);

                entity.Property(e => e.UplodedToCloud).HasDefaultValueSql("((0))");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Auto')");
            });

            modelBuilder.Entity<Awskeys>(entity =>
            {
                entity.ToTable("AWSkeys");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.Pallet)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.S3url)
                    .IsRequired()
                    .HasColumnName("S3URL")
                    .HasMaxLength(150);

                entity.Property(e => e.SerialImage)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.YearKey)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            this.OnModelBuilding(modelBuilder);
        }

        public DbSet<Log> Log { get; set; }
        public DbSet<BatchTask> BatchTasks { get; set; }
        public virtual DbSet<Awskeys> Awskeys { get; set; }

    }
}
