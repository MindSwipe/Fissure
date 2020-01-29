using System;
using System.Collections.Generic;
using System.Linq;
using Fissure.Ecs.Component;
using Fissure.Ecs.Context;
using Fissure.Ecs.Exceptions;
using Fissure.Ecs.Extensions;

namespace Fissure.Ecs.Entity
{
    public sealed class FissureEntity
    {
        public string Id { get; set; }

        public List<BaseComponent> Components { get; set; }

        public List<FissureEntity> Children { get; set; }

        public FissureEntity Parent { get; set; }

        public EntityState State { get; internal set; }

        public event FissureContext.EntityComponentChanged ComponentAdded;
        public event FissureContext.EntityComponentChanged ComponentRemoved;

        private void OnComponentAdded(FissureEntity entity, BaseComponent component)
        {
            ComponentAdded?.Invoke(entity, component);
        }

        private void OnComponentRemoved(FissureEntity entity, BaseComponent component)
        {
            ComponentRemoved?.Invoke(entity, component);
        }

        public FissureEntity(string id)
        {
            Id = id;
            State = EntityState.Active;
            Children = new List<FissureEntity>();
            Components = new List<BaseComponent>();
        }

        #region Public Methods

        public void Activate()
        {
            State = EntityState.Active;
        }

        public void Deactivate()
        {
            State = EntityState.Inacitve;
        }

        public void RemoveComponent(Type componentType)
        {
            if (!componentType.IsComponent())
                throw new NotComponentException($"The type {componentType.Name} is not of type BaseComponent, cannot remove non components from Entity");

            if (!TryGetComponent(componentType, out var component))
                throw new ComponentNotFoundException($"Cannot remove a component the Entity does not contain. Tried removing {componentType.Name}");

            Components.Remove(component);
            OnComponentRemoved(this, component);
        }

        public void RemoveComponent<T>() where T : BaseComponent
        {
            if (!TryGetComponent<T>(out var component))
                throw new ComponentNotFoundException($"Cannot remove a component the Entity does not contain.Tried removing {typeof(T).Name}");

            Components.Remove(component);
            OnComponentRemoved(this, component);
        }

        public void MoveComponent(BaseComponent component, FissureEntity destination)
        {
            if (component == null)
                throw new ComponentNotFoundException("Cannot move component to destination, the Component is null");
            if (!HasComponent(component.GetType()))
                throw new ComponentNotFoundException($"Cannot move component {component.GetType().Name} to destination, the Component does not exist in the source");

            destination.AddComponent(component);
            RemoveComponent(component.GetType());
        }

        public T GetComponent<T>() where T : BaseComponent
        {
            var foundComponents = Components.OfType<T>();
            if (foundComponents == null)
                throw new ComponentNotFoundException($"Cannot get component of type {typeof(T).Name} because the Entity does not contain it");

            return foundComponents.First();
        }

        public BaseComponent GetComponent(Type componentType)
        {
            if (!componentType.IsComponent())
                throw new NotComponentException($"The given type {componentType.Name} is not a Component");

            if (!HasComponent(componentType))
                throw new ComponentNotFoundException($"Cannot get component of type {componentType.Name} because the Entity does not contain it");

            return Components.First(component => component.GetType() == componentType);
        }

        public bool TryGetComponent(Type componentType, out BaseComponent component)
        {
            if (HasComponent(componentType))
            {
                component = Components.First(c => c.GetType() == componentType);
                return true;
            }

            component = null;
            return false;
        }

        public bool TryGetComponent<T>(out BaseComponent component) where T : BaseComponent
        {
            if (HasComponent<T>())
            {
                component = Components.OfType<T>().First();
                return true;
            }

            component = null;
            return false;
        }

        public bool HasComponent<TBaseComponent>() where TBaseComponent : BaseComponent
        {
            return Components.Any(component => component.GetType() == typeof(TBaseComponent));
        }

        public bool HasComponent(Type componentType)
        {
            return Components.Any(component => component.GetType() == componentType);
        }

        public void AddComponents(params BaseComponent[] components)
        {
            foreach (var component in components)
                AddComponent(component);
        }

        public void AddComponents(IEnumerable<BaseComponent> components)
        {
            foreach (var component in components)
                AddComponent(component);
        }

        #endregion

        #region Private Methods

        private void AddComponent(BaseComponent component)
        {
            if (HasComponent(component.GetType()))
                throw new ComponentAlreadyExistsException();

            Components.Add(component);
            OnComponentAdded(this, component);
        }

        #endregion
    }
}
