using AutoMapper;
using WebAggregator.Boundary.Request;
using WebAggregator.Boundary.Response;
using WebAggregator.Domain;

namespace WebAggregator.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<WebPageCreateRequestModel, WebPageDomainModel>();
        CreateMap<WebPageUpdateRequestModel, WebPageDomainModel>();

        CreateMap<WebPageDomainModel, WebPageResponseModel>();
    }
}
