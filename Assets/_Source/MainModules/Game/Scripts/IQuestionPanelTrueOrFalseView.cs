using System;
using Quiz.Models;

namespace Quiz.MainModules
{
    public interface IQuestionPanelTrueOrFalseView
    {
        void SetMainQuestionText(string text, int level);
        void SetButtons(bool isTrueAnswer);
        event Action<ButtonAnswer> AnswerClicked;
        event Action ContinueClicked;
        event Action MainMenuButtonClicked;
        void ShowContinueButton();
        void ThisDestroy();
    }
}