using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Silmoon.Memory
{
    public class Memory
    {
        public static void RemoveString(ref ArrayList array, string sDest)
        {
            if (array == null || array.Count == 0) return;

            int arrayLength = array.Count;

            for (int i = 0; i < arrayLength; i++)
            {
                if (array[i].ToString() == sDest)
                {
                    array.RemoveAt(i);
                    arrayLength--;
                }
            }

        }
        public static void RemoveString(ref string[] array, string sDest)
        {
            if (array == null || array.Length == 0) return;

            ArrayList tmpArr = new ArrayList();
            for (int i = 0; i < array.Length; i++)
                tmpArr.Add(array[i]);

            int arrayLength = tmpArr.Count;

            for (int i = 0; i < arrayLength; i++)
            {
                if (tmpArr[i].ToString() == sDest)
                {
                    tmpArr.RemoveAt(i);
                    arrayLength--;
                }
            }
            array = (string[])tmpArr.ToArray(typeof(string));

        }

        public static void Copy(ref ArrayList dArray1, ref ArrayList sArray2)
        {
            if (dArray1 == null || dArray1.Count == 0) return;
            if (sArray2 == null || sArray2.Count == 0) return;

            ArrayList array = new ArrayList();
            for (int i = 0; i < dArray1.Count; i++)
                array.Add(dArray1[i]);

            for (int i = 0; i < sArray2.Count; i++)
                array.Add(sArray2[i]);

            dArray1 = array;
        }
        public static void Copy(ref string[] dArray1, ref string[] sArray2)
        {
            if (dArray1 == null || dArray1.Length == 0) return;
            if (sArray2 == null || sArray2.Length == 0) return;

            ArrayList array = new ArrayList();
            for (int i = 0; i < dArray1.Length; i++)
                array.Add(dArray1.GetValue(i));

            for (int i = 0; i < sArray2.Length; i++)
                array.Add(sArray2.GetValue(i));

            dArray1 = (string[])array.ToArray(typeof(string));
        }
        public static void Copy(ref object[] dArray1, ref object[] sArray2)
        {
            if (dArray1 == null || dArray1.Length == 0) return;
            if (sArray2 == null || sArray2.Length == 0) return;

            ArrayList array = new ArrayList();
            for (int i = 0; i < dArray1.Length; i++)
                array.Add(dArray1.GetValue(i));

            for (int i = 0; i < sArray2.Length; i++)
                array.Add(sArray2.GetValue(i));

            dArray1 = (string[])array.ToArray(typeof(string));
        }

        public static int Find(ref ArrayList dArray1, ref object fObject)
        {
            int result = 0;

            if (dArray1 == null || dArray1.Count == 0) return result;

            for (int i = 0; i < dArray1.Count; i++)
            {
                if (dArray1[1] == fObject) result++;
            }
            return result;
        }
        public static int Find(ref string[] dArray1, ref string fString)
        {
            int result = 0;

            if (dArray1 == null || dArray1.Length == 0) return result;

            for (int i = 0; i < dArray1.Length; i++)
            {
                if (dArray1[1] == fString) result++;
            }
            return result;
        }
        public static int Find(ref object[] dArray1, ref object[] fObject)
        {
            int result = 0;

            if (dArray1 == null || dArray1.Length == 0) return result;

            for (int i = 0; i < dArray1.Length; i++)
            {
                if (dArray1[1] == fObject) result++;
            }
            return result;
        }

        public static Array ToArray(ref object obj)
        {
            Array array = (Array)obj;
            return array;
        }
    }
}
