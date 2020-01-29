using System;
using Fissure.Ecs.Context;
using Fissure.Ecs.Entity;
using Fissure.Ecs.System;

namespace Fissure.Example
{
    public class ExampleSystem2 : BaseSystem
    {
        public ExampleSystem2(FissureContext context, params Type[] compatibleTypes) : base(context, compatibleTypes)
        {
        }

        protected override void Execute(FissureEntity entity)
        {
            var num = entity.GetComponent<ExampleComponent2>();
            Console.WriteLine(num.Number);
        }
    }

    public class ExampleSystem3 : BaseSystem
    {
        public ExampleSystem3(FissureContext context, params Type[] compatibleTypes) : base(context, compatibleTypes)
        {
        }

        protected override void Execute(FissureEntity entity)
        {
            var msg = entity.GetComponent<ExampleComponent>();
            var num = entity.GetComponent<ExampleComponent2>();
            Console.WriteLine($"{msg.Message} : {num.Number}");
        }
    }
}
