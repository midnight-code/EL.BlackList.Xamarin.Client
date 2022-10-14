using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EL.BlackList.Models
{
    public class TaxiPoolModel
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("name")]
        public string Name { get; set; }
    }
}
