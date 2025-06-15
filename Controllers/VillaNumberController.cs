using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Villa_Services.Models;
using Villa_Services.Models.Dto;
using Villa_Services.Repository;
using Villa_Services.Repository.IRepository;

namespace Villa_Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberRepository _VillaN;
        private readonly IVillaRepository _Villa;
        private readonly IMapper _mapper;
        protected APIResponse _apiRes;
        public VillaNumberController(IVillaNumberRepository VillaN, IMapper mapper, IVillaRepository villa)
        {
            _VillaN = VillaN;
            _mapper = mapper;
            
            _Villa = villa;
        }
        [HttpGet("{id:int}", Name = "GetVillaNoAysnc")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNoAysnc(int id)
        {
            _apiRes = new APIResponse();
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                VillaNumber villaNo = await _VillaN.GetAsync(u => u.VillaNo == id, includeProperties:"Villa");
                if (villaNo == null)
                {
                    return NotFound();
                }
                _apiRes.Result = _mapper.Map<VillaNumberDto>(villaNo);
                _apiRes.StatusCode = HttpStatusCode.OK;
                _apiRes.IsSuccess = true;
                return Ok(_apiRes);
            }
            catch (Exception ex) {
                _apiRes.IsSuccess = false;
                _apiRes.ErrorMessage = new List<string>() { ex.Message };

            }
            return _apiRes;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllVilla()
        {
            _apiRes = new APIResponse();
            try
            {
                IEnumerable<VillaNumber> VillaNoList = await _VillaN.GetAllAsync(includeProperties: "Villa");
                _apiRes.Result = _mapper.Map<IEnumerable<VillaNumberDto>>(VillaNoList);
                _apiRes.StatusCode = HttpStatusCode.OK;
                _apiRes.IsSuccess = true;
                return Ok(_apiRes);

            }
            catch (Exception ex)
            {
                _apiRes.IsSuccess = false;
                _apiRes.ErrorMessage = new List<string>() { ex.Message };

            }
            return _apiRes;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<APIResponse>> CreateVillaNo([FromBody]VillaNumberCreate villaNo)
        {
            _apiRes = new APIResponse();
            try {
                if (await _VillaN.GetAsync(u => u.VillaNo == villaNo.VillaNo) != null)
                {
                    ModelState.AddModelError("CustomError", "Villa already exist");
                    return BadRequest(ModelState);
                }

                if(await _Villa.GetAsync(u=>u.Id == villaNo.VillaId) == null)
                {
                    ModelState.AddModelError("CustomError", "VillaId is Invalid");
                    return BadRequest(ModelState);
                }
                VillaNumber villa = _mapper.Map<VillaNumber>(villaNo);
                await _VillaN.CreateAsync(villa);
                await _VillaN.SaveAsync();
                _apiRes.Result = _mapper.Map<VillaNumber>(villa);
                _apiRes.StatusCode = HttpStatusCode.Created;
                _apiRes.IsSuccess = true;
                return CreatedAtRoute("GetVilla", new { id = villa.VillaNo }, _apiRes);

            }
            catch (Exception ex)
            {
                _apiRes.IsSuccess = false;
                _apiRes.ErrorMessage = new List<string>() { ex.Message };

            }
            return _apiRes;

        }

        [HttpPut("{id:int}", Name = "VillaNoUpdate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> VillaNoUpdate(int id, [FromBody] VillaNumberUpdate villaUp)
        {
            _apiRes = new APIResponse();
            try
            {

                if (villaUp == null || villaUp.VillaNo == 0)
                {
                    return BadRequest();
                }

                if (await _Villa.GetAsync(u => u.Id == villaUp.VillaId) == null)
                {
                    ModelState.AddModelError("CustomError", "VillaId is Invalid");
                    return BadRequest(ModelState);
                }

                VillaNumber villaNum = _mapper.Map<VillaNumber>(villaUp);
                await _VillaN.UpdateAsync(villaNum);
                _apiRes.StatusCode = HttpStatusCode.NoContent;
                _apiRes.IsSuccess = true;
                return Ok(_apiRes);
            }
            catch (Exception ex)
            {
                _apiRes.IsSuccess = false;
                _apiRes.ErrorMessage = new List<string>() { ex.Message };

            }
            return _apiRes;
        }

        [HttpDelete("{id:int}", Name ="DeleteVillaNo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Deletevillano(int id)
        {
            _apiRes = new APIResponse();
            try
            {
                if(id == 0)
                {
                    return BadRequest();
                }
                VillaNumber villa = await _VillaN.GetAsync(u => u.VillaNo == id);
                if(villa == null)
                {
                    return NotFound();
                }
                await _VillaN.DeleteAsync(villa);
                _apiRes.IsSuccess = true;
                _apiRes.StatusCode = HttpStatusCode.NoContent;
                _apiRes.IsSuccess = true;
                return Ok(_apiRes);
            }
            catch (Exception ex)
            {
                _apiRes.IsSuccess = false;
                _apiRes.ErrorMessage = new List<string>() { ex.Message };

            }
            return _apiRes;
        }


    }
}
