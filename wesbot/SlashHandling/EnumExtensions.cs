using System.ComponentModel;
using System.Reflection;

namespace wesbot.SlashHandling
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription(Enum value)
        {
            // Get the Description attribute value for the enum value
            FieldInfo? fi = value.GetType().GetField(value.ToString());
            if (fi != null)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    return attributes[0].Description;
                } 
            }
            
            return value.ToString();
        }
    }
}
