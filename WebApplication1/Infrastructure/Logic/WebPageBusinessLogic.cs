using HtmlAgilityPack;
using System.Collections.Concurrent;
using WebAggregator.Domain;
using WebAggregator.Infrastructure.Helpers;
using WebAggregator.Infrastructure.Logic.Interfaces;
using WebAggregator.Repository.Interfaces;

namespace WebAggregator.Infrastructure.Logic;

public class WebPageBusinessLogic (IWebPageRepository repository, IHttpClientFactory httpClientFactory) : IWebPageBusinessLogic
{
    private readonly IWebPageRepository _repository = repository;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<List<WebPageDomainModel>> CreateAsync(WebPageDomainModel model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        var client = _httpClientFactory.CreateClient("WebPageClient");
        var response = await client.GetAsync(model.Url!);
        var content = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            var baseUrl = StringHelper.ReturnBaseUrl(model.Url!);
            var tags = ParseHtml(content);
            return await CreateWebPagesAsync(tags, baseUrl);
        }

        return [];
    }

    private static List<HtmlNode> ParseHtml(string html)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        return htmlDoc.DocumentNode.Descendants("a")
            .Where(node => node.GetAttributeValue("href", "").Contains("/news/"))
            .ToList();
    }

    private async Task<List<WebPageDomainModel>> CreateWebPagesAsync(List<HtmlNode> tags, string baseUrl)
    {
        var webPages = new ConcurrentBag<WebPageDomainModel>();
        var client = _httpClientFactory.CreateClient("WebPageClient");
        client.BaseAddress = new Uri(baseUrl);

        await Parallel.ForEachAsync(tags, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount - 1 }, async (tag, cancellationToken) =>
        {
            var url = StringHelper.ExtractUrl(tag.OuterHtml);
            var response = await client.GetAsync(url, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            var title = StringHelper.RemoveNonAlphanumeric(tag.InnerText);

            var page = new WebPageDomainModel
            {
                Content = content,
                Title = title,
                Url = baseUrl + url
            };
            webPages.Add(page);
        });

        return await _repository.CreateListAsync(webPages.ToList());
    }

    public async Task<List<WebPageDomainModel>> GetFilteredDataAsync(string searchTerm)
    {
        ArgumentException.ThrowIfNullOrEmpty(searchTerm, nameof(searchTerm));

        return await _repository.GetFilteredDataAsync(searchTerm);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<WebPageDomainModel> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<List<WebPageDomainModel>> GetAllPeopleAsync()
    {
        return await _repository.GetAllAsync();
    }
}
