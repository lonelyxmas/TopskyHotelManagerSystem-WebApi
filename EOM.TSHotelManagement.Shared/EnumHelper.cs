using System.ComponentModel;
using System.Reflection;

namespace EOM.TSHotelManagement.Shared
{
    public class EnumHelper
    {
        public string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = field?
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

            return attribute?.Description ?? value.ToString();
        }

        public int GetEnumValue(Enum value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return Convert.ToInt32(value);
        }

        public int GetEnumValue(int value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            return Convert.ToInt32(value);
        }
    }
}
