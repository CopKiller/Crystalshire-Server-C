﻿namespace LoginServer.Network.PacketList;

public enum ServerPacketEnum
{
    SAlertMsg = 1,
    SLoginOk,
    SNewCharClasses,
    SClassesData,
    SInGame,
    SPlayerInv,
    SPlayerInvUpdate,
    SPlayerWornEq,
    SPlayerHp,
    SPlayerMp,
    SPlayerStats,
    SPlayerData,
    SPlayerMove,
    SNpcMove,
    SPlayerDir,
    SNpcDir,
    SPlayerXY,
    SPlayerXYMap,
    SAttack,
    SNpcAttack,
    SCheckForMap,
    SMapData,
    SMapItemData,
    SMapNpcData,
    SMapDone,
    SGlobalMsg,
    SAdminMsg,
    SPlayerMsg,
    SMapMsg,
    SSpawnItem,
    SItemEditor,
    SUpdateItem,
    SREditor,
    SSpawnNpc,
    SNpcDead,
    SNpcEditor,
    SUpdateNpc,
    SMapKey,
    SEditMap,
    SShopEditor,
    SUpdateShop,
    SSpellEditor,
    SUpdateSpell,
    SSpells,
    SLeft,
    SResourceCache,
    SResourceEditor,
    SUpdateResource,
    SSendPing,
    SDoorAnimation,
    SActionMsg,
    SPlayerEXP,
    SBlood,
    SAnimationEditor,
    SUpdateAnimation,
    SAnimation,
    SMapNpcVitals,
    SCooldown,
    SClearSpellBuffer,
    SSayMsg,
    SOpenShop,
    SResetShopAction,
    SStunned,
    SMapWornEq,
    SBank,
    STrade,
    SCloseTrade,
    STradeUpdate,
    STradeStatus,
    STarget,
    SHotbar,
    SHighIndex,
    SSound,
    STradeRequest,
    SPartyInvite,
    SPartyUpdate,
    SPartyVitals,
    SChatUpdate,
    SConvEditor,
    SUpdateConv,
    SStartTutorial,
    SChatBubble,
    SPlayerChars,
    SCancelAnimation,
    SPlayerVariables,
    SEvent,

    LoginToken = 254,
    UserData = 255
}
