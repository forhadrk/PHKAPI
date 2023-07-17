using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PHKAPI.Models;
using PMS.API.DBContext;
using System.Data;

namespace PHKAPI.Controllers
{
    [Route("api/ServicePrice")]
    [ApiController]
    public class ServicePriceController : ControllerBase
    {
        private readonly IDapper _dapper;
        public readonly DatabaseContext _dbContext;
        const string _spName = "SP_SET_SERVICE_LIST";

        public ServicePriceController(DatabaseContext dbContext, IDapper dapper)
        {
            _dbContext = dbContext;
            _dapper = dapper;
        }

        [HttpPost("SaveUpdateData")]
        public async Task<IActionResult> SaveUpdateData(ServicePriceDBModel _dbModel)
        {
            try
            {
                var dbparams = new DynamicParameters();

                dbparams.Add("QryOption", _dbModel.ServiceListID > 0 ? 2 : 1, DbType.Int32);
                dbparams.Add("ServiceListID", _dbModel.ServiceListID, DbType.Int32);
                dbparams.Add("ServiceTitle", _dbModel.ServiceTitle, DbType.String);
                dbparams.Add("PriceFor", _dbModel.PriceFor, DbType.String);
                dbparams.Add("Price", _dbModel.Price, DbType.Int32);
                dbparams.Add("Active", _dbModel.Active, DbType.Boolean);

                await Task.FromResult(_dapper.Save<ServicePriceDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetAllData")]
        public async Task<List<ServicePriceDBModel>> GetAllData()
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 3, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<ServicePriceDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
        }

        [HttpPost("GetSelectedData")]
        public async Task<List<ServicePriceDBModel>> GetSelectedData(ServicePriceDBModel _dbModel)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 4, DbType.Int32);
            dbparams.Add("ServiceListID", _dbModel.ServiceListID, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<ServicePriceDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
        }

        [HttpPost("DeleteSelectedData")]
        public async Task<IActionResult> DeleteSelectedData(ServicePriceDBModel _dbModel)
        {
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("QryOption", 5, DbType.Int32);
                dbparams.Add("ServiceListID", _dbModel.ServiceListID, DbType.Int32);

                await Task.FromResult(_dapper.Save<ServicePriceDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
