using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Configuration;

namespace MvcAuthorization.Configuration
{
    public class ControllerAuthorizationConfigurationCollection : ConfigurationElementCollection, IEnumerable<ControllerAuthorizationConfigurationElement>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ControllerAuthorizationConfigurationElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as ControllerAuthorizationConfigurationElement;
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
        public IEnumerable<ControllerAuthorizationConfigurationElement> Elements
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
        public new IEnumerator<ControllerAuthorizationConfigurationElement> GetEnumerator()
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
            return new ControllerAuthorizationConfigurationElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ControllerAuthorizationConfigurationElement)element).Controller;
        }
    }
}
