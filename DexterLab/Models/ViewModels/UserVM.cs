using DexterLab.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DexterLab.Models.ViewModels
{
    public class UserVM
    {
        public UserVM()
        {

        }
        public UserVM(UserDTO row)
        {
            Id = row.Id;
            FirstName = row.FirstName;
            LastName = row.LastName;
            EmailAddress = row.EmailAddress;
            PhoneNumber = row.PhoneNumber;
            Department = row.Department;
            Password = row.Password;
            EmailConfirm = row.EmailConfirm;
            ActivationCode = row.ActivationCode;
            CreatedOn = row.CreatedOn;
            ModifiedOn = row.ModifiedOn;

        }
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Department { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        public bool EmailConfirm { get; set; }
        public string ActivationCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
    public enum DepartmentEntity
    {
        Services,
        HR,
        Sales,
        Operations
    }

}