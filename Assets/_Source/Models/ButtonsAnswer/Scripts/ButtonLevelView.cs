using System;
using UnityEngine;
using UnityEngine.UI;

namespace Quiz.Models
{
    public class ButtonLevelView : MonoBehaviour
    {
        [SerializeField] private SimpleText _simpleText;
        [SerializeField] private Button _button;

        private int _level;

        public event Action<int> OnClicked;
        
        public void Init(int level)
        {
            _level = level;
            _simpleText.SetText(level.ToString());
        }

        public void ActivateButton()
        {
            _button.interactable = true;
        }

        public void DeactivateButton()
        {
            _button.interactable = false;
        }

        private void OnEnable() =>
            _button.onClick.AddListener(() => OnClicked?.Invoke(_level));

        private void OnDisable() =>
            _button.onClick.RemoveListener(() => OnClicked?.Invoke(_level));
    }
}