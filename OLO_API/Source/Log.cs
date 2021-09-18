using System;
using System.Collections.Generic;
using System.Text;

namespace OLO_API
{
    //Simple logging class that executed ToString() on any provided object that's not a String
    public static class Log
    {
        public static void Error(string message)
        {
            Console.WriteLine("[ERROR] " + message);
        }
        public static void Info(string message)
        {
            Console.WriteLine("[INFO] " + message);
        }
        public static void Error(object obj)
        {
            Error(obj.ToString());
        }
        public static void Info(object obj)
        {
            Info(obj.ToString());
        }


    }
}
