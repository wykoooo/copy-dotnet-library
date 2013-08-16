using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Types
{
    public enum StateCode
    {
        SERVER_FAIL = -13,
        FAIL = -12,
        QUOIT_LIMIT = -11,
        EXISTED = -10,
        USER_NOT_EXIST = -9,
        NOT_EXIST = -8,
        USER_LIMIT = -7,
        CONFLICT = -6,
        SOFT_LIMIT = -5,
        PARAM_ERROR = -4,
        ERROR = -3,
        PERMISSION_REJECT = -2,
        NOT_LOGIN = -1,
        None = 0,
        SUCCESS = 1,
        MULTI_SUCCESS = 2,
    }
}
