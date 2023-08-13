using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PHKAPI.Models;
using PMS.API.DBContext;
using System.Data;
using System.Net;
using System.Reflection;

namespace PHKAPI.Controllers
{
    [Route("api/Booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IDapper _dapper;
        public readonly DatabaseContext _dbContext;
        const string _spName = "SP_SET_BOOKING";
        public BookingController(DatabaseContext dbContext, IDapper dapper)
        {
            _dbContext = dbContext;
            _dapper = dapper;
        }

        [HttpGet("GetOtherServices")]
        public async Task<List<BookingDBModel>> GetOtherServices()
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 2, DbType.Int32);
            return await Task.FromResult(_dapper.GetAll<BookingDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
        }

        [HttpPost("SaveUpdateBooking")]
        public async Task<IActionResult> SaveUpdateBooking(BookingDBModel _dbModel)
        {
            try
            {
                var dbparams = new DynamicParameters();

                dbparams.Add("QryOption", _dbModel.BookingMasterID > 0 ? 4 : 3, DbType.Int32);
                dbparams.Add("CategoryPriceID", _dbModel.CategoryPriceID, DbType.Int32);
                dbparams.Add("ServicesID", _dbModel.ServicesID, DbType.Int32);
                dbparams.Add("ServiceDate", _dbModel.ServiceDate, DbType.String);
                dbparams.Add("BookingHour", _dbModel.BookingHour, DbType.String);
                dbparams.Add("City", _dbModel.City, DbType.String);
                dbparams.Add("BookingName", _dbModel.BookingName, DbType.String);
                dbparams.Add("Email", _dbModel.Email, DbType.String);
                dbparams.Add("Mobile", _dbModel.Mobile, DbType.String);
                dbparams.Add("Address", _dbModel.Address, DbType.String);
                dbparams.Add("Suburb", _dbModel.Suburb, DbType.String);
                dbparams.Add("PostCode", _dbModel.PostCode, DbType.String);
                dbparams.Add("SpecialNotes", _dbModel.SpecialNotes, DbType.String);
                dbparams.Add("OtherServicesList", _dbModel.OtherServicesList, DbType.String);
                dbparams.Add("Price", _dbModel.Price, DbType.Int32);

                await Task.FromResult(_dapper.Save<BookingDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
