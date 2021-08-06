using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace MyMusic.Wpf.Helpers
{
    /// <summary>
    /// Enum to DisplayName converter
    /// </summary>
    /// <seealso cref="System.ComponentModel.EnumConverter" />
    public class EnumDisplayTypeConverter : EnumConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDisplayTypeConverter"/> class.
        /// </summary>
        /// <param name="type">A <see cref="T:System.Type" /> that represents the type of enumeration to associate with this enumeration converter.</param>
        public EnumDisplayTypeConverter(Type type) : base(type)
        {

        }

        /// <summary>
        /// Converts the given value object to the specified destination type.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">An optional <see cref="T:System.Globalization.CultureInfo" />. If not supplied, the current culture is assumed.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <param name="destinationType">The <see cref="T:System.Type" /> to convert the value to.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that represents the converted <paramref name="value" />.
        /// </returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value != null)
                {
                    FieldInfo fi = value.GetType().GetField(value.ToString());
                    if (fi != null)
                    {
                        var attributes = (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), false);
                        return ((attributes.Length > 0) && (!string.IsNullOrEmpty(attributes[0].Name))) ? attributes[0].Name : value.ToString();
                    }
                }

                return string.Empty;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
