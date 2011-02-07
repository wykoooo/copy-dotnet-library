using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.SqlClient
{
    public abstract class SmMSSQLClientInternal : SqlCommonTemplate
    {
        SmMSSQLClient _dataSource;
        public SmMSSQLClient DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }
    }
}
