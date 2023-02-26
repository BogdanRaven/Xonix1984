using GameData;
using Infrastructure;
using UnityEngine;
using XonixFactory;
using Zenject;

namespace GameLogic
{
    public class Initializer : MonoBehaviour
    {
        private StateMachine _stateMachine;

        [SerializeField] private GameConfig gameConfig;

        [SerializeField] private UiContext _uiContext;
        [SerializeField] private MapRenderer _mapRenderer;
        [SerializeField] private GameContext _gameContext;

        private ITileFactory _tileFactory;
        private ISwipeService _swipeService;
        private IMovementService _movementService;

        private LevelMap _levelMap;

        [Inject]
        private void Context(ISwipeService swipeService, ITileFactory tileFactory, IMovementService movementService)
        {
            _swipeService = swipeService;
            _tileFactory = tileFactory;
            _movementService = movementService;
        }

        private void Start()
        {
            LevelMap levelMap = new LevelMap(gameConfig.StartLevel);
            var stateMachine = new StateMachine(gameConfig, levelMap, _swipeService, _movementService, _tileFactory,
                _gameContext, _uiContext, _mapRenderer, this);
            stateMachine.EnterState<StartState>();
            _gameContext.StartGameCore();
        }
    }
}