using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;
using System.Collections.Generic;
using ProAgil.WebAPI.Dtos;
using System.IO;
using System.Net.Http.Headers;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository _repository;
        private readonly IMapper _mapper;


        public EventoController(IProAgilRepository repository, IMapper mapper)
        {
            _mapper= mapper;
            _repository = repository;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
              var eventos = await _repository.GetAllEventoAsync(true);
              var results = _mapper.Map<IEnumerable<EventoDto>>(eventos);            
              return Ok(results);
            }
            catch (System.Exception)
            {
               return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                var evento = await _repository.GetAllEventoAsyncById(eventoId, true);
                var results = _mapper.Map<EventoDto>(evento);            
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }
        }

        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
              try
            {
                var results = await _repository.GetAllEventoAsyncByTema(tema, true);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if(file.Length > 0){
                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSave, filename.Replace("\"", " ").Trim());
                    using(var stream = new FileStream(fullPath, FileMode.Create)){
                        file.CopyTo(stream);
                    }
                }                
                return Ok();
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }
            return BadRequest("Erro ao tentar realizar upload");
        }



        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
              try
            {
                var evento = _mapper.Map<Evento>(model);
               _repository.Add(evento);
               if(await _repository.SaveChangesAsync())
               {
                return Created($"/api/evento/{model.EventoId}", _mapper.Map<EventoDto>(evento));
               }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }

            return BadRequest();
        }

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> Put(int eventoId, EventoDto model)
        {
            try
            {

               var evento = await _repository.GetAllEventoAsyncById(eventoId, false);

               if(evento == null)
               {
                return NotFound();
               }
                _mapper.Map(model, evento);


               _repository.Update(evento);
               if(await _repository.SaveChangesAsync())
               {
                return Created($"/api/evento/{model.EventoId}", _mapper.Map<EventoDto>(evento));
               }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }

            return BadRequest();
        }

        [HttpDelete("{eventoId}")]
        public async Task<IActionResult> Delete(int eventoId)
        {
            try
            {

               var evento = await _repository.GetAllEventoAsyncById(eventoId, false);
               if(evento == null)
               {
                  return NotFound();
               }
               _repository.Delete(evento);
               if(await _repository.SaveChangesAsync())
               {
                  return Ok();
               }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }

            return BadRequest();
        }

    }
}