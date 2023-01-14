namespace NvidiaCleaner
{
    public interface IMainForm
    {
        void DeleteDownloadedDrivers();
        void DeleteExtractedDrivers();
        void DeleteExtractedDrivers(string Text);
        void DeleteLogs();
        void DeleteRepository();
        void DeleteWindowsDrivers();
        void GenerateLog();
        int BarPercentage { get; }
    }
}