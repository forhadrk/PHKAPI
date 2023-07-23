using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PHKAPI.Models;
using PMS.API.DBContext;
using System.Data;

namespace PHKAPI.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IDapper _dapper;
        public readonly DatabaseContext _dbContext;
        const string _spName = "SP_GET_LOGIN";

        public LoginController(DatabaseContext dbContext, IDapper dapper)
        {
            _dbContext = dbContext;
            _dapper = dapper;
        }

        [HttpPost("LoginUser")]
        public async Task<List<LoginUserDBModel>> LoginUser(LoginUserDBModel _dbModel)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 1, DbType.Int32);
            dbparams.Add("UserName", _dbModel.UserName, DbType.String);
            dbparams.Add("Password", _dbModel.Password, DbType.String);
            return await Task.FromResult(_dapper.GetAll<LoginUserDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
        }
    }
}
