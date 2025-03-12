using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game;

/// <summary>
/// GlobeRotation Script.
/// </summary>
public class GlobeRotation : Script
{
    private float runSpeed, turnSpeed, mouseInput, hInput, frwRotation, landSpeed = 8f, waterSpeed = 6f, railroadSpeed = 12f, shipSpeed = 10f,
        landTurnSpeed = 4f, waterTurnSpeed = 3f, railroadTurnSpeed = 6f, shipTurnSpeed = 5f;
    
    private bool weAreRunning = true;
    private AnimGraphParameter isRunning;
    [Serialize, ShowInEditor] Actor player;
    [Serialize, ShowInEditor] UIControl startButton, shipTicketIcon, trainTicketIcon;
    [Serialize, ShowInEditor] Collider playerTrigger;
    private bool hasTrainTicket, hasShipTicket, landHasRailroads, inTheSea;
    public int health, sanity, lore, strength, influence, observation, maxHealth, maxSanity, will;
    public override void OnStart()
    {
        Screen.CursorLock = CursorLockMode.Locked;
        Screen.CursorVisible = false;
        startButton.Get<Button>().Clicked += StartRunning;
    }
    
    /// <inheritdoc/>
    public override void OnEnable()
    {        
        isRunning = player.As<AnimatedModel>().GetParameter("isRunning");
        playerTrigger.TriggerEnter += OnTriggerEnter;
    }

    /// <inheritdoc/>
    public override void OnDisable()
    {
        startButton.Get<Button>().Clicked -= StartRunning;
        playerTrigger.TriggerEnter -= OnTriggerEnter;
    }

    /// <inheritdoc/>
    public override void OnUpdate()
    {
        if (weAreRunning)
        {
            frwRotation = Time.DeltaTime * runSpeed;
            mouseInput = Input.GetAxis("Mouse X") * Time.DeltaTime * turnSpeed;
            hInput = -Input.GetAxis("Horizontal") * Time.DeltaTime * runSpeed;
            Actor.Orientation *= Quaternion.Euler(new Vector3(frwRotation, 0f, hInput));
            Actor.LocalOrientation *= Quaternion.Euler(new Vector3(0f, mouseInput, 0f));
            if (Input.GetAction("UseTicket"))
            {
                if ((inTheSea)&&(hasShipTicket))
                {
                    runSpeed = shipSpeed;
                    turnSpeed = shipTurnSpeed;
                    hasShipTicket = false;
                    shipTicketIcon.IsActive = false;
                }
                else if ((landHasRailroads)&&(hasTrainTicket))
                {
                    runSpeed = railroadSpeed;
                    turnSpeed = railroadTurnSpeed;
                    hasTrainTicket = false;
                    trainTicketIcon.IsActive = false;
                }
            }
        }
    }
    private void StartRunning()
    {
        isRunning.Value = true;
        weAreRunning = true;
        Screen.CursorLock = CursorLockMode.Locked;
        Screen.CursorVisible = false;
        startButton.IsActive = false;
    }
    private void OnTriggerEnter(PhysicsColliderActor collider)
    {
         if (collider.HasTag("railway"))
         {
            runSpeed = landSpeed;
            turnSpeed = landTurnSpeed;
            landHasRailroads = true;
            inTheSea = false;            
         }
        else if (collider.HasTag("land"))
        {
            runSpeed = landSpeed;
            turnSpeed = landTurnSpeed;
            landHasRailroads = false;
            inTheSea = false;
        }
        else if (collider.HasTag("water"))
        {
            runSpeed = waterSpeed;
            turnSpeed = waterTurnSpeed;
            landHasRailroads = false;
            inTheSea = true;
        }
        else if (collider.Parent.TryGetScript<Iticket>(out Iticket ticket))
        {
            ticket.BuyTicket(hasShipTicket, hasTrainTicket);
            hasTrainTicket = trainTicketIcon.IsActive;
            hasShipTicket = shipTicketIcon.IsActive;
        }
        else if (collider.HasTag("skill"))
        {
            health += collider.Parent.GetScript<SkillScript>().health;
            sanity += collider.Parent.GetScript<SkillScript>().sanity;
            will += collider.Parent.GetScript<SkillScript>().will;
            strength += collider.Parent.GetScript<SkillScript>().strength;
            influence += collider.Parent.GetScript<SkillScript>().influence;
            observation += collider.Parent.GetScript<SkillScript>().observation;
            lore += collider.Parent.GetScript<SkillScript>().lore;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            else if (sanity > maxSanity)
            {
                sanity = maxSanity;
            }
            else
            {
                collider.Parent.IsActive = false;
                collider.Parent.Parent.Parent.GetScript<POIScript>().hasItem = false;
            }
        }
    }
}
