using Quiz.MainModules;
using UnityEngine;
using Zenject;

namespace Quiz.Models
{
    public class GameFactory : BaseFactory
    {
        public GameFactory(DiContainer container) : base(container)
        {
        }

        public IQuestionPanelFourResponsesView CreatePanelFourResponses(Transform parent) =>
            CreateFromPrefab<QuestionPanelFourResponsesView>(parent);

        public IQuestionPanelTrueOrFalseView CreatePanelTrueOrFalse(Transform parent) =>
            CreateFromPrefab<QuestionPanelTrueOrFalseView>(parent);

        public ButtonAnswer CreateButtonAnswer(Transform parent) =>
            CreateFromPrefab<ButtonAnswer>(parent);

        public ButtonLevelView CreateButtonLevel(Transform parent) =>
            CreateFromPrefab<ButtonLevelView>(parent);
        
        public ChoicePanelView CreateChoicePanel(Transform parent) =>
            CreateFromPrefab<ChoicePanelView>(parent);
        
        public ChoiceLevelPanelView CreateChoiceLevelPanel(Transform parent) =>
            CreateFromPrefab<ChoiceLevelPanelView>(parent);

        public ButtonLevelView CreateLevelButton(Transform parent) =>
            CreateFromPrefab<ButtonLevelView>(parent);
        
        public IResultPanelView CreateResultPanel(Transform parent) =>
            CreateFromPrefab<ResultPanelView>(parent);
        
        public IRewardPanel CreateRewardPanel(Transform parent) =>
            CreateFromPrefab<RewardPanelView>(parent);
    }
}