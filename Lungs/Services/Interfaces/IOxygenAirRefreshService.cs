namespace Lungs.Services.Interfaces
{
    internal interface IOxygenAirRefreshService
    {
        Task PerformMotionAndRefreshAirAsync();
    }
}