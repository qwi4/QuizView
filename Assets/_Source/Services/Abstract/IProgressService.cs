namespace Quiz.Services
{
    public interface IProgressService
    {
        int UnlockedLevel { get; }
        void UnlockNextLevel();
    }
}