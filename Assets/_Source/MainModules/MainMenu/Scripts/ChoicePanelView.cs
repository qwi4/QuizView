using System;
using UnityEngine;
using UnityEngine.UI;

namespace Quiz.MainModules
{
    public class ChoicePanelView : MonoBehaviour
    {
        [SerializeField] private Button _choiceLevelButton;
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _infoButton;

        public event Action ChoiceLevelButtonClicked;
        public event Action NextLevelButtonClicked;
        public event Action InfoButtonClicked;

        public void ThisDestroy() =>
            Destroy(this.gameObject);
        
        private void OnEnable()
        {
            _choiceLevelButton.onClick.AddListener(() => ChoiceLevelButtonClicked?.Invoke());
            _nextLevelButton.onClick.AddListener(() => NextLevelButtonClicked?.Invoke());
            _infoButton.onClick.AddListener(() => InfoButtonClicked?.Invoke());
        }
        
        private void OnDisable()
        {
            _choiceLevelButton.onClick.RemoveListener(() => ChoiceLevelButtonClicked?.Invoke());
            _nextLevelButton.onClick.RemoveListener(() => NextLevelButtonClicked?.Invoke());
            _infoButton.onClick.RemoveListener(() => InfoButtonClicked?.Invoke());
        }
    }
}