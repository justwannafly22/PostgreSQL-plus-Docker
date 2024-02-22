using HtmlAgilityPack;
using System.Xml;
using WebAggregator.Domain;
using WebAggregator.Infrastructure.Logic.Interfaces;
using WebAggregator.Repository.Interfaces;

namespace WebAggregator.Infrastructure.Logic;

public class WebPageBusinessLogic (IWebPageRepository repository) : IWebPageBusinessLogic
{
    private readonly IWebPageRepository _repository = repository;

    public async Task<WebPageDomainModel> CreateAsync(WebPageDomainModel model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        var client = new HttpClient();
        var response = await client.GetAsync(model.Url!);
        var content = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            ParseHtml(content);
        }

        return await _repository.CreateAsync(model);
    }

    private List<string> ParseHtml(string html)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);
        

        // Parallel work -- ForEachAsync
        // happy path -- get the base url + the href = path to the news -- by this path we can get a content of the page means Content by words (the content of the page) and the title as well;
        // need to secure is it a base url or not;
        // innerText (title) + outerHTML (need to get href = content);
        var tags = htmlDoc.DocumentNode.Descendants("a")
            .Where(node => node.GetAttributeValue("href", "").Contains("/news/"))
            .ToList();

        List<string> wikiLink = new List<string>();

        return wikiLink;
    }

    public async Task<WebPageDomainModel> UpdateAsync(Guid id, WebPageDomainModel model)
    {
        ArgumentException.ThrowIfNullOrEmpty(id.ToString(), nameof(id));
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        return await _repository.UpdateAsync(id, model);
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
