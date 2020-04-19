using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BangView : MonoBehaviour
{
    public AnimationCurve SizeAnimation;

    float radius;
    float delay = 1f;

    void Update()
    {
        if (delay <= 0) {
            GameObject.Destroy(this.gameObject);
            return;
        }

        this.transform.localScale = Vector3.one * 2 * radius * Math.Max(0, SizeAnimation.Evaluate(1 - delay));

        delay -= Time.deltaTime;
    }

    public void Spawn(Vector3 center, float radius)
    {
        this.radius = radius;

        transform.position = center;
    }
}
