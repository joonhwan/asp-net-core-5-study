using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParameterBinding.Api.Models;

namespace ParameterBinding.Api.Repositories
{
    public class PetRepository : IPetRepository
    {
        private readonly ModelBindingContexts _contexts;

        public PetRepository(ModelBindingContexts contexts)
        {
            _contexts = contexts;
        }
        
        public async Task<PagedList<Pet>> GetAllAsync(PaginationFilter filter)
        {
            var itemsTotal = await _contexts.Pets.CountAsync();
            var pageTotal = Convert.ToInt32(Math.Ceiling((double) itemsTotal / (double) filter.PageSize));

            var items
                    = filter.PageNumber <= pageTotal
                        ? await _contexts
                            .Pets
                            .Skip(filter.Offset())
                            .Take(filter.PageSize)
                            .ToListAsync()
                        : new List<Pet>()
                ;

            var result = new PagedList<Pet>
            {
                Data = items,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                PageTotal = pageTotal,    
            };

            return result;
        }

        public async Task<Pet> GetAsync(int id)
        {
            return await _contexts.Pets.FirstOrDefaultAsync(pet => pet.Id == id);
        }
    }
}