namespace WebAggregator.Boundary.Response;

public class WebPageResponseModel
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Url { get; set; }
    public string? Content { get; set; }
}
