using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameData;
using Infrastructure;
using UnityEngine;

namespace GameLogic
{
    public class GameContext : MonoBehaviour
    {
        private IStateMachine _stateMachine;
        private Field _field;
        private Player _player;
        private Timer _timer;
        private MapRenderer _mapRenderer;
        private UiContext _uiContext;

        private IMovementService _movementService;
        private List<Enemy> _waterEnemies;
        private List<Enemy> _landEnemies;

        public Player Player => _player;

        public Field Field => _field;

        public Timer Timer => _timer;

        public bool IsPause { get; private set; }

        public void Context(IStateMachine stateMachine, Field field, Player player, List<Enemy> waterEnemies,
            List<Enemy> landEnemies, IMovementService movementService, MapRenderer mapRenderer, UiContext uiContext,
            Timer timer)
        {
            _stateMachine = stateMachine;

            _movementService = movementService;
            _mapRenderer = mapRenderer;
            _uiContext = uiContext;

            _field = field;
            _player = player;
            _timer = timer;

            _waterEnemies = waterEnemies;
            _landEnemies = landEnemies;
        }

        public void StartGameCore()
        {
            StartCoroutine(GameCore());
        }

        public void SetPauseGame(bool isPause)
        {
            IsPause = isPause;
        }

        public void Update()
        {
            _timer.Update();
        }

        private IEnumerator GameCore()
        {
            if (IsPause)
            {
                yield return new WaitForSeconds(0.6f);
            }
            else
            {
                yield return new WaitForSeconds(0.05f);
                //Move
                _movementService.MoveObject(_player, _field);
                MoveEnemies(_waterEnemies);
                MoveEnemies(_landEnemies);

                //Draw
                _mapRenderer.DrawMainTileMap();
                _mapRenderer.DrawPlayerTileMap();
                _mapRenderer.DrawEnemiesTileMap();

                //SetTrace
                SetPlayerTrace(_player);

                //TryFillArea
                if (TryFillTraceArea())
                {
                    if (HasPlayerWin())
                    {
                        _stateMachine.EnterState<VictoryGameState>();
                        yield return GameCore();
                    }
                }

                //Check hit
                if (DetectCollisionPlayer())
                {
                    _stateMachine.EnterState<CombatState>();
                    yield return GameCore();
                }

                _uiContext.UpdateTime((int) (_timer.Duration - _timer.ElapsedTime));
            }

            yield return GameCore();
        }

        private bool TryFillTraceArea()
        {
            if (_player.InSafeArea || _field.GetTile(_player.Position) != TileType.Land) return false;
            
            _player.SetSafeArea(true);
            _player.SetDirection(Vector2Int.zero);
            _field.FillTraceAre();
            DestroyEnemiesInArea(_waterEnemies, TileType.Land);
            
            return true;

        }

        private void SetPlayerTrace(IMovable player)
        {
            //SetTrack
            if (_field.GetTile(_player.Position) != TileType.Water) return;

            _player.SetSafeArea(false);
            _field.SetTile(_player.Position, TileType.Trace);
        }

        private void DestroyEnemiesInArea(List<Enemy> enemies, TileType tileArea)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (!_field.MovableObjectInArea(enemies[i], tileArea)) continue;
                enemies.Remove(enemies[i]);
                i--;
            }
        }

        private bool HasPlayerWin()
        {
            return _field.HasMaxPercentLand() || _waterEnemies.Count == 0;
        }

        private void MoveEnemies(List<Enemy> enemies)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.EnemyData.EnemyType == EnemyType.Water)
                {
                    _movementService.MoveObjectWithReflection(enemy, TileType.Land, _field);
                }

                if (enemy.EnemyData.EnemyType == EnemyType.Land)
                {
                    _movementService.MoveObjectWithReflection(enemy, TileType.Water, _field);
                }
            }
        }

        public void TapToPauseButton()
        {
            if (IsPause == false)
            {
                _stateMachine.EnterState<PauseState>();
            }
            else
            {
                _stateMachine.EnterState<GameState>();
            }
        }

        private bool DetectCollisionPlayer()
        {
            if (_player.InSafeArea)
            {
                return _landEnemies.Any(enemy => DetectCollision(_player, enemy));
            }
            else
            {
                return _waterEnemies.Any(enemy => DetectCollision(_player, enemy));
            }
        }

        private bool DetectCollision(IMovable player, IMovable movableObject)
        {
            var nextMovableObjectPosition = movableObject.Position + movableObject.Direction;
            var nextPlayerPosition = player.Position + player.Direction;

            if (_field.GetTile(nextMovableObjectPosition) == TileType.Trace ||
                (nextMovableObjectPosition) == (nextPlayerPosition))
                return true;
            if ((nextMovableObjectPosition) == player.Position || movableObject.Position == player.Position)
                return true;
            if (_field.GetTile(nextPlayerPosition) == TileType.Trace)
                return true;

            return false;
        }
    }
}