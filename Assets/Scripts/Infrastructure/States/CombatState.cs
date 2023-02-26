using GameData;
using GameLogic;
using UnityEngine;

namespace Infrastructure
{
    public class CombatState : IState
    {
        private StateMachine _stateMachine;

        private ISwipeService _swipeService;

        private GameContext _gameContext;
        private UiContext _uiContext;

        private EnemyInitializer _enemyInitializer;
        private Player _player;
        private Field _field;
        private MonoBehaviour _monoBehaviour;

        public CombatState(StateMachine stateMachine,
            ISwipeService swipeService,
            GameContext gameContext, UiContext uiContext, MonoBehaviour monoBehaviour)
        {
            _stateMachine = stateMachine;

            _swipeService = swipeService;

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
            _gameContext.SetPauseGame(true);
            _gameContext.Timer.Pause();
            _swipeService.EnableSwipe(false);

            _gameContext.Player.IncreaseLives(-1);
            _gameContext.Field.FillTrace(TileType.Water);

            _uiContext.ShowGameMessage("Damage", Color.red);

            CheckEndGame();
        }

        private void CheckEndGame()
        {
            if (_gameContext.Player.Lives == 0)
            {
                Utilities.Invoke(_monoBehaviour, (() => { _stateMachine.EnterState<GameOverState>(); }),
                    _uiContext.DurationGameMessage);
            }
            else
            {
                _gameContext.Player.SetDirection(Vector2Int.zero);
                _gameContext.Player.SetPosition(Vector2Int.zero);

                Utilities.Invoke(_monoBehaviour, (() => { _stateMachine.EnterState<GameState>(); }),
                    _uiContext.DurationGameMessage);
            }
        }
    }
}