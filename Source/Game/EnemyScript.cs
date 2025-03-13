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
    [Serialize, ShowInEditor] RigidBody portalRB;
    public int enemyHealth, healthDamage, sanityDamage;
    private Actor[] hitPoints;
    private int hpActive = 0;
    
    public override void OnStart()
    {
        
    }
    
    /// <inheritdoc/>
    public override void OnEnable()
    {
        enemyCollider = Actor.GetChild<Collider>();
        hitPoints = enemyCollider.GetChildren<Actor>();
        enemyHealth = RandomUtil.Random.Next(2, 7);
        enemyCollider.TriggerEnter += OnTriggerEnter;
    }

    /// <inheritdoc/>
    public override void OnDisable()
    {
        // Here you can add code that needs to be called when script is disabled (eg. unregister from events)
    }

    /// <inheritdoc/>
    public override void OnUpdate()
    {
        if (hpActive <= enemyHealth)
        {
            hitPoints[hpActive].IsActive = true;
            hpActive++;
        }
        if (enemyHealth <= 0)
        {
            portalRB.IsActive = true;
            Actor.IsActive = false;
        }
    }
    private void OnTriggerEnter(PhysicsColliderActor collider)
    {
        if (collider.HasTag("Player"))
        {
            if (healthDamage > 0)
            {
                enemyHealth -= collider.Parent.Parent.Parent.GetScript<GlobeRotation>().strength;
            }
            else
            {
                enemyHealth -= collider.Parent.Parent.Parent.GetScript<GlobeRotation>().lore;
            }
            if (enemyHealth > 0)
            {
                foreach (Actor hitPoint in hitPoints)
                {
                    hitPoint.IsActive = false;                
                }
                hpActive = 0;
            }
        }
    }
}
