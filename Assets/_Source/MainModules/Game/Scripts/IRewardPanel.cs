using System;

namespace Quiz.MainModules
{
    public interface IRewardPanel
    {
        int IDReward { get; }
        event Action RewardButtonClicked;
        void DestroyPanel();
    }
}