using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Silmoon.Arrays
{
    /// <summary>
    /// 操作数组的使用类型
    /// </summary>
    public class ArrayUtility
    {
        /// <summary>
        /// 从标有ID的类型数组中找出指定的对象
        /// </summary>
        /// <param name="array">可变的数组</param>
        /// <param name="id">IID的ID</param>
        /// <returns></returns>
        public static IID FindIIDFromArray(ArrayList array, int id)
        {
            IID o = null;
            lock (array)
            {
                foreach (object item in array)
                {
                    if (item != null)
                    {
                        IID iID = item as IID;
                        if (iID != null)
                        {
                            if (iID.ID == id)
                            {
                                o = iID;
                                break;
                            }
                        }
                    }
                }
            }
            return o;
        }
    }
}
