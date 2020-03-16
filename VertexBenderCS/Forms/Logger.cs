using System;
using System.Collections.Generic;

namespace VertexBenderCS.Forms
{
    public class Logger
    {
        public Action<string> OnItemLogged;
        public Action<string> OnLogCleaned;

        public Logger()
        {
            Log = new List<string>();
        }

        public readonly List<string> Log;

        public void Append(string text)
        {
            Log.Add(text);
            OnItemLogged?.Invoke(text);
        }

        public void Clear()
        {
            Log.Clear();
            OnLogCleaned?.Invoke("");
        }
    }

}
