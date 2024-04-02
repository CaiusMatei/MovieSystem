using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieSystemTMDB.Properties.Models
{
	public class PersonGenre
	{

        [Key]
        public int Id { get; set; }
        [ForeignKey("Person")]
        public int PersonId { get; set; }
        public virtual Person? Persons { get; set; }
        [ForeignKey("Genre")]
        public int GenreId { get; set; }
        public virtual Genre? Genres { get; set; }

    }
}

