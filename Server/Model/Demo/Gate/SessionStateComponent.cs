namespace ET
{
    public enum SessionState
    {  
        /// <summary>
        /// 未登录Gate网关
        /// </summary>
        None,
      
        /// <summary>
        /// 登录游戏后，在游戏中
        /// </summary>
        Game,
    }
 
    [ComponentOf(typeof(Session))]
    public class SessionStateComponent : Entity, IAwake, IDestroy
    {
        public SessionState curState;

    }
}