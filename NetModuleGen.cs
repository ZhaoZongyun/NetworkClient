using HT.Framework;
using Google.Protobuf;
using ProtobufGen;
using System.Collections.Generic;

/// <summary>
/// NetMgr接收网络消息部分
///     由编辑器生成
/// </summary>
public partial class NetModule : CustomModuleBase
{
    #region 发送消息

    /// <summary>
    /// 请求角色信息
    /// </summary>
    public void ReqCharacterInfo()
    {
        ReqCharacterInfo data = new ReqCharacterInfo();
        byte[] totalBytes = CreateTotalBytes(NetMessageId.ReqCharacterInfo, data);
        Send(NetMessageId.ReqCharacterInfo, totalBytes);
    }

    /// <summary>
    /// 请求创建角色
    /// </summary>
    /// <param name="nickname">昵称</param>
    /// <param name="sex">角色性别（1 男 2 女）</param>
    public void ReqCreateCharacter(string nickname, int sex)
    {
        ReqCreateCharacter data = new ReqCreateCharacter();
        data.Nickname = nickname;
        data.Sex = sex;
        byte[] totalBytes = CreateTotalBytes(NetMessageId.ReqCreateCharacter, data);
        Send(NetMessageId.ReqCreateCharacter, totalBytes);
    }

    /// <summary>
    /// 请求登录
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    public void ReqLogin(string username, string password)
    {
        ReqLogin data = new ReqLogin();
        data.Username = username;
        data.Password = password;
        byte[] totalBytes = CreateTotalBytes(NetMessageId.ReqLogin, data);
        Send(NetMessageId.ReqLogin, totalBytes);
    }

    /// <summary>
    /// 请求登出
    /// </summary>
    public void ReqLogout()
    {
        ReqLogout data = new ReqLogout();
        byte[] totalBytes = CreateTotalBytes(NetMessageId.ReqLogout, data);
        Send(NetMessageId.ReqLogout, totalBytes);
    }

    /// <summary>
    /// 请求注册
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    public void ReqRegister(string username, string password)
    {
        ReqRegister data = new ReqRegister();
        data.Username = username;
        data.Password = password;
        byte[] totalBytes = CreateTotalBytes(NetMessageId.ReqRegister, data);
        Send(NetMessageId.ReqRegister, totalBytes);
    }

    /// <summary>
    /// 请求游客登陆
    /// </summary>
    public void ReqVisitorLogin()
    {
        ReqVisitorLogin data = new ReqVisitorLogin();
        byte[] totalBytes = CreateTotalBytes(NetMessageId.ReqVisitorLogin, data);
        Send(NetMessageId.ReqVisitorLogin, totalBytes);
    }

    /// <summary>
    /// 请求叫地主或不叫地主
    /// </summary>
    /// <param name="reqCode">叫地主或不叫（1 叫地主 2 不叫）</param>
    public void ReqCallBoss(int reqCode)
    {
        ReqCallBoss data = new ReqCallBoss();
        data.ReqCode = reqCode;
        byte[] totalBytes = CreateTotalBytes(NetMessageId.ReqCallBoss, data);
        Send(NetMessageId.ReqCallBoss, totalBytes);
    }

    /// <summary>
    /// 请求出牌
    /// </summary>
    /// <param name="pushCards">出的牌</param>
    public void ReqPushCard(IEnumerable<int> pushCards)
    {
        ReqPushCard data = new ReqPushCard();
        data.PushCards.AddRange(pushCards);
        byte[] totalBytes = CreateTotalBytes(NetMessageId.ReqPushCard, data);
        Send(NetMessageId.ReqPushCard, totalBytes);
    }

    /// <summary>
    /// 请求不出
    /// </summary>
    public void ReqPassCard()
    {
        ReqPassCard data = new ReqPassCard();
        byte[] totalBytes = CreateTotalBytes(NetMessageId.ReqPassCard, data);
        Send(NetMessageId.ReqPassCard, totalBytes);
    }

