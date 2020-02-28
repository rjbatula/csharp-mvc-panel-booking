using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DexterLab.Models.ViewModels
{
    public class BookingVM
    {
        public BookingVM()
        {

        }

        public BookingVM(BookingVM row)
        {
            Id = row.Id;
            DeviceName = row.DeviceName;
            DeviceSerialNo = row.DeviceSerialNo;
            DeviceType = row.DeviceType;
            BookingDate = row.BookingDate;
            BookingPurpose = row.BookingPurpose;

        }

        public int Id { get; set; }
        [Required]
        public string DeviceName { get; set; }
        [Required]
        public string DeviceSerialNo { get; set; }
        [Required]
        public string DeviceType { get; set; }
 
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BookingDate { get; set; }
        [Required]
        public string BookingPurpose { get; set; }
    }
}