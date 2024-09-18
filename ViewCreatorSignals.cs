using System;
using Exerussus.EasyEcsModules.ViewCreator.MonoBehaviours;
using Leopotam.EcsLite;
using UnityEngine;

namespace Exerussus.EasyEcsModules.ViewCreator
{
    public static class ViewCreatorSignals
    {
        public struct CommandInitializeView
        {
            public string AssetName;
            public string AddressablePath;
            public Action<AssetViewApi> Execute;
        }
        
        public struct CommandCreateEntityView
        {
            public string AssetName;
            public string AddressablePath;
            public Vector3 Position;
            public Action<AssetViewApi, int> Execute;
        }
        
        public struct CommandCreateView
        {
            public EcsPackedEntity PackedEntity;
            public string AssetName;
            public string AddressablePath;
            public Vector3 Position;
            public Action<AssetViewApi> Execute;
        }
    }
}