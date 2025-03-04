using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof (DlgBag))]
    [FriendClassAttribute(typeof (ET.BagComponent))]
    public static class DlgBagSystem
    {
        public static void RegisterUIEvent(this DlgBag self)
        {
            self.RegisterCloseEvent<DlgBag>(self.View.E_CloseButton);
            self.View.E_TopButtonToggleGroup.AddListener(self.OnTopToggleSelectedHandler);
            self.View.E_PreviousButton.AddListener(self.OnPreviousPageHandler);
            self.View.E_NextButton.AddListener(self.OnNextPageHandler);

            self.View.E_BagItemsLoopVerticalScrollRect.AddItemRefreshListener(self.OnLoopItemRefreshHandler);
            ResourcesComponent.Instance.LoadBundle("icons.unity3d");
            self.saIcon = ResourcesComponent.Instance.GetAsset("icons.unity3d", "Icons") as SpriteAtlas;
        }

        public static void OnTopToggleSelectedHandler(this DlgBag self, int index)
        {
            self.curType = (ItemType)index;
            self.curPageIdx = 0;
            self.RefreshUI();
        }

        public static void OnNextPageHandler(this DlgBag self)
        {
            var bagCpt = self.ZoneScene().GetComponent<BagComponent>();
            int allItemTypeCount = bagCpt.GetCurBagItemCountByType(self.curType);
          
            if (self.curPageIdx + 1 > allItemTypeCount) return;
            ++self.curPageIdx;
            self.RefreshUI();
        }

        public static void OnPreviousPageHandler(this DlgBag self)
        {
            if (self.curPageIdx - 1 < 0) return;
            --self.curPageIdx;
            self.RefreshUI();
        }

        public static void RefreshUI(this DlgBag self)
        {
            //页数，
            var bagCpt = self.ZoneScene().GetComponent<BagComponent>();

            int allItemTypeCount = bagCpt.GetCurBagItemCountByType(self.curType);

            int allPage = Mathf.Max(1, (allItemTypeCount / self.onePageItemCount) + (allItemTypeCount % self.onePageItemCount == 0? 0 : 1));
            self.View.E_PageText.SetText($"{self.curPageIdx + 1}/{allPage}");
            //滚动列表刷新

            self.AddUIScrollItems(ref self.ScrollItemBagItems, allItemTypeCount);

            self.View.E_BagItemsLoopVerticalScrollRect.SetVisible(allItemTypeCount != 0, allItemTypeCount);
        }

        public static void OnLoopItemRefreshHandler(this DlgBag self, Transform transform, int index)
        {
            Scroll_Item_bagItem scrollItemBagItem = self.ScrollItemBagItems[index].BindTrans(transform);
            index = (self.curPageIdx * self.onePageItemCount) + index;
            //data    itemList[index]
            var bagCpt = self.ZoneScene().GetComponent<BagComponent>();
            Item item = bagCpt.mlItem[(int)self.curType][index];

            //ui  scrollItemBagItem
            scrollItemBagItem.E_IconImage.overrideSprite = self.saIcon.GetSprite(item.Config.Icon);
            scrollItemBagItem.E_QualityImage.color = item.ItemQualityColor();

            EUIHelper.AddListenerAsync(scrollItemBagItem.E_SelectButton,
                async () =>
                {
                    var uiCpt = self.ZoneScene().GetComponent<UIComponent>();
                    await uiCpt.ShowWindowAsync(WindowID.WindowID_ItemPopUp);
                    uiCpt.GetDlgLogic<DlgItemPopUp>().RefreshUI(item.Id, NetItemPut.Bag);
                });
        }

        public static void ShowWindow(this DlgBag self, Entity contextData = null)
        {
            self.View.E_WeaponToggle.IsSelected(true);
        }

        public static void HideWindow(this DlgBag self)
        {
            self.RemoveUIScrollItems(ref self.ScrollItemBagItems);
        }
    }
}