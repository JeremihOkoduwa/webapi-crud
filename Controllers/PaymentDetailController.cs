using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentAPI.Models;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentDetailController : ControllerBase
    {
        private readonly PaymentDetailContext _context;

        public PaymentDetailController(PaymentDetailContext context)
        {
            _context = context;
        }

        // GET: api/PaymentDetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDetail>>> GetPaymentDetails()
        {
            return await _context.PaymentDetails.ToListAsync();
        }



        // GET: api/PaymentDetail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDetail>> GetPaymentDetail(int id)
        {
            var paymentDetail = await _context.PaymentDetails.FindAsync(id);

            if (paymentDetail == null)
            {
                return NotFound();
            }

            return paymentDetail;
        }

        // PUT: api/PaymentDetail/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentDetail(int id, PaymentDetail paymentDetail)
        {
            if (id != paymentDetail.PaymentDetailId)
            {
                return BadRequest();
            }

            _context.Entry(paymentDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PaymentDetail
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PaymentDetail>> PostPaymentDetail(PaymentDetail paymentDetail)
        {
            _context.PaymentDetails.Add(paymentDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaymentDetail", new { id = paymentDetail.PaymentDetailId }, paymentDetail);
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<PaymentDetail>> PostlistPaymentDetail(List<PaymentDetail> paymentDetail)
        {
            try
            {
                foreach (var item in paymentDetail)
                {
                    if (string.IsNullOrEmpty(item.CardOwnerName))
                    {
                        return BadRequest("Card owner name is null or empty");
                    }
                    if (PaymentDetailExist(item.CardOwnerName))
                    {
                        return BadRequest($"A model exist with this name - {item.CardOwnerName}");
                    }
                    await _context.PaymentDetails.AddAsync(item);
                    await _context.SaveChangesAsync();
                }
                return Ok("List succesfully inserted");
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An internal server error occured please contact system admin");
            }



        }
        static List<PaymentDetail> paymentDetail = new List<PaymentDetail>()
        {
            new PaymentDetail() {PaymentDetailId = 2, CardOwnerName = "Frank Road", CardNumber = "05963749604826859", ExpirationDate = "03/19", SecurityCode = "435"},
            new PaymentDetail() {PaymentDetailId = 3, CardOwnerName = "James Gum", CardNumber = "749609890097859", ExpirationDate = "03/20", SecurityCode = "989" },
            new PaymentDetail() {PaymentDetailId = 4, CardOwnerName  = "Frank Road", CardNumber = "960482685999987689", ExpirationDate = "03/22", SecurityCode = "678" }
        };

        [Route("detail/{id}")]
        [HttpGet]
        public IEnumerable<PaymentDetail> Get(int id)
        {
            yield return paymentDetail.FirstOrDefault(p => p.PaymentDetailId == id);
        }




        // DELETE: api/PaymentDetail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentDetail(int id)
        {
            var paymentDetail = await _context.PaymentDetails.FindAsync(id);
            if (paymentDetail == null)
            {
                return NotFound();
            }

            _context.PaymentDetails.Remove(paymentDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        private bool PaymentDetailExists(int id)
        {
            return _context.PaymentDetails.Any(e => e.PaymentDetailId == id);
        }

        private bool PaymentDetailExist(string ownername)
        {
            return _context.PaymentDetails.Any(e => e.CardOwnerName == ownername);
        }
    }
}
