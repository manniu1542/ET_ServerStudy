
namespace ET
{
    /// <summary>
    /// 设置战斗随机数
    /// </summary>

    [NumericWatcher(NumericType.AdventureRandomSeed)]
    public class NumericWatcher_AdventureRandomSeed: INumericWatcher
    {
        public void Run(EventType.NumbericChange args)
        {
            //设置战斗随机数
            args.Parent.DomainScene().GetComponent<AdventureComponent>().ResetBattleRandom();
        }
    }
}