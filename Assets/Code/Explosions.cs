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
            var size = Physics.OverlapSphereNonAlloc(center, radius, colliders);
            for (var i = 0; i < size; i++) {
                var characterView = colliders[i].gameObject.GetComponent<CharacterView>();
                if (null != characterView) {
                    ApplyExplosionToCharacter(center, radius, characterView);
                }
            }
        }

        private void ApplyExplosionToCharacter(Vector3 center, float radius, CharacterView characterView) {
            characterView.entity.Destroy();
            GameObject.Destroy(characterView.gameObject);
        }
    }
}
