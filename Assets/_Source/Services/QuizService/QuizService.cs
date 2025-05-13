using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Quiz.Infrastructure;
using Quiz.MainModules;
using Quiz.Models;
using UnityEngine;
using YG;

namespace Quiz.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizDataLoader _loader;
        private readonly GameFactory _gameFactory;
        private readonly IProgressService _progress;
        private readonly ILifelineService _lifelineService;
        private QuizData _data;

        private int _currentLevelNumber;

        public event Action GoToMainMenu;

        public QuizService(IQuizDataLoader loader, GameFactory gameFactory, IProgressService progress, 
            ILifelineService lifelineService)
        {
            _loader = loader;
            _gameFactory = gameFactory;
            _progress = progress;
            _lifelineService = lifelineService;
        }

        public async UniTask StartLevelAsync(int levelNumber, Transform canvasParent)
        {
            if (_data == null)
                _data = await _loader.LoadAsync("questions");

            _currentLevelNumber = levelNumber;

            var level = _data.levels.First(l => l.levelNumber == levelNumber);
            var correctCount = 0;
            var totalCount = level.questions.Count;

            var exitToMain = false;

            foreach (var q in level.questions)
            {
                if (exitToMain) break;

                for (int i = canvasParent.childCount - 1; i >= 0; i--)
                    UnityEngine.Object.Destroy(canvasParent.GetChild(i).gameObject);

                if (string.Equals(q.type, "TrueFalse", StringComparison.OrdinalIgnoreCase))
                {
                    var tfView = _gameFactory.CreatePanelTrueOrFalse(canvasParent);
                    tfView.SetMainQuestionText(q.text, _currentLevelNumber);
                    tfView.SetButtons(q.correctAnswer);

                    void OnMainMenuTF()
                    {
                        tfView.MainMenuButtonClicked -= OnMainMenuTF;
                        exitToMain = true;
                        tfView.ThisDestroy();
                        GoToMainMenu?.Invoke();
                    }
                    tfView.MainMenuButtonClicked += OnMainMenuTF;

                    await HandleQuestionWithContinue(
                        tfView,
                        btn => btn.IsTrueAnswer == q.correctAnswer,
                        () => correctCount++);
                }
                else
                {
                    var mcView = _gameFactory.CreatePanelFourResponses(canvasParent);
                    mcView.SetMainQuestionText(q.text, _currentLevelNumber);

                    var parent = ((MonoBehaviour)mcView).transform;
                    var answers = new List<ButtonAnswer>();
                    for (var i = 0; i < q.options.Count; i++)
                    {
                        var btn = _gameFactory.CreateButtonAnswer(parent);
                        btn.Init(i == q.correctOptionIndex, q.options[i]);
                        answers.Add(btn);
                    }
                    mcView.SetAnswers(answers);

                    mcView.SetBoostCount(_lifelineService.RemainingBoosts);
                    void OnBoost()
                    {
                        if (_lifelineService.UseFiftyFifty())
                        {
                            RemoveTwoWrongAnswers(mcView);
                            mcView.SetBoostCount(_lifelineService.RemainingBoosts);
                        }
                    }
                    mcView.FiftyFiftyClicked += OnBoost;

                    void OnReward()
                    {
                        var rewardPanel = _gameFactory.CreateRewardPanel(canvasParent);
                        rewardPanel.RewardButtonClicked += () => OnRewardButtonClicked(mcView, rewardPanel);
                    }
                    mcView.RewardBoostClicked += OnReward;

                    void OnMainMenuMC()
                    {
                        mcView.MainMenuButtonClicked -= OnMainMenuMC;
                        exitToMain = true;
                        mcView.ThisDestroy();
                        GoToMainMenu?.Invoke();
                    }
                    mcView.MainMenuButtonClicked += OnMainMenuMC;

                    await HandleQuestionWithContinue(
                        mcView,
                        btn => btn.IsTrueAnswer,
                        () => correctCount++);

                    mcView.FiftyFiftyClicked -= OnBoost;
                    mcView.RewardBoostClicked -= OnReward;
                    mcView.MainMenuButtonClicked -= OnMainMenuMC;
                }
            }

            if (exitToMain)
                return;

            var passed = correctCount >= totalCount * 0.5f;

            if (passed && _progress.UnlockedLevel == _currentLevelNumber)
                _progress.UnlockNextLevel();

            var resultView = _gameFactory.CreateResultPanel(canvasParent);
            resultView.SetResults(correctCount, totalCount, passed);
            await HandleResultActions(resultView, passed, canvasParent);
        }

        private void OnRewardButtonClicked(IQuestionPanelFourResponsesView view, IRewardPanel rewardPanelView)
        {
            YG2.RewardedAdvShow(rewardPanelView.IDReward.ToString(), () =>
            {
                Debug.LogException(new Exception("Реклама идет"));
                _lifelineService.AddBoosts(1);
                view.SetBoostCount(_lifelineService.RemainingBoosts);
                rewardPanelView.DestroyPanel();
            });
        }

        private void RemoveTwoWrongAnswers(IQuestionPanelFourResponsesView view)
        {
            var wrong = view.GetAnswerButtons()
                .Where(b => !b.IsTrueAnswer)
                .OrderBy(_ => Guid.NewGuid())
                .Take(2);

            foreach (var w in wrong)
            {
                var btn = w.GetComponent<UnityEngine.UI.Button>();
                btn.interactable = false;
                btn.gameObject.SetActive(false);
            }
        }

        private UniTask HandleQuestionWithContinue<T>(T view, Func<ButtonAnswer, bool> isCorrect, Action onCorrect) 
            where T : class
        {
            var tcs = new UniTaskCompletionSource();

            void OnAnswer(ButtonAnswer btn)
            {
                if (isCorrect(btn))
                    onCorrect();

                switch (view)
                {
                    case IQuestionPanelFourResponsesView mc:
                        mc.AnswerClicked -= OnAnswer;
                        mc.ShowContinueButton();
                        mc.ContinueClicked += OnContinue;
                        break;
                    case IQuestionPanelTrueOrFalseView tf:
                        tf.AnswerClicked -= OnAnswer;
                        tf.ShowContinueButton();
                        tf.ContinueClicked += OnContinue;
                        break;
                }
            }

            void OnContinue()
            {
                switch (view)
                {
                    case IQuestionPanelFourResponsesView mc2:
                        mc2.ContinueClicked -= OnContinue;
                        mc2.ThisDestroy();
                        break;
                    case IQuestionPanelTrueOrFalseView tf2:
                        tf2.ContinueClicked -= OnContinue;
                        tf2.ThisDestroy();
                        break;
                }
                tcs.TrySetResult();
            }

            switch (view)
            {
                case IQuestionPanelFourResponsesView mcInit:
                    mcInit.AnswerClicked += OnAnswer;
                    break;
                case IQuestionPanelTrueOrFalseView tfInit:
                    tfInit.AnswerClicked += OnAnswer;
                    break;
            }

            return tcs.Task;
        }

        private UniTask HandleResultActions(IResultPanelView view, bool passed, Transform canvasParent)
        {
            var tcs = new UniTaskCompletionSource();

            void Cleanup()
            {
                view.ContinueClicked -= OnContinue;
                view.ReturnLevelClicked -= OnReturn;
                view.MainMenuClicked -= OnMainMenu;
            }

            async void OnContinue()
            {
                Cleanup();
                view.ThisDestroy();
                YG2.InterstitialAdvShow();

                if (passed)
                {
                    int next = _currentLevelNumber + 1;
                    if (next <= _progress.UnlockedLevel)
                        await StartLevelAsync(next, canvasParent);
                    else
                        GoToMainMenu?.Invoke();
                }

                tcs.TrySetResult();
            }

            async void OnReturn()
            {
                Cleanup();
                view.ThisDestroy();
                await StartLevelAsync(_currentLevelNumber, canvasParent);
                tcs.TrySetResult();
                YG2.InterstitialAdvShow();
            }

            void OnMainMenu()
            {
                Cleanup();
                view.ThisDestroy();
                GoToMainMenu?.Invoke();
                tcs.TrySetResult();
                YG2.InterstitialAdvShow();
            }

            view.ContinueClicked += OnContinue;
            view.ReturnLevelClicked += OnReturn;
            view.MainMenuClicked  += OnMainMenu;

            return tcs.Task;
        }
    }
}