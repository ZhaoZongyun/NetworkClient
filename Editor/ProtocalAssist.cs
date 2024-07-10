using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

/// <summary>
/// 协议工具类
/// </summary>
public class ProtocalAssist
{
    [MenuItem("proto/生成proto代码")]
    public static void RunBat()
    {
        string fileName = Application.dataPath + "/Network/Protobuf/Tool/protoGen.bat";
        Thread thread = new Thread(delegate ()
        {
            var info = new ProcessStartInfo();
            info.FileName = fileName;
            info.CreateNoWindow = false;
            info.UseShellExecute = false;
            info.RedirectStandardInput = false;
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;

            info.StandardErrorEncoding = Encoding.GetEncoding("GB2312");
            info.StandardOutputEncoding = Encoding.GetEncoding("GB2312");
            Process.Start(info);

            //using (var myProcess = Process.Start(info))
            //{
            //    var output = myProcess.StandardOutput.ReadToEnd();
            //    Debug.Log(output); // 输出标准输出的内容
            //    var err = myProcess.StandardError.ReadToEnd();
            //    Debug.Log(err); // 输出标准错误的内容

            //    myProcess.WaitForExit();

            //    var exitCode = myProcess.ExitCode;
            //    Debug.Log($"Process Exit Code {exitCode}");
            //}
        });
        thread.Start();
    }

