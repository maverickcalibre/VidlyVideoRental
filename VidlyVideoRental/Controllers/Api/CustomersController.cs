using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using VidlyVideoRental.Dtos;
using VidlyVideoRental.Models;

namespace VidlyVideoRental.Controllers.Api
{
    public class CustomersController : ApiController
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        //Get /api/customers
        //public IEnumerable<Customer> GetCustomers()           //Before implementing Data transfer Objects
        public IEnumerable<CustomerDto> GetCustomers()
        {
            return _context.Customers.ToList().Select(Mapper.Map<Customer,CustomerDto>);
        }

        //GET /api/customers/1
        //public Customer GetCustomer(int id)                   //Before implementing Data transfer Objects
        public CustomerDto GetCustomer(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return Mapper.Map<Customer,CustomerDto>(customer);
        }

        //POST  /api/customers
        [HttpPost]
        public CustomerDto CreateCustomer(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var customer = Mapper.Map<CustomerDto, Customer>(customerDto);
            _context.Customers.Add(customer);
            _context.SaveChanges();

            customerDto.Id = customer.Id;

            return customerDto;
        }

        //PUT   /api/customers/1
        [HttpPut]
        //public Customer UpdateCustomer(int id, Customer customer)
        public CustomerDto UpdateCustomer(int id, CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var customerInDB = _context.Customers.Single(c => c.Id == id);

            if (customerInDB == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            customerInDB.Name = customerDto.Name;
            customerInDB.IsSubscribedToNewsletter = customerDto.IsSubscribedToNewsletter;
            customerInDB.MembershipTypeId = customerDto.MembershipTypeId;
            customerInDB.Birthdate = customerDto.Birthdate;

            _context.SaveChanges();

            return customerDto;         
        }

        //Delete    /api/customers/1
        [HttpDelete]
        public void Delete(int id)
        {
            var customerInDB = _context.Customers.Single(c=>c.Id == id);

            if (customerInDB == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Customers.Remove(customerInDB);
            _context.SaveChanges();
        }

    }
}
