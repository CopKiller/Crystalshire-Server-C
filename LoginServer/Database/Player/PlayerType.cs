﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Database.Player
{
    public enum SexType
    {
        None,
        Male,
        Female,
        Other
    }
    public enum ClassType
    {
        None,
        Knight,
        Archer
    }

    public enum AccessType
    {
        Player,
        Moniter,
        Mapper,
        Administrator
    }
    public enum EntidadeType
    {
        Normal,
        PlayerKiller,
        Heroi
    }
}