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
            = ConfigurationManager.GetSection("authorizationConfiguration") as AuthorizationConfiguration;

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
        [ConfigurationCollection(typeof(AreaAuthorizationConfigurationCollection), AddItemName = "addArea", ClearItemsName = "clearAreas", RemoveItemName = "removeArea")]
        public AreaAuthorizationConfigurationCollection AreaAuthorizationMappings
        {
            get
            {
                return this["areas"] as AreaAuthorizationConfigurationCollection;
            }
        }
    }
}
