using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VidlyVideoRental.Models;

namespace VidlyVideoRental.ViewModel
{
    public class MovieFormViewModel
    {
        public MovieFormViewModel(Movie movie)
        {
            Id = movie.Id;
            Name = movie.Name;
            ReleaseDate = movie.ReleaseDate;
            NumberInStock = movie.NumberInStock;
            GenreId = movie.GenreId;
        }

        public MovieFormViewModel()
        {
            Id = 0;
        }

        //public Movie Movie { get; set; }
        public IEnumerable<Genre> Genres { get; set; }

        public int? Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Display(Name = "Release Date")]
        [Required]
        public DateTime? ReleaseDate { get; set; }

        [Display(Name = "Number in Stock")]
        [Range(1, 20)]
        [Required]
        public int? NumberInStock { get; set; }

        //Removing since not being captured in form
        //[Display(Name = "Date Added")]
        //public DateTime DateAdded { get; set; }

        //Removing since not being captured in form
        //public Genre Genre { get; set; }

        [Display(Name = "Genre")]
        [Required]
        public byte? GenreId { get; set; }

        public string Title 
        { 
            get
            {
                if (Id != 0)
                    return "Edit Form";

                return "New Form";
            }
        }
    }
}