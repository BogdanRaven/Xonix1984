using GameLogic;
using UnityEngine;

namespace Infrastructure
{
    public class VictoryGameState : IState
    {
        private StateMachine _stateMachine;

        private LevelMap _levelMap;
        private ISwipeService _swipeService;

        private GameContext _gameContext;
        private UiContext _uiContext;
        private MonoBehaviour _monoBehaviour;

        public VictoryGameState(StateMachine stateMachine,
            ISwipeService swipeService, LevelMap levelMap,
            GameContext gameContext, UiContext uiContext, MonoBehaviour monoBehaviour)
        {
            _stateMachine = stateMachine;

            _swipeService = swipeService;

            _levelMap = levelMap;
            _gameContext = gameContext;
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

            int newLevel = _levelMap.CurrentLevel + 1;
            _levelMap.SetLevel(newLevel);

            _uiContext.ShowGameMessage("Victory", Color.green);
            Utilities.Invoke(_monoBehaviour, (() => { _stateMachine.EnterState<StartState>(); }),
                _uiContext.DurationGameMessage);
        }
    }
}