using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public class PolicyAuthorizationDescriptor
    {
        public PolicyAuthorizationDescriptor(bool ignore, string name)
        {
            Ignore = ignore;
            Name = name ?? string.Empty;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int x = 17;
                x = x * 23 + Ignore.GetHashCode();
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
            return x.Ignore == Ignore && x.Name == Name;
        }

        public bool Ignore { get; private set; }
        public string Name { get; private set; }
    }
}
