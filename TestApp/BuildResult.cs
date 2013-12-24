namespace TestApp
{
    public class BuildResult
    {
        public BuildResult(bool success, string assemblyName, string failureMessage)
        {
            Success = success;
            AssemblyName = assemblyName;
            FailureMessage = failureMessage;
        }

        public bool Success { get; private set; }
        public string AssemblyName { get; private set; }
        public string FailureMessage { get; private set; }
    }
}