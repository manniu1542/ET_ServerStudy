namespace ET
{
    public static class AdventureDamageHelper
    {
        //计算伤害（护甲  攻击）
    public static int CaclutaionDamage(NumericComponent attNumCpt, NumericComponent targetNumCpt, ref SRandom random)
        {
            int damage = 0;
            //攻击值
            int damValue = attNumCpt.GetAsInt(NumericType.DamageValue);
            //防御值
            long DodgeValue = targetNumCpt[NumericType.Dodge];
            int ArmorValue = targetNumCpt.GetAsInt(NumericType.Armor);

            //随机的闪避概率 (随机在 0-100%多少，因为 闪避是万分值)
            int randomDodge = random.Range(0, 100_0000);
            if (randomDodge <= DodgeValue)
            {
                Log.Info("攻击被闪避了！");
                return 0;
            }

            damage = damValue - ArmorValue;

            if (damage <= 0)
            {
                damage = 1;
                Log.Info("最小攻击");
            }

            return damage;
        }
    }
}