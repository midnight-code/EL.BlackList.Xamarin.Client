using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EL.BlackList.Models
{
    public class DriverModel
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("firstName")]
        public string FirstName { get; set; }
        [Column("lastName")]
        public string LastName { get; set; }
        [Column("secondName")]
        public string SecondName { get; set; }
        [Column("inn")]
        public int INN { get; set; }
        [Column("passportID")]
        public int PassportID { get; set; }
        [Column("blackList")]
        public bool BlackList { get; set; }
        [Column("avatar")]
        public int Avatar { get; set; }
        [Column("dataRogden")]
        public DateTime DataRogden { get; set; }
        [Column("id_taxipool")]
        public int TaxiPoolID { get; set; }
        [Column("id_driverlicense")]
        public int DriverLicenseID { get; set; }
        //public TaxiPoolModel TaxiPoolModel { get; set; }
        public List<FeedBackModel> FeedBacks { get; set; }
    }
}
