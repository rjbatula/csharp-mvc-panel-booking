using DexterLab.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DexterLab.Models.ViewModels
{
    public class ResetPasswordVM
    {
        public ResetPasswordVM()
        {

        }
        public ResetPasswordVM(UserDTO row)
        {
            ResetCode = row.ResetCode;
            Password = row.Password;
            

        }
        public string ResetCode { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
