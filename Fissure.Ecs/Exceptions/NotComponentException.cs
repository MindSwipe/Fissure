using System;

namespace Fissure.Ecs.Exceptions
{
    public class NotComponentException : Exception
    {
        public NotComponentException()
        {
        }

        public NotComponentException(string message) : base(message)
        {
        }

        public NotComponentException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
