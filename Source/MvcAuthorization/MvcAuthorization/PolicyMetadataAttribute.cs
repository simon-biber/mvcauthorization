using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization
{
    public class PolicyMetadataAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
