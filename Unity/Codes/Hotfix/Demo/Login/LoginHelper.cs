using System;


namespace ET
{
    public static class LoginHelper
    {
        public static async ETTask<int> Login(Scene zoneScene, string address, string account, string password)
        {

            A2C_LoginAccount result = null;
            Session session = null;
            try
            {
                session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                password = MD5Helper.StringMD5(password);
                C2A_LoginAccount c2a_LoginAccount = new C2A_LoginAccount() { Account = account, Password = password };
                result = await session.Call(c2a_LoginAccount) as A2C_LoginAccount;


                Log.Info("请求的结果是：" + result);
            }
            catch (Exception e)
            {
                session?.Dispose();
                return ErrorCode.ERR_NetReqLoginTimeOut;


            }
            if (result.Error != ErrorCode.ERR_Success)
            {
                return result.Error;
            }

            zoneScene.AddComponent<SessionComponent>().Session = session;
            session.AddComponent<PingComponent>();

            return ErrorCode.ERR_Success;


        }
    }
}