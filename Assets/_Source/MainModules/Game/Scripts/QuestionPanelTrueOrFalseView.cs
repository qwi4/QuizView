using System;
using Quiz.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Quiz.MainModules
{
    public class QuestionPanelTrueOrFalseView : MonoBehaviour, IQuestionPanelTrueOrFalseView
    {
        [SerializeField] private SimpleText _mainQuestionText;
        [SerializeField] private SimpleText _levelText;
        [SerializeField] private ButtonAnswer _trueAnswer;
        [SerializeField] private ButtonAnswer _falseAnswer;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _mainMenuButton;

        public event Action<ButtonAnswer> AnswerClicked;
        public event Action ContinueClicked;
        public event Action MainMenuButtonClicked;

        private void Awake()
        {
            _continueButton.gameObject.SetActive(false);
            _continueButton.onClick.AddListener(OnContinueClicked);
            _mainMenuButton.onClick.AddListener(() => MainMenuButtonClicked?.Invoke());
        }
        
        private void OnDisable()
        {
            _continueButton.onClick.RemoveListener(OnContinueClicked);
            _trueAnswer.ButtonAnswerClicked  -= OnAnswerClicked;
            _falseAnswer.ButtonAnswerClicked -= OnAnswerClicked;
            _mainMenuButton.onClick.RemoveListener(() => MainMenuButtonClicked?.Invoke());
        }

        public void SetMainQuestionText(string text, int level)
        {
            _mainQuestionText.SetText(text);
            _levelText.SetText(level.ToString());
        }

        public void SetButtons(bool isTrueAnswer)
        {
            _trueAnswer.Init(isTrueAnswer);
            _falseAnswer.Init(!isTrueAnswer);

            _trueAnswer.ButtonAnswerClicked  -= OnAnswerClicked;
            _falseAnswer.ButtonAnswerClicked -= OnAnswerClicked;
            _trueAnswer.ButtonAnswerClicked  += OnAnswerClicked;
            _falseAnswer.ButtonAnswerClicked += OnAnswerClicked;

            _continueButton.gameObject.SetActive(false);
        }

        private void OnAnswerClicked(ButtonAnswer btn)
        {
            AnswerClicked?.Invoke(btn);
            _continueButton.gameObject.SetActive(true);
            _trueAnswer.GetComponent<Button>().interactable = false;
            _falseAnswer.GetComponent<Button>().interactable = false;
        }

        public void ShowContinueButton() =>
            _continueButton.gameObject.SetActive(true);

        private void OnContinueClicked() =>
            ContinueClicked?.Invoke();

        public void ThisDestroy() =>
            Destroy(gameObject);
    }
}