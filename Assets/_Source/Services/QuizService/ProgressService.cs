using YG;

namespace Quiz.Services
{
    public class ProgressService : IProgressService
    {
        public int UnlockedLevel => YG2.saves.unlockedLevel;
        public void UnlockNextLevel()
        {
            if (YG2.saves.unlockedLevel < 100)
                YG2.saves.unlockedLevel++;
            
            YG2.SaveProgress();
        }
    }
}

namespace YG
{
    public partial class SavesYG
    {
        public int unlockedLevel = 1;
    }
}