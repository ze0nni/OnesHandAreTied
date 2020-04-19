using Leopotam.Ecs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombView : MonoBehaviour
{
    public EcsEntity entity;

    public delegate void OnDetonate(EcsEntity entity);
    public OnDetonate onDetonate;

    void OnCollisionEnter(Collision collision) {
        onDetonate?.Invoke(entity);
    }
}
