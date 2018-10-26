using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace PingoDestroyer
{
    public class Logger
    {
        public static readonly int LOGLEVEL_NONE    = 5;
        public static readonly int LOGLEVEL_ERROR   = 4;
        public static readonly int LOGLEVEL_WARNING = 3;
        public static readonly int LOGLEVEL_INFO    = 2;
        public static readonly int LOGLEVEL_DEBUG = 1;
        public static readonly int LOGLEVEL_VERBOSE = 0;

        public static readonly int[] LOGLEVELS = {
                                                     LOGLEVEL_NONE,
                                                     LOGLEVEL_ERROR,
                                                     LOGLEVEL_WARNING,
                                                     LOGLEVEL_INFO,
                                                     LOGLEVEL_DEBUG,
                                                     LOGLEVEL_VERBOSE
                                                 };

        public static readonly Color[] LOGLEVEL_COLORS = {
                                                             Color.Green,   // NONE / FORCE
                                                             Color.Red,     // ERROR
                                                             Color.Orange,  // WARNING
                                                             Color.Blue,    // INFO
                                                             Color.Black,   // DEBUG
                                                             Color.DarkGray // VERBOSE
                                                         };

        private static object locker = new object();
        private static String _path = "log.txt";
        private static String _pathHash = "logging_" + sha512(Path.GetFullPath(_path));
        public static String path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                _pathHash = "logging_" + sha512(Path.GetFullPath(_path));
            }
        }
        public static int logLevel = LOGLEVEL_NONE;

        private Logger() { }

        public static String sha512(String input)
        {
            SHA512 sha = new SHA512Managed();
            StringBuilder sb = new StringBuilder();
            byte[] hashedBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (byte b in hashedBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        public static String md5(String input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            StringBuilder sb = new StringBuilder();
            byte[] hashedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (byte b in hashedBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        public static int parseLogLevel(String text)
        {
            switch (text.ToUpper())
            {
                case "VERBOSE":
                    return LOGLEVEL_VERBOSE;
                case "DEBUG":
                    return LOGLEVEL_DEBUG;
                case "INFO":
                    return LOGLEVEL_INFO;
                case "WARNING":
                    return LOGLEVEL_WARNING;
                case "ERROR":
                    return LOGLEVEL_ERROR;
            }

            return LOGLEVEL_NONE;
        }

        public static Color getLogLevelColor(int level)
        {
            int index = LOGLEVEL_COLORS.Length - level - 1;
            if (index >= 0 && index < LOGLEVEL_COLORS.Length)
                return LOGLEVEL_COLORS[index];
            return Color.Black;
        }

        public static void force(params String[] text)
        {
            foreach (String line in text)
            {
                log("[FORCE  ] " + line, LOGLEVEL_NONE);
            }
        }

        public static void error(params String[] text)
        {
            if (LOGLEVEL_ERROR >= logLevel)
            {
                foreach (String line in text)
                {
                    log("[ERROR  ] " + line, LOGLEVEL_ERROR);
                }
            }
        }
        public static void warning(params String[] text)
        {
            if (LOGLEVEL_WARNING >= logLevel)
            {
                foreach (String line in text)
                    log("[WARNING] " + line, LOGLEVEL_WARNING);
            }
        }
        public static void info(params String[] text)
        {
            if (LOGLEVEL_INFO >= logLevel)
            {
                foreach (String line in text)
                    log("[INFO   ] " + line, LOGLEVEL_INFO);
            }
        }
        public static void debug(params String[] text)
        {
            if (LOGLEVEL_DEBUG >= logLevel)
            {
                foreach (String line in text)
                    log("[DEBUG  ] " + line, LOGLEVEL_DEBUG);
            }
        }
        public static void verbose(params String[] text)
        {
            if (LOGLEVEL_VERBOSE >= logLevel)
            {
                foreach (String line in text)
                    log("[VERBOSE] " + line, LOGLEVEL_VERBOSE);
            }
        }

        private static void log(String line, int level)
        {
            lock (locker)
            {
                String now = DateTime.Now.ToString();
                using (Mutex m = new Mutex(false, _pathHash))
                {
                    try
                    {
                        m.WaitOne();
                    }
                    catch (AbandonedMutexException)
                    {
                        Logger.warning("[Logging] Found an abandoned mutex!");
                    }
                    using (StreamWriter file = new StreamWriter(_path, true))
                    {
                        file.WriteLine("[" + now + "]" + line);
                    }
                    m.ReleaseMutex();
                }
            }
        }
    }
}
