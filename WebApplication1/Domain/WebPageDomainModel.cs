namespace WebAggregator.Domain;

public class WebPageDomainModel
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Url { get; set; }
    public string? Content { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
