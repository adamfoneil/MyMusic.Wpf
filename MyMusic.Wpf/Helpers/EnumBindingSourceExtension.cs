using System;
using System.Windows.Markup;

namespace MyMusic.Wpf.Helpers
{
    /// <summary>
    /// Extension class to support enum in xaml binding.
    /// </summary>
    /// <seealso cref="MarkupExtension" />
    public class EnumBindingSourceExtension : MarkupExtension
    {
        private Type _enumType;

        /// <summary>
        /// Gets or sets the type of the enum.
        /// </summary>
        /// <value>
        /// The type of the enum.
        /// </value>
        /// <exception cref="ArgumentException">Type must be for an Enum.</exception>
        public Type EnumType
        {
            get { return _enumType; }
            set
            {
                if (value != this._enumType)
                {
                    if (null != value)
                    {
                        Type enumType = Nullable.GetUnderlyingType(value) ?? value;
                        if (!enumType.IsEnum)
                            throw new ArgumentException("Type must be for an Enum.");
                    }

                    this._enumType = value;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBindingSourceExtension"/> class.
        /// </summary>
        public EnumBindingSourceExtension()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBindingSourceExtension"/> class.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        public EnumBindingSourceExtension(Type enumType)
        {
            EnumType = enumType;
        }

        /// <summary>
        /// When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>
        /// The object value to set on the property where the extension is applied.
        /// </returns>
        /// <exception cref="InvalidOperationException">The EnumType must be specified.</exception>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (null == _enumType)
                throw new InvalidOperationException("The EnumType must be specified.");

            Type actualEnumType = Nullable.GetUnderlyingType(_enumType) ?? _enumType;
            Array enumValues = System.Enum.GetValues(actualEnumType);

            if (actualEnumType == _enumType)
                return enumValues;

            Array tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }
    }
}
