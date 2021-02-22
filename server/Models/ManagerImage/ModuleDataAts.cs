using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerImage.Models.ManagerImage
{
    public partial class ModuleDataAts
    {
        public string SerialNumberNest1 { get; set; }
        public DateTime? TestTime { get; set; }
        public double? Imax { get; set; }
        public double? Isc { get; set; }
        public double? Pmax { get; set; }
        public double? Rs { get; set; }
        public double? Rsh { get; set; }
        public double? Vmax { get; set; }
        public double? Voc { get; set; }
        public string ModuleGrade { get; set; }
        public float? Nest1TestPass { get; set; }
        public float? Nest1TestFail { get; set; }
        public float? PackLocation { get; set; }
        public float? PartInNest1 { get; set; }
        public int? PartType { get; set; }
        public float? PrintDone { get; set; }
        public float? PrintManual { get; set; }
        public float? PrintReject { get; set; }
        public float? SafetyZoneOk { get; set; }
        public bool? Reconciled { get; set; }
        public int Rowid { get; set; }
        public double? Ff { get; set; }
        public double? Stemp { get; set; }
        public double? Psun { get; set; }
        public double? Hpv { get; set; }
        public double? Hpa { get; set; }
        public double? Irr { get; set; }
    }
}
