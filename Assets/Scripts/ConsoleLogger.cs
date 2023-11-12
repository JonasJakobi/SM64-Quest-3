using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;
public class ConsoleLogger : MonoBehaviour
{
    public TextMeshPro logText; // Reference to your TextMeshPro component
    private void Start() {
 
        
    }
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logText, string stackTrace, LogType type)
    {
        string logMessage = $"{type}: {logText}";

        // Append the log message to the TextMeshPro component
        this.logText.text += logMessage + "\n";

        // Optionally, you can limit the number of lines displayed to prevent excessive growth
        int maxLines = 30;
        string[] lines = this.logText.text.Split('\n');
        if (lines.Length > maxLines)
        {
            this.logText.text = string.Join("\n", lines, lines.Length - maxLines, maxLines);
        }
    }

}
