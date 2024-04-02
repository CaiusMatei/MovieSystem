using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSystemTMDB.Properties.Models
{
    public class Person
    {
        [Key]
        [Required]
        public int PersonId { get; set; }
        [StringLength(100)]
        public string? FirstName { get; set; }
        [EmailAddress]
        [StringLength(75)]
        public string? Email { get; set; }

        public List<PersonGenre>? PersonGenres { get; set; }
        public List<MovieRating>? Ratings { get; set; }
        public List<Movie>? Movies { get; set; }

    }
}

   
