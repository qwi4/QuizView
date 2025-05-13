using Quiz.Services;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

namespace Quiz.Models
{
    public class SoundButton : MonoBehaviour
    {
        [SerializeField] Button _soundButton;
        [SerializeField] Image  _lockImage;

        ISound _soundService;

        [Inject]
        public void Construct(ISound soundService)
        {
            _soundService = soundService;
        }

        private void OnEnable()
        {
            _lockImage.enabled = YG2.saves.IsMuted;
            _soundButton.onClick.AddListener(OnClick);
        }
        
        private void OnDisable() =>
            _soundButton.onClick.RemoveListener(OnClick);

        private void OnClick()
        {
            _soundService.Toggle();
            _lockImage.enabled = YG2.saves.IsMuted;
        }
    }
}