using System;

namespace Fissure.Ecs.Exceptions
{
    public class ComponentAlreadyExistsException : Exception
    {
        public ComponentAlreadyExistsException()
        {
        }

        public ComponentAlreadyExistsException(string message) : base(message)
        {
        }

        public ComponentAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
