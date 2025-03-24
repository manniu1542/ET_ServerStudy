namespace ET
{
    public enum SceneType
    {
        Process = 0,
        Manager = 1,
        Realm = 2,
        Gate = 3,
        Http = 4,
        Location = 5,
        Map = 6,

        /// <summary>
        /// 账号登录服
        /// </summary>
        Account = 7,

        /// <summary>
        /// 账号中心服
        /// </summary>
        LoginCenter = 8,

        /// <summary>
        /// 缓存服
        /// </summary>
        UnitChache = 9,
        /// <summary>
        /// 排行榜服
        /// </summary>
        Rank = 10,
        /// <summary>
        /// 聊天服
        /// </summary>
        Chat = 11,
        // 客户端Model层
        Client = 30,
        Zone = 31,
        Login = 32,
        Robot = 33,
        Current = 34,
    }
}