    [MenuItem("proto/生成NetModule代码")]
    public static void GenNetModuleCode()
    {
        string protoPath = Application.dataPath + "/Network/Protobuf/ProtoFiles";

        string clientMessageIdFile = Application.dataPath + "/Network/NetMessageId.cs";
        string serverMessageIdFile = "E:/CSharpProject/TestUDPServer/Struct/NetMessageId.cs";

        string clientNetMgrFile = Application.dataPath + "/Network/NetModuleGen.cs";
        string serverNetMgrFile = "E:/CSharpProject/TestUDPServer/Mgr/NetMgrGen.cs";

        if (!File.Exists(clientMessageIdFile))
        {
            string dir = Path.GetDirectoryName(clientMessageIdFile);
            Directory.CreateDirectory(dir);
            File.Create(clientMessageIdFile);
        }
        if (!File.Exists(serverMessageIdFile))
        {
            string dir = Path.GetDirectoryName(serverMessageIdFile);
            Directory.CreateDirectory(dir);
            File.Create(serverMessageIdFile);
        }

        if (!File.Exists(clientNetMgrFile))
        {
            string dir = Path.GetDirectoryName(clientNetMgrFile);
            Directory.CreateDirectory(dir);
            File.Create(clientNetMgrFile);
        }
        if (!File.Exists(serverNetMgrFile))
        {
            string dir = Path.GetDirectoryName(serverNetMgrFile);
            Directory.CreateDirectory(dir);
            File.Create(serverNetMgrFile);
        }

        List<Message> list = new List<Message>();
        List<Message> reqlist = new List<Message>();
        List<Message> reslist = new List<Message>();
        string[] protoFiles = Directory.GetFiles(protoPath, "*.proto");
        for (int i = 0; i < protoFiles.Length; i++)
        {
            Message message = null;
            string file = protoFiles[i];
            string[] lines = File.ReadAllLines(file);

            for (int j = 0; j < lines.Length; j++)
            {
                string line = lines[j];
                line = line.Trim();

                if (line == "")
                    continue;

                if (message == null)
                {
                    if (line.StartsWith("//"))
                    {
                        message = new Message();
                        message.desc = line.Substring(line.LastIndexOf('/') + 1);
                        message.fieldList = new List<Field>();
                    }
                    continue;
                }

                if (line.StartsWith("message"))
                {
                    string[] splits = line.Split(' ');
                    if (splits[1].StartsWith("Res") || splits[1].StartsWith("Req"))
                    {
                        message.name = splits[1].Substring(0, splits[1].IndexOf('{'));
                        message.type = message.name.StartsWith("Req") ? MsgType.req : MsgType.res;
                    }
                    else
                    {
                        message = null;
                    }
                }
                else if (line.StartsWith("}"))
                {
                    list.Add(message);
                    if (message.type == MsgType.req)
                        reqlist.Add(message);
                    else
                        reslist.Add(message);
                    message = null;
                }
                else
                {
                    string[] splits = line.Split(' ');
                    Field field = new Field();
                    if (splits[0] == "repeated")
                    {
                        field.type = FieldType.list;
                        field.subType = splits[1];
                        field.name = splits[2];
                    }
                    else
                    {
                        field.type = FieldType.none;
                        field.subType = splits[0];
                        field.name = splits[1];
                    }

                    for (int k = 0; k < splits.Length; k++)
                    {
                        if (splits[k].StartsWith("//"))
                        {
                            field.desc = line.Substring(line.LastIndexOf('/') + 1);
                            break;
                        }
                    }
                    message.fieldList.Add(field);
                }
            }
        }

        //NetMessageId.cs
        StringBuilder messageIdSb = new StringBuilder();
        messageIdSb.AppendLine("/// <summary>");
        messageIdSb.AppendLine("/// 消息号");
        messageIdSb.AppendLine("/// 由编辑器生成");
        messageIdSb.AppendLine("/// </summary>");
        messageIdSb.AppendLine("public enum NetMessageId : ushort //(0 - 65535)");
        messageIdSb.AppendLine("{");

        for (int i = 0; i < list.Count; i++)
        {
            var msg = list[i];
            messageIdSb.AppendLine("    /// <summary>");
            messageIdSb.AppendLine("    /// " + msg.desc);
            messageIdSb.AppendLine("    /// </summary>");
            messageIdSb.AppendLine("    " + msg.name + " = " + (i + 1) + ",");
        }
        messageIdSb.AppendLine("}");

        File.WriteAllText(clientMessageIdFile, messageIdSb.ToString());

        StringBuilder tempSb = new StringBuilder();
        tempSb.AppendLine("namespace TestUDPServer");
        tempSb.AppendLine("{");
        messageIdSb.Insert(0, tempSb.ToString());
        messageIdSb.AppendLine("}");
        File.WriteAllText(serverMessageIdFile, messageIdSb.ToString());

        Debug.Log("NetMessageId.cs完成，消息数：" + list.Count + " 上发数：" + reqlist.Count + " 下发数：" + reslist.Count);

        //客户端 NetMgrGen.cs
        StringBuilder netMgrSb = new StringBuilder();
        netMgrSb.AppendLine("using HT.Framework;");
        netMgrSb.AppendLine("using Google.Protobuf;");
        netMgrSb.AppendLine("using ProtobufGen;");
        netMgrSb.AppendLine("using System.Collections.Generic;");
        netMgrSb.AppendLine();
        netMgrSb.AppendLine("/// <summary>");
        netMgrSb.AppendLine("/// NetMgr接收网络消息部分");
        netMgrSb.AppendLine("///     由编辑器生成");
        netMgrSb.AppendLine("/// </summary>");
        netMgrSb.AppendLine("public partial class NetModule : CustomModuleBase");
        netMgrSb.AppendLine("{");
        netMgrSb.AppendLine("    #region 发送消息");
        netMgrSb.AppendLine();

        for (int i = 0; i < reqlist.Count; i++)
        {
            var msg = reqlist[i];
            netMgrSb.AppendLine("    /// <summary>");
            netMgrSb.AppendLine("    /// " + msg.desc);
            netMgrSb.AppendLine("    /// </summary>");
            string paramStr = "";
            for (int j = 0; j < msg.fieldList.Count; j++)
            {
                var field = msg.fieldList[j];
                netMgrSb.AppendLine("    /// <param name=\"" + field.name + "\">" + field.desc + "</param>");
                if (j < msg.fieldList.Count - 1)
                    paramStr += GetType(field) + " " + field.name + ", ";
                else
                    paramStr += GetType(field) + " " + field.name;
            }
            netMgrSb.AppendLine("    public void " + msg.name + "(" + paramStr + ")");
            netMgrSb.AppendLine("    {");
            netMgrSb.AppendLine("        " + msg.name + " data = new " + msg.name + "();");
            for (int j = 0; j < msg.fieldList.Count; j++)
            {
                var field = msg.fieldList[j];
                string fieldName = field.name.Substring(0, 1).ToUpper() + field.name.Substring(1);
                if (field.type == FieldType.none)
                    netMgrSb.AppendLine("        data." + fieldName + " = " + field.name + ";");
                else
                    netMgrSb.AppendLine("        data." + fieldName + ".AddRange(" + field.name + ");");
            }
            netMgrSb.AppendLine("        byte[] totalBytes = CreateTotalBytes(NetMessageId." + msg.name + ", data);");
            netMgrSb.AppendLine("        Send(NetMessageId." + msg.name + ", totalBytes);");
            netMgrSb.AppendLine("    }");
            netMgrSb.AppendLine();
        }

        netMgrSb.AppendLine("    #endregion");
        netMgrSb.AppendLine();

        netMgrSb.AppendLine("    /// <summary>");
        netMgrSb.AppendLine("    /// 接收到消息");
        netMgrSb.AppendLine("    /// </summary>");
        netMgrSb.AppendLine("    public void OnReceive(NetMessageId messageId, byte[] contentBytes)");
        netMgrSb.AppendLine("    {");
        netMgrSb.AppendLine("        IMessage data = null;");
        netMgrSb.AppendLine("        switch (messageId)");
        netMgrSb.AppendLine("        {");
        for (int i = 0; i < reslist.Count; i++)
        {
            var msg = reslist[i];
            netMgrSb.AppendLine("            case NetMessageId." + msg.name + ":");
            netMgrSb.AppendLine("                data = ProtobufGen." + msg.name + ".Parser.ParseFrom(contentBytes);");
            netMgrSb.AppendLine("                break;");
        }
        netMgrSb.AppendLine("        }");
        netMgrSb.AppendLine();
        netMgrSb.AppendLine("        if (data != null)");
        netMgrSb.AppendLine("        {");
        netMgrSb.AppendLine("            CallNetMessageEvent(messageId, data);");
        netMgrSb.AppendLine("        }");
        netMgrSb.AppendLine("    }");
        netMgrSb.AppendLine("}");
        File.WriteAllText(clientNetMgrFile, netMgrSb.ToString());

        //服务器 NetMgrGen.cs
        netMgrSb.Clear();
        netMgrSb.AppendLine("using Google.Protobuf;");
        netMgrSb.AppendLine("using System.Collections.Generic;");
        netMgrSb.AppendLine();
        netMgrSb.AppendLine("namespace TestUDPServer");
        netMgrSb.AppendLine("{");
        netMgrSb.AppendLine("    /// <summary>");
        netMgrSb.AppendLine("    /// NetMgr接收网络消息部分");
        netMgrSb.AppendLine("    ///     由编辑器生成");
        netMgrSb.AppendLine("    /// </summary>");
        netMgrSb.AppendLine("    public partial class NetMgr");
        netMgrSb.AppendLine("    {");
        netMgrSb.AppendLine("        /// <summary>");
        netMgrSb.AppendLine("        /// 接收到消息");
        netMgrSb.AppendLine("        /// </summary>");
        netMgrSb.AppendLine("        private void OnReceive(NetMessageId messageId, byte[] contentBytes)");
        netMgrSb.AppendLine("        {");
        netMgrSb.AppendLine("            IMessage data = null;");
        netMgrSb.AppendLine("            switch (messageId)");
        netMgrSb.AppendLine("            {");
        for (int i = 0; i < reqlist.Count; i++)
        {
            var msg = reqlist[i];
            netMgrSb.AppendLine("                case NetMessageId." + msg.name + ":");
            netMgrSb.AppendLine("                    data = ProtobufGen." + msg.name + ".Parser.ParseFrom(contentBytes);");
            netMgrSb.AppendLine("                    break;");
        }
        netMgrSb.AppendLine("            }");
        netMgrSb.AppendLine();
        netMgrSb.AppendLine("            if (data != null)");
        netMgrSb.AppendLine("            {");
        netMgrSb.AppendLine("                CallNetMessageEvent(messageId, data);");
        netMgrSb.AppendLine("            }");
        netMgrSb.AppendLine("        }");
        netMgrSb.AppendLine();
        netMgrSb.AppendLine("        #region 发送消息");
        netMgrSb.AppendLine();

        for (int i = 0; i < reslist.Count; i++)
        {
            var msg = reslist[i];
            netMgrSb.AppendLine("        /// <summary>");
            netMgrSb.AppendLine("        /// " + msg.desc);
            netMgrSb.AppendLine("        /// </summary>");
            string paramStr = "";
            for (int j = 0; j < msg.fieldList.Count; j++)
            {
                var field = msg.fieldList[j];
                netMgrSb.AppendLine("        /// <param name=\"" + field.name + "\">" + field.desc + "</param>");
                if (j < msg.fieldList.Count - 1)
                    paramStr += GetType(field) + " " + field.name + ", ";
                else
                    paramStr += GetType(field) + " " + field.name;
            }
            netMgrSb.AppendLine("        public void " + msg.name + "(" + paramStr + ")");
            netMgrSb.AppendLine("        {");
            netMgrSb.AppendLine("            ProtobufGen." + msg.name + " data = new ProtobufGen." + msg.name + "();");
            for (int j = 0; j < msg.fieldList.Count; j++)
            {
                var field = msg.fieldList[j];
                string fieldName = field.name.Substring(0, 1).ToUpper() + field.name.Substring(1);
                if (field.type == FieldType.none)
                    netMgrSb.AppendLine("            data." + fieldName + " = " + field.name + ";");
                else
                    netMgrSb.AppendLine("            data." + fieldName + ".AddRange(" + field.name + ");");
            }
            netMgrSb.AppendLine("            byte[] totalBytes = CreateTotalBytes(NetMessageId." + msg.name + ", data);");
            netMgrSb.AppendLine("            Send(NetMessageId." + msg.name + ", totalBytes);");
            netMgrSb.AppendLine("        }");
            netMgrSb.AppendLine();
        }

        netMgrSb.AppendLine("        #endregion");
        netMgrSb.AppendLine("    }");
        netMgrSb.AppendLine("}");
        File.WriteAllText(serverNetMgrFile, netMgrSb.ToString());

        Debug.Log("NetMgrGen.cs完成");
    }

    private static string GetType(Field field)
    {
        if (field.type == FieldType.none)
        {
            return GetSubType(field.subType);
        }
        else if (field.type == FieldType.list)
        {
            return "IEnumerable<" + GetSubType(field.subType) + ">";
        }
        return "";
    }

    private static string GetSubType(string subType)
    {
        if (subType == "int32")
        {
            return "int";
        }
        else if (subType == "int64")
        {
            return "long";
        }
        else if (subType == "string")
        {
            return subType;
        }
        else
        {
            return "ProtobufGen." + subType;
        }
    }

    internal enum MsgType
    {
        none,
        req,
        res,
    }

    private class Message
    {
        internal string desc;
        internal MsgType type;
        internal string name;
        internal List<Field> fieldList;
    }

    internal enum FieldType
    {
        none,
        list,
    }

    private class Field
    {
        internal FieldType type;
        internal string subType;
        internal string name;
        internal string desc;
    }
}

