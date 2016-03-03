﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yuri.YuriHalation.ScriptPackage
{
    /// <summary>
    /// 游戏设置类
    /// </summary>
    [Serializable]
    class ConfigPackage
    {
        /// <summary>
        /// 开关数量
        /// </summary>
        public int MaxSwitchCount = 100;

        /// <summary>
        /// 文字层数量
        /// </summary>
        public int MaxMessageLayer = 10;

        /// <summary>
        /// BGS轨道数量
        /// </summary>
        public int BgsCount = 10;
    }
}
