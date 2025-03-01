using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace ET
{
    public partial class EntryConfigCategory
    {
        /// <summary>
        /// 词条的类型，【  词条的等级  】
        /// </summary>
        public Dictionary<int, MultiMap<int, EntryConfig>> dicMmEntryConfig = new();

        public override void AfterEndInit()
        {
            base.AfterEndInit();

            foreach (var config in this.dict)
            {
                //没有该词条类型

                MultiMap<int, EntryConfig> mmConfig;
                if (!dicMmEntryConfig.ContainsKey((int)config.Value.EntryType))
                    this.dicMmEntryConfig.Add((int)config.Value.EntryType, new MultiMap<int, EntryConfig>());

                mmConfig = this.dicMmEntryConfig[(int)config.Value.EntryType];

                mmConfig.Add((int)config.Value.EntryLevel, config.Value);
            }
        }

        /// <summary>
        /// 根据词条的类型及其等级获得表格
        /// </summary>
        public EntryConfig GetRandomConfigByEqpAffTypeAndLevel(EquipmentAffixesType type, int level)
        {
            EntryConfig config;
            try
            {
                MultiMap<int, EntryConfig> mmConfig = this.dicMmEntryConfig[(int)type];

                int maxCout = mmConfig[level].Count;
                int randIdx = RandomHelper.RandomNumber(0, maxCout);
                config = mmConfig[level][randIdx];
            }
            catch (Exception e)
            {
                Log.Error("获取表格失败：" + type.ToString() + level);
                config = null;
            }

            return config;
        }
    }
}