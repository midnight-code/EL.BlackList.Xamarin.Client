using EL.BlackList.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EL.BlackList.Data
{
    public class PaymentUserDB
    {
        private readonly SQLiteAsyncConnection _connection;
        public PaymentUserDB(string connectionString)
        {
            _connection = new SQLiteAsyncConnection(connectionString);
            _connection.CreateTableAsync<PaymentUserModel>().Wait();
        }

        public Task<PaymentUserModel> GetPaymentUserByUserId() => _connection.Table<PaymentUserModel>().FirstOrDefaultAsync();
        public async Task<int> UpdatePaymentUserAsync(PaymentUserModel userBase)
        {
            try
            {
                //List<UserBase> users = await App.BlackListDB.GetAllUserBaseAsync();

                if (!string.IsNullOrWhiteSpace(userBase.UserID))
                {
                    PaymentUserModel userBase1 = GetPaymentUserByUserId().Result;
                    if (GetPaymentUserByUserId().Result != null)
                        return await _connection.UpdateAsync(userBase);
                    else
                        return await _connection.InsertAsync(userBase);
                }
            }
            catch
            {
                await _connection.CreateTableAsync<PaymentUserModel>();
                return await _connection.InsertAsync(userBase);
            }
            finally
            {

            }
            return 0;// Task.Run(() => 0);
        }
    }
}
