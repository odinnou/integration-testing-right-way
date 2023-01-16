using Domain.Models;

namespace Domain.Ports.Driving;

public interface IPandaAppender
{
    Task<Panda> Execute(Panda panda);
}
