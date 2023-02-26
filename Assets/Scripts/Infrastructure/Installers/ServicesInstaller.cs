using GameLogic;
using Zenject;

namespace Infrastructure.Installers
{
    public class ServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindMovementService();
        }

        private void BindMovementService()
        {
            Container.Bind<IMovementService>().To<MovementService>().AsSingle();
        }
    }
}