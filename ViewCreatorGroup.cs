using Exerussus._1EasyEcs.Scripts.Core;
using Exerussus._1EasyEcs.Scripts.Custom;
using Leopotam.EcsLite;
using UnityEngine.AddressableAssets;

namespace ECS.Modules.Exerussus.ViewCreator
{
    public class ViewCreatorGroup : EcsGroup<ViewCreatorPooler>
    {
        public ViewCreatorSettings Settings = new();

        protected override void SetSharingData(EcsWorld world, GameShare gameShare)
        {
            GameShare.AddSharedObject(Settings);
        }

        public ViewCreatorGroup AddPreloadAsset(string name, AssetReference reference)
        {
            Settings.ReferencesInfos.Add(new AddressableReferencesInfo { name = name, reference = reference });
            return this;
        }

        public ViewCreatorGroup AddPreloadAsset(string name, string path)
        {
            Settings.PathsInfos.Add(new AddressablePathsInfo { name = name, path = path });
            return this;
        }
    }
}