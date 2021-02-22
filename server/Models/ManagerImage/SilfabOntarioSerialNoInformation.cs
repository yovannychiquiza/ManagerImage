using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerImage.Models.ManagerImage
{
    public partial class SilfabOntarioSerialNoInformation
    {
        public byte[] Timestamp { get; set; }
        public string ItemNo { get; set; }
        public string VariantCode { get; set; }
        public string SerialNo { get; set; }
        public string Description { get; set; }
        public byte Blocked { get; set; }
        public decimal IMax { get; set; }
        public decimal VM { get; set; }
        public decimal PM { get; set; }
        public decimal Vo { get; set; }
        public decimal Isc { get; set; }
        public decimal FF { get; set; }
        public string Tip { get; set; }
        public string PalletNo { get; set; }
        public string CellsLotNo { get; set; }
        public byte ModuleDataLoaded { get; set; }
        public string Product { get; set; }
        public string NewItemNo { get; set; }
        public string ProdOrderNo { get; set; }
        public string SearchString { get; set; }
        public int PalletLineNo { get; set; }
        public string OrderNo { get; set; }
        public int LineNo { get; set; }
        public string CombShipmentNo { get; set; }
        public int CombShipmentLineNo { get; set; }
        public string DeclassNo { get; set; }
        public string SignedByUserId { get; set; }
        public byte SignedWithErrors { get; set; }
        public DateTime SignatureDate { get; set; }
        public string KukaSerialNo { get; set; }
        public byte Reflashed { get; set; }
        public byte DuplicatePanel { get; set; }
        public decimal Temperature { get; set; }
        public decimal Ctm { get; set; }
        public string CellConsumedInKuka { get; set; }
        public int SpcExportImport { get; set; }
        public int FtdExportImport { get; set; }
        public DateTime FlashingDate { get; set; }
        public string DeclaredOn { get; set; }
        public DateTime PassedQc2On { get; set; }
        public string Qc2StationId { get; set; }
        public string SalesShipmentNo { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string BillOfLadingNo { get; set; }
        public DateTime LastFlashDateAndTime { get; set; }
        public DateTime ScannedSerialOnPallet { get; set; }
        public string LastPalletNo { get; set; }
        public string ModuleReclassDtUser { get; set; }
    }
}
