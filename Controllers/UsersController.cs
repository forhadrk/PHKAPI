using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PHKAPI.Models;
using PMS.API.DBContext;
using System.Data;

namespace PHKAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDapper _dapper;
        public readonly DatabaseContext _dbContext;
        const string _spName = "SP_SET_USERS";

        public UsersController(DatabaseContext dbContext, IDapper dapper)
        {
            _dbContext = dbContext;
            _dapper = dapper;
        }

        [HttpPost("SaveUpdateData")]
        public async Task<IActionResult> SaveUpdateData(UsersDBModel _dbModel)
        {
            try
            {
                var dbparams = new DynamicParameters();

                dbparams.Add("QryOption", _dbModel.LoginUserID > 0 ? 2 : 1, DbType.Int32);
                dbparams.Add("LoginUserID", _dbModel.LoginUserID, DbType.Int32);
                dbparams.Add("Name", _dbModel.Name, DbType.String);
                dbparams.Add("Email", _dbModel.Email, DbType.String);
                dbparams.Add("UserName", _dbModel.UserName, DbType.String);
                dbparams.Add("Password", _dbModel.Password, DbType.String);
                dbparams.Add("Active", _dbModel.Active, DbType.Boolean);

                await Task.FromResult(_dapper.Save<UsersDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetAllData")]
        public async Task<List<UsersDBModel>> GetAllData()
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 3, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<UsersDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
        }

        [HttpPost("GetSelectedData")]
        public async Task<List<UsersDBModel>> GetSelectedData(UsersDBModel _dbModel)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 4, DbType.Int32);
            dbparams.Add("LoginUserID", _dbModel.LoginUserID, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<UsersDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
        }

        [HttpPost("DeleteSelectedData")]
        public async Task<IActionResult> DeleteSelectedData(UsersDBModel _dbModel)
        {
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("QryOption", 5, DbType.Int32);
                dbparams.Add("LoginUserID", _dbModel.LoginUserID, DbType.Int32);

                await Task.FromResult(_dapper.Save<UsersDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
