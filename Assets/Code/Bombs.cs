using Leopotam.Ecs;
using Timers;
using UnityEngine;

namespace Client {

    sealed class SpawnBombsSystem : IEcsRunSystem {

        readonly GameObject bombPrefab;
        readonly float generationRadius;
        readonly float generationHeight;
        readonly SpawnTimer spawnTimer;

        public SpawnBombsSystem(
            GameObject bombPrefab,
            float generationRadius,
            float generationHeight,
            SpawnTimer spawnTimer
        )
        {
            this.bombPrefab = bombPrefab;
            this.generationRadius = generationRadius;
            this.generationHeight = generationHeight;
            this.spawnTimer = spawnTimer;
        }

        readonly EcsWorld _world = null;

        void IEcsRunSystem.Run () {
            if (spawnTimer.Tirgger(0, Time.deltaTime)) {
                var bomb = GameObject.Instantiate(bombPrefab);
                bomb.transform.position = new Vector3(
                    (Random.value - 0.5f) * generationRadius,
                    generationHeight,
                    (Random.value - 0.5f) * generationRadius
                );
            }
        }
    }
}