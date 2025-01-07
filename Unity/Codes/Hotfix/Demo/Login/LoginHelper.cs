using System;
using System.Net;
using System.Threading;
using static System.Collections.Specialized.BitVector32;

namespace ET
{
    [FriendClass(typeof (RoleInfo))]
    [FriendClass(typeof (ServerInfoComponent))]
    [FriendClass(typeof (AccountInfoComponent))]
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
                    new C2A_GetServerInfo() { AccountId = scrAccountInfo.AccountId, Token = scrAccountInfo.Token }) as A2C_GetServerInfo;
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
                info = await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_CreateRoleInfo()
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

            zoneScene.GetComponent<RoleInfoComponent>().SetRoleInfo(info.RoleInfo);

            return info.Error;
        }

        public static async ETTask<int> GetRoleInfo(Scene zoneScene)
        {
            var scrAccountInfo = zoneScene.GetComponent<AccountInfoComponent>();
            A2C_GetRoleInfoInServer info;

            try
            {
                info = await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_GetRoleInfoInServer()
                {
                    AccountId = scrAccountInfo.AccountId,
                    Token = scrAccountInfo.Token,
                    ServerId = zoneScene.GetComponent<ServerInfoComponent>().curServerId,
                }) as A2C_GetRoleInfoInServer;
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

            var scrRoleInfoCpt = zoneScene.GetComponent<RoleInfoComponent>();

            scrRoleInfoCpt.SetRoleInfo(info.RoleInfo);

            return info.Error;
        }

        public static async ETTask<int> DeleteRoleInfo(Scene zoneScene)
        {
            var scrAccountInfo = zoneScene.GetComponent<AccountInfoComponent>();
            A2C_DeleRoleInfo info;

            try
            {
                info = await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_DeleRoleInfo()
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

            zoneScene.GetComponent<RoleInfoComponent>().Remove();

            return info.Error;
        }

        public static async ETTask<int> EnterGameRealmGameToLoginGate(Scene zoneScene)
        {
            //通过账号服务器获取 负载均衡服务器 的地址 以及key
            var scrAccountInfo = zoneScene.GetComponent<AccountInfoComponent>();
            A2C_GetRealmGate info;
            try
            {
                info = await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_GetRealmGate()
                {
                    AccountId = scrAccountInfo.AccountId,
                    Token = scrAccountInfo.Token,
                    ServerId = zoneScene.GetComponent<ServerInfoComponent>().curServerId,
                }) as A2C_GetRealmGate;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ErrorCode.ERR_NetReqTimeOut;
            }

            if (info.Error != ErrorCode.ERR_Success)
            {
                Log.Error("获取负载均衡服务器Key失败！错误码：" + info.Error);
                return info.Error;
            }

            scrAccountInfo.KeyRealmGate = info.KeyRealmGate;
            scrAccountInfo.AdressRealmGate = info.AdressRealmGate;
            //通过负载均衡服务器 获取网关的地址 以及key
            Session realmSession;
            R2C_GetGate getGate;
            try
            {
                realmSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(scrAccountInfo.AdressRealmGate));
                getGate =
                        await realmSession.Call(new C2R_GetGate() { AccountId = scrAccountInfo.AccountId, Token = scrAccountInfo.KeyRealmGate, }) as
                                R2C_GetGate;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ErrorCode.ERR_NetReqTimeOut;
            }

            realmSession?.Dispose();
            if (getGate.Error != ErrorCode.ERR_Success)
            {
                Log.Error("网关服务器Key失败！错误码：" + getGate.Error);
                return getGate.Error;
            }

            scrAccountInfo.KeyGate = getGate.KeyGate;
            scrAccountInfo.AdressGate = getGate.AdressGate;

            //创建游戏网关

            Session gateSession;
            G2C_LinkGateLogin loginGate;

            try
            {
                gateSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(scrAccountInfo.AdressGate));

                loginGate = await gateSession.Call(new C2G_LinkGateLogin()
                {
                    AccountId = scrAccountInfo.AccountId, SessionKey = scrAccountInfo.KeyGate, RoleId = scrAccountInfo.AccountId,
                }) as G2C_LinkGateLogin;
                ;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ErrorCode.ERR_NetReqTimeOut;
            }

            if (loginGate.Error != ErrorCode.ERR_Success)
            {
                gateSession?.Dispose();
                Log.Error("网关服务器连接失败！错误码：" + loginGate.Error);
                return loginGate.Error;
            }

            //移除登录服务器的会话，会一并移除掉  账号服务器的pingcomponenet
            zoneScene.RemoveComponent<SessionComponent>();

            //重置网关服务器的会话
            zoneScene.AddComponent<SessionComponent>().Session = gateSession;
            gateSession.AddComponent<PingComponent>();

      

            return ErrorCode.ERR_Success;
        }

        public static async ETTask<int> EnterGame(Scene zoneScene)
        {
            Session gateSession = zoneScene.GetComponent<SessionComponent>().Session;
            G2C_EnterGame enterGame = null;
           
            try
            {
                enterGame = await gateSession.Call(new C2G_EnterGame()
                                {
                                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                                    SessionKey = zoneScene.GetComponent<AccountInfoComponent>().KeyGate,
                                }) as G2C_EnterGame;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ErrorCode.ERR_NetReqTimeOut;
            }
            if (gateSession.Error != ErrorCode.ERR_Success)
            {
                Log.Error("网关服务器连接失败！错误码：" + gateSession.Error);
            }
            return enterGame.Error;
        }
    }
}