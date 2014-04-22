﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertiesToMatchCollection.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SolutionValidator.Configuration
{
    using System.Configuration;
    using SolutionValidator.Configuration;

    [ConfigurationCollection(typeof (ConfigurationNameElement))]
    public class PropertiesToMatchCollection : ConfigurationElementCollection
    {
        #region Properties
        [ConfigurationProperty(SolutionValidatorConfigurationSection.CheckAttributeName, DefaultValue = "true")]
        public bool Check
        {
            get { return (bool) base[SolutionValidatorConfigurationSection.CheckAttributeName]; }
        }

        public PropertiesToMatchElement this[int idx]
        {
            get { return (PropertiesToMatchElement) BaseGet(idx); }
        }
        #endregion

        #region Methods
        protected override ConfigurationElement CreateNewElement()
        {
            return new PropertiesToMatchElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PropertiesToMatchElement) (element)).PropertyName + ((PropertiesToMatchElement) (element)).OtherPropertyName;
        }

        public void Add(PropertiesToMatchElement item)
        {
            base.BaseAdd(item);
        }
        #endregion
    }
}