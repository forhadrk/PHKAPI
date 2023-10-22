using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PHKAPI.Models;
using PMS.API.DBContext;
using System.Data;

namespace PHKAPI.Controllers
{
    [Route("api/ServiceCategoryPrice")]
    [ApiController]
    public class ServiceCategoryPriceController : ControllerBase
    {
        private readonly IDapper _dapper;
        public readonly DatabaseContext _dbContext;
        const string _spName = "SP_SET_SERVICE_CATEGORY_WISE_PRICE";
        public ServiceCategoryPriceController(DatabaseContext dbContext, IDapper dapper)
        {
            _dbContext = dbContext;
            _dapper = dapper;
        }

        [HttpPost("SaveUpdateData")]
        public async Task<IActionResult> SaveUpdateData(ServiceCategoryPriceDBModel _dbModel)
        {
            try
            {
                var dbparams = new DynamicParameters();

                dbparams.Add("QryOption", _dbModel.CategoryPriceID > 0 ? 2 : 1, DbType.Int32);
                dbparams.Add("ServicesID", _dbModel.ServicesID, DbType.Int32);
                dbparams.Add("Price", _dbModel.Price, DbType.Int32);
                dbparams.Add("Title", _dbModel.Title, DbType.String);
                dbparams.Add("PriceInfo", _dbModel.PriceInfo, DbType.String);

                await Task.FromResult(_dapper.Save<ServiceCategoryPriceDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }        
        [HttpGet("GetAllData")]
        public async Task<List<ServiceCategoryPriceDBModel>> GetAllData()
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 3, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<ServiceCategoryPriceDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
        }

        [HttpPost("GetSelectedData")]
        public async Task<List<ServiceCategoryPriceDBModel>> GetSelectedData(ServiceCategoryPriceDBModel _dbModel)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 4, DbType.Int32);
            dbparams.Add("CategoryPriceID", _dbModel.CategoryPriceID, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<ServiceCategoryPriceDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
        }

        [HttpPost("DeleteSelectedData")]
        public async Task<IActionResult> DeleteSelectedData(ServiceCategoryPriceDBModel _dbModel)
        {
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("QryOption", 5, DbType.Int32);
                dbparams.Add("CategoryPriceID", _dbModel.CategoryPriceID, DbType.Int32);

                await Task.FromResult(_dapper.Save<ServiceCategoryPriceDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
