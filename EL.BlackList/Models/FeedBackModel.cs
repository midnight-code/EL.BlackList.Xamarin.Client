using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EL.BlackList.Models
{
    public class FeedBackModel
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("id_driver")]
        public int DriverID { get; set; }
        [Column("id_taxpool")]
        public int TaxiPoolID { get; set; }
        [Column("subjest")]
        public string Subjest { get; set; }
        [Column("dateadd")]
        public DateTime DateADD { get; set; }
        [Column("id_user")]
        public string UserID { get; set; }
        [Column("id_city")]
        public int CityID { get; set; }
        public CityModel City { get; set; }
        public TaxiPoolModel TaxiPool { get; set; }
    }
}
