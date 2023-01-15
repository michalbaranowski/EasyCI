namespace EasyCI.Domain.Contracts.Models
{
    public class EnvironmentConfiguration
    {
        public EnvironmentConfiguration(
            string projectPath,
            string projectName,
            string httpPort,
            string httpsPort,
            List<string> environmentVariables)
        {
            ProjectPath = projectPath;
            ProjectName = projectName;
            HttpPort = httpPort;
            HttpsPort = httpsPort;
            EnvironmentVariables = environmentVariables;
                
        }

        public string ProjectPath { get; }

        public string ExactPath => ProjectPath.Replace("~", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

        public string ProjectName { get; }

        public string ImageName => ProjectName.ToLower();

        public string HttpPort { get; }
        public string HttpsPort { get; }
        public List<string> EnvironmentVariables { get; } = new List<string>();
    }
}
