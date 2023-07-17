using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PHKAPI.Models;
using PMS.API.DBContext;
using System.Data;

namespace PHKAPI.Controllers
{
    [Route("api/ServiceName")]
    [ApiController]
    public class ServiceNameController : ControllerBase
    {
        private readonly IDapper _dapper;
        public readonly DatabaseContext _dbContext;
        const string _spName = "SP_SET_SERVICES";

        public ServiceNameController(DatabaseContext dbContext, IDapper dapper)
        {
            _dbContext = dbContext;
            _dapper = dapper;
        }

        [HttpPost("SaveUpdateData")]
        public async Task<IActionResult> SaveUpdateData(ServiceNameDBModel _dbModel)
        {
            try
            {
                var dbparams = new DynamicParameters();
                
                dbparams.Add("QryOption", _dbModel.ServicesID > 0 ? 2 : 1, DbType.Int32);
                dbparams.Add("ServicesID", _dbModel.ServicesID, DbType.Int32);
                dbparams.Add("ServiceName", _dbModel.ServiceName, DbType.String);
                dbparams.Add("ServiceInfo", _dbModel.ServiceInfo, DbType.String);
                dbparams.Add("Active", _dbModel.Active, DbType.Boolean);

                await Task.FromResult(_dapper.Save<ServiceNameDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetAllData")]
        public async Task<List<ServiceNameDBModel>> GetAllData()
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 3, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<ServiceNameDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
        }

        [HttpPost("GetSelectedData")]
        public async Task<List<ServiceNameDBModel>> GetSelectedData(ServiceNameDBModel _dbModel)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 4, DbType.Int32);
            dbparams.Add("ServicesID", _dbModel.ServicesID, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<ServiceNameDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
        }

        [HttpPost("DeleteSelectedData")]
        public async Task<IActionResult> DeleteSelectedData(ServiceNameDBModel _dbModel)
        {
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("QryOption", 5, DbType.Int32);
                dbparams.Add("ServicesID", _dbModel.ServicesID, DbType.Int32);

                await Task.FromResult(_dapper.Save<ServiceNameDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
