using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParameterBinding.Api.Models;
using ParameterBinding.Api.Repositories;

namespace ParameterBinding.Api.Controllers
{
    [ApiController]
    [Route("api/pets")]
    public class PetsController : ControllerBase
    {
        private readonly IPetRepository _petRepository;
        private readonly ILogger<PetsController> _logger;

        public PetsController(IPetRepository petRepository, ILogger<PetsController> logger)
        {
            _petRepository = petRepository;
            _logger = logger;
        }


        // HttpGet() 어트리뷰트에  "{id}" 라는 이름을 명명. --> [FromRoute] 는 생략가능.
        [HttpGet("{id}")]
        public async Task< ActionResult<Pet> > Get([FromRoute] int id)
        {
            var result = await _petRepository.GetAsync(id);
            return Ok(result);
        }

        // [HttpGet]
        // public async Task<ActionResult<PagedList<Pet>>> Get([FromQuery] PaginationFilter filter)
        // {
        //     var result = await _petRepository.GetAllAsync(filter);
        //     return Ok(result);
        // }

        // api/pets?pageNumber=숫자&pageSize=숫자
        // HttpGet() 어트리뷰트에 그 어떤 "{변수}" 을 넣지 않은 경우,  [FromQuery]는 생략가능.
        [HttpGet]
        public async Task<ActionResult<PagedList<Pet>>> Get([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var result = await _petRepository.GetAllAsync(new PaginationFilter(pageNumber, pageSize));
            return Ok(result);
        }

        [HttpPost]
        // POST 시 복합 타입(="Pet 클래스")이면서 유일한 인자인 경우 [FromBody] 는 생략가능
        public ActionResult<Pet> Create([FromBody] Pet pet)  
        {
            _logger.LogInformation("Pet을 생성했습니다 : pet={pet}", pet);
            return Ok(pet);
        }

        [HttpPost("picture/{id}")]
        public ActionResult UploadImages([FromForm] IFormFileCollection formFiles)
        {
            foreach (var file in formFiles)
            {
                using var stream = file.OpenReadStream();
                var fullPath = Path.GetFullPath(file.FileName);
                
                _logger.LogInformation("파일 업로드 : {fullPath} 에 IFormFile 객체({file}) 를 저장합니다", fullPath, file.ToString());
                using var writer = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.Read);
                stream.CopyTo(writer);
            }
            return Ok();
        }
    }
}