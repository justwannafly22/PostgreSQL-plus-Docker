using WebAggregator.Domain;
using WebAggregator.Infrastructure.Factories.Interfaces;
using WebAggregator.Repository.Entities;

namespace WebAggregator.Infrastructure.Factories;

public class WebPageRepositoryFactory : IWebPageRepositoryFactory
{
    public WebPageDomainModel ToDomain(WebPage webPage)
    {
        ArgumentNullException.ThrowIfNull(webPage, nameof(webPage));

        return new WebPageDomainModel
        {
            Id = webPage.Id,
            Url = webPage.Url,
            Title = webPage.Title,
            Content = webPage.Content,
            CreatedAt = webPage.CreatedAt
        };
    }

    public WebPage ToEntity(WebPageDomainModel domain)
    {
        ArgumentNullException.ThrowIfNull(domain, nameof(domain));

        return new WebPage
        {
            Id = domain.Id,
            Url = domain.Url,
            Title = domain.Title,
            Content = domain.Content,
            CreatedAt = domain.CreatedAt
        };
    }
}
