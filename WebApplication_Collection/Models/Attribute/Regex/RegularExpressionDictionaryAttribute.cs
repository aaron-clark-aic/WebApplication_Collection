using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;
using WebApplication_Collection.Models.Attribute.Regex;

namespace WebApplicationV.Utils.Attribute
{
    /// <summary>
    /// Regular expression validation attribute
    /// [aaron.clark.aic][2015-01-09 15:53][解决正则验证的逻辑错误]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "aaron_clark_aic[0.01]")]
    public class RegularExpressionDictionaryAttribute : ValidationAttribute
    {
        /// <summary>
        /// Gets the regular expression pattern to use
        /// </summary>
        public string Pattern { get; private set; }

        private Regex Regex { get; set; }

        /// <summary>
        /// Constructor that accepts the regular expression pattern
        /// </summary>
        /// <param name="pattern">The regular expression to use.  It cannot be null.</param>
        public RegularExpressionDictionaryAttribute(EnumDictionaryDescription pattern){

            this.Pattern = EnumHelper.GetDescription(pattern);
        }

        /// <summary>
        /// Override of <see cref="ValidationAttribute.IsValid(object)"/>
        /// </summary>
        /// <remarks>This override performs the specific regular expression matching of the given <paramref name="value"/></remarks>
        /// <param name="value">The value to test for validity.</param>
        /// <returns><c>true</c> if the given value matches the current regular expression pattern</returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        /// <exception cref="ArgumentException"> is thrown if the <see cref="Pattern"/> is not a valid regular expression.</exception>
#if !SILVERLIGHT
        public
#else
        internal
#endif
        override bool IsValid(object value)
        {
            this.SetupRegex();

            // Convert the value to a string
            string stringValue = Convert.ToString(value, CultureInfo.CurrentCulture);

            // Automatically pass if value is null or empty. RequiredAttribute should be used to assert a value is not empty.
            //if ((String.IsNullOrEmpty(stringValue)) || (String.IsNullOrEmpty(stringValue) && Regex.ToString() != "N"))
            if (String.IsNullOrEmpty(stringValue))
            {
                return false;
            } else if (Regex.ToString()=="N") {
                return true;
            }


            Match m = this.Regex.Match(stringValue);

            // We are looking for an exact match, not just a search hit. This matches what
            // the RegularExpressionValidator control does
            //return (m.Success && m.Index == 0 && m.Length == stringValue.Length);
            return (m.Success && m.Index == 0 && m.Length == stringValue.Length);
        }

        /// <summary>
        /// Override of <see cref="ValidationAttribute.FormatErrorMessage"/>
        /// </summary>
        /// <remarks>This override provide a formatted error message describing the pattern</remarks>
        /// <param name="name">The user-visible name to include in the formatted message.</param>
        /// <returns>The localized message to present to the user</returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        /// <exception cref="ArgumentException"> is thrown if the <see cref="Pattern"/> is not a valid regular expression.</exception>
        public override string FormatErrorMessage(string name)
        {
            //this.SetupRegex();

            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, this.Pattern);
        }

        /// <summary>
        /// Sets up the <see cref="Regex"/> property from the <see cref="Pattern"/> property.
        /// </summary>
        /// <exception cref="ArgumentException"> is thrown if the current <see cref="Pattern"/> cannot be parsed</exception>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        private void SetupRegex()
        {
            if (this.Regex == null)
            {
                if (string.IsNullOrEmpty(Pattern))
                {
                    throw new InvalidOperationException("Regex is error: " + Pattern);
                }
                this.Regex = new Regex(Pattern);
            }
        }



    }
}