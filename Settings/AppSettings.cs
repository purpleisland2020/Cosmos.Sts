using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cosmos.Sts.Settings
{
    public class AppSettings
    {
        public BaseUrls BaseUrls { get; set; }
    }

    public class BaseUrls
    {
        public string BackOfficeWeb { get; set; }
        public string Sts { get; set; }
    }
}
