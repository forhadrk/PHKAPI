using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PHKAPI.Models;
using PMS.API.DBContext;
using System.Data;

namespace PHKAPI.Controllers
{
    [Route("api/ServiceCategoryPriceDetails")]
    [ApiController]
    public class ServiceCategoryPriceDetailsController : ControllerBase
    {
        private readonly IDapper _dapper;
        public readonly DatabaseContext _dbContext;
        const string _spName = "SP_SET_SERVICE_PRICE_DETAILS";
        public ServiceCategoryPriceDetailsController(DatabaseContext dbContext, IDapper dapper)
        {
            _dbContext = dbContext;
            _dapper = dapper;
        }

        [HttpPost("SaveUpdateData")]
        public async Task<IActionResult> SaveUpdateData(ServiceCategoryPriceDetailsDBModel _dbModel)
        {
            try
            {
                var dbparams = new DynamicParameters();

                dbparams.Add("QryOption", _dbModel.CategoryPriceDetailsID > 0 ? 2 : 1, DbType.Int32);
                dbparams.Add("CategoryPriceDetailsID", _dbModel.CategoryPriceDetailsID, DbType.Int32);
                dbparams.Add("CategoryPriceID", _dbModel.CategoryPriceID, DbType.Int32);
                dbparams.Add("Title", _dbModel.Title, DbType.String);

                await Task.FromResult(_dapper.Save<ServiceCategoryPriceDetailsDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetAllData")]
        public async Task<List<ServiceCategoryPriceDetailsDBModel>> GetAllData()
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 3, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<ServiceCategoryPriceDetailsDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
        }

        [HttpPost("GetSelectedData")]
        public async Task<List<ServiceCategoryPriceDetailsDBModel>> GetSelectedData(ServiceCategoryPriceDetailsDBModel _dbModel)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 4, DbType.Int32);
            dbparams.Add("CategoryPriceDetailsID", _dbModel.CategoryPriceDetailsID, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<ServiceCategoryPriceDetailsDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
        }

        [HttpPost("DeleteSelectedData")]
        public async Task<IActionResult> DeleteSelectedData(ServiceCategoryPriceDetailsDBModel _dbModel)
        {
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("QryOption", 5, DbType.Int32);
                dbparams.Add("CategoryPriceDetailsID", _dbModel.CategoryPriceDetailsID, DbType.Int32);

                await Task.FromResult(_dapper.Save<ServiceCategoryPriceDetailsDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("GetSelectedServicePrices")]
        public async Task<List<ServiceCategoryPriceDetailsDBModel>> GetSelectedServicePrices(ServiceCategoryPriceDetailsDBModel _dbModel)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 6, DbType.Int32);
            dbparams.Add("ServicesID", _dbModel.ServicesID, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<ServiceCategoryPriceDetailsDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
        }
    }
}
