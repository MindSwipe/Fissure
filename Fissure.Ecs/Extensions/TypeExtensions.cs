using System;
using Fissure.Ecs.Component;

namespace Fissure.Ecs.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsComponent(this Type self)
        {
            return typeof(BaseComponent).IsAssignableFrom(self);
        }
    }
}
