using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace ECS.Modules.Exerussus.ViewCreator
{
    [Serializable]
    public class ViewCreatorSettings
    {
        public List<AddressablePathsInfo> PathsInfos = new();
        public List<AddressableReferencesInfo> ReferencesInfos = new();
    }
    
    [Serializable]
    public struct AddressablePathsInfo
    {
        public string name;
        public string path;
    }
    
    [Serializable]
    public struct AddressableReferencesInfo
    {
        public string name;
        public AssetReference reference;
    }
}