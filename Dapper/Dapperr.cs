using Dapper;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace PHKAPI
{
    public class Dapperr : IDapper
    {

        private readonly IConfiguration _config;
        private string Connectionstring = "DbConnection";

        public Dapperr(IConfiguration config)
        {
            _config = config;
        }

        public void Dispose()
        {

        }

        public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            var result = db.Query(sp, parms, commandType: commandType);
            return 1;
        }

        public T Edit<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {

            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();

        }

        public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {

            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            return db.Query<T>(sp, parms, commandType: commandType).ToList();

        }

        public DbConnection GetDbconnection()
        {
            return new SqlConnection(_config.GetConnectionString(Connectionstring));
        }

        public T Save<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {

            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));

            try
            {

                if (db.State == ConnectionState.Closed)
                    db.Open();
                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }

                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }
            return result;
        }

        public T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {

            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {

                if (db.State == ConnectionState.Closed)
                    db.Open();
                using var tran = db.BeginTransaction();

                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }

                catch (Exception ex)
                {

                    tran.Rollback();
                    throw ex;

                }

            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {

                if (db.State == ConnectionState.Open)
                    db.Close();
            }
            return result;
        }

    }
}
