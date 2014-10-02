using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class pong : PhysicsGame
{
    public override void Begin()
    {
        PhysicsObject kissa = new PhysicsObject(400.0, 400.0);
        kissa.Shape = Shape.Circle;
        Add(kissa);
        kissa.X = -200.0;
        kissa.Y = 0.0;
        // TODO: Kirjoita ohjelmakoodisi tähän

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }
}
