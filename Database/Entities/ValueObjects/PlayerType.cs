﻿namespace Database.Entities.Player;

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
    Hero
}
public enum EquipmentType
{
    Weapon = 1,
    Armor,
    Helmet,
    Shield,
    Legs,
    Boots
}