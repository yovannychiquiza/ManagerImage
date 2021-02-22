using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerImage.Models.ManagerImage
{
    public class LogDto
    {
        public int Id { get; set; }
        public string PalletNo { get; set; }
        public string Sno { get; set; }
        public DateTime? DateModified { get; set; }
        public string Type { get; set; }
    }
}
