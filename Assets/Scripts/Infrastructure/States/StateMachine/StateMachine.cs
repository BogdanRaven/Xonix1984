using System;
using System.Collections.Generic;
using GameData;
using GameLogic;
using UnityEngine;
using XonixFactory;

namespace Infrastructure
{
    public class StateMachine : IStateMachine
    {
        private Dictionary<Type, IExitableState> States;
        private IExitableState Current { get; set; }

        public StateMachine(GameConfig gameConfig, LevelMap levelMap, ISwipeService swipeService,
            IMovementService movementService, ITileFactory tileFactory,
            GameContext gameContext, UiContext uiContext, MapRenderer mapRenderer, MonoBehaviour monoBehaviour)
        {
            States = new Dictionary<Type, IExitableState>();

            //register
            States[typeof(StartState)] = new StartState(this, gameConfig, levelMap, swipeService, movementService,
                tileFactory, gameContext, uiContext, mapRenderer, monoBehaviour);
            States[typeof(GameState)] = new GameState(this, swipeService, gameContext, uiContext);
            States[typeof(PauseState)] = new PauseState(this, swipeService, gameContext, uiContext);
            States[typeof(CombatState)] = new CombatState(this, swipeService, gameContext, uiContext, monoBehaviour);
            States[typeof(GameOverState)] = new GameOverState(this, swipeService, gameConfig, levelMap, gameContext,
                uiContext, monoBehaviour);
            States[typeof(VictoryGameState)] =
                new VictoryGameState(this, swipeService, levelMap, gameContext, uiContext,
                    monoBehaviour);
        }

        public void EnterState<TState>() where TState : IExitableState
        {
            Current?.Exit();
            var state = States[typeof(TState)];
            (state as IState).Enter();
            Current = state;
        }
    }
}