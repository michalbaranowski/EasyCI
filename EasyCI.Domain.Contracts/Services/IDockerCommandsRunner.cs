using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCI.Domain.Contracts.Services
{
    public interface IDockerCommandsRunner
    {
        void Build();
        void Run();
        void Stop();
        void RemoveContainers();
    }
}
