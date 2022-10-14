using EL.BlackList.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EL.BlackList.Services
{
    public interface ILoginRepository
    {
        Task<UserBase> Login(string username, string password);
    }
}
