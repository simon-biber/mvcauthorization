﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MvcAuthorization.Configuration
{
    public class PolicyAuthorizationConfigurationElement: ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired=true)]
        public string Type
        {
            get
            {
                return (string)this["type"];
            }
            set
            {
                this["type"] = value;
            }
        }
    }
}