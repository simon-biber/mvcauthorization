using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MvcAuthorization.Configuration
{
    public class AuthorizationConfiguration : ConfigurationSection
    {
        private static AuthorizationConfiguration _authorizationConfiguration
            = ConfigurationManager.GetSection("mvcAuthorization") as AuthorizationConfiguration;

        public static AuthorizationConfiguration Section
        {
            get
            {
                return _authorizationConfiguration;
            }
        }

        public static AreaAuthorizationConfigurationCollection AreaMappings
        {
            get
            {
                AreaAuthorizationConfigurationCollection result;
                if (Section == null || Section.AreaAuthorizationMappings == null)
                {
                    result = new AreaAuthorizationConfigurationCollection();
                }
                else
                {
                    result = Section.AreaAuthorizationMappings;
                }

                return result;
            }
        }        

        [ConfigurationProperty("areas")]
        [ConfigurationCollection(typeof(AreaAuthorizationConfigurationCollection), AddItemName = "area")]
        public AreaAuthorizationConfigurationCollection AreaAuthorizationMappings
        {
            get
            {
                return this["areas"] as AreaAuthorizationConfigurationCollection;
            }
        }

        public static PolicyAuthorizationConfigurationCollection PolicyData
        {
            get
            {
                PolicyAuthorizationConfigurationCollection result;
                if (Section == null || Section.Policies == null)
                {
                    result = new PolicyAuthorizationConfigurationCollection();
                }
                else
                {
                    result = Section.Policies;
                }

                return result;
            }
        }    

        [ConfigurationProperty("policies")]
        [ConfigurationCollection(typeof(PolicyAuthorizationConfigurationCollection), AddItemName = "policy")]
        public PolicyAuthorizationConfigurationCollection Policies
        {
            get
            {
                return this["policies"] as PolicyAuthorizationConfigurationCollection;
            }
        }
    }
}
