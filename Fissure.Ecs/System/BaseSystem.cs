using System;
using System.Collections.Generic;
using System.Linq;
using Fissure.Ecs.Component;
using Fissure.Ecs.Context;
using Fissure.Ecs.Entity;
using Fissure.Ecs.Exceptions;
using Fissure.Ecs.Extensions;

namespace Fissure.Ecs.System
{
    /// <summary>
    /// Central common ancestor for all systems. Implements features like splitting entities that are affected into their components
    /// and passing them to a method
    /// </summary>
    public abstract class BaseSystem
    {
        public FissureContext Context { get; set; }

        protected List<Type> CompatibleComponents { get; }

        private readonly List<FissureEntity> _entitiesToActOn;

        protected BaseSystem(FissureContext context, params Type[] compatibleTypes)
        {
            if (compatibleTypes.Any(x => !x.IsComponent()))
                throw new NotComponentException($"One or more types passed into System is not a Component");

            _entitiesToActOn = new List<FissureEntity>();

            Context = context;
            CompatibleComponents = new List<Type>(compatibleTypes);

            Context.EntityAdded += OnContextEntityAdded;
            Context.EntityRemoved += OnContextEntityRemoved;
            Context.EntityComponentAdded += OnContextEntityComponentAdded;
            Context.EntityComponentRemoved += OnContextEntityComponentRemoved;
        }

        public void AddCompatibleComponent(Type type)
        {
            if (!type.IsComponent())
                throw new NotComponentException($"The given Type {type.Name} is not a Component");

            CompatibleComponents.Add(type);
        }

        public void Run()
        {
            foreach (var entity in _entitiesToActOn.Where(x => x.State == EntityState.Active))
            {
                Execute(entity);
            }
        }

        protected abstract void Execute(FissureEntity entity);

        #region Private Methods

        private void OnContextEntityAdded(FissureEntity addedEntity)
        {
            if (addedEntity.Components.Select(x => x.GetType()).Intersect(CompatibleComponents).Count() == CompatibleComponents.Count)
            //if (addedEntity.Components.Any(component => CompatibleComponents.Contains(component.GetType())))
                _entitiesToActOn.Add(addedEntity);
        }

        private void OnContextEntityRemoved(FissureEntity removedEntity)
        {
            if (_entitiesToActOn.Select(x => x.Id).Contains(removedEntity.Id))
                _entitiesToActOn.Remove(removedEntity);
        }

        private void OnContextEntityComponentAdded(FissureEntity changedEntity, BaseComponent addedComponent)
        {
            if (CompatibleComponents.Contains(addedComponent.GetType()) && changedEntity.Components
                    .Select(x => x.GetType()).Intersect(CompatibleComponents).Count() ==
                CompatibleComponents.Count)
                _entitiesToActOn.Add(changedEntity);
        }

        private void OnContextEntityComponentRemoved(FissureEntity changedEntity, BaseComponent removedComponent)
        {
            if (CompatibleComponents.Any(x => x == removedComponent.GetType()))
            {
                _entitiesToActOn.Remove(changedEntity);
            }
        }

        #endregion
    }
}
