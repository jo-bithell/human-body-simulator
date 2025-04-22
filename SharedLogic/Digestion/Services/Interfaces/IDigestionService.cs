namespace SharedLogic.Digestion.Services.Interfaces
{
    public interface IDigestionService
    {
        IEnumerable<string[]> ReadCsvFile(string filePath);
        Task DigestFoodAsync(Func<IEnumerable<string[]>, Task> func);
        void WriteCsvFile(string filePath, List<string[]> records);
        Task PerformMotion(int atpThreshold);
        abstract Task DigestAsync();
    }
}