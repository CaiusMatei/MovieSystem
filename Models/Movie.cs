using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieSystemTMDB.Properties.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        [Required]
        [StringLength(50)]
        public string? MovieName { get; set; }
        [Required]
        [StringLength(100)]
        public string? MovieLink { get; set; }
        [ForeignKey("Person")]
        public int PersonId { get; set; }
        public virtual Person? Person { get; set; }
        [ForeignKey("Genre")]
        public int GenreId { get; set; }
        public virtual Genre? Genre { get; set; }

        public List<MovieRating>? Ratings { get; set; }
        
    }
}
