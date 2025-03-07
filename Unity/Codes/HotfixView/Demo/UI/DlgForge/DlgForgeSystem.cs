using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof (DlgForge))]
    public static class DlgForgeSystem
    {
        public static void RegisterUIEvent(this DlgForge self)
        {
            self.RegisterCloseEvent<DlgBag>(self.View.E_CloseButton);
            ResourcesComponent.Instance.LoadBundle("icons.unity3d");
            self.saIcon = ResourcesComponent.Instance.GetAsset("icons.unity3d", "Icons") as SpriteAtlas;

            self.listMakeQueue.Add(self.View.ES_MakeQueueOne);
            self.listMakeQueue.Add(self.View.ES_MakeQueueTwo);

            self.View.E_ProductionLoopVerticalScrollRect.AddItemRefreshListener(self.OnLoopItemRefreshHandler);
        }

        public static void ShowWindow(this DlgForge self, Entity contextData = null)
        {
        }

        public static void BeforeUnload(this DlgForge self)
        {
            self.RemoveUIScrollItems(ref self.ScrollItems);
        }

        public static void OnLoopItemRefreshHandler(this DlgForge self, Transform transform, int index)
        {
            Scroll_Item_production scrollItem = self.ScrollItems[index].BindTrans(transform);

            var config = ForgeProductionConfigCategory.Instance.Get(index);
            scrollItem.ES_EquipItem.RefreshUI(config.ItemConfigId, self.saIcon);

            var itemConfig = ItemConfigCategory.Instance.Get(config.ItemConfigId);
            scrollItem.E_ItemNameText.SetText(itemConfig.Name);
            var numConfig = PlayerNumericConfigCategory.Instance.Get(config.ConsumId);
            scrollItem.E_ConsumeTypeText.SetText(numConfig.Name);
            
            scrollItem.E_ConsumeCountText.SetText(config.ConsumeCount.ToString());
            
            EUIHelper.AddListenerAsync(scrollItem.E_MakeButton, async () =>
            {

                
                await ETTask.CompletedTask;
            });
            
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
        }

        public static void OnTopToggleSelectedHandler(this DlgForge self, int index)
        {
        }
    }
}