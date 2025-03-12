using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// PortalScript Script.
/// </summary>
public class PortalScript : Script
{
    [Serialize, ShowInEditor] Collider portalCollider;
    [Serialize, ShowInEditor] Actor player;
    [Serialize, ShowInEditor] List<Actor> eyes; 
    private float timer = 0f;
    private Quaternion playerOrientation;
    private int eyesToActivate = 0;


    public override void OnEnable()
    {
         portalCollider.TriggerEnter += EnterThePortal;        
    }


    public override void OnDisable()
    {
        portalCollider.TriggerEnter -= EnterThePortal;
    }

    public override void OnUpdate()
    {
        if (eyesToActivate > 0)
        {
            eyes[RandomUtil.Random.Next(0, 12)].IsActive = true;
            eyesToActivate -= 1;
        }
    }

    public void EnterThePortal(PhysicsColliderActor collider)
    {
        if (collider.HasTag("Player"))
        {
            playerOrientation = player.Orientation;
            player.Position = new Vector3(0, 0, 28000);
            foreach (Actor eye in eyes)
            {
                eye.IsActive = false;
            }
            eyesToActivate = player.GetScript<GlobeRotation>().observation * 2;
        }
    }
}
