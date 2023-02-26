using GameLogic;
using UnityEngine;

namespace Infrastructure
{
    public class PauseState : IState
    {
        private StateMachine _stateMachine;

        private ISwipeService _swipeService;
        private GameContext _gameContext;
        private UiContext _uiContext;

        public PauseState(StateMachine stateMachine,
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
            _gameContext.SetPauseGame(false);
            _swipeService.EnableSwipe(true);
            _gameContext.Timer.Resume();
            _uiContext.DisableGameMessage();
        }

        public void Enter()
        {
            _gameContext.SetPauseGame(true);
            _gameContext.Timer.Pause();
            _swipeService.EnableSwipe(false);

            _uiContext.ShowGameMessage("Pause", Color.magenta);
        }
    }
}