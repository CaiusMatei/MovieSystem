using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSystemTMDB.Properties.Models
{
    public class Genre
    {
        [Key]
        [Required]
        public int GenreId { get; set; }
        [StringLength(100)]
        public string? Title { get; set; }
        [StringLength(200)]
        public string? Description { get; set; }

        public List<PersonGenre>? PersonGenres { get; set; }
        



    }

}

