using UnityEngine;
using UnityEngine.TestTools;

public class AmazonTest
{
    public static class LogAssert
    {
        public static void Expect(LogType logType, string message)
        {
            UnityEngine.TestTools.LogAssert.Expect(logType, message);
            Debug.LogFormat("The previous {0} log was expected.", logType);
        }
    }
}
