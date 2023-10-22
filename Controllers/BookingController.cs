using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PHKAPI.Models;
using PMS.API.DBContext;
using SimplifyCommerce.Payments;
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

                Random random = new Random();
                int randomNumber = random.Next(1000, 10000);

                var _BookingID = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "-" + randomNumber.ToString();

                dbparams.Add("QryOption", _dbModel.BookingMasterID > 0 ? 4 : 3, DbType.Int32);
                dbparams.Add("CategoryPriceID", _dbModel.CategoryPriceID, DbType.Int32);
                dbparams.Add("BookingID", _BookingID, DbType.String);
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

                var _status = "";
                if (_dbModel.IsBookingWithPayment == false)
                {
                    _status = CompletePayment(_dbModel);
                }
                else
                {
                    _status = "Booking Without Payment";
                }

                return Ok(new { BookingID = _BookingID, Status = _status });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private string CompletePayment(BookingDBModel _dbModel)
        {
            /*Testing Key*/
            //PaymentsApi.PublicApiKey = "sbpb_YTU5MDczMmEtMzk2Yy00MjU4LTkzM2QtZTJlNmYxNWJlN2My";
            //PaymentsApi.PrivateApiKey = "+Cn4XpaSAOd5Lr0TfggadhLCIrd3Rz4QnvcUJqlzTN55YFFQL0ODSXAOkNtXTToq";
            //card.Cvc = "123";
            //card.ExpMonth = 11;
            //card.ExpYear = 26;
            //card.Number = "5555555555554444";

            /*Live Key*/
            PaymentsApi.PublicApiKey = "lvpb_MmJkOWQ2ODUtODlhZC00ODIzLWJlNmYtMWRkNDc4ZmI5Nzhk";
            PaymentsApi.PrivateApiKey = "sUbl8YpZ0HR93AZAcni3Nl/aiiUfjH58qGWexmwj5f15YFFQL0ODSXAOkNtXTToq";

            PaymentsApi api = new PaymentsApi();
            Payment payment = new Payment();
            payment.Amount = 10;
            Card card = new Card();

            card.Cvc = _dbModel.CVCNo;
            card.ExpMonth = Convert.ToInt32(_dbModel.ExpiryMonth);
            card.ExpYear = Convert.ToInt32(_dbModel.ExpiryYear);
            card.Number = _dbModel.CardNumber.Replace(" ", "");
            payment.Card = card;
            payment.Currency = "AUD";
            payment.Description = "Payment Description";
            try
            {
                payment = (Payment)api.Create(payment);

                return payment.PaymentStatus;
            }
            catch (Exception e)
            {
                return "Payment Error";
            }

            //return "Success";
        }

        [HttpGet("GetAllBookingDetails")]
        public async Task<List<BookingDBModel>> GetAllBookingDetails()
        {
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("QryOption", 4, DbType.Int32);
                return await Task.FromResult(_dapper.GetAll<BookingDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
