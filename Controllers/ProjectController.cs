using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.DTO;
using PersonalPortfolio.Models;
using PersonalPortfolio.Repository.IRepository;
using System.Linq.Expressions;
using System.Net;
using System.Text.Json;

namespace PersonalPortfolio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController: ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IPortFolioRepository _portfolioRepository;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public ProjectController(IProjectRepository projectRepository, IMapper mapper, ApiResponse response, IPortFolioRepository portfolioRepository)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
            _response = response;
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet("GetAllProjectsByPortfolio", Name = "GetAllProjectsByPortfolio")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(Duration = 60)] 
        public async Task<ActionResult<ApiResponse>> GetAllProjectsByPortfolio(string portfolioTitle)
        {
            try
            {
                IEnumerable<Project> projecttList;

                Expression<Func<Project, bool>> filter = p => p.PortfolioItem.Title == portfolioTitle;

                projecttList = await _projectRepository.GetAll(filter, includeProperties: "Category");



                _response.Result = _mapper.Map<List<ProjectDto>>(projecttList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpGet("GetProject {id:int}", Name = "GetProject")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ResponseCache(Duration = 60)]
        public async Task<ActionResult<ApiResponse>> GetProject(int id)
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            var project = await _projectRepository.Get(G => G.Id == id);

            if (project == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            _response.Result = _mapper.Map<ProjectDto>(project);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost("CreateProject", Name = "CreateProject")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]


        public async Task<ActionResult<ApiResponse>> CreateProject([FromBody] ProjectCreateDto createProject)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error");
                return BadRequest(_response);
            }
            if (createProject == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error No Project was given");
                return BadRequest(_response);
            }
            if (await _projectRepository.Get(u => u.Title.ToLower() == createProject.Title.ToLower()) != null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Project already exists");
                return BadRequest(_response);
            }

            var portfolio = await _portfolioRepository.Get(c => c.Title.ToLower() == createProject.PortfolioTitle.ToLower());
            if (portfolio == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("portfolio does not exists");
                return BadRequest(_response);
            }

            Project project = _mapper.Map<Project>(createProject);

            project.PortfolioItem = portfolio;
            project.PortfolioItemId = portfolio.Id;

            await _projectRepository.Create(project);
            _response.Result = _mapper.Map<ProjectCreateDto>(project);
            _response.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetProject", new { id = project.Id }, _response);
        }

        [HttpDelete("DeleteProject {id:int}", Name = "DeleteProject")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> DeleteProject(int id)
        {
            var project = await _projectRepository.Get(u => u.Id == id);
            if (project == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error this Product doesnt exists");
                return BadRequest(_response);
            }   
            await _projectRepository.Remove(project);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }


        [HttpPut("UpdateProject {id:int}", Name = "UpdateProject")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] ProjectUpdateDto projectDto)
        {
            var doesExist = await _projectRepository.DoesExist(V => V.Id == id);
            if (!doesExist)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error this Product doesnt exists");
                return BadRequest(_response);
            }



            if (projectDto == null || id != projectDto.Id)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error this Product doesnt exists");
                return BadRequest(_response);
            }

            var portfolio = await _portfolioRepository.Get(c => c.Title.ToLower() == projectDto.PortfolioTitle.ToLower());
            if (portfolio == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Category already exists");
                return BadRequest(_response);
            }

            Project project = _mapper.Map<Project>(projectDto);

            project.PortfolioItem = portfolio;
            project.PortfolioItemId = portfolio.Id;

            await _projectRepository.Update(project);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);

        }
    }
}
