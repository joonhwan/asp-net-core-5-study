using System;
using Bogus;
using Microsoft.EntityFrameworkCore;
using ParameterBinding.Api.Models;

namespace ParameterBinding.Api.Repositories
{
    public class ModelBindingContexts : DbContext
    {
        public ModelBindingContexts(DbContextOptions<ModelBindingContexts> options)
            : base(options)
        {

        }
        
        public DbSet<Pet> Pets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Randomizer.Seed = new Random(711211);
            var owners = new[] {"이준환", "이혜진", "임기훈", "이용규", "이종규"};
            var types = new[] {"Dog", "Cat", "Rabbit"};

            var petFaker = new Faker<Pet>()
                    .RuleFor(pet => pet.Id, f => f.IndexFaker + 1)
                    .RuleFor(pet => pet.Age, f => f.Random.Number(1, 10))
                    .RuleFor(pet => pet.Gender, f => f.PickRandom<Gender>())
                    .RuleFor(pet => pet.Owner, f => f.PickRandom(owners))
                    .RuleFor(pet => pet.Type, f => f.PickRandom(types))
                    .RuleFor(pet => pet.Name, f => f.Name.LastName())
                ;

            var pets = petFaker.Generate(100);

            modelBuilder.Entity<Pet>().HasData(pets);
        }
    }
}