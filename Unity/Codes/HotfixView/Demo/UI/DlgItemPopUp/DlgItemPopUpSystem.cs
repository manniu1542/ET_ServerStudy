using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof (DlgItemPopUp))]
    [FriendClassAttribute(typeof (ET.EquipInfoComponent))]
    [FriendClassAttribute(typeof (ET.EquipmentAffixes))]
    [FriendClassAttribute(typeof (ET.Scroll_Item_entry))]
    public static class DlgItemPopUpSystem
    {
        public static void RegisterUIEvent(this DlgItemPopUp self)
        {
            self.RegisterCloseEvent<DlgItemPopUp>(self.View.E_CloseButton);

            EUIHelper.AddListenerAsync(self.View.E_SellButton, self.OnSell);
            EUIHelper.AddListenerAsync(self.View.E_UnEquipButton, self.OnUnloadItem);
            EUIHelper.AddListenerAsync(self.View.E_EquipButton, self.OnDressUpItem);
            ResourcesComponent.Instance.LoadBundle("icons.unity3d");
            self.saIcon = ResourcesComponent.Instance.GetAsset("icons.unity3d", "Icons") as SpriteAtlas;

            self.View.E_EntrysLoopVerticalScrollRect.AddItemRefreshListener(self.OnLoopItemRefreshHandler);
        }

        public static void OnLoopItemRefreshHandler(this DlgItemPopUp self, Transform transform, int index)
        {
            Scroll_Item_entry scrollItemEntryItem = self.dicScroll_Item_entry[index].BindTrans(transform);
            //data   
            EquipInfoComponent equpCpt = self.item.GetComponent<EquipInfoComponent>();
            var eqpAff = equpCpt.listAffixes[index];
            //ui  
            var config = PlayerNumericConfigCategory.Instance.Get(eqpAff.numType);
            scrollItemEntryItem.E_EntryNameText.SetText(config.Name);
            scrollItemEntryItem.E_EntryValueText.SetText(config.isPrecent == 1? (eqpAff.numValue / 100_00).ToString("0.00") + "%"
                    : eqpAff.numValue.ToString());
            scrollItemEntryItem.uiTransform.GetComponent<UnityEngine.UI.Image>().color =
                    eqpAff.type == EquipmentAffixesType.Normal? Color.green : Color.red;
        }

        public static async ETTask OnSell(this DlgItemPopUp self)
        {
            bool isFinish = await ItemUseHelper.SellItem(self.ZoneScene(), self.item);
            if (isFinish)
            {
                self.ZoneScene().GetComponent<UIComponent>().GetDlgLogic<DlgBag>()?.RefreshUI();
                //关闭页面
                self.View.E_CloseButton.onClick.Invoke();
            }
        }

        public static async ETTask OnDressUpItem(this DlgItemPopUp self)
        {
            bool isFinish = await ItemUseHelper.DressUpItem(self.ZoneScene(), self.item);
            if (isFinish)
            {
                self.ZoneScene().GetComponent<UIComponent>().GetDlgLogic<DlgRoleInfo>()?.RefreshUI();
                self.ZoneScene().GetComponent<UIComponent>().GetDlgLogic<DlgBag>()?.RefreshUI();
                //关闭页面
                self.View.E_CloseButton.onClick.Invoke();
            }
        }

        public static async ETTask OnUnloadItem(this DlgItemPopUp self)
        {
            int roleItemPos = self.item.Config.EquipPosition; 
            bool isFinish = await ItemUseHelper.UnloadItem(self.ZoneScene(), roleItemPos);
            if (isFinish)
            {
                self.ZoneScene().GetComponent<UIComponent>().GetDlgLogic<DlgRoleInfo>()?.RefreshUI();
                self.ZoneScene().GetComponent<UIComponent>().GetDlgLogic<DlgBag>()?.RefreshUI();
                //关闭页面
                self.View.E_CloseButton.onClick.Invoke();
            }
        }

        public static void ShowWindow(this DlgItemPopUp self, Entity contextData = null)
        {
        }

        public static void RefreshUI(this DlgItemPopUp self, long itemId, NetItemPut type)
        {
            self.item = ItemHelper.GetItem(self.ZoneScene(), itemId, type);

            self.View.E_EquipButton.SetVisible(type == NetItemPut.Bag && self.item.Config.EquipPosition != (int)RoleEuipPosType.None);
            self.View.E_UnEquipButton.SetVisible(type == NetItemPut.Role);

            self.View.E_NameText.SetText(self.item.Config.Name);
            self.View.E_IconImage.sprite = self.saIcon.GetSprite(self.item.Config.Icon);
            self.View.E_QualityImage.color = self.item.ItemQualityColor();
            self.View.E_DescText.SetText(self.item.Config.Desc);
            self.View.E_PriceText.SetText(self.item.Config.SellBasePrice.ToString());

            EquipInfoComponent equpCpt = self.item.GetComponent<EquipInfoComponent>();
            self.View.E_EntrysLoopVerticalScrollRect.SetVisible(false);
            self.View.E_ScoreText.SetVisible(equpCpt != null);
            if (equpCpt != null)
            {
                self.View.E_ScoreText.SetText(equpCpt.score.ToString());
                self.AddUIScrollItems(ref self.dicScroll_Item_entry, equpCpt.listAffixes.Count);
                self.View.E_EntrysLoopVerticalScrollRect.SetVisible(true, equpCpt.listAffixes.Count);
            }
        }
    }
}