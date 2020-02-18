using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DexterLab.Models.ViewModels
{
    public class SearchDateVM
    {
        public SearchDateVM()
        {

        }

        public SearchDateVM(SearchDateVM row)
        {
            SearchDate = row.SearchDate;
        }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime SearchDate { get; set; }
    }
}