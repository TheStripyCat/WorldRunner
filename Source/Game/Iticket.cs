using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// Iticket interface.
/// </summary>
public interface Iticket
{
    public void BuyTicket(bool hasShipTicket, bool hasTrainTicket);
}
