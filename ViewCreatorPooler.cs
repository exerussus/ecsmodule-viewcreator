using System;
using Exerussus._1EasyEcs.Scripts.Core;
using Exerussus._1EasyEcs.Scripts.Custom;
using Exerussus._1OrganizerUI.Scripts.Pooling;
using Exerussus.EasyEcsModules.ViewCreator.MonoBehaviours;
using Leopotam.EcsLite;
using UnityEngine;

namespace Exerussus.EasyEcsModules.ViewCreator
{
    public class ViewCreatorPooler : IGroupPooler
    {
        public void BeforeInitialize(EcsWorld world, GameShare gameShare, GameContext gameContext, string groupName)
        {
            Settings = gameShare.GetSharedObject<ViewCreatorSettings>();
        }

        public void Initialize(EcsWorld world)
        {
            _world = world;
            AssetLoadingMark = new PoolerModule<ViewCreatorData.AssetLoadingMark>(world);
            AssetViewApi = new PoolerModule<ViewCreatorData.AssetViewApi>(world);
            
            if (Settings.AddressableInfos == null || Settings.AddressableInfos.Length == 0) return;

            foreach (var addressableInfo in Settings.AddressableInfos) _assetPooler.Initialize(addressableInfo.name, addressableInfo.path);
        }

        private EcsWorld _world;
        private readonly AssetPool<AssetViewApi> _assetPooler = new();
        public  ViewCreatorSettings Settings { get; private set; }
        public PoolerModule<ViewCreatorData.AssetLoadingMark> AssetLoadingMark { get; private set; }
        public PoolerModule<ViewCreatorData.AssetViewApi> AssetViewApi { get; private set; }

        public void CreateView(int entity, string assetName, string addressablePath, Vector3 position, Action<AssetViewApi> execute)
        {
            AssetLoadingMark.Add(entity);
            var packedEntity = _world.PackEntity(entity);
            
            _assetPooler.GetAndExecute(assetName, addressablePath, position, api =>
            {
                if (packedEntity.Unpack(_world, out var e))
                {
                    ref var newApi = ref AssetViewApi.Add(e);
                    newApi.Value = api;
                    AssetLoadingMark.Del(e);
                    newApi.Value.Release = () => _assetPooler.Release(api);
                    execute(api);
                }
                else _assetPooler.Release(api);
            });
        }
        
        public void CreateEntityView(string assetName, string addressablePath, Vector3 position, Action<AssetViewApi, int> execute)
        {
            var newEntity = _world.NewEntity();
            var packedEntity = _world.PackEntity(newEntity);
            AssetLoadingMark.Add(newEntity);
            _assetPooler.GetAndExecute(assetName, addressablePath, position, api =>
            {
                if (packedEntity.Unpack(_world, out var entity))
                {
                    ref var newApi = ref AssetViewApi.Add(entity);
                    newApi.Value = api;
                    AssetLoadingMark.Del(entity);
                    newApi.Value.Release = () => _assetPooler.Release(api);
                    execute(api, newEntity);
                }
                else _assetPooler.Release(api);
            });
        }
    }
}