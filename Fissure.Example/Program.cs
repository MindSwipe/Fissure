using System;
using Fissure.Ecs.Context;

namespace Fissure.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var ctx = new FissureContext();
            ctx.AddSystem(new ExampleSystem(ctx, typeof(ExampleComponent)));
            ctx.AddSystem(new ExampleSystem2(ctx, typeof(ExampleComponent2)));
            ctx.AddSystem(new ExampleSystem3(ctx, typeof(ExampleComponent), typeof(ExampleComponent2)));

            ctx.CreateEntity("Test").AddComponents(new ExampleComponent{Message = "Hello World"});
            ctx.CreateEntity("Test1").AddComponents(new ExampleComponent2 {Number = 1});
            var ent3 = ctx.CreateEntity("Test3");
            ent3.AddComponents(new ExampleComponent {Message = "Hello from the other side"},
                new ExampleComponent2 {Number = 1});

            ctx.Run();

            ent3.RemoveComponent<ExampleComponent>();

            ctx.Run();

            Console.ReadLine();
        }
    }
}
