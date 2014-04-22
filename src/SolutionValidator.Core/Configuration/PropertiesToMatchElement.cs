// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertiesToMatchElement.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SolutionValidator.Configuration
{
    using System.Configuration;

    public class PropertiesToMatchElement : ConfigurationElement
    {
        #region Constants
        private const string PropertyNameAttributeName = "propertyName";

        private const string OtherPropertyNameAttributeName = "otherPropertyName";
        #endregion

        #region Properties
        [ConfigurationProperty(PropertyNameAttributeName, DefaultValue = "", IsRequired = true)]
        public string PropertyName
        {
            get { return ((string) (this[PropertyNameAttributeName])); }
            set { this[PropertyNameAttributeName] = value; }
        }

        [ConfigurationProperty(OtherPropertyNameAttributeName, DefaultValue = "", IsRequired = true)]
        public string OtherPropertyName
        {
            get { return ((string) (this[OtherPropertyNameAttributeName])); }
            set { this[OtherPropertyNameAttributeName] = value; }
        }
        #endregion
    }
}