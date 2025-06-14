using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
        protected APIResponse _response;
        private readonly IVillaRepository _Villadb;
        private readonly IMapper _mapper;
        public VillaController(IVillaRepository Villadb, IMapper mapper)
        {
            _Villadb = Villadb;
            _mapper = mapper;
            _response = new APIResponse();
        }

        [HttpGet("{id:int}", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
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
                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.Message };
            }
            return _response;
        }

        [HttpGet(Name = "GetAllVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllVilla()
        {
            try
            {
            IEnumerable<Villa> villaList = await _Villadb.GetAllAsync();
            _response.Result = _mapper.Map<IEnumerable<VillaDto>>(villaList);
            _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.Message };
            }
            return Ok(_response);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDto createVilladto)
        {
            try
            {
                if (await _Villadb.GetAsync(u => u.Name.ToLower() == createVilladto.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("CustomError", "Villa already exist");
                    return BadRequest(ModelState);

                }
                if (createVilladto == null)
                {
                    return BadRequest(createVilladto);
                }
                Villa model = _mapper.Map<Villa>(createVilladto);


                await _Villadb.CreateAsync(model);

                _response.Result = _mapper.Map<VillaDto>(model);
                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                return CreatedAtRoute("GetVilla", new { id = model.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.Message };
            }
            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
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
                await _Villadb.DeleteAsync(villa);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.Message };
            }
            return _response;
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDto villaUpdatedto)
        {
            try
            {
                if (villaUpdatedto == null || id != villaUpdatedto.Id)
                {
                    return BadRequest();
                }
                Villa model = _mapper.Map<Villa>(villaUpdatedto);
                await _Villadb.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.Message };
            }
            return _response;
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
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