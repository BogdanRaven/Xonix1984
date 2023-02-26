using XonixFactory;
using Zenject;

namespace Infrastructure.Installers
{
    public class FactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindTileFactory();
        }

        private void BindTileFactory()
        {
            Container.Bind<ITileFactory>().To<TileFactory>().AsSingle();
        }
    }
}
