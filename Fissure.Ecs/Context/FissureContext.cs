using System.Collections.Generic;
using System.Linq;
using Fissure.Ecs.Component;
using Fissure.Ecs.Entity;
using Fissure.Ecs.Exceptions;
using Fissure.Ecs.System;

namespace Fissure.Ecs.Context
{
    /// <summary>
    /// The Context of the entire ECS. Sometimes also called 'world' in other ECSs
    /// Central store for all registered systems and entities
    /// </summary>
    public sealed class FissureContext
    {
        public List<FissureEntity> Entities { get; }

        private readonly List<BaseSystem> _systems;

        public FissureContext()
        {
            Entities = new List<FissureEntity>();
            _systems = new List<BaseSystem>();
        }

        #region Events

        public delegate void EntityChanged(FissureEntity entity);

        public event EntityChanged EntityAdded;
        public event EntityChanged EntityRemoved;

        public delegate void EntityComponentChanged(FissureEntity entity, BaseComponent component);

        public event EntityComponentChanged EntityComponentAdded;
        public event EntityComponentChanged EntityComponentRemoved;

        private void OnEntityAdded(FissureEntity entity)
        {
            EntityAdded?.Invoke(entity);
        }

        private void OnEntityRemoved(FissureEntity entity)
        {
            EntityRemoved?.Invoke(entity);
        }

        private void OnEntityComponentAdded(FissureEntity entity, BaseComponent component)
        {
            EntityComponentAdded?.Invoke(entity, component);
        }

        private void OnEntityComponentRemoved(FissureEntity entity, BaseComponent component)
        {
            EntityComponentRemoved?.Invoke(entity, component);
        }

        #endregion

        #region Public Methods

        public FissureEntity CreateEntity(string entityId)
        {
            if (HasEntity(entityId))
                throw new EntityAlreadyExistsException();

            var newEntity = new FissureEntity(entityId);
            newEntity.ComponentAdded += OnEntityComponentAdded;
            newEntity.ComponentRemoved += OnEntityComponentRemoved;

            Entities.Add(newEntity);
            OnEntityAdded(newEntity);
            return newEntity;
        }

        public FissureEntity GetEntity(string entityId)
        {
            var match = Entities.FirstOrDefault(entity => entity.Id == entityId);
            if (match != null)
                return match;

            throw new EntityNotFoundException();
        }

        public void DestroyEntity(ref FissureEntity entity)
        {
            if (HasEntity(entity.Id))
                throw new EntityNotFoundException($"Cannot destroy Entity with the id {entity.Id} as it does not exist");

            Entities.Remove(entity);
            OnEntityRemoved(entity);
            entity = null;
        }

        public void AddSystem(BaseSystem system)
        {
            // TODO duplicate checking
            _systems.Add(system);
        }

        public void Run()
        {
            foreach (var system in _systems)
                system.Run();
        }

        #endregion

        #region Private Methods

        private bool HasEntity(string entityId)
        {
            return Entities.Any(entity => entity.Id == entityId);
        }

        #endregion
    }
}
