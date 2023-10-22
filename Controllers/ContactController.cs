using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PHKAPI.Models;
using PMS.API.DBContext;
using System.Data;

namespace PHKAPI.Controllers
{
    [Route("api/Contact")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IDapper _dapper;
        public readonly DatabaseContext _dbContext;
        const string _spName = "SP_SET_CONTACT";

        public ContactController(DatabaseContext dbContext, IDapper dapper)
        {
            _dbContext = dbContext;
            _dapper = dapper;
        }

        [HttpPost("SaveUpdateData")]
        public async Task<IActionResult> SaveUpdateData(ContactDBModel _dbModel)
        {
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("QryOption", _dbModel.ContactID > 0 ? 2 : 1, DbType.Int32);
                dbparams.Add("ContactID", _dbModel.ContactID, DbType.Int32);
                dbparams.Add("FirstName", _dbModel.FirstName, DbType.String);
                dbparams.Add("Email", _dbModel.Email, DbType.String);
                dbparams.Add("MobileNumber", _dbModel.MobileNumber, DbType.String);
                dbparams.Add("Suburb", _dbModel.Suburb, DbType.String);
                dbparams.Add("City", _dbModel.City, DbType.String);
                dbparams.Add("Subject", _dbModel.Subject, DbType.String);
                dbparams.Add("Message", _dbModel.Message, DbType.String);

                await Task.FromResult(_dapper.Save<ContactDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetAllData")]
        public async Task<List<ContactDBModel>> GetAllData()
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 3, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<ContactDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
        }

        [HttpPost("GetSelectedData")]
        public async Task<List<ContactDBModel>> GetSelectedData(ContactDBModel _dbModel)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 4, DbType.Int32);
            dbparams.Add("ContactID", _dbModel.ContactID, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<ContactDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
        }

        [HttpPost("DeleteSelectedData")]
        public async Task<IActionResult> DeleteSelectedData(ContactDBModel _dbModel)
        {
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("QryOption", 5, DbType.Int32);
                dbparams.Add("ContactID", _dbModel.ContactID, DbType.Int32);

                await Task.FromResult(_dapper.Save<ContactDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost("SaveCommentDate")]
        public async Task<IActionResult> SaveCommentDate(ContactDBModel _dbModel)
        {
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("QryOption", 6, DbType.Int32);
                dbparams.Add("Name", _dbModel.Name, DbType.String);
                dbparams.Add("Message", _dbModel.Message, DbType.String);

                await Task.FromResult(_dapper.Save<ContactDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
