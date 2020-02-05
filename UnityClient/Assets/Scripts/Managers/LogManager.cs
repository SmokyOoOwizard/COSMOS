using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace COSMOS.Managers
{
    internal static class LogManager
    {
        public static void Init()
        {
            Delogger.LogMaster.Stop();
            Delogger.LogSettings.Processing.ExecuteLogProcessing = true;
            Delogger.LogMaster.Init();
            Delogger.LogMaster.RemoveLogProcessing(LogProcess);
            Delogger.LogMaster.AddLogProcessing(LogProcess);
        }
        public static void Deinit()
        {
            Delogger.LogMaster.Stop();
            Delogger.LogMaster.RemoveLogProcessing(LogProcess);
        }
        static void LogProcess(Delogger.LogString log)
        {
            if(log.level == Delogger.LogLevel.Info)
            {
                Debug.unityLogger.Log(LogType.Log, log.ToString());
            }
            if (log.level == Delogger.LogLevel.Warning || log.level == Delogger.LogLevel.Debug)
            {
                Debug.unityLogger.Log(LogType.Warning, log.ToString());
            }
            if(log.level == Delogger.LogLevel.Error || log.level == Delogger.LogLevel.Panic || log.level == Delogger.LogLevel.Fatal)
            {
                Debug.unityLogger.Log(LogType.Error, log.ToString());
            }
        }
    }
}
