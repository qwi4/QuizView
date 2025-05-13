using TMPro;
using UnityEngine;

namespace Quiz.Models
{
    public class SimpleText : MonoBehaviour, ISimpleText
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        public void SetText(string text)
        {
            _text.text = text;
        }
    }
}