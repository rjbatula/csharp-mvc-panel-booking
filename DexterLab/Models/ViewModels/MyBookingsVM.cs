using DexterLab.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DexterLab.Models.ViewModels
{
    public class MyBookingsVM
    {
        private BookingDTO dto;

        public MyBookingsVM()
        {

        }

        public MyBookingsVM(MyBookingsVM row)
        {
            Id = row.Id;
            DeviceName = row.DeviceName;
            DeviceSerialNo = row.DeviceSerialNo;
            DeviceSpace = row.DeviceSpace;
            BookingDate = row.BookingDate;
            BookingEndDate = row.BookingEndDate;
            PanelStart = row.PanelStart;
            PanelEnd = row.PanelEnd;
            BookingPurpose = row.BookingPurpose;
            ServerInstalled = row.ServerInstalled;
            ModifiedBy = row.ModifiedBy;
            CreatedBy = row.CreatedBy;
            IPAddress = row.IPAddress;
            Username = row.Username;
            Password = row.Password;
            DeviceType = row.DeviceType;
        }

        public MyBookingsVM(BookingDTO dto)
        {
            this.dto = dto;
        }

        public int Id { get; set; }
        [Required]
        public string DeviceName { get; set; }
        [Required]
        public string DeviceSerialNo { get; set; }
        [Required]
        public int DeviceSpace { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BookingDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BookingEndDate { get; set; }
        [Required]
        public int PanelStart { get; set; }
        [Required]
        public int PanelEnd { get; set; }
        [Required]
        public string BookingPurpose { get; set; }
        [Required]
        public bool ServerInstalled { get; set; }
        [Required]
        public string ModifiedBy { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public string IPAddress { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string DeviceType { get; set; }
    }
}