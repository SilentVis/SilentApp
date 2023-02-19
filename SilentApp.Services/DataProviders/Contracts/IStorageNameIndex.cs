namespace SilentApp.Services.DataProviders.Contracts
{
    public interface IStorageNameIndex
    {
        string GetTableName(string entityName);

        string GetQueueName(string entityName);
    }
}
