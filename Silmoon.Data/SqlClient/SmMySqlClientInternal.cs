using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.SqlClient
{
    public class SmMySqlClientInternal
    {
        SmMySqlClient _dataSource;
        public SmMySqlClient DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }
    }
}
