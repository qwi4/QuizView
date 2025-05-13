using Quiz.Infrastructure;
using Quiz.MainModules;
using Quiz.Models;
using Quiz.Services;
using UnityEngine;
using Zenject;

namespace Quiz.App
{
    public class MainInstaller : MonoInstaller
    {
        [Header("Prefabs")]
        [SerializeField] private QuestionPanelFourResponsesView _questionPanelFourResponsesView;
        [SerializeField] private QuestionPanelTrueOrFalseView _questionPanelTrueOrFalseView;
        [SerializeField] private ChoicePanelView _choicePanelView;
        [SerializeField] private ChoiceLevelPanelView _choiceLevelPanelView;
        [SerializeField] private ResultPanelView _resultPanelView;
        [SerializeField] private RewardPanelView _rewardPanelView;
        [SerializeField] private ButtonAnswer _buttonAnswer;
        [SerializeField] private ButtonLevelView _buttonLevelView;

        [Space, Header("In Scene")]
        [SerializeField] private RectTransform _canvasParent;
        [SerializeField] private AudioSource _audioSource;

        public override void InstallBindings()
        {
            Container.BindInstance(_canvasParent)
                .WithId("CanvasParent");
            Container.BindInstance(_audioSource)
                .AsSingle();
            
            Container.BindInstance(_questionPanelFourResponsesView).AsSingle();
            Container.BindInstance(_questionPanelTrueOrFalseView).AsSingle();
            Container.BindInstance(_buttonAnswer).AsSingle();
            Container.BindInstance(_buttonLevelView).AsSingle();
            Container.BindInstance(_choicePanelView).AsSingle();
            Container.BindInstance(_choiceLevelPanelView).AsSingle();
            Container.BindInstance(_resultPanelView).AsSingle();
            Container.BindInstance(_rewardPanelView).AsSingle();

            Container.Bind<IQuizDataLoader>()
                .To<LocalizedQuizDataLoader>()
                .AsSingle();

            Container.Bind<QuizData>()
                .FromMethod(ctx =>
                {
                    var loader = ctx.Container.Resolve<IQuizDataLoader>();
                    return loader.LoadAsync("questions")
                                 .GetAwaiter()
                                 .GetResult();
                })
                .AsSingle();

            Container.Bind<ISound>()
                .To<SoundService>()
                .AsSingle();
            Container.Bind<ILifelineService>()
                .To<LifelineService>()
                .AsSingle();
            Container.Bind<IProgressService>()
                .To<ProgressService>()
                .AsSingle();
            Container.Bind<IQuizService>()
                .To<QuizService>()
                .AsSingle()
                .NonLazy();

            Container.Bind<ChoiceLevelPanelFactory>()
                .AsSingle();
            Container.Bind<GameFactory>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<GameBootstrap>()
                .AsSingle()
                .NonLazy();
        }
    }
}