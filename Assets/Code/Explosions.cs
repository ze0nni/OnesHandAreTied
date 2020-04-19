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

    public interface ExplosionDamageComputer {
        float Compute(
            float currentDamage,
            Vector3 explosionCenter,
            float explsionRadius,
            Vector3 characterPosition,
            float distance
        );
    }

    sealed class ExplosionSystem : IEcsRunSystem
    {
        ExplosionDamageComputer[] damageComputers;

        public ExplosionSystem(
            params ExplosionDamageComputer[] damageComputers
        ) {
            this.damageComputers = damageComputers; ;
        }

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

        private void ApplyExplosion(Vector3 center, float radius) {
            foreach (var i in charactersFilter) {
                ref var character = ref charactersFilter.Get1(i);
                var distance = Vector3.Distance(center, character.position);
                if (distance <= radius) {
                    ApplyExplosionToCharacter(center, radius, distance, ref character);
                }
            }
        }

        private void ApplyExplosionToCharacter(Vector3 center, float radius, float distance, ref Character character) {
            var characterPosition = character.position;
            var damage = damageComputers
                .Aggregate(
                    1f,
                    (acc, computer) => computer.Compute(acc, center, radius, characterPosition, distance)
                )
            ;


            character.damage = new Damage(damage, character.damage);
        }
    }

    sealed class ExplosionDamageFromDistanceRatio : ExplosionDamageComputer
    {
        public float Compute(float currentDamage, Vector3 explosionCenter, float explsionRadius, Vector3 characterPosition, float distance)
        {
            return currentDamage * (1 - (distance / explsionRadius));
        }
    }
}
