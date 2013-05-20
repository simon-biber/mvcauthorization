using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.Policy
{
    public class PolicyMetadataAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
