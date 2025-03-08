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
    private float runSpeed = 10f, turnSpeed = 5f, mouseInput, hInput, frwRotation;
    
    private bool weAreRunning = true;
    private AnimGraphParameter isRunning;
    [Serialize, ShowInEditor] Actor player, worldCenter, camera;
    [Serialize, ShowInEditor] UIControl startButton;    
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
    }

    /// <inheritdoc/>
    public override void OnDisable()
    {
        startButton.Get<Button>().Clicked -= StartRunning;
    }

    /// <inheritdoc/>
    public override void OnUpdate()
    {
        if (weAreRunning)
        {
            frwRotation = Time.DeltaTime * runSpeed;
            mouseInput = Input.GetAxis("Mouse X") * Time.DeltaTime * turnSpeed;
            hInput = -Input.GetAxis("Horizontal") * Time.DeltaTime * runSpeed;
            player.Parent.Orientation *= Quaternion.Euler(new Vector3(frwRotation, 0f, hInput));
            player.Parent.LocalOrientation *= Quaternion.Euler(new Vector3(0f, mouseInput, 0f));

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
}
