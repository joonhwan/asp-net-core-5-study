using System.Threading.Tasks;
using ParameterBinding.Api.Models;

namespace ParameterBinding.Api.Repositories
{
    public interface IPetRepository
    {
        Task<PagedList<Pet>> GetAllAsync(PaginationFilter filter);
        Task<Pet> GetAsync(int id);
    }
}