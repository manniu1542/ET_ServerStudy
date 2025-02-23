using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof (DlgBag))]
    public static class DlgBagSystem
    {
        public static void RegisterUIEvent(this DlgBag self)
        {
            self.RegisterCloseEvent<DlgBag>(self.View.E_CloseButton);
            self.View.E_TopButtonToggleGroup.AddListener(self.OnTopToggleSelectedHandler);
            self.View.E_PreviousButton.AddListener(self.OnPreviousPageHandler);
            self.View.E_NextButton.AddListener(self.OnNextPageHandler);

            self.View.E_BagItemsLoopVerticalScrollRect.AddItemRefreshListener(self.OnLoopItemRefreshHandler);
        }

        public static void OnTopToggleSelectedHandler(this DlgBag self, int index)
        {
            self.curType = (ItemType)index;
            self.curPageIdx = 0;
            self.RefreshUI();
        }

        public static void OnNextPageHandler(this DlgBag self)
        {
            int allItemTypeCount = 10;
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
            int allItemTypeCount = 10;
            int allPage = Mathf.Min(1, (allItemTypeCount / self.onePageItemCount) + (allItemTypeCount % self.onePageItemCount == 0? 0 : 1));
            self.View.E_PageText.SetText($"{self.curPageIdx + 1}/{allPage}");
            //滚动列表刷新
            self.AddUIScrollItems(ref self.ScrollItemBagItems, self.onePageItemCount);
            self.View.E_BagItemsLoopVerticalScrollRect.SetVisible(true, self.onePageItemCount);
        }

        public static void OnLoopItemRefreshHandler(this DlgBag self, Transform transform, int index)
        {
            List<Item> itemList = null;
            Scroll_Item_bagItem scrollItemBagItem = self.ScrollItemBagItems[index].BindTrans(transform);
            index = (self.curPageIdx * self.onePageItemCount) + index;
            //data    itemList[index]

            //ui  scrollItemBagItem
            // scrollItemBagItem.E_IconImage =
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