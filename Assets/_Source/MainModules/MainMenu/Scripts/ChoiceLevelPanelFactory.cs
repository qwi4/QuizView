using Quiz.Models;
using Quiz.Services;
using UnityEngine;

namespace Quiz.MainModules
{
    public class ChoiceLevelPanelFactory
    {
        private readonly GameFactory _gameFactory;
        private readonly QuizData _quizData;
        private readonly IProgressService _progress;

        public ChoiceLevelPanelFactory(GameFactory gameFactory, QuizData quizData, IProgressService progress)
        {
            _gameFactory = gameFactory;
            _quizData = quizData;
            _progress = progress;
        }

        public IProgressService Progress => _progress;
        public ChoiceLevelPanelView Create(Transform canvasParent)
        {
            var panel = _gameFactory.CreateChoiceLevelPanel(canvasParent);

            foreach (var lvl in _quizData.levels)
            {
                var btn = _gameFactory.CreateButtonLevel(panel.Parent);
                btn.Init(lvl.levelNumber);
                if (lvl.levelNumber <= _progress.UnlockedLevel)
                    btn.ActivateButton();
                else
                    btn.DeactivateButton();

                btn.OnClicked += selected =>
                    panel.NotifyLevelSelected(selected);
            }

            return panel;
        }
    }
}