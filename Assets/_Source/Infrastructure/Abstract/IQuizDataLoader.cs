using Cysharp.Threading.Tasks;
using Quiz.Models;

namespace Quiz.Infrastructure
{
    public interface IQuizDataLoader
    {
        UniTask<QuizData> LoadAsync(string resourcePath);
    }
}