using EL.BlackList.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EL.BlackList.Data
{
    
    public class BlackListDB
    {
        private readonly SQLiteAsyncConnection _connection;
        public BlackListDB(string connectionString)
        {
            _connection = new SQLiteAsyncConnection(connectionString);
            _connection.CreateTableAsync<UserBase>().Wait();
        }
        public Task<UserBase> GetUserBaseAsync() => _connection.Table<UserBase>().FirstOrDefaultAsync();
        public Task<List<UserBase>> GetAllUserBaseAsync() => _connection.Table<UserBase>().ToListAsync();

        public Task<UserBase> GetUserBaseByIdAsync(string userid) => _connection.Table<UserBase>().Where(i => i.UserID == userid).FirstOrDefaultAsync();
        public async Task<int> UpdateUserBaseAsync(UserBase userBase)
        {
            try
            {
                List<UserBase> users = await App.BlackListDB.GetAllUserBaseAsync();

                if (!string.IsNullOrWhiteSpace(userBase.UserID))
                {
                    UserBase userBase1 = GetUserBaseByIdAsync(userBase.UserID).Result;
                    if (GetUserBaseByIdAsync(userBase.UserID).Result != null)
                        return await _connection.UpdateAsync(userBase);
                    else
                        return await _connection.InsertAsync(userBase);
                }
            }
            catch
            {
                await _connection.CreateTableAsync<UserBase>();
                return await _connection.InsertAsync(userBase);
            }
            finally
            {

            }
            return 0;// Task.Run(() => 0);
        }
        public async void DroupTable()
        {
            await _connection.DropTableAsync<UserBase>();
        }
    }
}
