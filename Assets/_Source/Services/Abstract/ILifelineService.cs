namespace Quiz.Services
{
    public interface ILifelineService
    {
        int RemainingBoosts { get; }
        
        bool UseFiftyFifty();
        
        void AddBoosts(int count);
    }
}