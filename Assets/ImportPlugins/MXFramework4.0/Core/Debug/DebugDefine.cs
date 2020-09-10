using UnityEngine;
using System;

namespace Mx.Log
{
    public sealed class DebugDefine
    {
        /// <summary>UTP端口号</summary>
        public const int UTP_PORT = 9621;

        /// <summary>是否打印日记</summary>
        public static bool IsPrintLog
        {
            get
            {
                #if PRINT_LOG
                bool printLog = true;
                #else
                bool printLog = false;
                #endif
                return printLog;
            }
        }

        /// <summary>是否是调试模式</summary>
        public static bool IsDebugMode
        {
            get
            {
                #if DEBUG_MODE
                bool debugMode = true;
                #else
                bool debugMode = false;
                #endif
                return debugMode;
            }
        }
    }

    [Serializable]
    public class DebugData
    {
        public string ID;
        public int Count = 1;
        public string Condition;
        public string StackTrace;
        public LogType Type;
        public string Tiem;
    }
}