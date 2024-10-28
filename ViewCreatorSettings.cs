using System;

namespace ECS.Modules.Exerussus.ViewCreator
{
    [Serializable]
    public class ViewCreatorSettings
    {
        public AddressableInfo[] AddressableInfos;
    }
    
    [Serializable]
    public struct AddressableInfo
    {
        public string name;
        public string path;
    }
}