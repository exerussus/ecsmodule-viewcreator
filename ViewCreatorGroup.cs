using Exerussus._1EasyEcs.Scripts.Core;
using Exerussus._1EasyEcs.Scripts.Custom;
using Leopotam.EcsLite;

namespace Exerussus.EasyEcsModules.ViewCreator
{
    public class ViewCreatorGroup : EcsGroup<ViewCreatorPooler>
    {
        public ViewCreatorSettings Settings = new();

        protected override void SetSharingData(EcsWorld world, GameShare gameShare)
        {
            GameShare.AddSharedObject(Settings);
        }
    }
}