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
        const string _spSCName = "SP_SET_CATEGORY_WISE_SERVICE";
        const string _spHOME = "SP_GET_HOME_DETAILS";
        const string _spBooking = "SP_SET_BOOKING";

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

        [HttpGet("GetServiceNames")]
        public async Task<List<ServicePriceDBModel>> GetServiceNames()
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 1, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<ServicePriceDBModel>(_spSCName, dbparams, commandType: CommandType.StoredProcedure));
        }

        [HttpPost("SaveCategoryWiseService")]
        public async Task<IActionResult> SaveCategoryWiseService(ServicePriceDBModel _dbModel)
        {
            try
            {
                var dbparams = new DynamicParameters();

                dbparams.Add("QryOption", 2, DbType.Int32);
                dbparams.Add("ServicesID", _dbModel.ServicesID, DbType.Int32);
                dbparams.Add("ServicesListID", _dbModel.ServicesListID, DbType.String);

                await Task.FromResult(_dapper.Save<ServicePriceDBModel>(_spSCName, dbparams, commandType: CommandType.StoredProcedure));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost("GetSelectedServiceNames")]
        public async Task<List<ServicePriceDBModel>> GetSelectedServiceNames(ServicePriceDBModel _dbModel)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 3, DbType.Int32);
            dbparams.Add("ServicesID", _dbModel.ServicesID, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<ServicePriceDBModel>(_spSCName, dbparams, commandType: CommandType.StoredProcedure));
        }

        [HttpPost("GetSelectedServicePrice")]
        public async Task<List<ServicePriceDBModel>> GetSelectedServicePrice(ServicePriceDBModel _dbModel)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 4, DbType.Int32);
            dbparams.Add("ServicesID", _dbModel.ServicesID, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<ServicePriceDBModel>(_spHOME, dbparams, commandType: CommandType.StoredProcedure));
        }

        [HttpPost("GetSelectedServicePriceDetails")]
        public async Task<List<ServicePriceDBModel>> GetSelectedServicePriceDetails(ServicePriceDBModel _dbModel)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 5, DbType.Int32);
            dbparams.Add("CategoryPriceID", _dbModel.CategoryPriceID, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<ServicePriceDBModel>(_spHOME, dbparams, commandType: CommandType.StoredProcedure));
        }

        [HttpPost("GetServiceWisePrice")]
        public async Task<List<ServicePriceDBModel>> GetServiceWisePrice(ServicePriceDBModel _dbModel)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 1, DbType.Int32);
            dbparams.Add("ServicesID", _dbModel.ServicesID, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<ServicePriceDBModel>(_spBooking, dbparams, commandType: CommandType.StoredProcedure));
        }
    }
}
