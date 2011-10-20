using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MvcAuthorization.Configuration
{
    public class ActionAuthorizationConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("action", IsRequired = true)]
        public string Action
        {
            get
            {
                return (string)this["action"];
            }
            set
            {
                this["action"] = value;
            }
        }

        [ConfigurationProperty("roles", IsRequired = false)]
        public string Roles
        {
            get
            {
                return (string)this["roles"];
            }
            set
            {
                this["roles"] = value;
            }
        }

        //[ConfigurationProperty("customAuthorization")]
        //public string CustomAuthorization
        //{
        //    get
        //    {
        //        return (string)this["customAuthorization"];
        //    }
        //    set
        //    {
        //        this["customAuthorization"] = value;
        //    }
        //}
    }
}
