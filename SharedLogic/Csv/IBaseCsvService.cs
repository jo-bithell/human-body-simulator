namespace CsvCommon
{
    public interface IBaseCsvService
    {
        List<string[]> ReadCsvFile(string filePath);
        Task DigestFood(List<string[]> records, string outputDirectory);
    }
}