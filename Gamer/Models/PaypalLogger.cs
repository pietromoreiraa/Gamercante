﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Gamer.Models
{
    public class PaypalLogger
    {
        public static string LogDirectoryPath = "C:\\Users\\pl_ta\\source\\repos\\pietromoreiraa\\Gamercante\\Gamer\\Error";
        public static void log(String messages)
        {
            try
            {
                StreamWriter strw = new StreamWriter(LogDirectoryPath + "\\PaypalError.log", true);
                strw.WriteLine("{0}--->{1}", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), messages);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}