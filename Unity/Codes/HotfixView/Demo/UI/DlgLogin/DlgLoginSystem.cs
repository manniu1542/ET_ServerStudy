using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;

namespace ET
{
    public static class DlgLoginSystem
    {

        public static void RegisterUIEvent(this DlgLogin self)
        {
            //self.View.E_LoginButton.AddListenerAsync(() => self.OnLoginClickHandler());
            self.View.E_LoginButton.AddListener(() =>
            {
                self.OnLoginClickHandler().Coroutine();
            });
        }

        public static void ShowWindow(this DlgLogin self, Entity contextData = null)
        {

        }

        public static async ETTask OnLoginClickHandler(this DlgLogin self)
        {
            Log.Info("使用的登录服务器是Game么：" + self.DomainScene().Name);
            int err = await LoginHelper.Login(
                      self.DomainScene(),
                      ConstValue.LoginAddress,
                      self.View.E_AccountInputField.GetComponent<InputField>().text,
                      self.View.E_PasswordInputField.GetComponent<InputField>().text);

            Log.Info("登录信息：" + err);

            if (err == ErrorCode.ERR_Success)
            {
                self.ZoneScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Login);
                self.ZoneScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Lobby);

                A2C_GetServerInfo info = await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(new C2A_GetServerInfo()) as A2C_GetServerInfo;

                //4.服务器写 游戏服务器管理类， 在数据库读取 游戏服务器这个共享类 列表。
                //info.ListServerInfo

            }

            await ETTask.CompletedTask;
        }

        public static void HideWindow(this DlgLogin self)
        {

        }

    }
}
