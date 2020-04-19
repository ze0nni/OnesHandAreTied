using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Client {
    struct Explosion
    {
        public Vector3 center;
        public float radius;
    }

    sealed class ExplosionSystem : IEcsRunSystem
    {
        readonly EcsWorld world = null;
        readonly EcsFilter<Explosion> explosionsFilter = null;
        readonly EcsFilter<Character> charactersFilter = null;

        public void Run()
        {
            foreach (var i in explosionsFilter) {
                var explosionEntity = explosionsFilter.GetEntity(i);
                var explosion = explosionsFilter.Get1(i);

                ApplyExplosion(explosion.center, explosion.radius);

                explosionEntity.Destroy();
            }
        }

        readonly private Collider[] colliders = new Collider[256];

        private void ApplyExplosion(Vector3 center, float radius) {
            foreach (var i in charactersFilter) {
                ref var character = ref charactersFilter.Get1(i);
                // Конечно же тут не стоит делать ' character.view.transform.position' лучше сохранить
                // Vector3 прямо character, тогда будет меньше промохов кеша
                var distance = Vector3.Distance(center, character.view.transform.position);

                if (distance <= radius) {
                    ApplyExplosionToCharacter(center, radius, distance, ref character);
                }
            }
        }

        private void ApplyExplosionToCharacter(Vector3 center, float radius, float distance, ref Character character) {
            character.damage = new Damage(1, character.damage);
        }
    }
}
