using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DexterLab.Models.ViewModels
{
    public class EditBookingsVM
    {
        public EditBookingsVM()
        {

        }

        public EditBookingsVM(EditBookingsVM row)
        {
            Id = row.Id;
            DeviceSerialNo = row.DeviceSerialNo;
            BookingPurpose = row.BookingPurpose;
            ServerInstalled = row.ServerInstalled;

        }
        public int Id { get; set; }
        [Required]
        public string DeviceSerialNo { get; set; }
        [Required]
        public string BookingPurpose { get; set; }
        [Required]
        public bool ServerInstalled { get; set; }

    }
}