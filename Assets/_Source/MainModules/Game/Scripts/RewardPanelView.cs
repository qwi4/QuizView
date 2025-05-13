using System;
using UnityEngine;
using UnityEngine.UI;

namespace Quiz.MainModules
{
    public class RewardPanelView : MonoBehaviour, IRewardPanel
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _rewardButton;
        [SerializeField] private int _idReward = 1;

        public int IDReward => _idReward;
        public event Action RewardButtonClicked; 

        private void OnEnable()
        {
            _exitButton.onClick.AddListener(DestroyPanel);
            _rewardButton.onClick.AddListener(() => RewardButtonClicked?.Invoke());
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveListener(DestroyPanel);
            _rewardButton.onClick.RemoveListener(() => RewardButtonClicked?.Invoke());
        }

        public void DestroyPanel() =>
            Destroy(gameObject);
    }
}