using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class EquipInfoComponentAwakeSystem: AwakeSystem<EquipInfoComponent>
    {
        public override void Awake(EquipInfoComponent self)
        {
            self.CheckAffixes();
        }
    }

    [ObjectSystem]
    public class EquipInfoComponentDestroySystem: DestroySystem<EquipInfoComponent>
    {
        public override void Destroy(EquipInfoComponent self)
        {
        }
    }

    [ObjectSystem]
    public class EquipInfoComponentDeserializeSystem: DeserializeSystem<EquipInfoComponent>
    {
        public override void Deserialize(EquipInfoComponent self)
        {
            self.listAffixes.Clear();
            foreach (var tmp in self.Children)
            {
                var item = tmp.Value as EquipmentAffixes;
                if (item != null)
                {
                    self.listAffixes.Add(item);
                }
            }
        }
    }

    [FriendClass(typeof (EquipInfoComponent))]
    [FriendClassAttribute(typeof (ET.Item))]
    [FriendClassAttribute(typeof (ET.EquipmentAffixes))]
    public static class EquipInfoComponentSystem
    {
        /// <summary>
        /// 检查自身的词条情况（没有则生成）
        /// </summary>
        /// <param name="self"></param>
        public static void CheckAffixes(this EquipInfoComponent self)
        {
            if (self.isCreateAffixes) return;
            self.isCreateAffixes = true;
            self.ResetAffixes();
            self.GenerateAffixes();
        }

        public static void ResetAffixes(this EquipInfoComponent self)
        {
            self.score = 0;
            foreach (var aff in self.listAffixes)
            {
                aff.Dispose();
            }

            self.listAffixes.Clear();
        }

        /// <summary>
        /// 生成所有小词条     #(根据词条等级以及词条类型来随机词条的)
        /// </summary>
        /// <param name="self"></param>
        /// <param name="configId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static void GenerateAffixes(this EquipInfoComponent self)
        {
            var item = self.GetParent<Item>();
            //最小普通词条数量
            int minNormlCount = (int)item.quality;
            //获取关于词条的 随机 配置表
            var entryRandomConfig = EntryRandomConfigCategory.Instance.Get(item.Config.EntryRandomId);

            EntryConfig config;
            EquipmentAffixes equipmentAffixes;
            //生成普通词条
            int normalCount = minNormlCount + RandomHelper.RandomNumber(entryRandomConfig.NormalRandMinCount, entryRandomConfig.NormalRandMaxCount);
            for (int i = 0; i < normalCount; i++)
            {
                config = EntryConfigCategory.Instance.GetRandomConfigByEqpAffTypeAndLevel(EquipmentAffixesType.Normal, entryRandomConfig.NormalLevel);
                equipmentAffixes = self.AddChild<EquipmentAffixes>();
                equipmentAffixes.type = (EquipmentAffixesType)config.EntryType;
                equipmentAffixes.numType = config.AttributeType;
                equipmentAffixes.numValue = RandomHelper.RandomNumber(config.AttributeMinValue, config.AttributeMaxValue);
                self.score += config.EntryScore;
                self.listAffixes.Add(equipmentAffixes);
            }

            //生成特殊词条
            int specialCount = RandomHelper.RandomNumber(entryRandomConfig.SpecialRandMinCount, entryRandomConfig.SpecialRandMaxCount);
            for (int i = 0; i < specialCount; i++)
            {
                config = EntryConfigCategory.Instance.GetRandomConfigByEqpAffTypeAndLevel(EquipmentAffixesType.Special,
                    entryRandomConfig.SpecialEntryLevel);
                equipmentAffixes = self.AddChild<EquipmentAffixes>();
                equipmentAffixes.type = (EquipmentAffixesType)config.EntryType;
                equipmentAffixes.numType = config.AttributeType;
                equipmentAffixes.numValue = RandomHelper.RandomNumber(config.AttributeMinValue, config.AttributeMaxValue);
                self.score += config.EntryScore;
                self.listAffixes.Add(equipmentAffixes);
            }
        }

        /// <summary>
        /// 转换消息数据
        /// </summary>
        /// <param name="self"></param>
        /// <param name="configId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static EquipInfo ToMsgData(this EquipInfoComponent self)
        {
            EquipInfo equipInfo = new EquipInfo();
            equipInfo.Score = self.score;
            equipInfo.IsCreateAffixes = self.isCreateAffixes;

            if (self.isCreateAffixes)
            {
                EquipmentAffixesInfo equipmentAffixesInfo;
                for (int i = 0; i < self.listAffixes.Count; i++)
                {
                    equipmentAffixesInfo = self.listAffixes[i].ToMsgData();
                    equipInfo.EquipmentAffixesInfos.Add(equipmentAffixesInfo);
                }
            }

            return equipInfo;
        }
    }
}