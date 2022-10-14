using System;
using System.Collections.Generic;
using System.Text;

namespace EL.BlackList.Models
{
    public class UserRegClass
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondName { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberPublic { get; set; }
        public string NameCompPublic { get; set; }
        public int CityID { get; set; }
        public string CityName { get; set; }
        public int TaxiPoolID { get; set; }
    }
}
