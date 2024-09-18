using Exerussus._1EasyEcs.Scripts.Core;
using Exerussus._1EasyEcs.Scripts.Custom;
using Leopotam.EcsLite;

namespace Exerussus.EasyEcsModules.ViewCreator
{
    public class ViewCreatorPooler : IGroupPooler
    {
        public void Initialize(EcsWorld world)
        {
            AssetLoadingMark = new PoolerModule<ViewCreatorData.AssetLoadingMark>(world);
            AssetViewApi = new PoolerModule<ViewCreatorData.AssetViewApi>(world);
        }

        public PoolerModule<ViewCreatorData.AssetLoadingMark> AssetLoadingMark { get; private set; }
        public PoolerModule<ViewCreatorData.AssetViewApi> AssetViewApi { get; private set; }
    }
}