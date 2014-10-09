using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class Pixelpeli : PhysicsGame
{
    const double nopeus = 125;
    const double hyppyNopeus = 490;
    const int RUUDUN_KOKO = 45;

    Image Tahti1 = LoadImage("Tahti1");
    Image SuperTahti1 = LoadImage("SuperTahti1");
    Image Pelaaja1 = LoadImage("Pelaaja1");

    PlatformCharacter pelaaja;

    SoundEffect maaliAani = LoadSoundEffect("maali");

    public override void Begin()
    {
        

        LuoKentta();
        LisaaNappaimet();
        

       

    }
    void LuoKentta()
    {
        TileMap ruudut = TileMap.FromLevelAsset("kentta1");
        ruudut.SetTileMethod('N', LuoPelaaja);
        ruudut.SetTileMethod('#', LuoPalikka);
        ruudut.SetTileMethod('*', LuoTahti);
        ruudut.SetTileMethod('+', LuoSuperTahti);
        ruudut.Execute(20, 20);

        Level.Background.CreateGradient(Color.Azure, Color.DarkBlue);
        Camera.Follow(pelaaja);
        Camera.ZoomFactor = 1.2;
        Camera.StayInLevel = true;
        Gravity = new Vector(0, -1000);
        LuoPistelaskuri();
        LuoAikaLaskuri();
    }

    void LuoPelaaja(Vector paikka, double leveys, double korkeus)
    {
        pelaaja = new PlatformCharacter(10, 10);
        pelaaja.Position = paikka;
        pelaaja.Image = Pelaaja1;
        pelaaja.Shape = Shape.Circle;
        //pelaaja.Color = Color.White;
        pelaaja.CanRotate = true;

        AddCollisionHandler(pelaaja, "tahti", TormaaTahteen);
        AddCollisionHandler(pelaaja, "supertahti", TormaaSuperTahteen);
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
        PhysicsObject tahti = new PhysicsObject(16, 15);
        tahti.IgnoresCollisionResponse = true;
        tahti.Position = paikka;
        tahti.Image = Tahti1;
        //tahti.Shape = Shape.Star;
        //tahti.Color = Color.Gold;
        tahti.IgnoresGravity = true;
        tahti.Tag = "tahti"; 
        Add(tahti, 1);
    }
   
    void LuoSuperTahti(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject supertahti = new PhysicsObject(19, 19);
        supertahti.IgnoresCollisionResponse = true;
        supertahti.Position = paikka;
        supertahti.Image = SuperTahti1;
        //supertahti.Shape = Shape.Star;
        //supertahti.Color = Color.Silver;
        supertahti.IgnoresGravity = true;
        supertahti.Tag = "supertahti";
        Add(supertahti, 1);

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

    void TormaaTahteen(PhysicsObject pelaaja, PhysicsObject tahti)
    {
      
        MessageDisplay.Add("+1 Tähti!");
        tahti.Destroy();
        pisteLaskuri.Value += 1;
    }

    void TormaaSuperTahteen(PhysicsObject pelaaja, PhysicsObject tahti)
    {

        MessageDisplay.Add("+1 SuperTähti!");
        tahti.Destroy();
        pisteLaskuri.Value += 10;

    }
    
    IntMeter pisteLaskuri;

    void LuoPistelaskuri()
    {
        pisteLaskuri = new IntMeter(0);
        IntMeter keratytEsineet = new IntMeter(0);
        pisteLaskuri.MaxValue = 20;
        pisteLaskuri.UpperLimit += KaikkiKeratty;


        Label pisteNaytto = new Label();
        pisteNaytto.X = Screen.Left + 1700;
        pisteNaytto.Y = Screen.Top - 100;
        pisteNaytto.TextColor = Color.Black;
        pisteNaytto.Color = Color.White;

        pisteNaytto.BindTo(pisteLaskuri);
        Add(pisteNaytto);
    }
    void KaikkiKeratty()
    {
        MessageDisplay.Add("Pelaaja voitti pelin.");

        
    }
    void LuoAikaLaskuri()
    {
        Timer aikaLaskuri = new Timer()

        aikaLaskuri.Interval = 30;
        aikaLaskuri.Timeout += AikaLoppui;
        aikaLaskuri.Start(1);

        Label aikaNaytto = new Label();
        aikaNaytto.TextColor = Color.White;
        aikaNaytto.DecimalPlaces = 1;
        aikaNaytto.BindTo(aikaLaskuri.SecondCounter);
        Add(aikaNaytto);

    }

    void AikaLoppui()
    {
        MessageDisplay.Add("Aika loppui...");
        

        ClearAll ();
        LuoKentta();
        LisaaNappaimet();
       
    }

}