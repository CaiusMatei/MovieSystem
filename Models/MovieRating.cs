using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MovieSystemTMDB.Properties.Models
{
    public class MovieRating
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Person")]
        public int PersonId { get; set; }
        public virtual Person? Person { get; set; }
        [ForeignKey("Movie")]
        public int MovieId { get; set; }
        public virtual Movie? Movie { get; set; }
        [Required]
        public int Rating { get; set; }

    }
}