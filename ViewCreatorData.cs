using Exerussus._1EasyEcs.Scripts.Core;

namespace Exerussus.EasyEcsModules.ViewCreator
{
    public static class ViewCreatorData
    {
        public struct AssetLoadingMark : IEcsComponent
        {  }
        public struct AssetViewApi : IEcsComponent { public Exerussus.EasyEcsModules.ViewCreator.MonoBehaviours.AssetViewApi Value; }
    }
}