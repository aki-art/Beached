using System;

namespace Beached.Utils
{
    public class NoteAttribute : Attribute
    {
        public string message;

        public NoteAttribute(string message)
        {
            this.message = message;
        }
    }
}
