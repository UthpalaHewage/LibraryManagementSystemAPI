using AutoMapper;
using LibrarySystem.Dtos;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LibrarySystem.Controllers
{
    public class CustomersController : ApiController
    {
        LibraryContext _context;
        public CustomersController()
        {
            _context = new LibraryContext();
        }

        //GET /api/Customers
        [HttpGet]
        //public IEnumerable<CustomerDto> GetCustomers()
        public IHttpActionResult GetCustomers()
        {
            return Ok(_context.Customers.ToList().Select(Mapper.Map<Customer, CustomerDto>));
        }


        //GET /api/Customers/1
        [HttpGet]
        public IHttpActionResult GetCustomer(int id)
        {
            Customer customer = _context.Customers.SingleOrDefault(x => x.Id == id);

            if (customer == null)
            {
                return NotFound();
                //throw new HttpResponseException(HttpStatusCode.NotFound);

            }

            return Ok(Mapper.Map<Customer, CustomerDto>(customer));
        }


        //POST /api/Customers
        [HttpPost]
        public IHttpActionResult CreateCustomer(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();   

            /*{
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }*/

            var customer = Mapper.Map<CustomerDto, Customer>(customerDto);
            _context.Customers.Add(customer);
            _context.SaveChanges();


            //return Ok();

            customerDto.Id = customer.Id;


            return Created(new Uri(Request.RequestUri + "/" + customer.Id), customerDto);
            
        }
        
        //PUT /api/Customers/1
        [HttpPut]
        public void UpdateCustomer(int id, CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customerInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Mapper.Map(customerDto, customerInDb);

            //Mapper.Map<CustomerDto, Customer>(customerDto, customerInDb);
            /*
            customerInDb.CustomerRegistartionNumber = customerDto.CustomerRegistartionNumber;
            customerInDb.Name = customerDto.Name;
            customerInDb.Email = customerDto.Email;
            customerInDb.TelephoneNumber = customerDto.TelephoneNumber;
            customerInDb.DateOfBirth = customerDto.DateOfBirth;
            customerInDb.RegisteredDate = customerDto.RegisteredDate;
            customerInDb.Address = customerDto.Address;
            */
            _context.SaveChanges();
        }

        //DELETE /api/Customers/1
        [HttpDelete]
        public void DeleteCustomer(int id)
        {
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customerInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Customers.Remove(customerInDb);
            _context.SaveChanges();
        }
    }
}
