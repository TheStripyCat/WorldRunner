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
    private float runSpeed, turnSpeed, mouseInput, hInput, frwRotation, landSpeed = 6f, waterSpeed = 5f, railroadSpeed = 8f, shipSpeed = 8f,
        landTurnSpeed = 4f, waterTurnSpeed = 3f, railroadTurnSpeed = 6f, shipTurnSpeed = 5f;
    
    private bool weAreRunning = false;
    private AnimGraphParameter isRunning, isDead;
    [Serialize, ShowInEditor] Actor player;
    [Serialize, ShowInEditor] UIControl startButton, shipTicketIcon, trainTicketIcon, healthUI, sanityUI, strengthUI, loreUI, influenceUI,
        observationUI, willUI, portalsClosedUI, weaponUI, spellUI;
    [Serialize, ShowInEditor] Collider playerTrigger;
    [Serialize, ShowInEditor] POIManager poiManager;
    public Actor portalToClose;
    public Quaternion returnOrientation;
    private int eyesCollected = 0;
    private bool hasTrainTicket, hasShipTicket, landHasRailroads, inTheSea;
    public int health = 7, sanity = 7, lore = 1, strength = 1, influence = 1, observation = 2, maxHealth, maxSanity, will = 1, portalsClosed = 0,
        weapon = 0, spell = 0;
    public override void OnStart()
    {
        //Screen.CursorLock = CursorLockMode.Locked;
        //Screen.CursorVisible = false;
        startButton.Get<Button>().Clicked += StartRunning;
        maxHealth = health;
        maxSanity = sanity;
    }
    
    /// <inheritdoc/>
    public override void OnEnable()
    {        
        isRunning = player.As<AnimatedModel>().GetParameter("isRunning");
        isDead = player.As<AnimatedModel>().GetParameter("isDead");
        playerTrigger.TriggerEnter += OnTriggerEnter;
        UpdateUI();
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
        UpdateUI();
        if ((health <= 0) || (sanity <= 0))
        {
            weAreRunning = false;
            isDead.Value = true;
        }

    }
    private void StartRunning()
    {
        isRunning.Value = true;
        weAreRunning = true;
        Screen.CursorLock = CursorLockMode.Locked;
        Screen.CursorVisible = false;
        poiManager.gameStarted = true;
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
        else if (collider.HasTag("weapon"))
        {
            weapon = influence;
            weaponUI.IsActive = true;
            weaponUI.GetChild<UIControl>().Get<Label>().Text = Convert.ToString(weapon);
            collider.Parent.IsActive = false;
            collider.Parent.Parent.Parent.GetScript<POIScript>().hasItem = false;

        }
        else if (collider.HasTag("spell"))
        {
            spell = influence;
            spellUI.IsActive = true;
            spellUI.GetChild<UIControl>().Get<Label>().Text = Convert.ToString(spell);
            collider.Parent.IsActive = false;
            collider.Parent.Parent.Parent.GetScript<POIScript>().hasItem = false;
        }
        else if (collider.HasTag("danger"))
        {
            health--;
            //Debug.Log("player health " + health);
            healthUI.Get<Label>().Text = Convert.ToString(health);
            if (health <= 0)
            {
                weAreRunning = false;
                isDead.Value = true;
            }
        }
        else if (collider.HasTag("eye"))
        {
            eyesCollected++;
            collider.Parent.Parent.IsActive = false;
            if (eyesCollected == 3)
            {
                portalToClose.IsActive = false;
                portalToClose.Parent.GetScript<POIScript>().hasPortal = false;
                Actor.Position = new Vector3(0, 0, 0);
                Actor.Orientation = returnOrientation;
                portalsClosed++;
                eyesCollected = 0;
            }
        }
        
            
            
            
        
        
    }
    private void UpdateUI()
    {
        healthUI.Get<Label>().Text = Convert.ToString(health);
        sanityUI.Get<Label>().Text = Convert.ToString(sanity);
        strengthUI.Get<Label>().Text = Convert.ToString(strength);
        loreUI.Get<Label>().Text = Convert.ToString(lore);
        influenceUI.Get<Label>().Text = Convert.ToString(influence);
        observationUI.Get<Label>().Text = Convert.ToString(observation);
        willUI.Get<Label>().Text = Convert.ToString(will);
        portalsClosedUI.Get<Label>().Text = Convert.ToString(portalsClosed);
    }
}
