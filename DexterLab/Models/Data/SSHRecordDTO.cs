using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DexterLab.Models.Data
{
    [Table("tblSSHRecords")]
    public class SSHRecordDTO
    {
        [Key]
        public int Id { get; set; }
        public string SSHUser { get; set; }
        public string SSHPassword { get; set; }
    }
}