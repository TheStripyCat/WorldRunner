using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// Tickets Script.
/// </summary>
public class TicketSpawns : Script
{
    [Serialize, ShowInEditor] Actor shipTicketsParent, trainTicketsParent;
    private Actor[] shipTickets, trainTickets;
    private float timer;
    public override void OnStart()
    {
        shipTickets = shipTicketsParent.GetChildren<Actor>();
        trainTickets = trainTicketsParent.GetChildren<Actor>();
    }
    
    /// <inheritdoc/>
    public override void OnEnable()
    {
        // Here you can add code that needs to be called when script is enabled (eg. register for events)
    }

    /// <inheritdoc/>
    public override void OnDisable()
    {
        // Here you can add code that needs to be called when script is disabled (eg. unregister from events)
    }

    /// <inheritdoc/>
    public override void OnUpdate()
    {
        timer += Time.DeltaTime;
        if (timer > 10f)
        {
            timer = 0f;
            shipTickets[RandomUtil.Random.Next(0, shipTickets.Length)].IsActive = true;
            trainTickets[RandomUtil.Random.Next(0, trainTickets.Length)].IsActive = true;
        }
    }
}
