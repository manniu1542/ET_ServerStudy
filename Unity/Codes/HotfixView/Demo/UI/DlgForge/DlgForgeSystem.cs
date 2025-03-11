using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace ET
{
    [Timer(TimerType.SpawnForgeRemainTime)]
    public class SpawnForgeRemainTimeTimer: ATimer<DlgForge>
    {
        public override void Run(DlgForge self)
        {
            try
            {
                self?.RefreshMakeItemIntervalTimeUI();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    [FriendClass(typeof (DlgForge))]
    [FriendClassAttribute(typeof (ET.ForgeComponent))]
    [FriendClassAttribute(typeof (ET.ES_MakeQueue))]
    public static class DlgForgeSystem
    {
        public static void RegisterUIEvent(this DlgForge self)
        {
            self.RegisterCloseEvent<DlgForge>(self.View.E_CloseButton);
            ResourcesComponent.Instance.LoadBundle("icons.unity3d");
            self.saIcon = ResourcesComponent.Instance.GetAsset("icons.unity3d", "Icons") as SpriteAtlas;

            self.listMakeQueue.Add(self.View.ES_MakeQueueOne);
            self.listMakeQueue.Add(self.View.ES_MakeQueueTwo);

            self.View.E_ProductionLoopVerticalScrollRect.AddItemRefreshListener(self.OnLoopItemRefreshHandler);
        }

        public static void ShowWindow(this DlgForge self, Entity contextData = null)
        {
            self.RefreshUI();
        }

        public static void OnHideWindow(this DlgForge self)
        {
            TimerComponent.Instance.Remove(ref self.SpawnForgeRemainTimeID);
        }

        public static void BeforeUnload(this DlgForge self)
        {
            self.RemoveUIScrollItems(ref self.ScrollItems);
        }

        public static void OnLoopItemRefreshHandler(this DlgForge self, Transform transform, int index)
        {
            Scroll_Item_production scrollItem = self.ScrollItems[index].BindTrans(transform);

            var config = ForgeProductionConfigCategory.Instance.GetForgeConfigByIdx(index);
            scrollItem.ES_EquipItem.RefreshUI(config.ItemConfigId, self.saIcon);

            var itemConfig = ItemConfigCategory.Instance.Get(config.ItemConfigId);
            scrollItem.E_ItemNameText.SetText(itemConfig.Name);
            var numConfig = PlayerNumericConfigCategory.Instance.Get(config.ConsumId);
            scrollItem.E_ConsumeTypeText.SetText(numConfig.Name);

            scrollItem.E_ConsumeCountText.SetText(config.ConsumeCount.ToString());

            EUIHelper.AddListenerAsync(scrollItem.E_MakeButton, () => { return self.ReqMakeProduction(config.Id); });
        }

        public static void RefreshUI(this DlgForge self)
        {
            var numCpt = UnitHelper.GetMyUnitNumericComponent(self.ZoneScene().CurrentScene());
            self.View.E_IronStoneCountText.SetText(numCpt[NumericType.Ironstone].ToString());
            self.View.E_FurCountText.SetText(numCpt[NumericType.Leather].ToString());

            //刷新滚动列表
            int level = numCpt.GetAsInt(NumericType.Level);
            int count = ForgeProductionConfigCategory.Instance.GetCurCanForgeCountByLeavl(level);
            self.AddUIScrollItems(ref self.ScrollItems, count);
            self.View.E_ProductionLoopVerticalScrollRect.SetVisible(true, count);

            self.RefreshMakeItemUI();
        }

        public static void RefreshMakeItemUI(this DlgForge self)
        {
            var forgeCpt = self.ZoneScene().GetComponent<ForgeComponent>();
            int makeProCount = forgeCpt.listProductions.Count;

            for (var i = 0; i < self.listMakeQueue.Count; i++)
            {
                self.listMakeQueue[i].uiTransform.SetVisible(makeProCount > i);
                if (makeProCount > i)
                {
                    self.listMakeQueue[i].RefreshUI(forgeCpt.listProductions[i], self.saIcon);
                }
            }

            TimerComponent.Instance.Remove(ref self.SpawnForgeRemainTimeID);
            if (forgeCpt.IsNeedSpwanMakingTime())
                self.SpawnForgeRemainTimeID = TimerComponent.Instance.NewRepeatedTimer(1000, TimerType.SpawnForgeRemainTime, self);
        }

        public static void RefreshMakeItemIntervalTimeUI(this DlgForge self)
        {
            var forgeCpt = self.ZoneScene().GetComponent<ForgeComponent>();

            int makeProCount = forgeCpt.listProductions.Count;
            for (var i = 0; i < self.listMakeQueue.Count; i++)
            {
                self.listMakeQueue[i].uiTransform.SetVisible(makeProCount > i);
                if (makeProCount > i)
                {
                    self.listMakeQueue[i].IntervalRefreshUI(forgeCpt.listProductions[i]);
                    if (forgeCpt.listProductions[i].IsNeedReceive())
                    {
                        Game.EventSystem.Publish(new EventType.RefreshForgeRedPoint() { ZoneScene = self.ZoneScene() });
                    }
                }
            }

            if (!forgeCpt.IsNeedSpwanMakingTime())
            {
                TimerComponent.Instance.Remove(ref self.SpawnForgeRemainTimeID);
            }
        }

        public static async ETTask ReqMakeProduction(this DlgForge self, int configID)
        {
            //客户端先判断 有没有 位置了(当前 正在打造的 数量，当前等级可以打造的 数量)
            var isCanForge = self.ZoneScene().GetComponent<ForgeComponent>().IsCanForgeNewItem();
            if (!isCanForge) return;

            //判断材料是否 足够 
            var numCpt = UnitHelper.GetMyUnitNumericComponent(self.ZoneScene().CurrentScene());
            var config = ForgeProductionConfigCategory.Instance.Get(configID);
            long curConsumCount = numCpt[config.ConsumId];
            if (curConsumCount < config.ConsumeCount) return;
            //做请求
            M2C_ForgeItem m2c_ForgeItem;
            var gateSession = self.ZoneScene().GetComponent<SessionComponent>().Session;
            try
            {
                m2c_ForgeItem = await gateSession.Call(new C2M_ForgeItem() { ForgeProductionConfigId = configID }) as M2C_ForgeItem;
                if (m2c_ForgeItem.Error != ErrorCode.ERR_Success)
                {
                    Log.Error("请求 制作失败 错误码是：" + m2c_ForgeItem.Error);
                    return;
                }
                else
                {
                    self.ZoneScene().GetComponent<ForgeComponent>().AddProduction(m2c_ForgeItem.ForgeProInfo);
                    self.RefreshUI();
                }
            }
            catch (Exception e)
            {
                Log.Error("请求出错：" + e.ToString());
            }
        }
    }
}