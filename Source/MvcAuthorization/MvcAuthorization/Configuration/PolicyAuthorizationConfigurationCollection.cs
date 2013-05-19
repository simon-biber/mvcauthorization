using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections;

namespace MvcAuthorization.Configuration
{
    public class PolicyAuthorizationConfigurationCollection : ConfigurationElementCollection, IEnumerable<PolicyAuthorizationConfigurationElement>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PolicyAuthorizationConfigurationElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as PolicyAuthorizationConfigurationElement;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<PolicyAuthorizationConfigurationElement> Elements
        {
            get
            {
                for (int i = 0; i < base.Count; ++i)
                {
                    yield return this[i];
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public new IEnumerator<PolicyAuthorizationConfigurationElement> GetEnumerator()
        {
            for (int i = 0; i < base.Count; ++i)
            {
                yield return this[i];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new PolicyAuthorizationConfigurationElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PolicyAuthorizationConfigurationElement)element).Type;
        }
    }
}
