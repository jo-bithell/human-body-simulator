namespace SharedLogic.Motion.Services
{
    public interface IMotionService
    {
        Task PerformMotionAsync(int atpThreshold);
        Task<bool> CanPerformMotionAsync(int atpThreshold);
    }
}
