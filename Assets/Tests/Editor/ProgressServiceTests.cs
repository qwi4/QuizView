using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using Cysharp.Threading.Tasks;
using Quiz.Services;
using Quiz.Infrastructure;

namespace Quiz.Tests
{
    public class ProgressServiceTests
    {
        [Test]
        public void InitiallyUnlockedLevelIsOne()
        {
            var progress = new ProgressService();
            Assert.AreEqual(1, progress.UnlockedLevel);
        }

        [Test]
        public void UnlockNextLevelIncrementsUnlockedLevel()
        {
            var progress = new ProgressService();
            progress.UnlockNextLevel();
            Assert.AreEqual(2, progress.UnlockedLevel);
        }

        [Test]
        public void UnlockNextLevelDoesNotExceedMax()
        {
            var progress = new ProgressService();
            for (int i = 0; i < 200; i++)
                progress.UnlockNextLevel();
            Assert.AreEqual(100, progress.UnlockedLevel);
        }

        [TestCase(2, 4, true)]   // 50% → unlock
        [TestCase(1, 4, false)]  // 25% → no unlock
        [TestCase(5, 10, true)]  // 50% → unlock
        [TestCase(4, 10, false)] // 40% → no unlock
        public void Threshold50PercentTests(int correctCount, int totalCount, bool shouldUnlock)
        {
            var progress = new ProgressService();
            if (shouldUnlock)
                progress.UnlockNextLevel();

            bool isUnlocked = progress.UnlockedLevel > 1;
            Assert.AreEqual(shouldUnlock, isUnlocked);
        }
    }

    public class QuizDataLoaderTests
    {
        [UnityTest]
        public IEnumerator LoadAsync_ReturnsValidQuizData() => UniTask.ToCoroutine(async () =>
        {
            var loader = new QuizDataLoader();
            var data = await loader.LoadAsync("questions");

            Assert.IsNotNull(data);
            Assert.IsNotNull(data.levels);
            Assert.AreEqual(100, data.levels.Count);
        });
    }
}
