using System;
using Quiz.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Quiz.MainModules
{
    public class BoostButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private SimpleText _simpleText;

        private int _boostCount;

        public event Action BoostButtonClicked;
        public event Action BoostButtonRewardPanelClicked;

        public void Init(int boostCount)
        {
            _boostCount = boostCount;
            _simpleText.SetText(_boostCount.ToString());
        }
        
        private void OnEnable() =>
            _button.onClick.AddListener(OnBoostClick);

        private void OnDisable() =>
            _button.onClick.RemoveListener(OnBoostClick);

        private void OnBoostClick()
        {
            if (_boostCount > 0)
            {
                _boostCount--;
                _simpleText.SetText(_boostCount.ToString());
                _button.interactable = false;
                BoostButtonClicked?.Invoke();
            }
            else
                BoostButtonRewardPanelClicked?.Invoke();
        }

        public void DisableButton() =>
            _button.interactable = false;
    }
}