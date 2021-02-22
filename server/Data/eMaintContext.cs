using ManagerImage.Models.ManagerImage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerImage.Data
{
    public partial class eMaintContext : DbContext
    {
        public eMaintContext()
        {
        }

        public eMaintContext(DbContextOptions<eMaintContext> options)
            : base(options)
        {
        }


        public virtual DbSet<PmtCompletion> PmtCompletion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PmtCompletion>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("PMT_COMPLETION");

                entity.Property(e => e.Percentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.WeeklyPm).HasColumnName("WeeklyPM");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
