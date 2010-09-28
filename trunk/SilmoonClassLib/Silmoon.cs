using System;
using System.Collections.Generic;
using System.Text;
using Silmoon.Base;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;
using Silmoon.Reflection;

namespace Silmoon
{
    public class Copyright
    {
        public static string CopyUsingString = "hare";
        public static string Author = "song weibin";
    }

    public class SmType
    {
        public static string ExtensionNameToContentType(string ExtensionName)
        {
            string restring = "";
            if (ExtensionName.ToLower() == "jpg") { restring = "image/jpeg"; }
            if (ExtensionName.ToLower() == "gif") { restring = "image/gif"; }
            if (ExtensionName.ToLower() == "png") { restring = "image/png"; }
            return restring;
        }
    }
    public class SmString
    {
        /// <summary>
        /// 剪裁字符串
        /// </summary>
        /// <param name="s">要处理的字符串</param>
        /// <param name="c">保留长度</param>
        /// <returns></returns>
        public static string CutString(string s, int c)
        {
            if (s.Length > c) { s = s.Substring(0, c); }
            return s;
        }
        /// <summary>
        /// 剪裁字符串，并对多出的字符做处理。
        /// </summary>
        /// <param name="s">要处理的字符串</param>
        /// <param name="c">保留长度</param>
        /// <param name="output">多出来的字符替换成的字符</param>
        /// <returns></returns>
        public static string CutString(string s, int c, string output)
        {
            if (s.Length > c) { s = s.Substring(0, c) + output; }
            return s;
        }
        /// <summary>
        /// 检查字段是否为空，如果不是，返回字段，否则抛出异常。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string CheckEmpty(string s)
        {
            if (s.Length < 1)
            {
                throw new Exception("参数s为空！");
            }
            return s;
        }
        /// <summary>
        /// 如果字符串为NULL，则把字符串变成零长度的字符串。
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        public static string FixNullString(object s)
        {
            return FixNullString(s.ToString());
        }
        /// <summary>
        /// 如果字符串为NULL，则把字符串变成零长度的字符串。
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="onNullReturn">当字符串为NULL返回字符串</param>
        /// <returns></returns>
        public static string FixNullString(string s, string onNullReturn = "")
        {
            if (s == null) return onNullReturn;
            else return s.ToString();
        }
        /// <summary>
        /// 将一个字符串转化成BOOL类型
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <returns></returns>
        public static bool StringToBool(string s)
        {
            switch (s.ToLower())
            {
                case null:
                    return false;
                case "":
                    return false;
                case "1":
                    return true;
                case "0":
                    return false;
                case "y":
                    return true;
                case "n":
                    return false;
                case "yes":
                    return true;
                case "no":
                    return false;
                case "是":
                    return true;
                case "否":
                    return false;
                case "对":
                    return true;
                case "错":
                    return false;
                case "真":
                    return true;
                case "假":
                    return false;
                case "禁用":
                    return false;
                case "启用":
                    return true;
                case "true":
                    return true;
                case "false":
                    return false;
                case "enable":
                    return true;
                case "disable":
                    return false;
                case "enabled":
                    return true;
                case "disabled":
                    return false;
                default:
                    return false;
            }
        }
        /// <summary>
        /// 在一个字符串数组里面寻找一个字符串
        /// </summary>
        /// <param name="sArr">字符串数组</param>
        /// <param name="findString">字符串</param>
        /// <returns></returns>
        public static bool FindFormStringArray(string[] sArr,string findString)
        {
            foreach (string s in sArr)
            {
                if (s == findString)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 保持一个字符串的长度
        /// </summary>
        /// <param name="s">原字符串</param>
        /// <param name="maxlen">最大长度</param>
        /// <param name="str">衔接字符串</param>
        /// <returns></returns>
        public static string KeepStringLenght(string s, int maxlen, string str)
        {
            if (s.Length > maxlen)
            {
                int halflen = (maxlen - str.Length) / 2;
                string result = s.Substring(0, halflen);
                result += str + s.Substring(s.Length - halflen, halflen);
                return result;
            }
            else return s;
        }
        /// <summary>
        /// 从一个字符串中获取一个指定索引号的元素的字符串。
        /// </summary>
        /// <param name="array">字符串数组</param>
        /// <param name="index">索引号</param>
        /// <returns></returns>
        public static string FormArrayGetString(string[] array, int index)
        {
            return FormArrayGetString(array, index, false);
        }
        /// <summary>
        /// 从一个字符串中获取一个指定索引号的元素的字符串。
        /// </summary>
        /// <param name="array">字符串数组</param>
        /// <param name="index">索引号</param>
        /// <param name="outIndexReturnNull">如果超出索引，是否返回null</param>
        /// <returns></returns>
        public static string FormArrayGetString(string[] array, int index, bool outIndexReturnNull)
        {
            if (array.Length < (index + 1))
            {
                if (outIndexReturnNull) return null; else return "";
            }
            else return array[index];
        }
        /// <summary>
        /// 把字符串数组中的所有字符全部合并。
        /// </summary>
        /// <param name="array">字符串数组。</param>
        /// <param name="SplitString">分隔符</param>
        /// <returns></returns>
        public static string MergeStringArray(string[] array, string SplitString)
        {
            return MergeStringArray(array, SplitString, true);
        }
        /// <summary>
        /// 把字符串数组中的所有字符全部合并。
        /// </summary>
        /// <param name="array">字符串数组。</param>
        /// <param name="SplitString">分隔符</param>
        /// <param name="RemoveLastSplitString">是否移除最后一个分隔符</param>
        /// <returns></returns>
        public static string MergeStringArray(string[] array, string SplitString, bool RemoveLastSplitString = true)
        {
            string result = "";
            if (array == null || array.Length == 0) return result;
            foreach (string s in array)
                result += s + SplitString;
            if (RemoveLastSplitString)
                result = result.Substring(0, result.Length - SplitString.Length);
            return result;
        }
        /// <summary>
        /// 对ArrayList进行排序
        /// </summary>
        /// <param name="array">以排序的ArrayList</param>
        /// <returns></returns>
        public static ArrayList SortArray(ArrayList array)
        {
            Comparer c = new Comparer(System.Globalization.CultureInfo.CurrentCulture);
            array.Sort();
            return array;
        }
        /// <summary>
        /// 从数组中分析出名字与值的集合
        /// </summary>
        /// <param name="array">数组</param>
        /// <param name="nameValueSeparator">名字与值之间的分隔符</param>
        /// <param name="perNameChar">名字前缀</param>
        /// <returns></returns>
        public static NameValueCollection AnalyzeNameValue(string[] array, string nameValueSeparator = ":", string perNameChar = "")
        {
            NameValueCollection result = new NameValueCollection();
            if (array == null || array.Length == 0) return result;
            foreach (string s1 in array)
            {
                if (s1.Length < perNameChar.Length && perNameChar != "" && s1.Substring(0, perNameChar.Length) != perNameChar) continue;
                string[] sArr = s1.Split(new string[] { nameValueSeparator }, 2, StringSplitOptions.None);
                if (sArr.Length == 2)
                    result.Add(sArr[0], sArr[1]);
            }

            return result;
        }
        /// <summary>
        /// 填充或者保持字符串长度
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <param name="length">长度</param>
        /// <param name="fillStr">补充字符串</param>
        /// <param name="onAfter">是否在后面补充</param>
        /// <returns></returns>
        public static string FillLength(string s, int length, string fillStr, bool onAfter)
        {
            int fInChrC = length - s.Length;
            if (fInChrC < 1) return s;

            for (int i = 0; i < fInChrC; i++)
            {
                if (onAfter)
                    s += fillStr;
                else s = fillStr + s;
            }
            return s;
        }
        /// <summary>
        /// 从数组中取出重复的字符串，组合成新的数组。
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string[] RemoveRepeaterString(string[] array)
        {
            List<string> listString = new List<string>();
            foreach (string eachString in array)
            {
                if (!listString.Contains(eachString))
                    listString.Add(eachString);
            }
            return listString.ToArray();
        }
    }
    public class SmInt
    {
        public static bool ChkIntLengthMin(int sint, int minlen)
        {
            bool rebool = true;
            int sintlen = Convert.ToString(sint).Length;
            if (sintlen < minlen)
            {
                rebool = false;
            }
            return rebool;
        }
        public static bool ChkIntLengthMax(int sint, int maxlen)
        {
            bool rebool = true;
            int sintlen = Convert.ToString(sint).Length;
            if (sintlen > maxlen)
            {
                rebool = false;
            }
            return rebool;
        }
        public static bool ChkIntLength(int sint, int minlen, int maxlen)
        {
            bool rebool = true;
            int sintlen = Convert.ToString(sint).Length;
            if (sintlen > maxlen || sintlen < minlen)
            {
                rebool = false;
            }
            return rebool;
        }
        public static int ChkIntLengthMinThrowEx(int sint, int minlen)
        {
            int sintlen = Convert.ToString(sint).Length;
            if (sintlen < minlen)
            {
                throw new Exception("sint length(min) reject");
            }
            return sint;
        }
        public static int ChkIntLengthMaxThrowEx(int sint, int maxlen)
        {
            int sintlen = Convert.ToString(sint).Length;
            if (sintlen > maxlen)
            {
                throw new Exception("sint length(max) reject");
            }
            return sint;
        }
        public static int ChkIntLengthThrowEx(int sint, int minlen, int maxlen)
        {
            int sintlen = Convert.ToString(sint).Length;
            if (sintlen > maxlen || sintlen < minlen)
            {
                throw new Exception("sint length reject");
            }
            return sint;
        }
        public static bool ChkIntValue(int sint, int min, int max)
        {
            if (sint < min || sint > max) return false; else return true;
        }
        public static int ControlIntValue(int sint, int min, int max, bool throwException)
        {
            if (sint < min)
            {
                if (throwException) throw new ArgumentException("参数 数字 应大于！" + min.ToString());
                sint = min;
            }
            else if (sint > max)
            {
                if (throwException) throw new ArgumentException("参数 数字 应小于！" + max.ToString());
                sint = max;
            }
            return sint;
        }
        public static int ControlIntValueMin(int sint, int min)
        {
            int reint = sint;
            if (sint < min) { reint = min; }
            return reint;
        }
        public static int ControlIntValueMax(int sint, int max)
        {
            int reint = sint;
            if (sint > max) { reint = max; }
            return reint;
        }
        public static int BoolInt(int sint, int NoBoolToInt)
        {
            int reint = sint;
            if (sint != 1 && sint != 0) { reint = NoBoolToInt; }
            return reint;
        }
        public static int BoolIntThrowEx(int sint)
        {
            int reint = sint;
            if (sint != 1 && sint != 0) { throw new Exception("Parameter reject"); }
            return reint;
        }
        public static int BoolToInt(bool b)
        {
            if (b) return 1;
            else return 0;
        }
    }

    public sealed class ConsoleEx
    {
        public enum InputMode
        {
            LineInput = 0,
            EchoInput = 1,
        }
        public enum ConsoleColor
        {
            Black = 0,
            Blue = Win32Native.FOREGROUND_BLUE,
            Green = Win32Native.FOREGROUND_GREEN,

            SkyBlue = Win32Native.FOREGROUND_BLUE + Win32Native.FOREGROUND_GREEN,

            Red = Win32Native.FOREGROUND_RED,
            Purple = Win32Native.FOREGROUND_BLUE + Win32Native.FOREGROUND_RED,
            Brown = Win32Native.FOREGROUND_GREEN + Win32Native.FOREGROUND_RED,
            White = Win32Native.FOREGROUND_BLUE + Win32Native.FOREGROUND_GREEN +
            Win32Native.FOREGROUND_RED,
            Gray = Win32Native.FOREGROUND_INTENSIFY,
            BlueForte = Win32Native.FOREGROUND_BLUE + Win32Native.FOREGROUND_INTENSIFY,
            GreenForte = Win32Native.FOREGROUND_GREEN + Win32Native.FOREGROUND_INTENSIFY,
            SkyBlueForte = Win32Native.FOREGROUND_BLUE + Win32Native.FOREGROUND_GREEN +
            Win32Native.FOREGROUND_INTENSIFY,
            RedForte = Win32Native.FOREGROUND_RED + Win32Native.FOREGROUND_INTENSIFY,
            PurpleForte = Win32Native.FOREGROUND_BLUE + Win32Native.FOREGROUND_RED +
            Win32Native.FOREGROUND_INTENSIFY,
            Yellow = Win32Native.FOREGROUND_GREEN + Win32Native.FOREGROUND_RED +
            Win32Native.FOREGROUND_INTENSIFY,
            WhiteForte = Win32Native.FOREGROUND_BLUE + Win32Native.FOREGROUND_GREEN +
            Win32Native.FOREGROUND_RED + Win32Native.FOREGROUND_INTENSIFY

        }
        public enum CursorType
        {
            Off = 0,
            SingleLine = 1,
            Block = 2,
        }

        private IntPtr hConsoleIn;
        private IntPtr hConsoleOut;
        private Win32Native.CONSOLE_INFO conInfo;
        private Win32Native.CURSOR_INFO cursorInfo;
        private int backColor;
        private short backgroundAttrib;

        public string Title
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder(128);
                Win32Native.GetConsoleTitle(stringBuilder, 128);
                return stringBuilder.ToString();
            }
            set
            {
                Win32Native.SetConsoleTitle(value);
            }
        }
        public int Columns
        {
            get
            {
                return conInfo.MaxSize.x;
            }
        }
        public int Rows
        {
            get
            {
                return conInfo.MaxSize.y;
            }
        }
        public int CursorX
        {
            get
            {
                updateConsoleInfo();
                return conInfo.CursorPosition.x;
            }
        }
        public int CursorY
        {
            get
            {
                updateConsoleInfo();
                return conInfo.CursorPosition.y;
            }
        }
        public ConsoleEx()
        {
            Win32Native.AllocConsole();
            hConsoleIn = Win32Native.GetStdHandle(-10);
            hConsoleOut = Win32Native.GetStdHandle(-11);
            conInfo = new Win32Native.CONSOLE_INFO();
            updateConsoleInfo();
            cursorInfo = new Win32Native.CURSOR_INFO();
            SetCursorType(CursorType.SingleLine);
            backgroundAttrib = 7;
        }

