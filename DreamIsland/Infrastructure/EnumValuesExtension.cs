namespace DreamIsland.Infrastructure
{
    using System;
    using System.Linq;
    using System.ComponentModel;

    public static class EnumValuesExtension
    {
        public static string GetDescriptionFromEnum(Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
