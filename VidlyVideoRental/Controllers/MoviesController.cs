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
    public class MoviesController : Controller
    {
        ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Movies/Random
        public ActionResult Random()        //Q:ActionResult is return type even though View method return type is ViewResult?
        {                                   //A:Application may have different execution path at runtime which might return difference result types
                                            //  having ActionResult as return type enables us to return any of its sub types as it is the base class     
            var movie = new Movie() { Name = "Shrek!"};

            var customers = new List<Customer>()
                                {
                                    new Customer(){Name = "Customer 1"},
                                    new Customer(){Name = "Customer 2"}
                                };

            var viewModel = new RandomMovieViewModel()
                                {
                                    Movie = movie,
                                    Customers = customers
                                };

            return View(viewModel);  
            //View Data
            //ViewData["Movie"] = movie;
            //View Bag
            //ViewBag.Movie = movie;

            //return View();

            //var viewResult = new ViewResult();
            //viewResult.ViewData.Model = movie; //Inside view method of controller, passed in object model is mapped to ViewData.Model property--> This is a good concept of how implementation is abstracted in framework

            //return View(movie);             //View is Helper method in base Controller class which returns ViewResult

            //return Content("Hello World!");
            //return HttpNotFound("Nothing was found with this!!");
            //return new EmptyResult();       //No Helper method for returning empty result 
            //return RedirectToAction("Index", "Home", new { page = 1, sortBy = "name" });

        }

        //public ActionResult Edit(int id)
        //{
        //    return Content("id=" + id);
        //}

        [Route("Movies/Edit/{Id}")]
        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.Single(c => c.Id == id);

            var viewModel = new MovieFormViewModel(movie)
                            {                                
                                Genres = _context.Genres
                            };

            return View("MovieForm", viewModel);
        }

        public ActionResult Index(int? pageIndex, string sortBy)
        {
            if (!pageIndex.HasValue)
                pageIndex = 1;

            if (String.IsNullOrWhiteSpace(sortBy))
                sortBy = "Name";

            return Content(String.Format("pageIndex={0}&sortBy={1}",pageIndex,sortBy));
        }

        [Route("movies/released/{year}/{month:regex(\\d{2}):range(1,12)}")]
        public ActionResult ByReleaseDate(int year, byte month)
        {
            return Content(year + "/" + month);
        }

        [Route("Movies")]
        public ActionResult Index()
        {
            var movies = _context.Movies.Include(m => m.Genre).ToList();
            return View(movies);
        }

        [Route("Movies/Details/{Id}")]
        public ActionResult Details(int Id)
        {
            var movie = _context.Movies.Include(m => m.Genre).Single(m => m.Id == Id);
            return View(movie);
        }

        public ActionResult MovieForm()
        {
            var viewModel = new MovieFormViewModel()
                                {
                                    //Movie = new Movie(),
                                    Genres = _context.Genres.ToList()
                                };


            return View(viewModel);
        }

        [HttpPost]                      //This is very crutial. Otherwise model will not be binded.
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                var movieFormViewModel = new MovieFormViewModel(movie)
                                        {   
                                            Genres = _context.Genres.ToList()
                                        };

                return View("MovieForm", movieFormViewModel);
            }


            if (movie.Id == 0)
                _context.Movies.Add(movie);
            else
            {
                //Option 1 to update model in DB
                //TryUpdateModel(viewModel.Movie);

                //Option 2 to update model in DB
                var movieInDB = _context.Movies.Single(m=>m.Id == movie.Id);
                movieInDB.Name = movie.Name;
                movieInDB.ReleaseDate = movie.ReleaseDate;
                movieInDB.GenreId = movie.GenreId;
                movieInDB.NumberInStock = movie.NumberInStock;
            }

            _context.SaveChanges();

            //try
            //{
            //    _context.SaveChanges();
            //}
            //catch (System.Data.Entity.Validation.DbEntityValidationException dbEntityValEx)
            //{
                
            //    throw;
            //}
            return RedirectToAction("Index");
        }
    }
}