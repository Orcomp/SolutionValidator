﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertiesToCheckCollection.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SolutionValidator.Configuration
{
    using System.Configuration;
    using SolutionValidator.Configuration;

    [ConfigurationCollection(typeof (ConfigurationNameElement))]
    public class PropertiesToCheckCollection : ConfigurationElementCollection
    {
        #region Properties
        [ConfigurationProperty(SolutionValidatorConfigurationSection.CheckAttributeName, DefaultValue = "true")]
        public bool Check
        {
            get { return (bool) base[SolutionValidatorConfigurationSection.CheckAttributeName]; }
        }

        public PropertyToCheckElement this[int idx]
        {
            get { return (PropertyToCheckElement) BaseGet(idx); }
        }
        #endregion

        #region Methods
        protected override ConfigurationElement CreateNewElement()
        {
            return new PropertyToCheckElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PropertyToCheckElement) (element)).PropertyName;
        }

        public void Add(PropertyToCheckElement item)
        {
            base.BaseAdd(item);
        }
        #endregion
    }
}