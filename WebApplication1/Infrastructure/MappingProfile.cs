using AutoMapper;
using WebAggregator.Boundary.Request;
using WebAggregator.Boundary.Response;
using WebAggregator.Domain;

namespace WebAggregator.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ToDo: possibly need to delete.
        CreateMap<WebPageCreateRequestModel, WebPageDomainModel>();

        CreateMap<WebPageDomainModel, WebPageResponseModel>();
    }
}
