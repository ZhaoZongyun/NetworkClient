/// <summary>
/// 消息号
/// 由编辑器生成
/// </summary>
public enum NetMessageId : ushort //(0 - 65535)
{
    /// <summary>
    /// 请求角色信息
    /// </summary>
    ReqCharacterInfo = 1,
    /// <summary>
    /// 下发角色信息
    /// </summary>
    ResCharacterInfo = 2,
    /// <summary>
    /// 请求创建角色
    /// </summary>
    ReqCreateCharacter = 3,
    /// <summary>
    /// 下发创建角色
    /// </summary>
    ResCreateCharacter = 4,
    /// <summary>
    /// 下发错误提示
    /// </summary>
    ResErrorNotice = 5,
    /// <summary>
    /// 请求登录
    /// </summary>
    ReqLogin = 6,
    /// <summary>
    /// 下发登录
    /// </summary>
    ResLogin = 7,
    /// <summary>
    /// 请求登出
    /// </summary>
    ReqLogout = 8,
    /// <summary>
    /// 下发登出
    /// </summary>
    ResLogout = 9,
    /// <summary>
    /// 请求注册
    /// </summary>
    ReqRegister = 10,
    /// <summary>
    /// 下发注册
    /// </summary>
    ResRegister = 11,
    /// <summary>
    /// 请求游客登陆
    /// </summary>
    ReqVisitorLogin = 12,
    /// <summary>
    /// 下发手牌变化
    /// </summary>
    ResHandCards = 13,
    /// <summary>
    /// 下发叫地主座次（一人只能叫一次）
    /// </summary>
    ResWillCallBossSeat = 14,
    /// <summary>
    /// 请求叫地主或不叫地主
    /// </summary>
    ReqCallBoss = 15,
    /// <summary>
    /// 下发叫地主或不叫地主
    /// </summary>
    ResCallBoss = 16,
    /// <summary>
    /// 下发底牌
    /// </summary>
    ResHoldCard = 17,
    /// <summary>
    /// 下发出牌座次
    /// </summary>
    ResPushSeat = 18,
    /// <summary>
    /// 请求出牌
    /// </summary>
    ReqPushCard = 19,
    /// <summary>
    /// 下发出牌
    /// </summary>
    ResPushCard = 20,
    /// <summary>
    /// 请求不出
    /// </summary>
    ReqPassCard = 21,
    /// <summary>
    /// 下发不出
    /// </summary>
    ResPassCard = 22,
    /// <summary>
    /// 下发游戏结束
    /// </summary>
    ResGameOver = 23,
    /// <summary>
    /// 请求进入房间
    /// </summary>
    ReqEnterRoom = 24,
    /// <summary>
    /// 下发进入房间
    /// </summary>
    ResEnterRoom = 25,
    /// <summary>
    /// 请求退出房间
    /// </summary>
    ReqExitRoom = 26,
    /// <summary>
    /// 下发退出房间
    /// </summary>
    ResExitRoom = 27,
    /// <summary>
    /// 下发其他玩家进入房间
    /// </summary>
    ResOtherEnterRoom = 28,
    /// <summary>
    /// 下发其他玩家退出房间
    /// </summary>
    ResOtherExitRoom = 29,
    /// <summary>
    /// 请求准备
    /// </summary>
    ReqReady = 30,
    /// <summary>
    /// 下发准备
    /// </summary>
    ResReady = 31,
    /// <summary>
    /// 请求取消准备
    /// </summary>
    ReqQuitReady = 32,
    /// <summary>
    /// 下发取消准备
    /// </summary>
    ResQuitReady = 33,
    /// <summary>
    /// 请求快捷语音
    /// </summary>
    ReqQuickVoice = 34,
    /// <summary>
    /// 下发快捷语音
    /// </summary>
    ResQuickVoice = 35,
    /// <summary>
    /// 下发下注信息
    /// </summary>
    ResBet = 36,
}
