using WebAggregator.Domain;

namespace WebAggregator.Repository.Interfaces;

public interface IWebPageRepository
{
    public Task<WebPageDomainModel> CreateAsync(WebPageDomainModel model);
    public Task<List<WebPageDomainModel>> GetAllAsync();
    public Task<WebPageDomainModel> GetByIdAsync(Guid? id);
    public Task<WebPageDomainModel> UpdateAsync(Guid id, WebPageDomainModel model);
    public Task DeleteAsync(Guid? id);
    public Task<List<WebPageDomainModel>> GetFilteredDataAsync(string searchTerm);
}
