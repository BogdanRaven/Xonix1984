using GameData;
using GameLogic;
using UnityEngine;

namespace Infrastructure
{
    public class GameOverState : IState
    {
        private StateMachine _stateMachine;

        private ISwipeService _swipeService;

        private GameContext _gameContext;
        private UiContext _uiContext;
        private GameConfig _gameConfig;
        private LevelMap _levelMap;

        private MonoBehaviour _monoBehaviour;

        public GameOverState(StateMachine stateMachine,
            ISwipeService swipeService, GameConfig gameConfig, LevelMap levelMap,
            GameContext gameContext, UiContext uiContext, MonoBehaviour monoBehaviour)
        {
            _stateMachine = stateMachine;

            _swipeService = swipeService;

            _gameContext = gameContext;
            _gameConfig = gameConfig;
            _levelMap = levelMap;
            _uiContext = uiContext;
            _monoBehaviour = monoBehaviour;
        }

        public void Exit()
        {
            _uiContext.DisableGameMessage();
        }

        public void Enter()
        {
            _gameContext.Timer.StopTimer();

            _gameContext.SetPauseGame(true);

            _swipeService.EnableSwipe(false);

            _levelMap.SetLevel(_gameConfig.StartLevel);
            //show game over
            _uiContext.ShowGameMessage("GameOver", Color.red);

            Utilities.Invoke(_monoBehaviour, (() => { _stateMachine.EnterState<StartState>(); }),
                _uiContext.DurationGameMessage);
        }
    }
}