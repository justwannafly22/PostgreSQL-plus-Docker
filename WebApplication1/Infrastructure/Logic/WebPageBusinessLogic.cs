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

        return await _repository.CreateAsync(model);
    }

    public async Task<WebPageDomainModel> UpdateAsync(Guid id, WebPageDomainModel model)
    {
        ArgumentException.ThrowIfNullOrEmpty(id.ToString(), nameof(id));
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        return await _repository.UpdateAsync(id, model);
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
