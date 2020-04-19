using Leopotam.Ecs;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timers;
using UnityEngine;

namespace Client
{
    public struct Character
    {
        public CharacterView view;
        public Damage damage;
    }

    sealed public class Damage {
        
        public float value;
        public Damage next;

        public Damage(float value, Damage next)
        {
            this.value = value;
            this.next = next;
        }
    }

    sealed class SpawnCharactersSystem : IEcsRunSystem
    {

        readonly GameObject characterPrefab;
        readonly float generationRadius;
        readonly SpawnTimer spawnTimer;

        public SpawnCharactersSystem(
            GameObject characterPrefab,
            float generationRadius,
            SpawnTimer spawnTimer
        )
        {
            this.characterPrefab = characterPrefab;
            this.generationRadius = generationRadius;
            this.spawnTimer = spawnTimer;
        }

        readonly EcsWorld world = null;
        readonly EcsFilter<Character> charactersFilter = null;

        void IEcsRunSystem.Run()
        {
            if (false == spawnTimer.Tirgger(charactersFilter.GetEntitiesCount(), Time.deltaTime))
            {
                return;
            }

            var characterView = GameObject.Instantiate(characterPrefab).GetComponent<CharacterView>();
            characterView.transform.position = new Vector3(
                (Random.value - 0.5f) * generationRadius,
                2,
                (Random.value - 0.5f) * generationRadius
            );

            var characterEntity = world.NewEntity();
            ref var character = ref characterEntity.Set<Character>();

            character.view = characterView;

            characterView.entity = characterEntity;
        }
    }
}
