using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エフェクト終了後自動削除
/// </summary>
public class ParticleAutoDestruction : MonoBehaviour
{
    private ParticleSystem[] particleSystems;

    void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    void Update()
    {
        bool allStopped = true;

        foreach (ParticleSystem ps in particleSystems)
        {
            if (!ps.isStopped)
            {
                allStopped = false;
            }
        }

        if (allStopped)
            GameObject.Destroy(gameObject);
    }
}
