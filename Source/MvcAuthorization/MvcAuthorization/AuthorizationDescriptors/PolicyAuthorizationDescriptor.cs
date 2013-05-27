using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public class PolicyAuthorizationDescriptor
    {
        public PolicyAuthorizationDescriptor(bool ignoreInherited, string name)
        {
            IgnoreInherited = ignoreInherited;
            Name = name ?? string.Empty;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int x = 17;
                x = x * 23 + IgnoreInherited.GetHashCode();
                x = x * 23 + Name.GetHashCode();
                return x;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }
            PolicyAuthorizationDescriptor x = (PolicyAuthorizationDescriptor)obj;
            return x.IgnoreInherited == IgnoreInherited && x.Name == Name;
        }

        public bool IgnoreInherited { get; private set; }
        public string Name { get; private set; }
    }
}
