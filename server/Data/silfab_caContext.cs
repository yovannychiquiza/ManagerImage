using ManagerImage.Models.ManagerImage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerImage.Data
{
    public partial class Silfab_caContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public Silfab_caContext(DbContextOptions<Silfab_caContext> options) : base(options)
        {
        }

        public Silfab_caContext()
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        public DbSet<ModuleData> ModuleData { get; set; }
        public DbSet<ModuleDataAts> ModuleDataAts { get; set; }
        public DbSet<SilfabOntarioSerialNoInformation> SilfabOntarioSerialNoInformation { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);


            builder.Entity<ModuleData>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ModuleData");

                entity.Property(e => e.CalModule).HasMaxLength(50);

                entity.Property(e => e.CalibrationTime).HasColumnType("datetime");

                entity.Property(e => e.Classification).HasMaxLength(50);

                entity.Property(e => e.Ff).HasColumnName("FF");

                entity.Property(e => e.IatVload).HasColumnName("IAtVload");

                entity.Property(e => e.LotName).HasMaxLength(50);

                entity.Property(e => e.LotNotes).HasMaxLength(50);

                entity.Property(e => e.Manufacturer).HasMaxLength(50);

                entity.Property(e => e.ModuleNumber).HasMaxLength(50);

                entity.Property(e => e.Notes).HasMaxLength(50);

                entity.Property(e => e.Operator).HasMaxLength(50);

                entity.Property(e => e.Product).HasMaxLength(50);

                entity.Property(e => e.ProductFf).HasColumnName("ProductFF");

                entity.Property(e => e.Sn)
                            .HasColumnName("SN")
                            .HasMaxLength(50);

                entity.Property(e => e.Time).HasColumnType("datetime");
            });


            builder.Entity<ModuleDataAts>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ModuleDataATS");

                entity.Property(e => e.Ff).HasColumnName("ff");

                entity.Property(e => e.Hpa).HasColumnName("hpa");

                entity.Property(e => e.Hpv).HasColumnName("hpv");

                entity.Property(e => e.Imax).HasColumnName("imax");

                entity.Property(e => e.Irr).HasColumnName("irr");

                entity.Property(e => e.Isc).HasColumnName("isc");

                entity.Property(e => e.ModuleGrade)
                    .HasColumnName("module_grade")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nest1TestFail).HasColumnName("nest_1_test_fail");

                entity.Property(e => e.Nest1TestPass).HasColumnName("nest_1_test_pass");

                entity.Property(e => e.PackLocation).HasColumnName("pack_location");

                entity.Property(e => e.PartInNest1).HasColumnName("part_in_nest_1");

                entity.Property(e => e.PartType).HasColumnName("part_type");

                entity.Property(e => e.Pmax).HasColumnName("pmax");

                entity.Property(e => e.PrintDone).HasColumnName("print_done");

                entity.Property(e => e.PrintManual).HasColumnName("print_manual");

                entity.Property(e => e.PrintReject).HasColumnName("print_reject");

                entity.Property(e => e.Psun).HasColumnName("psun");

                entity.Property(e => e.Reconciled).HasColumnName("reconciled");

                entity.Property(e => e.Rowid)
                    .HasColumnName("rowid")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Rs).HasColumnName("rs");

                entity.Property(e => e.Rsh).HasColumnName("rsh");

                entity.Property(e => e.SafetyZoneOk).HasColumnName("safety_zone_ok");

                entity.Property(e => e.SerialNumberNest1)
                    .HasColumnName("serial_number_nest_1")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Stemp).HasColumnName("stemp");

                entity.Property(e => e.TestTime)
                    .HasColumnName("test_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Vmax).HasColumnName("vmax");

                entity.Property(e => e.Voc).HasColumnName("voc");
            });


            builder.Entity<SilfabOntarioSerialNoInformation>(entity =>
            {
                entity.HasKey(e => new { e.ItemNo, e.VariantCode, e.SerialNo })
                    .HasName("Silfab Ontario$Serial No_ Information$0");

                entity.ToTable("Silfab Ontario$Serial No_ Information");

                entity.HasIndex(e => new { e.SerialNo, e.ItemNo, e.VariantCode })
                    .HasName("$1")
                    .IsUnique();

                entity.HasIndex(e => new { e.KukaSerialNo, e.ItemNo, e.VariantCode, e.SerialNo })
                    .HasName("$4")
                    .IsUnique();

                entity.HasIndex(e => new { e.SearchString, e.ItemNo, e.VariantCode, e.SerialNo })
                    .HasName("$2")
                    .IsUnique();

                entity.HasIndex(e => new { e.PalletNo, e.PalletLineNo, e.ItemNo, e.VariantCode, e.SerialNo })
                    .HasName("$3")
                    .IsUnique();

                entity.Property(e => e.ItemNo)
                    .HasColumnName("Item No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.VariantCode)
                    .HasColumnName("Variant Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SerialNo)
                    .HasColumnName("Serial No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BillOfLadingNo)
                    .IsRequired()
                    .HasColumnName("Bill of Lading No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CellConsumedInKuka)
                    .IsRequired()
                    .HasColumnName("Cell Consumed in Kuka")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.CellsLotNo)
                    .IsRequired()
                    .HasColumnName("Cells Lot No_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CombShipmentLineNo).HasColumnName("Comb_ Shipment Line No_");

                entity.Property(e => e.CombShipmentNo)
                    .IsRequired()
                    .HasColumnName("Comb_ Shipment No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Ctm)
                    .HasColumnName("CTM")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.DeclaredOn)
                    .IsRequired()
                    .HasColumnName("Declared On")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.DeclassNo)
                    .IsRequired()
                    .HasColumnName("Declass No_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DuplicatePanel).HasColumnName("Duplicate Panel");

                entity.Property(e => e.FF)
                    .HasColumnName("F_F_")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.FlashingDate)
                    .HasColumnName("Flashing Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.FtdExportImport).HasColumnName("FTD Export Import");

                entity.Property(e => e.IMax)
                    .HasColumnName("I Max")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Isc).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.KukaSerialNo)
                    .IsRequired()
                    .HasColumnName("Kuka Serial No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastFlashDateAndTime)
                    .HasColumnName("Last Flash Date and Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.LastPalletNo)
                    .IsRequired()
                    .HasColumnName("Last Pallet No_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LineNo).HasColumnName("Line No_");

                entity.Property(e => e.ModuleDataLoaded).HasColumnName("Module Data Loaded");

                entity.Property(e => e.ModuleReclassDtUser)
                    .IsRequired()
                    .HasColumnName("Module Reclass_ DT - User")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NewItemNo)
                    .IsRequired()
                    .HasColumnName("New Item No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OrderNo)
                    .IsRequired()
                    .HasColumnName("Order No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PM)
                    .HasColumnName("P_M_")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.PalletLineNo).HasColumnName("Pallet Line No_");

                entity.Property(e => e.PalletNo)
                    .IsRequired()
                    .HasColumnName("Pallet No_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PassedQc2On)
                    .HasColumnName("Passed QC2 On")
                    .HasColumnType("datetime");

                entity.Property(e => e.ProdOrderNo)
                    .IsRequired()
                    .HasColumnName("Prod_ Order No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qc2StationId)
                    .IsRequired()
                    .HasColumnName("QC2 Station ID")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.SalesShipmentNo)
                    .IsRequired()
                    .HasColumnName("Sales Shipment No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ScannedSerialOnPallet)
                    .HasColumnName("Scanned Serial on Pallet")
                    .HasColumnType("datetime");

                entity.Property(e => e.SearchString)
                    .IsRequired()
                    .HasColumnName("Search String")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.ShipmentDate)
                    .HasColumnName("Shipment Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.SignatureDate)
                    .HasColumnName("Signature Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.SignedByUserId)
                    .IsRequired()
                    .HasColumnName("Signed By User ID")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SignedWithErrors).HasColumnName("Signed With Errors");

                entity.Property(e => e.SpcExportImport).HasColumnName("SPC Export Import");

                entity.Property(e => e.Temperature).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.Tip)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.VM)
                    .HasColumnName("V_M_")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Vo)
                    .HasColumnName("VO")
                    .HasColumnType("decimal(38, 20)");
            });


            this.OnModelBuilding(builder);

        }
    }
}
