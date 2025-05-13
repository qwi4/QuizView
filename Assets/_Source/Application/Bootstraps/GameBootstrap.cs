using System;
using Cysharp.Threading.Tasks;
using Quiz.Services;
using Quiz.MainModules;
using Quiz.Models;
using UnityEngine;
using Zenject;

namespace Quiz.App
{
    public class GameBootstrap : IInitializable, IDisposable
    {
        private readonly GameFactory _gameFactory;
        private readonly ChoiceLevelPanelFactory _levelFactory;
        private readonly IQuizService _quizService;
        private readonly RectTransform _canvasParent;

        private ChoicePanelView _mainMenu;
        private ChoiceLevelPanelView _levelPanel;

        public GameBootstrap(GameFactory gameFactory, ChoiceLevelPanelFactory levelFactory, IQuizService quizService,
            [Inject(Id = "CanvasParent")] RectTransform canvasParent)
        {
            _gameFactory = gameFactory;
            _levelFactory = levelFactory;
            _quizService = quizService;
            _canvasParent = canvasParent;
        }

        public void Initialize()
        {
            ShowMainMenu();
            _quizService.GoToMainMenu += ShowMainMenu;
        }

        private void ShowMainMenu()
        {
            if (_mainMenu != null)
            {
                _mainMenu.ChoiceLevelButtonClicked -= OnChoiceLevel;
                _mainMenu.NextLevelButtonClicked -= OnNextLevel;
                _mainMenu.ThisDestroy();
                _mainMenu = null;
            }

            _mainMenu = _gameFactory.CreateChoicePanel(_canvasParent);
            _mainMenu.ChoiceLevelButtonClicked += OnChoiceLevel;
            _mainMenu.NextLevelButtonClicked += OnNextLevel;
        }

        private void OnChoiceLevel()
        {
            _mainMenu.ChoiceLevelButtonClicked -= OnChoiceLevel;
            _mainMenu.ThisDestroy();
            _mainMenu = null;

            _levelPanel = _levelFactory.Create(_canvasParent);
            _levelPanel.LevelSelected += OnLevelSelected;
        }

        private async void OnNextLevel()
        {
            _mainMenu.NextLevelButtonClicked -= OnNextLevel;
            _mainMenu.ThisDestroy();
            _mainMenu = null;

            await PlayLevel(_levelFactory, null, useUnlocked: true);

            ShowMainMenu();
        }

        private async void OnLevelSelected(int level)
        {
            _levelPanel.LevelSelected -= OnLevelSelected;
            _levelPanel.ThisDestroy();
            _levelPanel = null;

            await PlayLevel(_levelFactory, level, useUnlocked: false);

            ShowMainMenu();
        }

        private async UniTask PlayLevel(ChoiceLevelPanelFactory factory, int? level, bool useUnlocked)
        {
            var lvl = useUnlocked
                ? factory.Progress.UnlockedLevel
                : level.Value;

            await _quizService.StartLevelAsync(lvl, _canvasParent);
        }

        public void Dispose()
        {
            if (_mainMenu != null)
            {
                _mainMenu.ChoiceLevelButtonClicked -= OnChoiceLevel;
                _mainMenu.NextLevelButtonClicked   -= OnNextLevel;
            }
            
            if (_levelPanel != null)
                _levelPanel.LevelSelected -= OnLevelSelected;
            
            _quizService.GoToMainMenu -= ShowMainMenu;
        }
    }
}