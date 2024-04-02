using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MovieSystemTMDB.Properties.Models;

namespace MovieSystemTMDB.Properties.Data
{
    public class MovieTmdbDbContext : DbContext
    {
        

        public MovieTmdbDbContext(DbContextOptions<MovieTmdbDbContext> options) : base(options)
        {

        }

        
        public DbSet<Person> Persons { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<PersonGenre> PersonGenres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieRating> MovieRatings { get; set; }

        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonGenre>()
                .HasKey(pg => pg.Id);
            modelBuilder.Entity<PersonGenre>()
                .HasOne(pg => pg.Person)
                .WithMany(p => p.PersonGenres)
                .HasForeignKey(pg => pg.PersonId);
            modelBuilder.Entity<PersonGenre>()
                .HasOne(pg => pg.Genre)
                .WithMany()
                .HasForeignKey(pg => pg.GenreId);

            modelBuilder.Entity<MovieRating>()
                .HasKey(r => r.Id);
            modelBuilder.Entity<MovieRating>()
                .HasOne(r => r.Person)
                .WithMany(p => p.MovieRatings)
                .HasForeignKey(r => r.PersonId);
            modelBuilder.Entity<MovieRating>()
                .HasOne(r => r.Movie)
                .WithMany()
                .HasForeignKey(r => r.MovieId);

            modelBuilder.Entity<Movie>()
                .HasKey(m => m.MovieId);
            modelBuilder.Entity<Movie>()
                .Property(m => m.MovieName)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Movie>()
                .Property(m => m.MovieLink)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Person)
                .WithMany(p => p.Movies)
                .HasForeignKey(m => m.PersonId);
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Genre)
                .WithMany()
                .HasForeignKey(m => m.GenreId);
        }
        */





    }







}   




