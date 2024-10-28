using System;
using ECS.Modules.Exerussus.ViewCreator.MonoBehaviours;
using Exerussus._1EasyEcs.Scripts.Core;
using Exerussus._1EasyEcs.Scripts.Custom;
using Exerussus._1OrganizerUI.Scripts.Pooling;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ECS.Modules.Exerussus.ViewCreator
{
    public class ViewCreatorPooler : IGroupPooler
    {
        public void Initialize(EcsWorld world)
        {
            _world = world;
            AssetLoadingMark = new PoolerModule<ViewCreatorData.AssetLoadingMark>(world);
            AssetViewApi = new PoolerModule<ViewCreatorData.AssetViewApi>(world);

            if (Settings.PathsInfos is { Count: > 0 })
            {
                foreach (var addressableInfo in Settings.PathsInfos) _assetPooler.Initialize(addressableInfo.name, addressableInfo.path);
            }
            if (Settings.ReferencesInfos is { Count: > 0 })
            {
                foreach (var addressableInfo in Settings.ReferencesInfos) _assetPooler.Initialize(addressableInfo.name, addressableInfo.reference);
            }
        }

        private EcsWorld _world;
        private readonly AssetPool<AssetViewApi> _assetPooler = new();
        [InjectSharedObject] public ViewCreatorSettings Settings { get; private set; }
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
        
        public void CreateView(int entity, string assetName, AssetReference assetReference, Vector3 position, Action<AssetViewApi> execute)
        {
            AssetLoadingMark.Add(entity);
            var packedEntity = _world.PackEntity(entity);
            
            _assetPooler.GetAndExecute(assetName, assetReference, position, api =>
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
        
        public void CreateEntityView(string assetName, AssetReference assetReference, Vector3 position, Action<AssetViewApi, int> execute)
        {
            var newEntity = _world.NewEntity();
            var packedEntity = _world.PackEntity(newEntity);
            AssetLoadingMark.Add(newEntity);
            _assetPooler.GetAndExecute(assetName, assetReference, position, api =>
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

        public void ReleaseView(int entity)
        {
            if (!AssetViewApi.Has(entity)) return;
            
            ref var apiData = ref AssetViewApi.Get(entity);
            apiData.Value?.Release();
            AssetViewApi.Del(entity);
        }
    }
}