using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MvcAuthorization.Configuration
{
    public class PolicyAuthorizationConfigurationElement: ConfigurationElement
    {
        [ConfigurationProperty("ignoreInherited", IsRequired = false, DefaultValue = false)]
        public bool IgnoreInherited
        {
            get
            {
                return (bool)this["ignoreInherited"];
            }
            set
            {
                this["ignoreInherited"] = value;
            }
        }

        [ConfigurationProperty("name", IsRequired=true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }
    }
}