        ~ConsoleEx()
        {
            //base.Finalize();
            //FreeConsole();
        }

        public void SetMode(InputMode mode)
        {
            int i = 0;

            Win32Native.GetConsoleMode(hConsoleIn, ref i);
            if (mode == InputMode.EchoInput)
            {
                i &= -7;
            }
            else
            {
                i |= 6;
            }
            Win32Native.SetConsoleMode(hConsoleIn, i);
        }
        public void Clear()
        {
            int i = 0;
            Win32Native.COORD cOORD = new Win32Native.COORD();
            cOORD.x = 0;
            cOORD.y = 0;
            Win32Native.FillConsoleOutputCharacter(hConsoleOut, ' ', (short)(conInfo.MaxSize.x * conInfo.MaxSize.y), cOORD, ref i);
            Win32Native.FillConsoleOutputAttribute(hConsoleOut, backgroundAttrib, (short)(conInfo.MaxSize.x * conInfo.MaxSize.y), cOORD, ref i);
            MoveCursor(1, 1);
        }
        public void EchoInput(bool value)
        {
            int i = 0;

            Win32Native.GetConsoleMode(hConsoleIn, ref i);
            if (value)
            {
                i |= 4;
            }
            else
            {
                i &= -5;
            }
            Win32Native.SetConsoleMode(hConsoleIn, i);
        }
        public void SetColor(ConsoleColor foreColor, ConsoleColor backColor)
        {
            this.backColor = (int)backColor;
            SetColor(foreColor);
        }
        public void SetColor(ConsoleColor foreColor)
        {
            Win32Native.SetConsoleTextAttribute(hConsoleOut, (int)foreColor + 16 * backColor);
        }
        public void SetClsColor(ConsoleColor backColor)
        {
            backgroundAttrib = (short)((short)backColor * 16);
        }
        public void MoveCursor(int x, int y)
        {
            conInfo.CursorPosition.x = (short)(x - 1);
            conInfo.CursorPosition.y = (short)(y - 1);
            if (cursorInfo.Visible)
            {
                int i = conInfo.CursorPosition.x + conInfo.CursorPosition.y * 65536;
                Win32Native.SetConsoleCursorPosition(hConsoleOut, i);
            }
        }
        public void SetCursorType(CursorType newType)
        {
            switch (newType)
            {
                case CursorType.Block:
                    cursorInfo.Size = 100;
                    cursorInfo.Visible = true;
                    break;

                case CursorType.SingleLine:
                    cursorInfo.Size = 10;
                    cursorInfo.Visible = true;
                    break;

                case CursorType.Off:
                    cursorInfo.Size = 100;
                    cursorInfo.Visible = false;
                    break;
            }
            Win32Native.SetConsoleCursorInfo(hConsoleOut, ref cursorInfo);
            MoveCursor(conInfo.CursorPosition.x, conInfo.CursorPosition.y);
        }
        public void FreeConsole()
        {
            try
            {
                Win32Native.FreeConsole();
                Win32Native.CloseHandle(hConsoleIn);
                Win32Native.CloseHandle(hConsoleOut);
                Win32Native.FreeConsole();
            }
            catch
            {
            }
        }
        private void updateConsoleInfo()
        {
            Win32Native.GetConsoleScreenBufferInfo(hConsoleOut, ref conInfo);
        }
    }
    class Win32Native
    {
        public enum BACKGROUNDCOLOR
        {
            bBLUE = 16,
            bGREEN = 32,
            bRED = 64,
            bBRIGHT = 128,
        }
        internal struct COORD
        {
            internal short x;
            internal short y;
        }
        internal struct RECT
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        internal struct CONSOLE_INFO
        {
            internal COORD Size;
            internal COORD CursorPosition;
            internal short Attribute;
            internal RECT Window;
            internal COORD MaxSize;
        }
        internal struct CURSOR_INFO
        {
            internal int Size;
            internal bool Visible;
        }

