using System;
using Fissure.Ecs.Context;
using Fissure.Ecs.Entity;
using Fissure.Ecs.System;

namespace Fissure.Example
{
    public class ExampleSystem : BaseSystem
    {
        public ExampleSystem(FissureContext context, params Type[] compatibleTypes) : base(context, compatibleTypes)
        {
        }

        protected override void Execute(FissureEntity entity)
        {
            var messageComponent = entity.GetComponent<ExampleComponent>();
            Console.WriteLine(messageComponent.Message);
        }
    }
}
