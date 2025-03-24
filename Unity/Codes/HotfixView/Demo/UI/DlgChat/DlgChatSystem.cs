using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof (DlgChat))]
    [FriendClassAttribute(typeof (ET.ChatInfo))]
    public static class DlgChatSystem
    {
        public static void RegisterUIEvent(this DlgChat self)
        {
            self.View.E_ChatLoopVerticalScrollRect.AddItemRefreshListener(self.OnLoopItemRefreshHandler);

            self.View.E_MessageInputField.AddListener(self.OnMessageInputField);

            self.RegisterCloseEvent<DlgChat>(self.View.E_CloseButton);
            self.View.E_SendButton.AddListenerAsync(self.OnSendMessage);
        }

        public static async ETTask OnSendMessage(this DlgChat self)
        {
            if(string.IsNullOrEmpty(self.message))return;
            Chat2C_SendMsg m2c_ForgeItem;
            var gateSession = self.ZoneScene().GetComponent<SessionComponent>().Session;
            try
            {
                m2c_ForgeItem = await gateSession.Call(new C2Chat_SendMsg() { msg = self.message }) as Chat2C_SendMsg;
                if (m2c_ForgeItem.Error != ErrorCode.ERR_Success)
                {
                    Log.Error("请求 制作失败 错误码是：" + m2c_ForgeItem.Error);
                }
                else
                {
                    self.RefreshUI();
                }
            }
            catch (Exception e)
            {
                Log.Error("请求出错：" + e.ToString());
            }
        }

        public static void OnMessageInputField(this DlgChat self, string newStr)
        {
            self.message = newStr;
        }

        public static void ShowWindow(this DlgChat self, Entity contextData = null)
        {
            self.RefreshUI();
        }

        public static void HideWindow(this DlgChat self)
        {
            self.RemoveUIScrollItems(ref self.ScrollItemChats);
        }

        public static void OnLoopItemRefreshHandler(this DlgChat self, Transform transform, int index)
        {
            Scroll_Item_chat scrollItemChat = self.ScrollItemChats[index].BindTrans(transform);
            var chatCpt = self.ZoneScene().GetComponent<ChatComponent>();
            var chatinfo = chatCpt.GetChatMessageByIndex(index);
            scrollItemChat.E_NameText.text = chatinfo.name;
            scrollItemChat.E_ChatText.text = chatinfo.content;
        }

        public static void RefreshUI(this DlgChat self)
        {
            //页数，
            var chatCpt = self.ZoneScene().GetComponent<ChatComponent>();
            int count = chatCpt.GetCurCount();

            self.AddUIScrollItems(ref self.ScrollItemChats, count);

            self.View.E_ChatLoopVerticalScrollRect.SetVisible(count != 0, count);
        }
    }
}