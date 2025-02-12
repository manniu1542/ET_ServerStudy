namespace ET
{
    public static class NumericNoticeComponentSystem
    {
        public static void Notify(this NumericNoticeComponent self, int numericType, long newValue)
        {
            var unit = self.Parent as Unit;
            if (unit == null) return;

            //推送 客户端 监听事件
            MessageHelper.SendToClient(unit, new M2C_NumbericChange() { UnitID = unit.Id, NumType = numericType, NumValue = newValue });
        }
    }
}