using Leopotam.Ecs;
using Timers;
using UnityEngine;

namespace Client {

    sealed class SpawnBombsSystem : IEcsRunSystem {

        readonly GameObject bombPrefab;
        readonly SpawnTimer spawnTimer;

        public SpawnBombsSystem(
            GameObject bombPrefab,
            SpawnTimer spawnTimer
        )
        {
            this.bombPrefab = bombPrefab;
            this.spawnTimer = spawnTimer;
        }

        readonly EcsWorld _world = null;

        void IEcsRunSystem.Run () {
            if (spawnTimer.Tirgger(0, Time.deltaTime)) {
                GameObject.Instantiate(bombPrefab);
            }
        }
    }
}