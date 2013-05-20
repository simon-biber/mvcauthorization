using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public class PolicyAuthorizationDescriptor
    {
        public PolicyAuthorizationDescriptor(string type, string value)
        {
            Type = type ?? string.Empty;
            Value = value ?? string.Empty;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int x = 17;
                x = x * 23 + Type.GetHashCode();
                x = x * 23 + Value.GetHashCode();
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
            return x.Type == Type && x.Value == Value;
        }

        public string Type { get; private set; }
        public string Value { get; private set; }
    }
}
