namespace SharedLogic.Logging
{
    public class BaseLogger
    {
        public virtual string LogBloodSent() => "Blood sent";
        public virtual string LogBloodReceived() => "Blood received";
        public virtual string LogCsvReceived() => "Csv received";
        public virtual string LogCsvSent() => "Csv sent";
    }
}
