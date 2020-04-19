using Leopotam.Ecs;
using Timers;
using UnityEngine;

namespace Client {

    struct Bomb {
        public BombView view;
    }

    struct DetonatedBomb {
        
    }

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

        readonly EcsWorld world = null;
        readonly EcsFilter<Bomb> bombsFilter = null;

        void IEcsRunSystem.Run () {
            if (false == spawnTimer.Tirgger(bombsFilter.GetEntitiesCount(), Time.deltaTime))
            {
                return;
            }

            var bombView = GameObject.Instantiate(bombPrefab).GetComponent<BombView>();
            bombView.transform.position = new Vector3(
                (Random.value - 0.5f) * generationRadius,
                generationHeight,
                (Random.value - 0.5f) * generationRadius
            );

            var bombEntity = world.NewEntity();
            ref var bomb = ref bombEntity.Set<Bomb>();

            bomb.view = bombView;

            bombView.entity = bombEntity;
            bombView.onDetonate = OnDetonate;
        }

        private void OnDetonate(EcsEntity bombEntity) {
            bombEntity.Set<DetonatedBomb>();
        }
    }

    sealed class DetonatedBombBombSystem : IEcsRunSystem
    {
        readonly GameObject bangPrefab;

        public DetonatedBombBombSystem(GameObject bangPrefab)
        {
            this.bangPrefab = bangPrefab;
        }

        readonly EcsWorld world = null;
        readonly EcsFilter<Bomb, DetonatedBomb> bombsFilter = null;

        public void Run()
        {
            foreach (var i in bombsFilter) {
                var bombEntity = bombsFilter.GetEntity(i);
                var bomb = bombsFilter.Get1(i);

                ref var explosion = ref world.NewEntity().Set<Explosion>();
                explosion.center = bomb.view.transform.position;
                explosion.radius = 5;

                GameObject.Destroy(bomb.view.gameObject);
                bombEntity.Destroy();

                GameObject.Instantiate(bangPrefab).GetComponent<BangView>()
                    .Spawn(explosion.center, explosion.radius);
            }
        }
    }
}