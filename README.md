# MxDebug
Unity远程调试工具
MxDebug 是一个跨平台日记管理工具，是 MXFramework框架 的一个子模块。可以不需链接USB线，输出Unity所有级别的Log信息，整个模块解耦合，
低入侵，简单易用，对已经开发好的项目只需要一句API就可以集成进去。不需要时候可以随时移除。 整个UI界面和Unity “Console”UI界面基本一样。
工具分为接收端和输入端。接收端已经开发好了提供源码，输入端是集成到你现有项目中去的，只需要一句代码就会输出你原有项目中的(Debug和Print和异常信息)。
还可以关闭所有日记输出以节省性能。

API

1.打开调试功能(默认是开启的,正式上线的时候建议关闭)

public static void OpenDebug(){}

功能默认是打开，会输入Unity所有级别的Log信息

2.关闭调试功能（建议项目正式上线的时候关闭所有日记输出）

public static void CloseDebug(){}

该工能是关闭Unity的所有日记输出，关闭所有级别的log信息，建议项目正式上线的时候关闭所有日记输出以便节省性能。

3.远程调试

public static void RemoteDebug(){} 

该功能是用于项目远程调试时候使用，将所有log信息会通过UDP协议发送给电脑Debug工具(接收端)，这个就可以不需要连接USB线，和不通过ADB命令和Xcode软件也能查看全部log。
当应用发生异常或者错误的时候会根据时间保存一份txt格式的错误信息到本地，会详细的记录发生异常的原因，时间，和是由哪些类，引发的异常或错误。


视频教程：https://www.bilibili.com/video/BV1sK411A7Bd
博客教程：https://blog.csdn.net/a451319296/article/details/108985330

QQ交流群：1079467433 
