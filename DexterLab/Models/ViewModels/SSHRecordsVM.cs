using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DexterLab.Models.ViewModels
{
    public class SSHRecordsVM
    {
        public SSHRecordsVM()
        {

        }

        public SSHRecordsVM(SSHRecordsVM row)
        {
            Id = row.Id;
            SSHUser = row.SSHUser;
            SSHPassword = row.SSHPassword;
            

        }

        public int Id { get; set; }
        [Required]
        public string SSHUser { get; set; }
        [Required]
        public string SSHPassword { get; set; }
        [Required]
        public string SSHPasswordConfirm { get; set; }
    }
}