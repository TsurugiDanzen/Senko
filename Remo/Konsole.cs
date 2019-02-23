﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Remo
{
    public class Konsole
    {
        public enum Prefix { None, Prompt, Indent, Auto }
        public enum NewLine { None, Above, Below, Both }
        public enum KonsoleWrite { Inline, Newline }

        static public bool VerboseMode = true;
        static public bool IgnoreColor = false;
        static public Prefix ForcePrefix = Prefix.Auto;
        static public Prefix LastPrefix = Prefix.Prompt;
        static public KonsoleWrite LastWrite = KonsoleWrite.Newline;
      
        static public string Prompt = "REMO> ";
        static public ConsoleColor formerColor;
        public ConsoleColor Color { get; set; }

        public Konsole(ConsoleColor color, string prompt)
        {
            Console.CursorVisible = false;
            Prompt = prompt;
            Color = color;

            SetFormerColor();
        }

        public Konsole(ConsoleColor color)
        {
            Console.CursorVisible = false;
            Color = color;

            SetFormerColor();
        }

        public Konsole(string prompt)
        {
            Console.CursorVisible = false;
            Prompt = prompt;

            SetFormerColor();
        }

        public Konsole()
        {
            Console.CursorVisible = false;
            SetFormerColor();
        }

        // Write methods and overrides

        public void Write(NewLine newline, Prefix prefix, object text, params object[] args)
        {
            string newString = "";
            
            string oldString = (text == null) ? "" : text.ToString();

            if (VerboseMode)
            {
                ToggleColor();

                if (ForcePrefix != Prefix.Auto)
                {
                    prefix = ForcePrefix;
                }

                if (prefix == Prefix.Auto)
                {
                    prefix = LastPrefix;
                }

                LastPrefix = prefix;

                if (prefix == Prefix.Prompt)
                {
                    if (LastWrite == KonsoleWrite.Newline || (newline == NewLine.Above || newline == NewLine.Both))
                    {
                        newString = Prompt;
                    }
                }
                else if (prefix == Prefix.Indent)
                {
                    if (LastWrite == KonsoleWrite.Newline || (newline == NewLine.Above || newline == NewLine.Both))
                    {
                        for (int i = 0; i < Prompt.Length; i++)
                        {
                            newString += " ";
                        }
                    }
                }

                for (int i = 0; i < args.Length; i++)
                {
                    oldString = oldString.Replace("{" + i + "}", args[i].ToString());
                }

                newString += oldString;

                if (newline == NewLine.Above || newline == NewLine.Both)
                {
                    newString = Environment.NewLine + newString;
                }

                if (newline == NewLine.Below || newline == NewLine.Both)
                {
                    newString += Environment.NewLine;
                }

                LastWrite = (newline == NewLine.Above || newline == NewLine.None) ? KonsoleWrite.Inline : KonsoleWrite.Newline;
                
                Console.Write(newString);
                ToggleColor();
            }
        }

        public void Write(Prefix prefix, object text, params object[] args)
        {
            Write(NewLine.None, prefix, text, args);
        }

        public void Write(object text, params object[] args)
        {
            Write(NewLine.None, Prefix.Auto, text, args);
        }        

        public void WriteLine(Prefix prefix, object text, params object[] args)
        {
            Write(NewLine.Below, prefix, text, args);
        }

        public void WriteLine(object text, params object[] args)
        {
            Write(NewLine.Below, Prefix.Auto, text, args);
        }

        public void WriteLine()
        {
            Prefix formerPrefix = LastPrefix;

            Write(NewLine.Below, Prefix.None, null);

            LastPrefix = formerPrefix;
        }

        public void SetFormerColor()
        {
            formerColor = Console.ForegroundColor;
        }

        public void ToggleColor()
        {
            if (!IgnoreColor)
            {
                if (Console.ForegroundColor != Color)
                {
                    SetFormerColor();
                    Console.ForegroundColor = Color;
                }
                else
                {
                    if (Color != formerColor)
                    {
                        Console.ForegroundColor = formerColor;
                    }
                }
            }
            else
            {
                Console.ResetColor();
            }
        }
    }
}
