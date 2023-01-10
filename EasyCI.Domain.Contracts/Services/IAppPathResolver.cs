using EasyCI.Domain.Contracts.Models;

namespace EasyCI.Domain.Contracts.Services
{
    public interface IAppPathResolver
    {
        string ResolveAppPath(AvailableApps app);
    }
}
