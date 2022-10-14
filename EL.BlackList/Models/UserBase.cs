using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EL.BlackList.Models
{
    /// <summary>
    /// Local base
    /// </summary>
    public partial class UserBase
    {
        [PrimaryKey]
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondName { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberPublic { get; set; }
        public string NameCompPublic { get; set; }
        public string Token { get; set; }
        public int PinCode { get;set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public int CityID { get; set; }
        public string CityName { get; set; }
        public int TaxiPoolID { get; set; }
        public string LoginUser { get; set; }

    }
}
