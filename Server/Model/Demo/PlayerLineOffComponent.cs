﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ComponentOf(typeof(Player))]
    public class PlayerLineOffComponent : Entity, IAwake, IDestroy
    {
        public long TimeId;
     
    }
}