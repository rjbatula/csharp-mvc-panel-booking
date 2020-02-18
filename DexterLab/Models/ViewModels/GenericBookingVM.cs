using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DexterLab.Models.ViewModels
{
    public class GenericBookingVM
    {
        public GenericBookingVM()
        {

        }

        public GenericBookingVM(GenericBookingVM row)
        {
            Id = row.Id;
            DeviceName = row.DeviceName;
            BookingDate = row.BookingDate;
            PanelStart = row.PanelStart;
            PanelEnd = row.PanelEnd;
            ServerInstalled = row.ServerInstalled;

        }

        public int Id { get; set; }
        [Required]
        public string DeviceName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BookingDate { get; set; }
        [Required]
        public int PanelStart { get; set; }
        [Required]
        public int PanelEnd { get; set; }
        [Required]
        public bool ServerInstalled { get; set; }

    }
}