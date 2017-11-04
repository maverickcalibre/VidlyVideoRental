using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VidlyVideoRental.Models;

namespace VidlyVideoRental.Controllers.Api
{
    public class MoviesController : ApiController
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        //GET /api/movies
        [HttpGet]
        public IEnumerable<Movie> GetMovies()
        {
            var movies = _context.Movies.ToList();
            if (movies == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return movies;            
        }

        //GET /api/movies/1
        [HttpGet]
        public Movie GetMovie(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);

            if (movie == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return movie;
        }

        //POST /api/movies
        [HttpPost]
        public Movie CreateMovie(Movie movie)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
                
            _context.Movies.Add(movie);
            _context.SaveChanges();

            return movie;
        }

        //PUT /api/movies/2
        [HttpPut]
        public Movie UpdateMovie(Movie movie)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var movieInDB = _context.Movies.Single(m => m.Id == movie.Id);

            if (movieInDB == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            movieInDB.Name = movie.Name;
            movieInDB.NumberInStock = movie.NumberInStock;
            movieInDB.ReleaseDate = movie.ReleaseDate;
            movieInDB.GenreId = movie.GenreId;

            try
            {
                _context.SaveChanges();
            }
            catch(System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var error in ex.EntityValidationErrors)
                    error.ToString();
            }
            return movie;
        }

        //DELETE /api/movies/1
        [HttpDelete]
        public void DeleteMovie(int id)
        {
            var movie = _context.Movies.Single(m => m.Id == id);

            if (movie == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            _context.Movies.Remove(movie);
            _context.SaveChanges();
        }
    }
}
