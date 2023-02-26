using System.Collections.Generic;
using GameData;
using GameLogic;
using UnityEngine;
using XonixFactory;

namespace Infrastructure
{
    public class StartState : IState
    {
        private StateMachine _stateMachine;

        private LevelMap _levelMap;
        private GameConfig _gameConfig;

        private ISwipeService _swipeService;
        private IMovementService _movementService;
        private ITileFactory _tileFactory;

        private GameContext _gameContext;
        private UiContext _uiContext;
        private MonoBehaviour _monoBehaviour;
        private MapRenderer _mapRenderer;
        private Timer _timer;

        private EnemyInitializer _enemyInitializer;
        private Player _player;
        private Field _field;
        private List<Enemy> _waterEnemies;
        private List<Enemy> _landEnemies;

        public StartState(StateMachine stateMachine, GameConfig gameConfig, LevelMap levelMap, ISwipeService swipeService,
            IMovementService movementService,
            ITileFactory tileFactory, GameContext gameContext, UiContext uiContext, MapRenderer mapRenderer,
            MonoBehaviour monoBehaviour)
        {
            _stateMachine = stateMachine;

            _gameConfig = gameConfig;
            _levelMap = levelMap;
            _swipeService = swipeService;
            _movementService = movementService;

            _tileFactory = tileFactory;
            _gameContext = gameContext;
            _uiContext = uiContext;
            _monoBehaviour = monoBehaviour;
            _mapRenderer = mapRenderer;
        }

        public void Exit()
        {
        }

        public void Enter()
        {
            InitializeGameComponents();

            InitializeGame();

            AddListeners();

            UpdateUI();

            _uiContext.ShowGameMessage("Start", Color.green);
            Utilities.Invoke(_monoBehaviour, (() => { _stateMachine.EnterState<GameState>(); }),
                _uiContext.DurationGameMessage);
        }

        private void InitializeGameComponents()
        {
            _tileFactory.Load();

            _field = new Field(_gameConfig.FieldWidth, _gameConfig.FieldHeight, _gameConfig.WaterSpace,
                _gameConfig.PercentOfLandToWin);

            _player = new Player(Vector2Int.zero, Vector2Int.zero, _gameConfig.NumberPlayerLives);
            _timer = new Timer((float) _gameConfig.GameDuration, () => _stateMachine.EnterState<GameOverState>());

            _enemyInitializer = new EnemyInitializer();
            InitializeEnemies();
        }

        private void InitializeGame()
        {
            _swipeService.EnableSwipe(true);

            _timer.StartTimer();
            _timer.Pause();

            _gameContext.Context(_stateMachine, _field, _player, _waterEnemies, _landEnemies, _movementService,
                _mapRenderer, _uiContext, _timer);
            _gameContext.SetPauseGame(true);
            _mapRenderer.Context(_field, _player, _waterEnemies, _landEnemies, _tileFactory);
        }

        private void InitializeEnemies()
        {
            _waterEnemies =
                _enemyInitializer.InitializeEnemies(EnemyType.Water,
                    _gameConfig.NumberWaterEnemies + _levelMap.CurrentLevel);
            _landEnemies =
                _enemyInitializer.InitializeEnemies(EnemyType.Land, _gameConfig.NumberLandEnemies);

            _enemyInitializer.SetPositionEnemies(_waterEnemies, _field, TileType.Water);
            _enemyInitializer.SetDirection(_waterEnemies);

            _enemyInitializer.SetPositionEnemies(_landEnemies, _field, TileType.Land);
            _enemyInitializer.SetDirection(_landEnemies);
        }

        private void UpdateUI()
        {
            _uiContext.UpdateLevelTxt(_levelMap.CurrentLevel);
            _uiContext.UpdateLivesTxt(_player.Lives);
        }

        private void AddListeners()
        {
            _swipeService.OnSwipe += _player.SetDirection;

            _uiContext.SetButtonPauseListener(_gameContext.TapToPauseButton);

            _levelMap.OnLevelChanged += _uiContext.UpdateLevelTxt;

            _player.OnLivesChanged += _uiContext.UpdateLivesTxt;
        }
    }
}