    /// <summary>
    /// 请求进入房间
    /// </summary>
    public void ReqEnterRoom()
    {
        ReqEnterRoom data = new ReqEnterRoom();
        byte[] totalBytes = CreateTotalBytes(NetMessageId.ReqEnterRoom, data);
        Send(NetMessageId.ReqEnterRoom, totalBytes);
    }

    /// <summary>
    /// 请求退出房间
    /// </summary>
    public void ReqExitRoom()
    {
        ReqExitRoom data = new ReqExitRoom();
        byte[] totalBytes = CreateTotalBytes(NetMessageId.ReqExitRoom, data);
        Send(NetMessageId.ReqExitRoom, totalBytes);
    }

    /// <summary>
    /// 请求准备
    /// </summary>
    public void ReqReady()
    {
        ReqReady data = new ReqReady();
        byte[] totalBytes = CreateTotalBytes(NetMessageId.ReqReady, data);
        Send(NetMessageId.ReqReady, totalBytes);
    }

    /// <summary>
    /// 请求取消准备
    /// </summary>
    public void ReqQuitReady()
    {
        ReqQuitReady data = new ReqQuitReady();
        byte[] totalBytes = CreateTotalBytes(NetMessageId.ReqQuitReady, data);
        Send(NetMessageId.ReqQuitReady, totalBytes);
    }

    /// <summary>
    /// 请求快捷语音
    /// </summary>
    /// <param name="cfgId">配置id</param>
    public void ReqQuickVoice(int cfgId)
    {
        ReqQuickVoice data = new ReqQuickVoice();
        data.CfgId = cfgId;
        byte[] totalBytes = CreateTotalBytes(NetMessageId.ReqQuickVoice, data);
        Send(NetMessageId.ReqQuickVoice, totalBytes);
    }

    #endregion

    /// <summary>
    /// 接收到消息
    /// </summary>
    public void OnReceive(NetMessageId messageId, byte[] contentBytes)
    {
        IMessage data = null;
        switch (messageId)
        {
            case NetMessageId.ResCharacterInfo:
                data = ProtobufGen.ResCharacterInfo.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResCreateCharacter:
                data = ProtobufGen.ResCreateCharacter.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResErrorNotice:
                data = ProtobufGen.ResErrorNotice.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResLogin:
                data = ProtobufGen.ResLogin.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResLogout:
                data = ProtobufGen.ResLogout.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResRegister:
                data = ProtobufGen.ResRegister.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResHandCards:
                data = ProtobufGen.ResHandCards.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResWillCallBossSeat:
                data = ProtobufGen.ResWillCallBossSeat.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResCallBoss:
                data = ProtobufGen.ResCallBoss.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResHoldCard:
                data = ProtobufGen.ResHoldCard.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResPushSeat:
                data = ProtobufGen.ResPushSeat.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResPushCard:
                data = ProtobufGen.ResPushCard.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResPassCard:
                data = ProtobufGen.ResPassCard.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResGameOver:
                data = ProtobufGen.ResGameOver.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResEnterRoom:
                data = ProtobufGen.ResEnterRoom.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResExitRoom:
                data = ProtobufGen.ResExitRoom.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResOtherEnterRoom:
                data = ProtobufGen.ResOtherEnterRoom.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResOtherExitRoom:
                data = ProtobufGen.ResOtherExitRoom.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResReady:
                data = ProtobufGen.ResReady.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResQuitReady:
                data = ProtobufGen.ResQuitReady.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResQuickVoice:
                data = ProtobufGen.ResQuickVoice.Parser.ParseFrom(contentBytes);
                break;
            case NetMessageId.ResBet:
                data = ProtobufGen.ResBet.Parser.ParseFrom(contentBytes);
                break;
        }

        if (data != null)
        {
            CallNetMessageEvent(messageId, data);
        }
    }
}
