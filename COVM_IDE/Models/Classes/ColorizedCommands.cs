using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace COVM_IDE
{
    public class ColorizedCommands : Singleton<ColorizedCommands>
    {
        public struct HighlightCommand
        {
            public string command;
            public System.Windows.Media.Brush forecolor;
            public System.Windows.Media.Brush background;
            public bool bold;
            public bool italic;
            public int size;   
        }


        public void Add(HighlightCommand command)
        {
            Commands.Add(command);
        }

        public void Delete(HighlightCommand command)
        {
            Commands.Remove(command);
        }

        public List<HighlightCommand> Get()
        {
            return Commands;
        }

        private static List<HighlightCommand> Commands = new List<HighlightCommand>();
    }
}
