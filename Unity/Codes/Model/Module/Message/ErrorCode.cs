namespace ET
{
    public static partial class ErrorCode
    {
        public const int ERR_Success = 0;

        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000-109999是Core层的错误

        // 110000以下的错误请看ErrorCore.cs

        // 这里配置逻辑层的错误码
        // 110000 - 200000是抛异常的错误



        // 200001以上不抛异常
        /// <summary> 请求超时 </summary>
        public const int ERR_NetReqTimeOut = 200002;
        /// <summary> 客户端发送的请求的地址 ：切换场景服务器错误 </summary>
        public const int ERR_SwitchSceneSever = 200003;
        /// <summary> 账号不存在 </summary>
        public const int ERR_LoginAccount = 200004;
        /// <summary> 密码错误 </summary>
        public const int ERR_LoginPassword = 200005;
        /// <summary> 重复点击的登录 </summary>
        public const int ERR_LoginRepaetReq = 200006;
        /// <summary> 该账号服务器上已经下线 </summary>
        public const int ERR_TokenError = 200007;
        /// <summary> 角色已经在数据库上了 </summary>
        public const int ERR_RoleInfoInDBAlready = 200007;
        /// <summary> 角色不存在数据库 </summary>
        public const int ERR_RoleInfoNotExistDB = 200008;
        /// <summary> 数据库没有角色表 </summary>
        public const int ERR_DBNotExistRoleInfo = 200008;
        /// <summary> 该账号在负载均衡服务器上已经下线 </summary>
        public const int ERR_RealmKeyError = 200009;
        /// <summary> 该账号在网关服务器上已经下线 </summary>
        public const int ERR_GateKeyError = 200010;

        /// <summary> 其他机器也进入登录网关请求 </summary>
        public const int ERR_OtherMechineLoginGateReq = 200011;

        /// <summary> 该用户的Seesion已经登录游戏了 </summary>
        public const int ERR_SessionEnterGameRealdy = 200012;
        /// <summary> 该用户的Seesion已经登录游戏了 </summary>
        public const int ERR_SessionPlayerError = 200013;
        /// <summary> 玩家的数据已经被释放掉 </summary>
        public const int ERR_PlayerDisposeError = 200014;
        /// <summary> 玩家的数据已经被释放掉 </summary>
        public const int ERR_EnterGameError = 200015;
        
        /// <summary> 不可以加点 </summary>
        public const int ERR_AttributeAddPointCant = 200016;
        
        /// <summary> 加点数不足 </summary>
        public const int ERR_AttributeAddPointDontEnough = 200017;
        
        /// <summary> 不能进入战斗 </summary>
        public const int ERR_CantGoToAdventure = 200018;
        
        
        /// <summary> 战斗结束的验证 </summary>
        public const int ERR_AdventureEndCheckCant = 200019;
        /// <summary> 升级失败 </summary>
        public const int ERR_NumUpLevelCant = 200020;
        
        /// <summary> 售卖失败 </summary>
        public const int ERR_SellItemFail = 200021;
        
        /// <summary> 穿衣失败 </summary>
        public const int ERR_DressUpItemFail = 200022;
        /// <summary> 脱衣失败 </summary>
        public const int ERR_UnloadItemFail = 200023;
        /// <summary> 打造item失败 </summary>
        public const int ERR_ForgeItemFail = 200024;
        /// <summary> 接收打造产品失败 </summary>
        public const int ERR_ReceiveProductionFail = 200024;

    }
}