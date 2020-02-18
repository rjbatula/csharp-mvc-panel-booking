﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DexterLab.Models.Data
{
    [Table("tblBookings")]
    public class BookingDTO
    {
        [Key]
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public string DeviceSerialNo { get; set; }
        public int DeviceSpace { get; set; }
        public DateTime BookingDate { get; set; }
        public int PanelStart { get; set; }
        public int PanelEnd { get; set; }
        public string BookingPurpose { get; set; }
        public bool ServerInstalled { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
    }
}