using CRM.Contexts;
using CRM.DTOs.Customer;
using CRM.DTOs.User;
using CRM.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace CRM.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("getAllCustomers")]
        public IActionResult GetCustomers()
        {
            var customers = _context.Customers.ToList();

            var list = new List<GetAllCustomersDTO>();

            foreach (var customer in customers)
            {
                var dto = new GetAllCustomersDTO()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    CompanyName = customer.CompanyName,
                    PhoneNumber = customer.PhoneNumber,
                };
                list.Add(dto);
            }
            return Ok(list);
        }

        [HttpPost]
        [Route("AddCustomer")]
        public IActionResult AddCustomer([FromForm] AddCustomerDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }

            var customer = new Customer()
            {
                Name= dto.Name,
                CompanyName= dto.CompanyName,
                PhoneNumber = dto.PhoneNumber
            };
            _context.Customers.Add(customer);
            _context.SaveChanges();

            return Ok("User created successfully!");
        }

        /*public string GetJwtToken()
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.ReadJwtToken(accessToken);

            return token.ToString(); 
        }*/
    }
}
