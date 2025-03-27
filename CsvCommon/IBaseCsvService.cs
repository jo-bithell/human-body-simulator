namespace CsvCommon
{
    public interface IBaseCsvService
    {
        List<string[]> ReadCsvFile(string filePath);
        void ProcessCsvFile(List<string[]> records, string outputDirectory);
    }
}