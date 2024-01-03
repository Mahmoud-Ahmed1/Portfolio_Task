using AutoMapper;
using PersonalPortfolio.DTO;
using PersonalPortfolio.Models;

namespace PersonalPortfolio.Helper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ProjectCreateDto, Project>().ReverseMap();
            CreateMap<ProjectUpdateDto, Project>().ReverseMap();
            CreateMap<Project, ProjectDto>().ReverseMap();

            CreateMap<PortFolioCreateDto, PortfolioItem>().ReverseMap();
            CreateMap<PortFolioUpdateDto, PortfolioItem>().ReverseMap();
            CreateMap<PortfolioItem, PortFolioDto>().ReverseMap();

            CreateMap<SkillCreateDto, Skill>().ReverseMap();
            CreateMap<SkillUpdateDto, Skill>().ReverseMap();
            CreateMap<Skill, SkillDto>().ReverseMap();

        }
    }
}
