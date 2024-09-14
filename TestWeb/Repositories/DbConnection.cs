using Dapper;
using MySqlConnector;
using System.Collections.Immutable;
using System.Data;
using TestWeb.Controllers;

namespace TestWeb.Repositories
{
    public class DbConnection : IDisposable
    {
        private IDbConnection? db = null;
        private string _connectStr;
        private readonly ILogger<HomeController>? _logger;
        public DbConnection()
        {        
            string connectionStr = @"server=localhost;user id=DevAuth;password=Dev123456;database=DevDb;port=3306;";
            _connectStr = connectionStr;
            db = new MySqlConnection(_connectStr);
            //db.Open();
        }

        public IEnumerable<T> Query<T>(string sql)
        {
            try
            {
                return db.Query<T>(sql);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                throw;
            }
        }
        public IEnumerable<T> Query<T>(string sql, object param)
        {
            try
            {
                return db.Query<T>(sql, param);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                throw;
            }
        }
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param)
        {
            try
            {
                var result = await db.QueryAsync<T>(sql, param);
                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                throw;
            }          
        }
        public async Task<int> ExecuteAsync(string sql, object param)
        {
            return await db.ExecuteAsync(sql, param);
        }
        //release connection
        public void Dispose()
        {
            db?.Dispose();
        }
        //public void Dispose() => db?.Dispose();
    }
}
