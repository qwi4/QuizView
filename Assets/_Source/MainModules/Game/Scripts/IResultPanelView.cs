using System;

namespace Quiz.MainModules
{
    public interface IResultPanelView
    {
        void SetResults(int correctCount, int totalCount, bool passed);
        event Action ContinueClicked;
        event Action MainMenuClicked;
        event Action ReturnLevelClicked;
        void ThisDestroy();
    }
}