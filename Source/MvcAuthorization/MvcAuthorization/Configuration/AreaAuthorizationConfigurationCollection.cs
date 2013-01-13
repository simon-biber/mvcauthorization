using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections;

namespace MvcAuthorization.Configuration
{
    public class AreaAuthorizationConfigurationCollection : ConfigurationElementCollection, IEnumerable<AreaAuthorizationConfigurationElement>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public AreaAuthorizationConfigurationElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as AreaAuthorizationConfigurationElement;
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
        public IEnumerable<AreaAuthorizationConfigurationElement> Elements
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
        public new IEnumerator<AreaAuthorizationConfigurationElement> GetEnumerator()
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
            return new AreaAuthorizationConfigurationElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AreaAuthorizationConfigurationElement)element).Area;
        }
    }
}
