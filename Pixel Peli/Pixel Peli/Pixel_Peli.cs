using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class Pixel_Peli : PhysicsGame
{
    const double nopeus = 200;
    const double hyppyNopeus = 750;
    const int RUUDUN_KOKO = 40;

    PlatformCharacter pelaaja;

    Image pelaajanKuva = LoadImage("norsu");
    Image tahtiKuva = LoadImage("tahti");

    SoundEffect maaliAani = LoadSoundEffect("maali");

    public override void Begin()
    {
        Gravity = new Vector(0, -1000);

        LuoKentta();
        LisaaNappaimet();

        Camera.Follow(pelaaja);
        Camera.ZoomFactor = 1.2;
        Camera.StayInLevel = true;

    }
    void LuoKentta()
    {
        TileMap ruudut = TileMap.FromLevelAsset("kentta1");
        ruudut.SetTileMethod('N', LuoPelaaja);
        ruudut.SetTileMethod('#', LuoPalikka);
        ruudut.SetTileMethod('*', LuoTahti);
        ruudut.Execute(20, 20);
    }

    void LuoPelaaja(Vector paikka, double leveys, double korkeus)
    {
        pelaaja = new PlatformCharacter(10, 10);
        pelaaja.Position = paikka;
        AddCollisionHandler(pelaaja, "tahti", TormaaTahteen);
        Add(pelaaja);
    }

    void LuoPalikka(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject taso = PhysicsObject.CreateStaticObject(leveys, korkeus);
        taso.Position = paikka;
        taso.Shape = Shape.Rectangle;
        taso.Color = Color.Black;
        Add(taso);
    }

    void LuoTahti(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject tahti = new PhysicsObject(5, 5);
        tahti.IgnoresCollisionResponse = true;
        tahti.Position = paikka;
        tahti.Shape = Shape.Star;
        tahti.Color = Color.Gold;
        Add(tahti, 1);
    }

    void LisaaNappaimet()
    {
        Keyboard.Listen(Key.F1, ButtonState.Pressed, ShowControlHelp, "Näytä ohjeet");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");

        Keyboard.Listen(Key.Left, ButtonState.Down, Liikuta, "Liikkuu vasemmalle", pelaaja, -nopeus);
        Keyboard.Listen(Key.Right, ButtonState.Down, Liikuta, "Liikkuu vasemmalle", pelaaja, nopeus);
        Keyboard.Listen(Key.Up, ButtonState.Pressed, Hyppaa, "Pelaaja hyppää", pelaaja, hyppyNopeus);

        ControllerOne.Listen(Button.Back, ButtonState.Pressed, Exit, "Poistu pelistä");

        ControllerOne.Listen(Button.DPadLeft, ButtonState.Down, Liikuta, "Pelaaja liikkuu vasemmalle", pelaaja, -nopeus);
        ControllerOne.Listen(Button.DPadRight, ButtonState.Down, Liikuta, "Pelaaja liikkuu oikealle", pelaaja, nopeus);
        ControllerOne.Listen(Button.A, ButtonState.Pressed, Hyppaa, "Pelaaja hyppää", pelaaja, hyppyNopeus);

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
    }

    void Liikuta(PlatformCharacter hahmo, double nopeus)
    {
        hahmo.Walk(nopeus);
    }

    void Hyppaa(PlatformCharacter hahmo, double nopeus)
    {
        hahmo.Jump(nopeus);
    }

    void TormaaTahteen(PhysicsObject hahmo, PhysicsObject tahti)
    {
        maaliAani.Play();
        MessageDisplay.Add("Keräsit tähden!");
        tahti.Destroy();
    }
}