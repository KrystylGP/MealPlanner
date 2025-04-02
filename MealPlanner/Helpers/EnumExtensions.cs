using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MealPlanner.Helpers;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        var member = enumValue.GetType()
                              .GetMember(enumValue.ToString())
                              .FirstOrDefault();

        if (member != null)
        {
            var displayAttr = member.GetCustomAttribute<DisplayAttribute>();
            if (displayAttr != null)
            {
                return displayAttr.Name!;
            }
        }
        
        return enumValue.ToString();

    }
}
