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
        public  int GetCurCanForgeCountByLeavl(int level)
        {
            for (int i = 0; i < this.list.Count; i++)
            {
                if (level >=this.list[i].NeedLevel)
                    return i;
                 
            }

            return 1;
        }
    }
}