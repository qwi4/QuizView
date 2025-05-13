using System;
using System.Collections.Generic;
using Quiz.Models;

namespace Quiz.MainModules
{
    public interface IQuestionPanelFourResponsesView
    {
        void SetMainQuestionText(string text, int level);
        void SetAnswers(List<ButtonAnswer> answers);
        event Action<ButtonAnswer> AnswerClicked;
        event Action ContinueClicked;
        event Action MainMenuButtonClicked;
        event Action RewardBoostClicked;
        void SetBoostCount(int remainingBoosts);
        event Action FiftyFiftyClicked;
        void ShowContinueButton();
        void ThisDestroy();
        IReadOnlyList<ButtonAnswer> GetAnswerButtons();
    }
}