        internal const int STD_OUTPUT_HANDLE = -11;
        internal const int STD_INPUT_HANDLE = -10;
        internal const short ENABLE_LINE_INPUT = 2;
        internal const short ENABLE_ECHO_INPUT = 4;
        internal const int FOREGROUND_BLUE = 1;
        internal const int FOREGROUND_GREEN = 2;
        internal const int FOREGROUND_RED = 4;
        internal const int FOREGROUND_INTENSIFY = 8;
        internal const int BACKGROUND_BLUE = 16;
        internal const int BACKGROUND_GREEN = 32;
        internal const int BACKGROUND_INTENSIFY = 128;


        [DllImportAttribute("kernel32")]
        public static extern void SetConsoleTitle(string lpTitleStr);
        [DllImportAttribute("kernel32")]
        public static extern void GetConsoleTitle(StringBuilder lpBuff, int buffSize);
        [DllImportAttribute("kernel32")]
        public static extern int SetConsoleTextAttribute(IntPtr hConsoleOutput, int wAttributes);
        [DllImportAttribute("kernel32")]
        public static extern int FillConsoleOutputCharacter(IntPtr Handle, char uChar, int Len, COORD start, ref int written);
        [DllImportAttribute("kernel32")]
        public static extern bool FillConsoleOutputAttribute(IntPtr Handle, short att, int Len, COORD start, ref int writted);
        [DllImportAttribute("kernel32")]
        public static extern void GetConsoleScreenBufferInfo(IntPtr Handle, ref CONSOLE_INFO info);
        [DllImportAttribute("kernel32")]
        public static extern bool SetConsoleCursorInfo(IntPtr Handle, ref CURSOR_INFO info);
        [DllImportAttribute("kernel32")]
        public static extern bool SetConsoleCursorPosition(IntPtr handle, int coord);
        [DllImportAttribute("kernel32")]
        public static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImportAttribute("kernel32")]
        public static extern void GetConsoleMode(IntPtr hConsoleHandle, ref int lpMode);
        [DllImportAttribute("kernel32")]
        public static extern void SetConsoleMode(IntPtr hConsoleHandle, int dwMode);
        [DllImportAttribute("kernel32")]
        public static extern int CloseHandle(IntPtr hObject);
        [DllImportAttribute("kernel32")]
        public static extern int AllocConsole();
        [DllImportAttribute("kernel32")]
        public static extern int FreeConsole();
    }

    public class StateFlag
    {
        int _id = -1;
        bool _doubleStateFlag = false;
        int _intStateFlag = -1;
        object _objectRef = null;
        string _message = string.Empty;
        string _stringFlag = string.Empty;
        ArrayList _objectArray = new ArrayList();
        NameValueCollection _parameters = new NameValueCollection();
        NameObjectCollection _objects = new NameObjectCollection();



        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public bool DoubleStateFlag
        {
            get { return _doubleStateFlag; }
            set { _doubleStateFlag = value; }
        }
        public int IntStateFlag
        {
            get { return _intStateFlag; }
            set { _intStateFlag = value; }
        }
        public object ObjectRef
        {
            get { return _objectRef; }
            set { _objectRef = value; }
        }
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        public string StringFlag
        {
            get { return _stringFlag; }
            set { _stringFlag = value; }
        }
        public ArrayList ObjectArray
        {
            get { return _objectArray; }
            set { _objectArray = value; }
        }
        public NameValueCollection Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }
        public NameObjectCollection Objects
        {
            get { return _objects; }
            set { _objects = value; }
        }

    }
}
