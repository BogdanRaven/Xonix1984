using GameLogic;
using UnityEngine;
using Zenject;

public class PlayerInputInstaller : MonoInstaller
{
    [SerializeField] private SwipeService _swipeServicePrefab;
    public override void InstallBindings()
    {
        BindPlayerSwipeService();
    }

    private void BindPlayerSwipeService()
    {
        Container.Bind<ISwipeService>().To<SwipeService>().FromComponentInNewPrefab(_swipeServicePrefab).AsSingle();
    }
}
