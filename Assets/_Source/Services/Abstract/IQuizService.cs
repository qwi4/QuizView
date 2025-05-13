using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Quiz.Services
{
    public interface IQuizService
    {
        UniTask StartLevelAsync(int levelNumber, Transform canvasParent);
        event Action GoToMainMenu;
    }
}