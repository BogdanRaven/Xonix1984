using GameLogic;
using UnityEngine;

namespace Infrastructure
{
    public class GameState : IState
    {
        private StateMachine _stateMachine;

        private ISwipeService _swipeService;

        private GameContext _gameContext;
        private UiContext _uiContext;

        public GameState(StateMachine stateMachine,
            ISwipeService swipeService,
            GameContext gameContext, UiContext uiContext)
        {
            _stateMachine = stateMachine;

            _swipeService = swipeService;

            _gameContext = gameContext;
            _uiContext = uiContext;
        }

        public void Exit()
        {
        }

        public void Enter()
        {
            _gameContext.SetPauseGame(false);
            _gameContext.Timer.Resume();
            _swipeService.EnableSwipe(true);
            _uiContext.DisableGameMessage();
        }
    }
}