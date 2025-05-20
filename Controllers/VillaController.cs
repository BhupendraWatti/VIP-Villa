using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Villa_Services.Data;
using Villa_Services.Models;
using Villa_Services.Models.Dto;
using Villa_Services.Repository.IRepository;

namespace Villa_Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly IVillaRepository _Villadb;
        private readonly IMapper _mapper;
        public VillaController(IVillaRepository Villadb, IMapper mapper)
        {
            _Villadb = Villadb;
            _mapper = mapper;
        }

        [HttpGet("{id:int}", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = await _Villadb.GetAsync(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<VillaDto>(villa));
        }

        [HttpGet(Name ="GetAllAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult< IEnumerable<VillaDto>>> GetAllVilla()
        {
            IEnumerable<Villa> villaList = await _Villadb.GetAllAsync();
            
            //var villa = await _Villadb.GetAllAsync(u => u.Id == id);
            //if (villaList == null)
            //{
            //    return NotFound();
            //}
            return Ok(_mapper.Map<IEnumerable<VillaDto>>(villaList));
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Villa>> CreateVilla([FromBody] VillaCreateDto createVilladto)
        {
            if(await _Villadb.GetAsync(u=>u.Name.ToLower() == createVilladto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError","Villa already exist");
                return BadRequest(ModelState);

            }
            if (createVilladto == null)
            {
                return BadRequest(createVilladto);
            }
            Villa model = _mapper.Map<Villa>(createVilladto);


            await _Villadb.CreateAsync(model);
            return CreatedAtRoute("GetVilla", new { id = model.Id }, model);
        }

        [HttpDelete("{id:int}", Name ="Deletevilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var villa = await _Villadb.GetAsync(u=> u.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            await _Villadb.DeleteAsync(villa);
            return NoContent();

        }

        [HttpPut("{id:int}", Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto villaUpdatedto)
        {
            if (villaUpdatedto == null || id != villaUpdatedto.Id)
            {
                return BadRequest();
            }
            Villa model = _mapper.Map<Villa>(villaUpdatedto);
            await _Villadb.UpdateAsync(model);
            await _Villadb.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}", Name ="UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            if(patchDto == null || id == 0)
            {
                return BadRequest();
            }
            var villa = await _Villadb.GetAsync(u=> u.Id == id, tracked:false);
            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);
            if(villa == null)
            {
                return BadRequest();
            }
            patchDto.ApplyTo(villaDto);
            Villa model = _mapper.Map<Villa>(villaDto);
            await _Villadb.UpdateAsync(villa);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}