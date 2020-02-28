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
            DeviceType = row.DeviceType;
            IPAddress = row.IPAddress;
            Username = row.Username;
            Password = row.Password;


        }
        public int Id { get; set; }
        [Required]
        public string DeviceSerialNo { get; set; }
        [Required]
        public string BookingPurpose { get; set; }
        [Required]
        public bool ServerInstalled { get; set; }
        public string DeviceType { get; set; }
        public string IPAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }


    }
}