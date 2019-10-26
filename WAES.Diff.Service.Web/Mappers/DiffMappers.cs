using AutoMapper;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Web.Models.Responses;

namespace WAES.Diff.Service.Web.Mappers
{
    public class DiffMappers : Profile
    {
        public DiffMappers()
        {
            CreateMap<DiffResponse, DiffResult>();
            CreateMap<Models.Responses.Diff, Domain.Entities.Diff>();
        }
    }
}
