using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EL.BlackList.Models
{
    public class PaymentUserModel
    {
        [PrimaryKey]
        public string UserID { get; set; }
        public DateTime DatePayment { get; set; }
        public int PeriodPayment { get; set; }
        public double Contributed { get; set; }
        public bool Current { get; set; }
    }
}
