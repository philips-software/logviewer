using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Analogy.DataTypes
{
    public enum FontSelectionType
    {
        Normal,
        Large,
        VeryLarge,
        Manual
    }
    public enum LogLevelSelectionType
    {
        Single,
        Multiple
    }
    public enum BuiltInSearchPanelMode
    {
        Search,
        Filter,
    }
    public enum UpdateMode
    {
        [Display(Name = "Never")] Never,
        [Display(Name = "Each Startup")] EachStartup,
        [Display(Name = "Once a Week")] OnceAWeek,
        [Display(Name = "Once a Month")] OnceAMonth,
    }    
    /// <summary>
    /// Represents custom filter item types.
    /// </summary>
    public enum DateRangeFilter
    {
        /// <summary>
        /// No filter
        /// </summary>
        None,
        /// <summary>
        /// Current date
        /// </summary>
        Today,
        /// <summary>
        /// Current date and yesterday
        /// </summary>
        Last2Days,
        /// <summary>
        /// Today, yesterday and the day before yesterday
        /// </summary>
        Last3Days,
        /// <summary>
        /// Last 7 days
        /// </summary>
        LastWeek,
        /// <summary>
        /// Last 2 weeks
        /// </summary>
        Last2Weeks,
        /// <summary>
        /// Last one month
        /// </summary>
        LastMonth
    }

    public static class EnumUtils
    {
        public static string GetDisplay(this Enum value)

        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            DisplayAttribute[] attributes = (DisplayAttribute[])value.GetType().GetField(value.ToString())
                .GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attributes.Count() != 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            return attributes[0].Name;

        }

        public static Dictionary<string, string> GetDisplayValues(this Type enumType)
        {
            var enumValues = new Dictionary<string, string>();
            foreach (Enum value in Enum.GetValues(enumType))
            {
                enumValues.Add(value.ToString(), value.GetDisplay());
            }

            return enumValues;
        }
    }
}