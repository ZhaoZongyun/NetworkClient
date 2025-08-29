### 一、UDP_TCP 分支：UDP和TCP消息收发
#### 1. 消息接收方式：（TCP或UDP）
（1）方式一，异步调用 + 尾递归
（2）方式二，线程阻塞
（3）方式三，Task + async/await
#### 2. TCP 消息发送方式：
（1）方式一，用 NetworkStream
（2）方式二，用 TcpClient.Client.Send (内部为 Socket)
#### 3. UDP 消息发送方式：
UdpClient.Send (内部为 Socket)
### 二、UDP_Protobuf 分支：支持UDP消息收发和Protobuf解析、封装消息
####
1. Protobuf 版本为 v3.12.0
2. Protobuf 地址：https://github.com/protocolbuffers/protobuf/releases/tag/v3.12.0
3. Protobuf 文档：https://protobuf.dev/getting-started/csharptutorial/
