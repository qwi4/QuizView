using System;
using Cysharp.Threading.Tasks;
using Quiz.Models;
using UnityEngine;
using YG;

namespace Quiz.Infrastructure
{
    public class LocalizedQuizDataLoader : IQuizDataLoader
    {
        public async UniTask<QuizData> LoadAsync(string resourceKey)
        {
            var lang = YG2.lang; 
            var fileName = $"{resourceKey}_{lang}";
            
            var ta = Resources.Load<TextAsset>(fileName)
                     ?? Resources.Load<TextAsset>($"{resourceKey}_en");
            
            if (ta == null)
                throw new InvalidOperationException($"Cannot load quiz data: {fileName}");
            
            return JsonUtility.FromJson<QuizData>(ta.text);
        }
    }
}