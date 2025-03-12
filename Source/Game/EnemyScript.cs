using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// EnemyScript Script.
/// </summary>
public class EnemyScript : Script
{
    private Collider enemyCollider;
    private int enemyHealth;
    private Actor[] hitPoints;
    
    public override void OnStart()
    {
        // Here you can add code that needs to be called when script is created, just before the first game update
    }
    
    /// <inheritdoc/>
    public override void OnEnable()
    {
        enemyCollider = Actor.GetChild<Collider>();
        hitPoints = enemyCollider.GetChildren<Actor>();
        enemyHealth = RandomUtil.Random.Next(2, 7);
    }

    /// <inheritdoc/>
    public override void OnDisable()
    {
        // Here you can add code that needs to be called when script is disabled (eg. unregister from events)
    }

    /// <inheritdoc/>
    public override void OnUpdate()
    {
        for (int i = 0; i <= enemyHealth; i++)
        {
            hitPoints[i].IsActive = true;
        }
    }
}
