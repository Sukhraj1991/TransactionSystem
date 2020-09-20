using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionSystem.Models
{
    public class AddTransactionModel
    {
        [Required(ErrorMessage ="Select Transaction Type")]
        public string TransactionType {get;set;}
        [Required(ErrorMessage = "Description is required")]
        
        public string Descreption { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid amount")]
        public double Amount { get; set; }
        
    }
}
