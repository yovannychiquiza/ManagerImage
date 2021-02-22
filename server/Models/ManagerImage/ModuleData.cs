using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerImage.Models.ManagerImage
{
    public partial class ModuleData
    {
        public int? N { get; set; }
        public string Sn { get; set; }
        public string ModuleNumber { get; set; }
        public string Manufacturer { get; set; }
        public string Product { get; set; }
        public string CalModule { get; set; }
        public DateTime? CalibrationTime { get; set; }
        public double? CalFactor { get; set; }
        public double? Voc { get; set; }
        public double? Isc { get; set; }
        public double? Pmp { get; set; }
        public double? Vmp { get; set; }
        public double? Imp { get; set; }
        public double? Ff { get; set; }
        public int? Irradiance { get; set; }
        public double? Vload { get; set; }
        public double? IatVload { get; set; }
        public double? PowerAtVload { get; set; }
        public double? CellEff { get; set; }
        public double? ModuleEff { get; set; }
        public double? Tamb { get; set; }
        public double? Tref { get; set; }
        public string Classification { get; set; }
        public double? SlopeAtVoc { get; set; }
        public double? SlopeAtIsc { get; set; }
        public DateTime? Time { get; set; }
        public string Operator { get; set; }
        public string Notes { get; set; }
        public string LotName { get; set; }
        public string LotNotes { get; set; }
        public double? Idiff { get; set; }
        public double? Irec { get; set; }
        public double? Rshunt { get; set; }
        public double? Rser { get; set; }
        public double? Isun { get; set; }
        public double? TempCorrAlpha { get; set; }
        public double? TempCorrBeta { get; set; }
        public double? TempCorrCurve { get; set; }
        public double? TempCorrSeriesR { get; set; }
        public double? ModuleLength { get; set; }
        public double? ModuleWidth { get; set; }
        public double? CellArea { get; set; }
        public short? CellsParallel { get; set; }
        public short? CellsSerial { get; set; }
        public double? ProductVoc { get; set; }
        public double? ProductIsc { get; set; }
        public double? ProductFf { get; set; }
        public double? ProductPmax { get; set; }
        public double? ProductPpass { get; set; }
    }
}
