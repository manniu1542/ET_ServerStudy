namespace ET
{
    // 这个可弄个配置表生成
    public static class NumericType
    {
        public const int Max = 10000;

        #region 示例
        public const int Speed = 1000;
        public const int SpeedBase = Speed * 10 + 1;
        public const int SpeedAdd = Speed * 10 + 2;
        public const int SpeedPct = Speed * 10 + 3;
        public const int SpeedFinalAdd = Speed * 10 + 4;
        public const int SpeedFinalPct = Speed * 10 + 5;

   

        public const int MaxHp = 1002;
        public const int MaxHpBase = MaxHp * 10 + 1;
        public const int MaxHpAdd = MaxHp * 10 + 2;
        public const int MaxHpPct = MaxHp * 10 + 3;
        public const int MaxHpFinalAdd = MaxHp * 10 + 4;
        public const int MaxHpFinalPct = MaxHp * 10 + 5;

        public const int AOI = 1003;
        public const int AOIBase = AOI * 10 + 1;
        public const int AOIAdd = AOI * 10 + 2;
        public const int AOIPct = AOI * 10 + 3;
        public const int AOIFinalAdd = AOI * 10 + 4;
        public const int AOIFinalPct = AOI * 10 + 5;


        

        #endregion

        #region 有加成推导的属性

         
        public const int DamageValue = 1011;         //伤害
        public const int DamageValueBase = DamageValue * 10 + 1;
        public const int DamageValueAdd = DamageValue * 10 + 2;
        public const int DamageValuePct = DamageValue * 10 + 3;
        public const int DamageValueFinalAdd = DamageValue * 10 + 4;
        public const int DamageValueFinalPct = DamageValue * 10 + 5;
	    
        public const int AdditionalDdamage = 1012;         //伤害追加
        
        
         
        public const int Hp = 1013;  // 生命值
        public const int HpBase = Hp * 10 + 1;
        public const int HpAdd = Hp * 10 + 2;
        public const int HpPct = Hp * 10 + 3;
        public const int HpFinalAdd = Hp * 10 + 4;
        public const int HpFinalPct = Hp * 10 + 5;
	    

	    
        public const int MP = 1014; //法力值
        public const int MPBase = MP * 10 + 1;
        public const int MPAdd = MP * 10 + 2;
        public const int MPPct = MP * 10 + 3;
        public const int MPFinalAdd = MP * 10 + 4;
        public const int MPFinalPct = MP * 10 + 5;
	    

        public const int Armor = 1015; //护甲
        public const int ArmorBase = Armor * 10 + 1;
        public const int ArmorAdd = Armor * 10 + 2;
        public const int ArmorPct = Armor * 10 + 3;
        public const int ArmorFinalAdd = Armor * 10 + 4;
        public const int ArmorFinalPct = Armor * 10 + 5;
	    
        public const int ArmorAddition = 1015; //护甲追加
        
        
        

        #endregion


        #region 没有加成推导的属性
        /// <summary>
        /// 力量
        /// </summary>
        public const int Power = 3001; //力量
        /// <summary>
        /// 体力
        /// </summary>
        public const int PhysicalStrength = 3002; //
        /// <summary>
        /// 敏捷值
        /// </summary>
        public const int Agile = 3003; 
        /// <summary>
        /// 精神
        /// </summary>
        public const int Spirit = 3004; //精神
	    
        public const int AttributePoint = 3005; //属性点
        /// <summary>
        /// 战力值
        /// </summary>
        public const int CombatEffectiveness = 3006; 
	    
        public const int Level = 3007; //等级
	    
        public const int Gold  = 3008;//金币
	    
        public const int Exp   = 3009;//经验

        #endregion
    

    }
}