using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.DTO;
using PersonalPortfolio.Models;
using PersonalPortfolio.Repository;
using PersonalPortfolio.Repository.IRepository;
using System.Net;
using Microsoft.AspNetCore.Http;



namespace PersonalPortfolio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortFolioRepository _portfolioRepository;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public PortfolioController(IPortFolioRepository portfolioRepository, IMapper mapper, ApiResponse response)
        {
            _portfolioRepository = portfolioRepository;
            _mapper = mapper;
            _response = response;
        }

        [HttpGet("GetAllPortfolios", Name = "GetAllPortfolios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetPortfolios()
        {
            try
            {
                IEnumerable<PortfolioItem> PortfolioList = await _portfolioRepository.GetAll();
                _response.Result = _mapper.Map<List<PortFolioDto>>(PortfolioList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("{id:int}", Name = "GetPortfolio")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetPortfolio(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var portfolio = await _portfolioRepository.Get(u => u.Id == id);
                if (portfolio == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<PortFolioDto>(portfolio);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;

        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreatePortfolio([FromBody] PortFolioCreateDto portfolioDto)
        {
            try
            {
                if (await _portfolioRepository.Get(u => u.Title.ToLower() == portfolioDto.Title.ToLower()) != null)
                {
                    ModelState.AddModelError("CustomError", "Portfolio already Exists!");
                    return BadRequest(ModelState);
                }

                if (portfolioDto == null)
                {
                    return BadRequest(portfolioDto);
                }
                //if (portfolioDto.Id > 0)
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError);
                //}
                PortfolioItem portfolio = _mapper.Map<PortfolioItem>(portfolioDto);


                await _portfolioRepository.Create(portfolio);
                _response.Result = _mapper.Map<PortFolioCreateDto>(portfolio);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetPortfolio", new { id = portfolio.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}", Name = "DeletePortfolio")]
        public async Task<ActionResult<ApiResponse>> DeletePortfolio(int id)
        {
            try
            {
                if (id == 0)
                    return BadRequest(ModelState);

                var portfolio = await _portfolioRepository.Get(v => v.Id == id);
                if (portfolio == null)
                    return NotFound();
                await _portfolioRepository.Remove(portfolio);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;

        }


        [HttpPut("UpdatePortfolio {id:int}", Name = "UpdatePortfolio")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePortfolio(int id, [FromBody] PortFolioUpdateDto portfolioDto)
        {
            var doesExist = await _portfolioRepository.DoesExist(V => V.Id == id);
            if (!doesExist)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error this Portfolio doesnt exists");
                return BadRequest(_response);
            }



            if (portfolioDto == null || id != portfolioDto.Id)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error this Portfolio doesnt exists");
                return BadRequest(_response);
            }

            PortfolioItem product = _mapper.Map<PortfolioItem>(portfolioDto);

            await _portfolioRepository.Update(product);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);

        }

    }
}
