using System;
using System.Collections.Generic;

namespace Engine.Core
{
    public static class Logger
    {
        public static Action<string> OnItemLogged;
        public static Action<string> OnLogCleaned;

        public static readonly object lockObject = new object();

        public static readonly List<string> LogItems = new List<string>();

        public static void Log(string text)
        {
            lock (lockObject)
            {
                LogItems.Add(text);
                OnItemLogged?.Invoke(text);
            }
        }

        public static void Clear()
        {
            lock (lockObject)
            {
                LogItems.Clear();
                OnLogCleaned?.Invoke("");
            }
        }
    }
}
