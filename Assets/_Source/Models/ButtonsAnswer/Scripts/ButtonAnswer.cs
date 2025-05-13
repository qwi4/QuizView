using System;
using UnityEngine;
using UnityEngine.UI;

namespace Quiz.Models
{
    public class ButtonAnswer : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private SimpleText _simpleText;
        
        private bool _isTrueAnswer;

        public bool IsTrueAnswer => _isTrueAnswer;
        
        public event Action<ButtonAnswer> ButtonAnswerClicked;

        public void Init(bool isTrueAnswer, string text = null)
        {
            _isTrueAnswer = isTrueAnswer;

            if (text != null)
                _simpleText.SetText(text);
        }

        private void OnEnable() =>
            _button.onClick.AddListener(() => ButtonAnswerClicked?.Invoke(this));
        
        private void OnDisable() =>
            _button.onClick.RemoveListener(() => ButtonAnswerClicked?.Invoke(this));
    }
}