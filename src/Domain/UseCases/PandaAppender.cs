using Domain.Models;
using Domain.Ports.Driven;
using Domain.Ports.Driving;

namespace Domain.UseCases;

public class PandaAppender : IPandaAppender
{
    private readonly IPandaPersistencePort _pandaPersistencePort;

    public PandaAppender(IPandaPersistencePort pandaPersistencePort)
    {
        _pandaPersistencePort = pandaPersistencePort;
    }

    public async Task<Panda> Execute(Panda panda)
    {
        panda.Id = Guid.NewGuid();

        return await _pandaPersistencePort.AddPanda(panda);
    }
}
