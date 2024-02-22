using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq.Expressions;
using WebAggregator.Domain;
using WebAggregator.Infrastructure.Exceptions;
using WebAggregator.Infrastructure.Factories.Interfaces;
using WebAggregator.Repository.Entities;
using WebAggregator.Repository.Interfaces;

namespace WebAggregator.Repository;

public class WebPageRepository(AppDbContext context, IWebPageRepositoryFactory factory) : IWebPageRepository
{
    private readonly AppDbContext _context = context;
    private readonly IWebPageRepositoryFactory _factory = factory;

    public async Task<WebPageDomainModel> CreateAsync(WebPageDomainModel model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        var entity = _factory.ToEntity(model);
        await _context.WebPages.AddAsync(entity);
        await _context.SaveChangesAsync();

        Log.Information($"The country: {entity} was successfully created.");

        return _factory.ToDomain(entity);
    }

    public async Task DeleteAsync(Guid? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id.ToString(), nameof(id));

        var entity = await GetWebPagesByExpression(e => e.Id.Equals(id)).SingleOrDefaultAsync();
        if (entity is null)
        {
            Log.Warning($"The country with id {id} doesn`t exist in the database.");
            throw new NotFoundException($"The country with id {id} doesn`t exist in the database.");
        }

        _context.Remove(entity!);

        await _context.SaveChangesAsync();

        Log.Information($"The country: {entity} was successfully deleted.");
    }

    public async Task<List<WebPageDomainModel>> GetFilteredDataAsync(string searchTerm)
    {
        ArgumentException.ThrowIfNullOrEmpty(searchTerm, nameof(searchTerm));

        var entities = GetWebPagesByExpression(e => EF.Functions.ILike(e.Content, $"%{searchTerm}%") || EF.Functions.ILike(e.Title, $"%{searchTerm}%"))
            .Select(e => _factory.ToDomain(e))
            .AsNoTracking();

        Log.Information($"The country table was triggered. Returned count of rows: {entities.Count()}.");

        return await entities.ToListAsync();
    }

    public async Task<List<WebPageDomainModel>> GetAllAsync()
    {
        var entities = await GetAllWebPages()
            .Select(e => _factory.ToDomain(e))
            .AsNoTracking()
            .ToListAsync();

        Log.Information($"The country table was triggered. Returned count of rows: {entities.Count}.");

        return entities;
    }

    public async Task<WebPageDomainModel> GetByIdAsync(Guid? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id.ToString(), nameof(id));

        var entity = await GetWebPagesByExpression(e => e.Id.Equals(id)).SingleOrDefaultAsync();
        if (entity is null)
        {
            Log.Warning($"The country with id {id} doesn`t exist in the database.");
            throw new NotFoundException($"The country with id {id} doesn`t exist in the database.");
        }

        return _factory.ToDomain(entity!);
    }

    public async Task<WebPageDomainModel> UpdateAsync(Guid id, WebPageDomainModel model)
    {
        ArgumentException.ThrowIfNullOrEmpty(id.ToString(), nameof(id));
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        var entity = await GetWebPagesByExpression(e => e.Id.Equals(id)).SingleOrDefaultAsync();
        if (entity is null)
        {
            Log.Warning($"The country with id {id} doesn`t exist in the database.");
            throw new NotFoundException($"The country with id {id} doesn`t exist in the database.");
        }

        entity.Url = model.Url;
        entity.Title = model.Title;
        entity.Content = model.Content;

        await _context.SaveChangesAsync();

        Log.Information($"The country: {entity} was successfully updated.");

        return _factory.ToDomain(entity);
    }

    private IQueryable<WebPage> GetAllWebPages() =>
        _context.Set<WebPage>();

    private IQueryable<WebPage> GetWebPagesByExpression(Expression<Func<WebPage, bool>> expression) =>
        _context.Set<WebPage>()
                .Where(expression);
}
