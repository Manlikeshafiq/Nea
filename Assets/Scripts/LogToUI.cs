using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LogToUI : MonoBehaviour
{
    public TMP_Text logText; // Assign this in the inspector
    private Queue<string> logQueue = new Queue<string>();
    public int maxLines = 10; // Max lines to keep on screen

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string colorTag = type == LogType.Warning ? "<color=yellow>" : (type == LogType.Error ? "<color=red>" : "");
        string message = $"{colorTag}{logString}</color>";

        logQueue.Enqueue(message);

        if (logQueue.Count > maxLines)
            logQueue.Dequeue();

        logText.text = string.Join("\n", logQueue.ToArray());
    }
}