using ImGuiNET;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Beached.ModDevTools
{
    // just a log filtered to only my mods messages
    public class ConsoleDevTool : DevTool
    {
        public const int LENGTH = 512;
        private static readonly Queue<LogEntry> logEntries = new();
        private static Color debugColor = new(0.7f, 0.7f, 0.7f);
        private static Color warningColor = Util.ColorFromHex("ee7539");

        public ConsoleDevTool()
        {
        }

        public static void AddToLog(LogType type, string msg)
        {
            msg = $"[{System.DateTime.UtcNow:HH:mm:ss.fffffff}] {msg}";
            logEntries.Enqueue(new LogEntry()
            {
                type = type,
                message = msg
            });

            if (logEntries.Count > LENGTH)
            {
                logEntries.Dequeue();
            }
        }

        public override void RenderTo(DevPanel panel)
        {
            foreach (var entry in logEntries)
            {
                switch (entry.type)
                {
                    case LogType.Debug:
                        ImGui.TextColored(debugColor, entry.message);
                        break;
                    case LogType.Info:
                        ImGui.Text(entry.message);
                        break;
                    case LogType.Warning:
                        ImGui.TextColored(warningColor, entry.message);
                        break;
                }
            }

            if(ImGui.Button("Copy"))
            {
                var msg = new StringBuilder();
                foreach (var entry in logEntries)
                {
                    msg.AppendLine(entry.message);
                }

                ImGui.SetClipboardText(msg.ToString());
            }
        }

        struct LogEntry
        {
            public LogType type;
            public string message;
        }

        public enum LogType
        {
            Debug,
            Warning,
            Info
        }
    }
}
