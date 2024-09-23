
using Exerussus._1EasyEcs.Scripts.Core;
using Exerussus._1OrganizerUI.Scripts.Pooling;
using Exerussus.EasyEcsModules.ViewCreator.MonoBehaviours;
using Leopotam.EcsLite;
using UnityEngine;

namespace Exerussus.EasyEcsModules.ViewCreator.System
{
    public class ViewCreatorSystem : EcsSignalListener<ViewCreatorPooler, 
        ViewCreatorSignals.CommandCreateView,
    ViewCreatorSignals.CommandCreateEntityView>
    {
        private readonly AssetPool<AssetViewApi> _assetPooler = new();
        private ViewCreatorSettings _settings;

        protected override void Initialize()
        {
            GameShare.GetSharedObject(ref _settings);
            
            if (_settings.AddressableInfos == null || _settings.AddressableInfos.Length == 0) return;

            foreach (var addressableInfo in _settings.AddressableInfos) _assetPooler.Initialize(addressableInfo.name, addressableInfo.path);
        }
        
        protected override void OnSignal(ViewCreatorSignals.CommandCreateEntityView data)
        {
            var newEntity = World.NewEntity();
            var packedEntity = World.PackEntity(newEntity);
            Pooler.AssetLoadingMark.Add(newEntity);
            _assetPooler.GetAndExecute(data.AssetName, data.AddressablePath, data.Position, api =>
            {
                if (packedEntity.Unpack(World, out var entity))
                {
                    ref var newApi = ref Pooler.AssetViewApi.Add(entity);
                    newApi.Value = api;
                    Pooler.AssetLoadingMark.Del(entity);
                    newApi.Value.Release = () => _assetPooler.Release(api);
                    data.Execute(api, newEntity);
                }
                else
                {
                    _assetPooler.Release(api);
                }
            });
        }

        protected override void OnSignal(ViewCreatorSignals.CommandCreateView data)
        {
            if (data.PackedEntity.Unpack(World, out var unpackedEntity))
            {
                Pooler.AssetLoadingMark.Add(unpackedEntity);
            
                _assetPooler.GetAndExecute(data.AssetName, data.AddressablePath, data.Position, api =>
                {
                    if (data.PackedEntity.Unpack(World, out var entity))
                    {
                        ref var newApi = ref Pooler.AssetViewApi.Add(entity);
                        newApi.Value = api;
                        Pooler.AssetLoadingMark.Del(entity);
                        newApi.Value.Release = () => _assetPooler.Release(api);
                        data.Execute(api);
                    }
                    else
                    {
                        _assetPooler.Release(api);
                    }
                });
            }
        }
    }
}