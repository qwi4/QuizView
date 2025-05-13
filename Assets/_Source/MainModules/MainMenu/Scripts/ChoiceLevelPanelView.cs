using System;
using UnityEngine;

namespace Quiz.MainModules
{
    public class ChoiceLevelPanelView : MonoBehaviour
    {
        [SerializeField] private RectTransform _parent;
        
        public RectTransform Parent => _parent;
        
        public event Action<int> LevelSelected;

        public void ThisDestroy() =>
            Destroy(gameObject);
        
        public void NotifyLevelSelected(int level) =>
            LevelSelected?.Invoke(level);
    }
}