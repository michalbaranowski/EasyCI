namespace EasyCI.Domain.Contracts.Services
{
    public interface IDockerCommandsRunner
    {
        Dictionary<string, string> GetDockerContainerIdsWithImageIds();
        void Build();
        void Run();
        void StopContainers(List<string> containerIds);
        void RemoveContainers(List<string> containerIds);
        void RemoveImages(List<string> imageIds);
    }
}
