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
    public static class DlgItemPopUpSystem
    {
        public static void RegisterUIEvent(this DlgItemPopUp self)
        {
            self.RegisterCloseEvent<DlgItemPopUp>(self.View.E_CloseButton);

            EUIHelper.AddListenerAsync(self.View.E_SellButton, self.OnSell);

            ResourcesComponent.Instance.LoadBundle("icons.unity3d");
            self.saIcon = ResourcesComponent.Instance.GetAsset("icons.unity3d", "Icons") as SpriteAtlas;
        }

        public static async ETTask OnSell(this DlgItemPopUp self)
        {
            try
            {
                C2M_SellItem msg = new C2M_SellItem() { ItemUid = self.item.Id };

                var gateSession = self.ZoneScene().GetComponent<SessionComponent>().Session;

                M2C_SellItem response = await gateSession.Call(msg) as M2C_SellItem;

                if (response.Error == ErrorCode.ERR_Success)
                {
                    self.ZoneScene().GetComponent<UIComponent>().GetDlgLogic<DlgBag>().RefreshUI();
                    //关闭页面
                    self.View.E_CloseButton.onClick.Invoke();
                }
                else
                {
                    Log.Error("售卖道具失败：错误码：" + response.Error);
                }
            }
            catch (Exception e)
            {
                Log.Error("售卖道具失败：" + e.ToString());
            }
        }

        public static void ShowWindow(this DlgItemPopUp self, Entity contextData = null)
        {
        }

        public static void RefreshUI(this DlgItemPopUp self, long itemId)
        {
            var bagCpt = self.ZoneScene().GetComponent<BagComponent>();
            self.item = bagCpt.GetItem(itemId);

            self.View.E_NameText.SetText(self.item.Config.Name);
            self.View.E_IconImage.sprite = self.saIcon.GetSprite(self.item.Config.Icon);
            self.View.E_DescText.SetText(self.item.Config.Desc);
            self.View.E_PriceText.SetText(self.item.Config.SellBasePrice.ToString());

            // self.item.GetComponent<EquipInfoComponent>();
            // self.View.E_ScoreText.SetText(self.item.);
        }
    }
}