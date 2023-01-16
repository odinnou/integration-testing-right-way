using AutoMapper;
using Domain.Models;
using Domain.Ports.Driven;
using Microsoft.EntityFrameworkCore;
using Service.DrivenAdapters.DatabaseAdapters.Entities;

namespace Service.DrivenAdapters.DatabaseAdapters;

public class PandaPersistenceAdapter : IPandaPersistencePort
{
    private readonly PandaContext _pandaContext;
    private readonly IMapper _mapper;

    public PandaPersistenceAdapter(PandaContext pandaContext, IMapper mapper)
    {
        _pandaContext = pandaContext;
        _mapper = mapper;
    }

    public async Task<Panda> AddPanda(Panda panda)
    {
        PandaEntity pandaEntity = _mapper.Map<PandaEntity>(panda);

        _pandaContext.Add(pandaEntity);

        await _pandaContext.SaveChangesAsync();

        return _mapper.Map<Panda>(pandaEntity);
    }

    public async Task<Panda?> GetById(Guid pandaId)
    {
        PandaEntity? panda = await _pandaContext.Pandas.Where(rule => rule.Id == pandaId)
                                                       .SingleOrDefaultAsync();

        return panda != null ? _mapper.Map<Panda>(panda) : null;
    }
}

