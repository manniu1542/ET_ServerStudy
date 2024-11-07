using UnityEngine.UI;

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
                      self.ZoneScene(),
                      ConstValue.LoginAddress,
                      self.View.E_AccountInputField.GetComponent<InputField>().text,
                      self.View.E_PasswordInputField.GetComponent<InputField>().text);

            Log.Info("登录信息：" + err);

            if (err == ErrorCode.ERR_Success)
            {
                self.ZoneScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Login);
                self.ZoneScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_ServerList);


            }

            await ETTask.CompletedTask;
        }

        public static void HideWindow(this DlgLogin self)
        {


        }

    }
}
