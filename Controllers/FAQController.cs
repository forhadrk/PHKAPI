using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PHKAPI.Models;
using PMS.API.DBContext;
using System.Data;

namespace PHKAPI.Controllers
{
    [Route("api/faq")]
    [ApiController]
    public class FAQController : ControllerBase
    {
        private readonly IDapper _dapper;
        public readonly DatabaseContext _dbContext;
        const string _spName = "SP_SET_FAQ";

        public FAQController(DatabaseContext dbContext, IDapper dapper)
        {
            _dbContext = dbContext;
            _dapper = dapper;
        }

        [HttpPost("SaveUpdateData")]
        public async Task<IActionResult> SaveUpdateData(FAQDBModel _dbModel)
        {
            try
            {
                var dbparams = new DynamicParameters();

                dbparams.Add("QryOption", _dbModel.FAQDID > 0 ? 2 : 1, DbType.Int32);
                dbparams.Add("FAQDID", _dbModel.FAQDID, DbType.Int32);
                dbparams.Add("QuestionTitle", _dbModel.QuestionTitle, DbType.String);
                dbparams.Add("QuestionAnswer", _dbModel.QuestionAnswer, DbType.String);

                await Task.FromResult(_dapper.Save<FAQDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetAllData")]
        public async Task<List<FAQDBModel>> GetAllData()
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 3, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<FAQDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
        }

        [HttpPost("GetSelectedData")]
        public async Task<List<FAQDBModel>> GetSelectedData(FAQDBModel _dbModel)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 4, DbType.Int32);
            dbparams.Add("FAQDID", _dbModel.FAQDID, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<FAQDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
        }

        [HttpPost("DeleteSelectedData")]
        public async Task<IActionResult> DeleteSelectedData(FAQDBModel _dbModel)
        {
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("QryOption", 5, DbType.Int32);
                dbparams.Add("FAQDID", _dbModel.FAQDID, DbType.Int32);

                await Task.FromResult(_dapper.Save<FAQDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
