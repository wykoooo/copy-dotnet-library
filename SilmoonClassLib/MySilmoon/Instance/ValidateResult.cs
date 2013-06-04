using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.MySilmoon.Instance
{
    public class ValidateResult
    {
        public Exception Error;
        public int min_exit_version;
        public int min_pop_version;
        public int latest_version;

        public ValidateResult()
        {

        }
    }
}
