using System;
using System.Collections.Generic;
using Quiz.Models;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Quiz.MainModules
{
    public class ResultPanelView : MonoBehaviour, IResultPanelView
    {
        [SerializeField] private SimpleText _resultText;
        [SerializeField] private SimpleText _statusText;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _goToMainMenuButton;
        [SerializeField] private Button _returnLevelButton;

        public event Action ContinueClicked;
        public event Action MainMenuClicked;
        public event Action ReturnLevelClicked;

        private static readonly Dictionary<string, (string resultFmt, string passed, string failed)> LocTexts =
            new Dictionary<string, (string, string, string)>
        {
            ["ru"] = ("Правильных ответов: {0} из {1}", "Уровень пройден!", "Уровень не пройден!"),
            ["en"] = ("Correct answers: {0} of {1}",   "Level passed!",     "Level failed!"),
            ["tr"] = ("Doğru cevaplar: {0} / {1}",      "Seviye tamamlandı!", "Seviye başarısız oldu!")
        };

        public void SetResults(int correctCount, int totalCount, bool passed)
        {
            var lang = YG2.lang;
            if (!LocTexts.TryGetValue(lang, out var texts))
                texts = LocTexts["en"];

            _resultText.SetText(string.Format(texts.resultFmt, correctCount, totalCount));
            _statusText.SetText(passed ? texts.passed : texts.failed);

            _continueButton.gameObject.SetActive(passed);
        }

        private void Awake()
        {
            _continueButton.onClick.AddListener(() => ContinueClicked?.Invoke());
            _goToMainMenuButton.onClick.AddListener(() => MainMenuClicked?.Invoke());
            _returnLevelButton.onClick.AddListener(() => ReturnLevelClicked?.Invoke());
        }

        private void OnDestroy()
        {
            _continueButton.onClick.RemoveAllListeners();
            _goToMainMenuButton.onClick.RemoveAllListeners();
            _returnLevelButton.onClick.RemoveAllListeners();
        }

        public void ThisDestroy() => Destroy(gameObject);
    }
}