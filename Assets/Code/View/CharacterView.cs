using Leopotam.Ecs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    public EcsEntity entity;

    public float health;

    private SpriteRenderer bar;
    void Start() {
        this.bar = transform.Find("Health").Find("Bar").GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        this.bar.transform.localScale = new Vector3(
            Mathf.Max(0, this.health),
            1,
            1
        );
    }
}
