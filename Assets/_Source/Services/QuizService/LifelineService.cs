using YG;

namespace Quiz.Services
{
    public class LifelineService : ILifelineService
    {
        public int RemainingBoosts => YG2.saves.remainingBoosts;

        public bool UseFiftyFifty()
        {
            if (YG2.saves.remainingBoosts <= 0) return false;
            YG2.saves.remainingBoosts--;
            YG2.SaveProgress();
            return true;
        }

        public void AddBoosts(int count)
        {
            if (count <= 0) return;
            YG2.saves.remainingBoosts += count;
            YG2.SaveProgress();
        }
    }
}

namespace YG
{
    public partial class SavesYG
    {
        public int remainingBoosts = 3;
    }
}