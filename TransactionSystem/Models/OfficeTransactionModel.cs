using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionSystem.Models
{
    public class OfficeTransactionModel
    {
        public int TransactionID { get; set; }
        public string TransactionDate { get; set; }
        public string Descreption { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }

        public string RunningBalance { get; set; }
    }
}
