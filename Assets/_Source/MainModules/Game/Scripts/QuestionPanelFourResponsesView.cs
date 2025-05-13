using System;
using System.Collections.Generic;
using Quiz.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Quiz.MainModules
{
    public class QuestionPanelFourResponsesView : MonoBehaviour, IQuestionPanelFourResponsesView
    {
        [SerializeField] private SimpleText _mainQuestionText;
        [SerializeField] private SimpleText _levelText;
        [SerializeField] private Transform _answersParent;
        [SerializeField] private BoostButton _fiftyFiftyButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _mainMenuButton;

        private readonly List<ButtonAnswer> _answers = new List<ButtonAnswer>();

        public event Action<ButtonAnswer> AnswerClicked;
        public event Action ContinueClicked;
        public event Action MainMenuButtonClicked;
        public event Action FiftyFiftyClicked;
        public event Action RewardBoostClicked;

        private void OnEnable()
        {
            _continueButton.gameObject.SetActive(false);
            _continueButton.onClick.AddListener(OnContinueClicked);
            _fiftyFiftyButton.BoostButtonClicked += OnFiftyFiftyClicked;
            _fiftyFiftyButton.BoostButtonRewardPanelClicked += OnRewardBoostClicked;
            _mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        }

        private void OnDisable()
        {
            _fiftyFiftyButton.BoostButtonClicked -= OnFiftyFiftyClicked;
            _fiftyFiftyButton.BoostButtonRewardPanelClicked -= OnRewardBoostClicked;
            _mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
            
            _continueButton.onClick.RemoveListener(OnContinueClicked);
            foreach (var a in _answers)
                a.ButtonAnswerClicked -= OnAnswerClicked;
        }

        public void SetMainQuestionText(string text, int level)
        {
            _mainQuestionText.SetText(text);
            _levelText.SetText(level.ToString());
        }

        public void SetBoostCount(int count) =>
            _fiftyFiftyButton.Init(count);

        public void SetAnswers(List<ButtonAnswer> answers)
        {
            foreach (var old in _answers)
                old.ButtonAnswerClicked -= OnAnswerClicked;

            _answers.Clear();

            foreach (var ans in answers)
            {
                ans.transform.SetParent(_answersParent, false);
                ans.ButtonAnswerClicked += OnAnswerClicked;
                _answers.Add(ans);
            }

            _continueButton.gameObject.SetActive(false);
        }

        public IReadOnlyList<ButtonAnswer> GetAnswerButtons() => _answers;

        private void OnAnswerClicked(ButtonAnswer chosen)
        {
            foreach (var ans in _answers)
            {
                var btn = ans.GetComponent<Button>();
                btn.interactable = false;
                
                var img = btn.image;
                if (ans == chosen)
                    img.color = ans.IsTrueAnswer ? Color.green : Color.red;
                else if (ans.IsTrueAnswer)
                    img.color = Color.green;
            }

            AnswerClicked?.Invoke(chosen);
            _continueButton.gameObject.SetActive(true);
        }

        public void ShowContinueButton() =>
            _continueButton.gameObject.SetActive(true);

        private void OnContinueClicked() =>
            ContinueClicked?.Invoke();

        private void OnMainMenuClicked() =>
            MainMenuButtonClicked?.Invoke();

        private void OnFiftyFiftyClicked() =>
            FiftyFiftyClicked?.Invoke();

        private void OnRewardBoostClicked() =>
            RewardBoostClicked?.Invoke();

        public void ThisDestroy() =>
            Destroy(gameObject);
    }
}