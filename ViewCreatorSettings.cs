using System;

namespace Exerussus.EasyEcsModules.ViewCreator
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