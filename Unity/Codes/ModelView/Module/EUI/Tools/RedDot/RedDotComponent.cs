using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public static class RedDotType
    {
        public static readonly string Role = "Role";

        public static readonly string Role_Level = "Role_Level";
        public static readonly string Role_AttributePoint = "Role_AttributePoint";
    }
    
    
    
    [ComponentOf(typeof(Scene))]
    public class RedDotComponent: Entity,IAwake,IDestroy
    {
        public Dictionary<string,  ListComponent<string>  > RedDotNodeParentsDict = new Dictionary<string, ListComponent<string>>();

        public HashSet<string> RedDotNodeNeedShowSet = new HashSet<string>();
        
        public Dictionary<string, int> RetainViewCount = new Dictionary<string, int>();
        
        public Dictionary<string, string> ToParentDict = new Dictionary<string, string>();
        
        public Dictionary<string, int> RedDotNodeRetainCount = new Dictionary<string, int>(); 
        
        public Dictionary<string, RedDotMonoView> RedDotMonoViewDict = new Dictionary<string, RedDotMonoView>();
    }
}