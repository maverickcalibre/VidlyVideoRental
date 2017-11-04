using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VidlyVideoRental.Models;
using VidlyVideoRental.ViewModel;

namespace VidlyVideoRental.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [Route("Customers/New")]
        public ActionResult New()
        {
            var viewModel = new CustomerFormViewModel() 
            {
                Customer = new Customer(),
                MembershipTypes = _context.MembershipTypes.ToList()
            };
            return View("CustomerForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CustomerFormViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {

                var customerFormViewModel = new CustomerFormViewModel()
                                    {
                                        Customer = viewModel.Customer,
                                        MembershipTypes = _context.MembershipTypes
                                    };
                return View("CustomerForm", customerFormViewModel);
            }

            if(viewModel.Customer.Id == 0)
                _context.Customers.Add(viewModel.Customer);
            else
            {
                var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == viewModel.Customer.Id);

                //Option 1 to update properties
                //TryUpdateModel(customerInDb, new string[] { "Name", "IsSubscribedToNewsletter" }); // 2 problems a) Security hole b) Magic string
                
                //Option 2 to update properties
                customerInDb.IsSubscribedToNewsletter = viewModel.Customer.IsSubscribedToNewsletter;
                customerInDb.MembershipTypeId = viewModel.Customer.MembershipTypeId;
                customerInDb.Name = viewModel.Customer.Name;
                customerInDb.Birthdate = viewModel.Customer.Birthdate;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Customers");
        }

        // GET: Customers
        [Route("Customers")]
        public ActionResult Index()
        {
            //var customers = GetCustomers();
            var customers = _context.Customers.Include(c => c.MembershipType).ToList();

            return View(customers);
        }

        [Route("Customers/Details/{Id}")]
        public ActionResult Details(int Id)
        {
            //var customers = GetCustomers();            

            var customer = _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == Id);

            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }

        [Route("Customers/Edit/{Id}")]
        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customer == null)
                return HttpNotFound();

            var viewModel = new CustomerFormViewModel()
                                {
                                    Customer = customer,
                                    MembershipTypes = _context.MembershipTypes.ToList()

                                };
            return View("CustomerForm", viewModel);
        }

        public IEnumerable<Customer> GetCustomers()
        {
            var customers = new List<Customer>()
                            {
                                //new Customer(){Name="John Smith",Id = 1},
                                //new Customer(){Name="Mary Williams", Id = 2}
                            };

            return customers;
        }
        
    }
}