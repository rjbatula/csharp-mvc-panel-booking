using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DexterLab.Models.ViewModels
{
    public class DeleteSSHVM
    {
        public DeleteSSHVM()
        {

        }

        public DeleteSSHVM(DeleteSSHVM row)
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
    }
}