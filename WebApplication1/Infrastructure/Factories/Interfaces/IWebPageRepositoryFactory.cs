using WebAggregator.Domain;
using WebAggregator.Repository.Entities;

namespace WebAggregator.Infrastructure.Factories.Interfaces;

public interface IWebPageRepositoryFactory
{
    public WebPageDomainModel ToDomain(WebPage webPage);
    public WebPage ToEntity(WebPageDomainModel domain);
}
