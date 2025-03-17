namespace ET
{
    [ChildType(typeof (RankInfoComponent))]
#if SERVER
    public class RankInfo: Entity, IAwake, IDestroy
#else
    public class RankInfo: Entity, IAwake, IDestroy
#endif
    {
        public long unitId;
  
        public string name;
        
        public long count;


    }
}