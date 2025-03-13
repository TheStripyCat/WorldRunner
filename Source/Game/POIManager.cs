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
    private int maxPortals, portalsOpened, randomPOI, randomEnemy, enemyHealth;
    private float timer;
    private POIScript poiScript;
    private bool poiFound = false;
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
        Debug.Log("first portal opened");
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
        if ((timer >= 60f) && (portalsOpened < maxPortals))
        {
            OpenPortal();
        }
        else if ((timer >= 60f) && (portalsOpened == maxPortals)) Debug.Log("gameover");
    }
    private void OpenPortal()
    {
        if (!poiFound)
        {
            randomPOI = RandomUtil.Random.Next(0, maxPortals);
            poiScript = allCities[randomPOI].GetScript<POIScript>();
            if (!poiScript.portal.IsActive)
            {
                poiScript.portal.IsActive = true;
                poiScript.itemSpawn.IsActive = false;
                poiFound = true;
                portalsOpened++;
                poiScript.hasPortal = true;
                randomEnemy = RandomUtil.Random.Next(0, poiScript.enemies.Count);
                poiScript.enemies[randomEnemy].IsActive = true;                
                poiFound = false;
                timer = 0f;
            }
        }      
        
    }
}
