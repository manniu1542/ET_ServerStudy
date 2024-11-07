using System;
using System.Threading;


namespace ET
{

    [FriendClass(typeof(RoleInfo))]
    [FriendClass(typeof(ServerInfoComponent))]
    [FriendClass(typeof(AccountInfoComponent))]
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
                return ErrorCode.ERR_NetReqTimeOut;


            }
            if (result.Error != ErrorCode.ERR_Success)
            {
                return result.Error;
            }


            zoneScene.GetComponent<AccountInfoComponent>().ReadAccountInfo(result.AccountId, result.Token);
            zoneScene.AddComponent<SessionComponent>().Session = session;
            session.AddComponent<PingComponent>();



            return ErrorCode.ERR_Success;


        }

        public static async ETTask<int> GetServerInfo(Scene zoneScene)
        {
            var scrAccountInfo = zoneScene.GetComponent<AccountInfoComponent>();
            var scrServerInfo = zoneScene.GetComponent<ServerInfoComponent>();
            A2C_GetServerInfo info;
            try
            {
                info = await zoneScene.GetComponent<SessionComponent>().Session.Call(
                           new C2A_GetServerInfo()
                           {
                               AccountId = scrAccountInfo.AccountId,
                               Token = scrAccountInfo.Token
                           }) as A2C_GetServerInfo;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ErrorCode.ERR_NetReqTimeOut;
            }
            if (info.Error != ErrorCode.ERR_Success)
            {
                Log.Error("获取大区列表失败！错误码：" + info.Error);
                return info.Error;
            }

            scrServerInfo.SetServerInfoList(info.ListServerInfo);
            return ErrorCode.ERR_Success;
        }

        public static async ETTask<int> CreateRoleInfo(Scene zoneScene, string name)
        {
            var scrAccountInfo = zoneScene.GetComponent<AccountInfoComponent>();

            A2C_CreateRoleInfo info;

            try
            {
                info = await zoneScene.GetComponent<SessionComponent>().Session.Call(
                        new C2A_CreateRoleInfo()
                        {
                            AccountId = scrAccountInfo.AccountId,
                            Token = scrAccountInfo.Token,
                            ServerId = zoneScene.GetComponent<ServerInfoComponent>().curServerId,
                            Name = name,

                        }) as A2C_CreateRoleInfo;


            }
            catch (Exception e)
            {

                Log.Error(e);
                return ErrorCode.ERR_NetReqTimeOut;
            }

            if (info.Error != ErrorCode.ERR_Success)
            {
                Log.Error("创建角色信息失败！错误码：" + info.Error);
                return info.Error;
            }

         
            zoneScene.GetComponent<RoleInfoComponent>().Add(info.RoleInfo);



            return info.Error;

        }


        public static async ETTask<int> GetAllRoleInfo(Scene zoneScene)
        {
            var scrAccountInfo = zoneScene.GetComponent<AccountInfoComponent>();
            A2C_GetListRoleInfo info;

            try
            {
                info = await zoneScene.GetComponent<SessionComponent>().Session.Call(
                        new C2A_GetListRoleInfo()
                        {
                            AccountId = scrAccountInfo.AccountId,
                            Token = scrAccountInfo.Token,
                            ServerId = zoneScene.GetComponent<ServerInfoComponent>().curServerId,

                        }) as A2C_GetListRoleInfo;


            }
            catch (Exception e)
            {

                Log.Error(e);
                return ErrorCode.ERR_NetReqTimeOut;
            }

            if (info.Error != ErrorCode.ERR_Success)
            {
                Log.Error("获取所有角色信息失败！错误码：" + info.Error);
                return info.Error;
            }

            //添加自己的信息

            var scrRoleInfoCpt = Game.Scene.GetComponent<RoleInfoComponent>();

     
            foreach (var item in info.RoleInfo)
            {
                scrRoleInfoCpt.Add(item);
            }



            return info.Error;

        }
        public static async ETTask<int> DeleteRoleInfo(Scene zoneScene)
        {
            var scrAccountInfo = zoneScene.GetComponent<AccountInfoComponent>();
            A2C_DeleRoleInfo info;

            try
            {
                info = await zoneScene.GetComponent<SessionComponent>().Session.Call(
                        new C2A_DeleRoleInfo()
                        {
                            AccountId = scrAccountInfo.AccountId,
                            Token = scrAccountInfo.Token,
                            ServerId = zoneScene.GetComponent<ServerInfoComponent>().curServerId,

                        }) as A2C_DeleRoleInfo;


            }
            catch (Exception e)
            {

                Log.Error(e);
                return ErrorCode.ERR_NetReqTimeOut;
            }

            if (info.Error != ErrorCode.ERR_Success)
            {
                Log.Error("删除角色信息失败！错误码：" + info.Error);
                return info.Error;
            }

            zoneScene.GetComponent<RoleInfoComponent>().Remove(scrAccountInfo.AccountId);


            return info.Error;

        }
    }
}