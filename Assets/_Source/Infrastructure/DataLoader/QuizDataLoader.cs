using Cysharp.Threading.Tasks;
using Quiz.Models;
using UnityEngine;

namespace Quiz.Infrastructure
{
    public class QuizDataLoader : IQuizDataLoader
    {
        public async UniTask<QuizData> LoadAsync(string resourcePath)
        {
            var textAsset = await Resources.LoadAsync<TextAsset>(resourcePath) as TextAsset;
            return JsonUtility.FromJson<QuizData>(textAsset.text);
        }
    }
}