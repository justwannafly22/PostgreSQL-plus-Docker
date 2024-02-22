using WebAggregator.Domain;

namespace WebAggregator.Infrastructure.Logic.Interfaces;

public interface IWebPageBusinessLogic
{
    public Task<WebPageDomainModel> CreateAsync(WebPageDomainModel model);
    public Task<WebPageDomainModel> UpdateAsync(Guid id, WebPageDomainModel model);
    public Task DeleteAsync(Guid id);
    public Task<WebPageDomainModel> GetByIdAsync(Guid id);
    public Task<List<WebPageDomainModel>> GetAllPeopleAsync();
    public Task<List<WebPageDomainModel>> GetFilteredDataAsync(string searchTerm);
}
