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
        string ReleaseVersion
        {
            get;
            set;
        }
        bool InitProductInfo(string productString, string releaseVersion);
        void onOutputText(string message);
        void onOutputText(string message, int flag);

        void onInputText(string message);
        void onInputText(string message, int flag);
    }
}
