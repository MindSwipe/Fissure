using System;

namespace Fissure.Ecs.Exceptions
{
    public class ComponentNotFoundException : Exception
    {
        public ComponentNotFoundException()
        {
        }

        public ComponentNotFoundException(string message) : base(message)
        {
        }

        public ComponentNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
