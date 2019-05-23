using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Engine
{
    public enum RepositoryType
    {
        None = 0,
        Sql = 1,
        ElasticSearch = 2,
        External = 3
    }
}
