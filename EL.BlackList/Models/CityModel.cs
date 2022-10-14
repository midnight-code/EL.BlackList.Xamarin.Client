using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EL.BlackList.Models
{
    public class CityModel
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("region")]
        public string Region { get; set; }
    }
}
