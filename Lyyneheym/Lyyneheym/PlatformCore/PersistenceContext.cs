﻿using System;
using System.Collections.Generic;
using Yuri.Utils;

namespace Yuri.PlatformCore
{
    /// <summary>
    /// 持久类：为游戏提供持久性上下文，它不会被回滚和存读档影响
    /// </summary>
    internal static class PersistenceContext
    {
        /// <summary>
        /// 保存持久上下文到稳定储存器
        /// </summary>
        public static void SaveToSteadyMemory() => IOUtils.Serialization(PersistenceContext.symbols, GlobalDataContext.PersistenceFileName);

        /// <summary>
        /// 从稳定储存器将持久上下文读入内存
        /// </summary>
        public static void LoadFromSteadyMemory() => PersistenceContext.symbols =
            IOUtils.Unserialization(GlobalDataContext.PersistenceFileName) as Dictionary<string, object>;

        /// <summary>
        /// 从持久容器中取一个变量
        /// </summary>
        /// <param name="varName">变量名</param>
        /// <returns>变量的引用</returns>
        public static object Fetch(string varName) => PersistenceContext.symbols?[varName];

        /// <summary>
        /// 将一个变量放入持久容器中，如果指定变量名已存在，就覆盖原来的对象
        /// </summary>
        /// <param name="varName">变量名</param>
        /// <param name="varObj">要存入的对象</param>
        public static void Assign(string varName, object varObj) => PersistenceContext.symbols[varName] = varObj;
        
        /// <summary>
        /// 持久符号表
        /// </summary>
        private static Dictionary<string, object> symbols = new Dictionary<string, object>();
    }
}