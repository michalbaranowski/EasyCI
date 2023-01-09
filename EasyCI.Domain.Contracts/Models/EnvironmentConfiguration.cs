namespace EasyCI.Domain.Contracts.Models
{
    public class EnvironmentConfiguration
    {
        public EnvironmentConfiguration(
            string projectPath,
            string projectName,
            string port,
            List<string> environmentVariables)
        {
            ProjectPath = projectPath;
            ProjectName = projectName;
            Port = port;
            EnvironmentVariables = environmentVariables;
                
        }

        public string ProjectPath { get; }
        public string ProjectName { get; }

        public string ImageName => ProjectName.ToLower();

        public string Port { get; }
        public List<string> EnvironmentVariables { get; } = new List<string>();
    }
}
