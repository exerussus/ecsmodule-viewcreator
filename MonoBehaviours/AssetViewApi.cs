﻿using System;
using Exerussus._1OrganizerUI.Scripts.Pooling;
using UnityEngine;

namespace ECS.Modules.Exerussus.ViewCreator.MonoBehaviours
{
    public class AssetViewApi : MonoBehaviour, ILoadAsset
    {
        public string assetName;
        public string AssetName => assetName;
        public Action Release { get; set; }
    }
}