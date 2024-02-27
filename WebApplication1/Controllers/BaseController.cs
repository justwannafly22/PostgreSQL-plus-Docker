using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace WebAggregator.Controllers;

public class BaseController : Controller
{
    public static string GetErrorMessage(ModelStateDictionary modelState)
    {
        return string.Join(" ", modelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
    }
}
