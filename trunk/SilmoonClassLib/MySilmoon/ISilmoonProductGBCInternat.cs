using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.MySilmoon
{
    public interface ISilmoonProductGBCInternat
    {
        string ProductString
        {
            get;
            set;
        }
        int Revision
        {
            get;
            set;
        }
        bool InitProductInfo(string productString, int revision);
        void onOutputText(string message);
        void onOutputText(string message, int flag);

        void onInputText(string message);
        void onInputText(string message, int flag);
    }
}
