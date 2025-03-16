using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class StartSceneConfig555Category : ProtoObject, IMerge
    {
        public static StartSceneConfig555Category Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, StartSceneConfig555> dict = new Dictionary<int, StartSceneConfig555>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<StartSceneConfig555> list = new List<StartSceneConfig555>();
		
        public StartSceneConfig555Category()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            StartSceneConfig555Category s = o as StartSceneConfig555Category;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (StartSceneConfig555 config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public StartSceneConfig555 Get(int id)
        {
            this.dict.TryGetValue(id, out StartSceneConfig555 item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (StartSceneConfig555)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, StartSceneConfig555> GetAll()
        {
            return this.dict;
        }

        public StartSceneConfig555 GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class StartSceneConfig555: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>所属进程</summary>
		[ProtoMember(2)]
		public int Process { get; set; }
		/// <summary>所属区</summary>
		[ProtoMember(3)]
		public int Zone { get; set; }
		/// <summary>类型</summary>
		[ProtoMember(4)]
		public string SceneType { get; set; }
		/// <summary>名字</summary>
		[ProtoMember(5)]
		public string Name { get; set; }
		/// <summary>外网端口</summary>
		[ProtoMember(6)]
		public int OuterPort { get; set; }

	}
}
