using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// Ticket Script.
/// </summary>
public class Ticket : Script, Iticket
{
    [Serialize, ShowInEditor] bool isTrainTicket;
    [Serialize, ShowInEditor] UIControl ticketIcon;
    
    public void BuyTicket(bool hasShipTicket, bool hasTrainTicket)
    {
        if (((!isTrainTicket) && (!hasShipTicket)) || ((isTrainTicket) && (!hasTrainTicket)))
        {
            ticketIcon.IsActive = true;
            Actor.IsActive = false;
        }
        else return;
    }
}
