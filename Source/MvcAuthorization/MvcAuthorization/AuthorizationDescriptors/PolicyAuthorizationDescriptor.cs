using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public class PolicyAuthorizationDescriptor
    {
        public PolicyAuthorizationDescriptor(bool loadByTypeName, string name)
        {
            LoadByTypeName = loadByTypeName;
            Name = name ?? string.Empty;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int x = 17;
                x = x * 23 + LoadByTypeName.GetHashCode();
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
            return x.LoadByTypeName == LoadByTypeName && x.Name == Name;
        }

        public bool LoadByTypeName { get; private set; }
        public string Name { get; private set; }
    }
}
