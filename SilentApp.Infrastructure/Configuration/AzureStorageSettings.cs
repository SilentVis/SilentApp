namespace SilentApp.Infrastructure.Configuration
{
    public struct AzureStorageSettings
    {
        public AzureStorageSettings(string connectionString)
        {
            ConnectionString = connectionString;
        }


        public string ConnectionString { get; }
    }
}
