using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.SqlClient
{
    public class SmMySqlClientInternal : SqlCommonTemplate
    {
        SmMySqlClient _dataSource;
        public SmMySqlClient DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }
    }
}
