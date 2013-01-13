using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MvcAuthorization.Configuration
{
    public class AreaAuthorizationConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = false)]
        public string Area
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

        [ConfigurationProperty("controllers")]
        [ConfigurationCollection(typeof(AreaAuthorizationConfigurationCollection), AddItemName = "addController", ClearItemsName = "clearControllers", RemoveItemName = "removeController")]
        public ControllerAuthorizationConfigurationCollection ControllerAuthorizationMappings
        {
            get
            {
                return this["controllers"] as ControllerAuthorizationConfigurationCollection;
            }
        }

        //[ConfigurationProperty("policy")]
        //public string Policy
        //{
        //    get
        //    {
        //        return (string)this["policy"];
        //    }
        //    set
        //    {
        //        this["policy"] = value;
        //    }
        //}
    }
}
