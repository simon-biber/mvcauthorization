using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MvcAuthorization.Configuration
{
    public class PolicyAuthorizationConfigurationElement: ConfigurationElement
    {
        [ConfigurationProperty("loadByTypeName", IsRequired=false, DefaultValue=false)]
        public bool LoadByTypeName
        {
            get
            {
                return (bool)this["loadByTypeName"];
            }
            set
            {
                this["loadByTypeName"] = value;
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
