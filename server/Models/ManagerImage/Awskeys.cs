﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerImage.Models.ManagerImage
{
    public partial class Awskeys
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string S3url { get; set; }
        public string YearKey { get; set; }
        public string Pallet { get; set; }
        public string SerialImage { get; set; }
        public string Size { get; set; }
    }
}
