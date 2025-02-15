namespace ET
{
    // 派送到数值修改消息到客户端
    /// <summary>
    /// 监视hp数值变化，改变血条值
    /// </summary>
    [NumericWatcher(NumericType.Spirit)]
    [NumericWatcher(NumericType.Agile)]
    [NumericWatcher(NumericType.PhysicalStrength)]
    [NumericWatcher(NumericType.Power)]
    public class NumericChange_AttributeAddPoint: INumericWatcher
    {
        public void Run(EventType.NumbericChange args)
        {
            // 根据 策划的配置 1点属性（加在体力上，公式换算后 增加多少生命值）
            var numCpt = args.Parent?.GetComponent<NumericComponent>();
            if (numCpt == null)
            {
                return;
            }

            //1点体力 = 上海增加5
            if (args.NumericType == NumericType.Power)
            {
                numCpt[NumericType.DamageValueAdd] += 5;
                return;
            }
            //1点精神 = mp整体增加1%
            if (args.NumericType == NumericType.Spirit)
            {
                numCpt[NumericType.MPFinalPct] += 1*10000;
            }
            //1点敏捷 = 防御整体增加5
            if (args.NumericType == NumericType.Agile)
            {
                numCpt[NumericType.ArmorFinalAdd] +=5;
            }

            if (args.NumericType == NumericType.PhysicalStrength)
            {
                numCpt[NumericType.HpPct] += 1*10000;//因为小数都是万分制 ， Pct 表示 百分值 的血量 ，整体是 增加百分之1的血量
                return;
            }
        }
    }
}