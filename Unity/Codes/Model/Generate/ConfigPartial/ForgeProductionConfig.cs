using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    public partial class ForgeProductionConfigCategory
    {
        /// <summary>
        /// 获取对应等级可以打造表格的数量
        /// </summary>
        public int GetCurCanForgeCountByLeavl(int level)
        {
            int count = 0;
            for (int i = 0; i < this.list.Count; i++)
            {
                if (level >= this.list[i].NeedLevel)
                    count++;
            }

            return count;
        }

        public ForgeProductionConfig GetForgeConfigByIdx(int idx)
        {
            if (this.list.Count > idx)
            {
                return this.list[idx];
            }

            return null;
        }
    }
}