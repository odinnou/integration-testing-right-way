using AutoMapper;
using Domain.Models;
using Domain.Ports.Driving;
using Microsoft.AspNetCore.Mvc;
using Service.DrivingAdapters.RestAdapters.Dtos;
using System.Net.Mime;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Service.DrivingAdapters.RestAdapters;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("api/pandas")]
public class PandasRestAdapter : ControllerBase
{
    private readonly IMapper _mapper;

    public PandasRestAdapter(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Get panda name and last location for id
    /// </summary>
    /// <param name="pandaId" example="54322345-5432-2345-5432-543223455432">Panda Id to fetch</param>
    /// <response code="200">OK, Panda fetched</response>
    /// <response code="404">Panda not found</response>
    [HttpGet("{pandaId:guid:required}")]
    [ProducesResponseType(typeof(PandaDto), Status200OK)]
    [ProducesResponseType(typeof(void), Status404NotFound)]
    public async Task<PandaDto> Get([FromServices] IPandaFetcher pandaFetcher, Guid pandaId)
    {
        Panda panda = await pandaFetcher.Execute(pandaId);

        return _mapper.Map<PandaDto>(panda);
    }

    /// <summary>
    /// Add a new panda to the datastore
    /// </summary>
    /// <param name="panda">Panda model to apply</param>
    /// <response code="200">Ok, correctly added panda</response>
    /// <response code="400">BadRequest, model is invalid</response>
    [HttpPost]
    [ProducesResponseType(typeof(PandaDto), Status200OK)]
    [ProducesResponseType(typeof(void), Status400BadRequest)]
    public async Task<PandaDto> Add([FromServices] IPandaAppender pandaAppender, InsertPandaDto panda)
    {
        Panda pandaToAdd = _mapper.Map<Panda>(panda);
        Panda addedPanda = await pandaAppender.Execute(pandaToAdd);

        return _mapper.Map<PandaDto>(addedPanda);
    }
}