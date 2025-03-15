using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// POIManager Script.
/// </summary>
public class POIManager : Script
{
    [Serialize, ShowInEditor] List<Actor> allCities;
    private int maxPortals, portalsOpened, randomPOI, randomEnemy, enemyHealth, randomItemPOI, randomItem;
    private float portalTimer, itemTimer;
    private POIScript poiScript, itemPOIScript;
    public bool gameStarted;
    public override void OnStart()
    {
        maxPortals = allCities.Count;
        portalsOpened = 0;
    }
    
    /// <inheritdoc/>
    public override void OnEnable()
    {
        randomPOI = RandomUtil.Random.Next(0, maxPortals);
        poiScript = allCities[randomPOI].GetScript<POIScript>();
        poiScript.portal.IsActive = true;
        poiScript.hasPortal = true;
        randomEnemy = RandomUtil.Random.Next(0, poiScript.enemies.Count);
        poiScript.enemies[randomEnemy].IsActive = true;
        portalsOpened++;        
    }

    /// <inheritdoc/>
    public override void OnDisable()
    {
        // Here you can add code that needs to be called when script is disabled (eg. unregister from events)
    }

    /// <inheritdoc/>
    public override void OnUpdate()
    {
        if (gameStarted)
        {
            portalTimer += Time.DeltaTime;
            itemTimer += Time.DeltaTime;
            if ((portalTimer >= 120f) && (portalsOpened < maxPortals))
            {
                OpenPortal();
            }
            else if ((portalTimer >= 60f) && (portalsOpened == maxPortals)) Debug.Log("gameover");

            if ((itemTimer >= 20f) && (portalsOpened < maxPortals))
            {
                SpawnItem();
            }
        }
    }
    private void OpenPortal()
    {
            randomPOI = RandomUtil.Random.Next(0, maxPortals);
            poiScript = allCities[randomPOI].GetScript<POIScript>();
            if (!poiScript.portal.IsActive)
            {
                poiScript.portal.IsActive = true;
                poiScript.itemSpawn.IsActive = false;                
                portalsOpened++;
                poiScript.hasPortal = true;
                randomEnemy = RandomUtil.Random.Next(0, poiScript.enemies.Count);
                poiScript.enemies[randomEnemy].IsActive = true;
                portalTimer = 0f;
            }        
        
    }
    private void SpawnItem()
    {
        randomItemPOI = RandomUtil.Random.Next(0, maxPortals);
        itemPOIScript = allCities[randomItemPOI].GetScript<POIScript>();
        if ((!itemPOIScript.portal.IsActive)&&(!itemPOIScript.hasItem))
        {
            randomItem = RandomUtil.Random.Next(0, itemPOIScript.skills.Count);
            itemPOIScript.skills[randomItem].IsActive = true;
            itemPOIScript.hasItem = true;
            itemTimer = 0f;
        }
    }
}
