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

            ctx.CreateEntity("Test").AddComponents(new ExampleComponent{Message = "Hello World"});

            ctx.Run();

            Console.ReadLine();
        }
    }
}
