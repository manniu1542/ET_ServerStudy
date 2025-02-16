using System;
using System.Collections.Generic;

namespace ET
{
	[ComponentOf(typeof(Unit))]
	public class UnitSaveDBComponent : Entity, IAwake, IDestroy
	{
		/// <summary>
		/// 需要被保存到数据库的 unit挂载的IChaCheUnit类型
		/// </summary>
		public HashSet<Type> hsNeedSaveDBCpt;

		/// <summary>
		/// 定时器id
		/// </summary>
		public long TimerId;
        /// <summary>
        /// 间隔10检查一次保存到数据库
        /// </summary>
		public long timeInterval = 10 * 1000;
	}
}