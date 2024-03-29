﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAggregator.Boundary.Request;
using WebAggregator.Boundary.Response;
using WebAggregator.Domain;
using WebAggregator.Infrastructure.Exceptions;
using WebAggregator.Infrastructure.Logic.Interfaces;

namespace WebAggregator.Controllers;

[ApiController]
[Route("api/v1/webpages")]
[ApiExplorerSettings(GroupName = "v1")]
public class WebPageController(IWebPageBusinessLogic webPageBusinessLogic, IMapper mapper) : BaseController
{
    private readonly IWebPageBusinessLogic _webPageBusinessLogic = webPageBusinessLogic;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Returns all pages
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = _mapper.Map<List<WebPageResponseModel>>(await _webPageBusinessLogic.GetAllPeopleAsync());

        return Ok(response);
    }

    /// <summary>
    /// Returns filtered pages
    /// </summary>
    [HttpGet("search")]
    public async Task<IActionResult> GetFilteredPages([FromQuery] string searchTerm)
    {
        var response = _mapper.Map<List<WebPageResponseModel>>(await _webPageBusinessLogic.GetFilteredDataAsync(searchTerm));

        return Ok(response);
    }

    /// <summary>
    /// Returns a web page
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        try
        {
            var webPage = await _webPageBusinessLogic.GetByIdAsync(id);

            return Ok(_mapper.Map<WebPageResponseModel>(webPage));
        }
        catch (NotFoundException)
        {
            return NotFound(($"WebPage with id: {id} doesn`t exist in the database.", HttpStatusCode.NotFound));
        }
    }

    /// <summary>
    /// Create web pages
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] WebPageCreateRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new BaseResponseModel(GetErrorMessage(ModelState), HttpStatusCode.BadRequest));
        }

        var addedWebPages = await _webPageBusinessLogic.CreateAsync(_mapper.Map<WebPageDomainModel>(model));

        return CreatedAtAction(nameof(Create), _mapper.Map<List<WebPageResponseModel>>(addedWebPages));
    }


    /// <summary>
    /// Delete a web page
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        try
        {
            await _webPageBusinessLogic.DeleteAsync(id);

            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound(($"WebPage with id: {id} doesn`t exist in the database.", HttpStatusCode.NotFound));
        }
    }
}
