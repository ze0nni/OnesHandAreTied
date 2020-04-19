using Client;
using Leopotam.Ecs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setup : MonoBehaviour
{
    public GameObject Plane;

    public GameObject bombPrefab;
    public GameObject characterPrefab;

    private EcsSystems systems;

    void Start() {
        var world = new EcsWorld();
#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(world);
#endif

        this.systems = new EcsSystems(world)
            .Add(new SpawnCharactersSystem(characterPrefab, 30, new Timers.SpawnTimer(10, 0.5f, 3)))
            .Add(new CharacterSyncSystem())

            .Add(new SpawnBombsSystem(bombPrefab, 30, 15, new Timers.SpawnTimer(5, 1, 3)))
            .Add(new DetonatedBombBombSystem())
            .Add(new ExplosionSystem(new ExplosionDamageFromDistanceRatio()))

            .Add(new CharacterDamageReleaseSystem());
        ;

        this.systems.Init();
#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(systems);
#endif
    }

    private void Update()
    {
        systems.Run();
    }
}
