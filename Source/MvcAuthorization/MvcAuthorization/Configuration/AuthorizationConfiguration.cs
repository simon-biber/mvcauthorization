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

        public static ControllerAuthorizationConfigurationCollection ControllerMappings
        {
            get
            {
                ControllerAuthorizationConfigurationCollection result;
                if (Section == null || Section.ControllerAuthorizationMappings == null)
                {
                    result = new ControllerAuthorizationConfigurationCollection();
                }
                else
                {
                    result = Section.ControllerAuthorizationMappings;
                }

                return result;
            }
        }

        [ConfigurationProperty("controllerAuthorizationMappings")]
        public ControllerAuthorizationConfigurationCollection ControllerAuthorizationMappings
        {
            get
            {
                return this["controllerAuthorizationMappings"] as ControllerAuthorizationConfigurationCollection;
            }
        }
    }
}
