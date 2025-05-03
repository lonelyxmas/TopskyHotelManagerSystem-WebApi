using System;

namespace EOM.TSHotelManagement.Common.Util
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class UIDisplayAttribute : Attribute
    {
        public string DisplayName { get; }

        public bool IsNumber { get; set; } = false;

        public bool IsVisible { get; set; } = true;

        public UIDisplayAttribute(string displayName, bool isNumber = false, bool isVisible = true)
        {
            DisplayName = displayName;
            IsNumber = isNumber;
            IsVisible = isVisible;
        }
    }
}
