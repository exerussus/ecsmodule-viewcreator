using Exerussus._1EasyEcs.Scripts.Core;

namespace ECS.Modules.Exerussus.ViewCreator
{
    public static class ViewCreatorData
    {
        public struct AssetLoadingMark : IEcsComponent
        {  }
        public struct AssetViewApi : IEcsComponent { public MonoBehaviours.AssetViewApi Value; }
    }
}