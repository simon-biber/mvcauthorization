using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MvcAuthorization.Configuration
{
    public class PolicyAuthorizationConfigurationElement: ConfigurationElement
    {
        [ConfigurationProperty("ignore", IsRequired = false, DefaultValue = false)]
        public bool Ignore
        {
            get
            {
                return (bool)this["ignore"];
            }
            set
            {
                this["ignore"] = value;
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
