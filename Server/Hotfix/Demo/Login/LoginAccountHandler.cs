using System;

namespace ET
{
    [FriendClass(typeof(Account))]
    public class LoginAccountHandler : AMRpcHandler<C2A_LoginAccount, A2C_LoginAccount>
    {
        protected override async ETTask Run(Session session, C2A_LoginAccount request, A2C_LoginAccount response, Action reply)
        {

            //移除超时 还未响应的 组件 (只在这里移除，LoginAccount是Account服务器的第一个消息，需要移除session超时)
            session.RemoveComponent<SessionAcceptTimeoutComponent>();
            //请求的服务器类型
            SceneType st = session.DomainScene().SceneType;
            if (st != SceneType.Account)
            {
                response.Error = ErrorCode.ERR_LoginSceneSever;
                reply();
                session.Disconnect().Coroutine();
                Log.Error("请求的账号，场景服务器错误！" + st);
                return;
            }
            //验证输入账号密码
            if (string.IsNullOrEmpty(request.Account))
            {
                response.Error = ErrorCode.ERR_LoginAccount;
                reply();
                session.Disconnect().Coroutine();
                Log.Error("登录的账号错误！");
                return;

            }
            if (string.IsNullOrEmpty(request.Password))
            {
                response.Error = ErrorCode.ERR_LoginPassword;
                reply();
                session.Disconnect().Coroutine();
                Log.Error("登录的密码错误！" + st);
                return;
            }
            //不是多余,using执行完就dispose了。   （多余每次登录的 session 都不同。所以 添加这个 毫无用处。只有在相同的session下才 有效）
            var rsc = session.GetComponent<RepeatClickServerComponent>();
            if (rsc != null)
            {
                response.Error = ErrorCode.ERR_LoginRepaetReq;
                reply();
                session.Disconnect().Coroutine();
                Log.Error("重复请求！客户端防不住的通过处理");
                return;

            }

            //防止一个客户端的 等待时候的重复点击。
            using (session.AddComponent<RepeatClickServerComponent>())
            {
                int ahc = request.Account.GetHashCode();


                //防止多个 客户端，同一时刻 请求同一个账号（注册）登录。  TODO:不知道为什么用第二个相同账号的session登录 没有等待，答：因为using 执行完就把他释放了
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginCommonAccount, ahc))
                {

                    //请求的数据库内容
                    Account acount = null;

                    var dbc = DBManagerComponent.Instance.GetZoneDB(session.DomainZone());
                    var listAccount = await dbc.Query<Account>(a => a.AccountName.Equals(request.Account));
                    if (listAccount.Count > 0)
                    {
                        acount = listAccount[0];

                        if (acount.Password != request.Password)
                        {
                            response.Error = ErrorCode.ERR_LoginPassword;
                            reply();
                            session.Disconnect().Coroutine();
                            Log.Error("登录的密码错误！" + st);
                            return;
                        }



                    }
                    else
                    {
                        //没有就创建
                        acount = session.AddChild<Account>();
                        acount.AccountName = request.Account;
                        acount.Password = request.Password;
                        acount.CreateTime = TimeHelper.ServerNow().ToString();
                        acount.Type = AccountType.Normal;
                        await dbc.Save(acount);


                    }
                    string token = TimeHelper.ServerNow().ToString() + RandomHelper.RandomNumber(int.MinValue, int.MaxValue).ToString();


                    #region 给账号中心服务器，发送通知给Gate或指定游戏内map的服务器 （已经登录的该账号踢下线）
                    //获取该账号用了那个网关
                    StartSceneConfig config = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), "LoginCenter");
                    //给那个网关发送,强制下线的消息
                    L2A_ForcePlayerLogOut forcePlayerLogOut = (L2A_ForcePlayerLogOut)await ActorMessageSenderComponent.Instance.Call(
                     config.InstanceId, new A2L_ForcePlayerLogOut() { AccountID = acount.Id });
                    if (forcePlayerLogOut.Error != ErrorCode.ERR_Success)
                    {
                        response.Error = forcePlayerLogOut.Error;
                        reply();
                        session.Disconnect().Coroutine();
                        acount?.Dispose();
                        return;
                    }
                    #endregion



                    
                    //在账号服务器上连接 （用户踢下线功能)
                    AccountLoginSessionComponent scrAcountSession = session.DomainScene().GetComponent<AccountLoginSessionComponent>();
                    Session lastLoginSession = scrAcountSession.Get(acount.Id);
                    // Game.EventSystem.Get(lastLoginAccountId) as Session;
                    lastLoginSession?.Send(new A2C_Disconnect() { Error = 0 });
                    lastLoginSession?.Disconnect().Coroutine();
                    scrAcountSession.Add(acount.Id, session);

                    //异常登录超时，不进入游戏踢下线的组件
                    session.RemoveComponent<AccountLoginOutTimeCheckComponent>();
                    session.AddComponent<AccountLoginOutTimeCheckComponent, long>(acount.Id);

                    //玩家登录的 唯一身份标记码
                    TokenComponent scrToken = session.DomainScene().GetComponent<TokenComponent>();
                    scrToken.Remove(acount.Id);
                    scrToken.Add(acount.Id, token);

                    response.AccountId = acount.Id;
                    response.Token = token;


                    reply();




                    acount?.Dispose();
                }


            }


            await ETTask.CompletedTask;
        }
    }
}
