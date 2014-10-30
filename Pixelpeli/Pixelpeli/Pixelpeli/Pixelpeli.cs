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
    const double hyppyNopeus = 480;
    const int RUUDUN_KOKO = 45;

    Image Tahti1 = LoadImage("Tahti1");
    Image SuperTahti1 = LoadImage("SuperTahti1");
    Image Pelaaja1 = LoadImage("Pelaaja1");
    Image Tausta = LoadImage("Tausta");
    Image Taso1 = LoadImage("Taso1");
    Image Taso2 = LoadImage("Taso2");
    Image Sableye = LoadImage("Sableye");

    PlatformCharacter pelaaja;

    SoundEffect maaliAani = LoadSoundEffect("maali");

    public override void Begin()
    {
        Level.Background.CreateGradient(Color.DarkBlue, Color.Black);
        MultiSelectWindow alkuValikko = new MultiSelectWindow("Kerää kaikki tähdet 30 sekunnissa",
"Aloita peli", "Lopeta");
        alkuValikko.ItemSelected += AloitusValikonNappia;
        Add(alkuValikko);
        alkuValikko.Color = Color.White;
   

    }
    void AloitusValikonNappia(int valinta)
    {

        switch (valinta)
        {
            case 0:
                LuoKentta();
                LisaaNappaimet();
                
                break;
            case 1:
                MultiSelectWindow lisäValikko = new MultiSelectWindow("Ihan varma?",
"Aloita peli", "Joo, Lopeta");
        lisäValikko.ItemSelected += lisaValikonNappia;
        Add(lisäValikko);
                break;
            
        }
    }
    void lisaValikonNappia(int valinta)
    {

        switch (valinta)
        {
            case 0:
                LuoKentta();
                LisaaNappaimet();
                break;
            case 1:
                 MultiSelectWindow lisäValikko = new MultiSelectWindow("Eiks mun peli oo tarpeeks hyvä?",
"Okei pelataan sit, Aloita peli", "En haluu pelata, Lopeta");
        lisäValikko.ItemSelected += lisa2ValikonNappia;
        Add(lisäValikko);
                break;

        }

    }
    void lisa2ValikonNappia(int valinta)
    {

        switch (valinta)
        {
            case 0:
                LuoKentta();
                LisaaNappaimet();
                break;
            case 1:
                MultiSelectWindow lisäValikko = new MultiSelectWindow("PELAA!",
"Pelaa peliä", "En");
                lisäValikko.ItemSelected += lisa3ValikonNappia;
                Add(lisäValikko);
                break;

        }
    }
    void lisa3ValikonNappia(int valinta)
    {

        switch (valinta)
        {
            case 0:
                LuoKentta();
                LisaaNappaimet();
                break;
            case 1:
                MultiSelectWindow lisäValikko = new MultiSelectWindow(">:D Nyt ei oo muuta vaihto ehtoo",
"Pelaa peliä");
                lisäValikko.ItemSelected += lisa3ValikonNappia;
                Add(lisäValikko);
                break;

        }
    }
    void LuoKentta()
    {
        TileMap ruudut = TileMap.FromLevelAsset("kentta2");
        ruudut.SetTileMethod('N', LuoPelaaja);
        ruudut.SetTileMethod('#', LuoPalikka);
        ruudut.SetTileMethod('3', LuoPalikka2);
        ruudut.SetTileMethod('*', LuoTahti);
        ruudut.SetTileMethod('+', LuoSuperTahti);
        ruudut.SetTileMethod('S', Luosableye);
        ruudut.Execute(19, 19);

        Level.Background.Image = Tausta; 
        
        Camera.Follow(pelaaja);
        Camera.ZoomFactor = 1.2;
        Camera.StayInLevel = true;
        Gravity = new Vector(0, -1000);
       
        LuoAikaLaskuri();
        LuoPistelaskuri();
        
    }

    void LuoPelaaja(Vector paikka, double leveys, double korkeus)
    {
        pelaaja = new PlatformCharacter(10, 10);
        pelaaja.Position = paikka;
        pelaaja.Image = Pelaaja1;
        pelaaja.Shape = Shape.Circle;
        //pelaaja.Color = Color.White;
        //pelaaja.CanRotate = true;

        AddCollisionHandler(pelaaja, "tahti", TormaaTahteen);
        AddCollisionHandler(pelaaja, "supertahti", TormaaSuperTahteen);
        AddCollisionHandler(pelaaja, "sableye", Tormaasableye);
        Add(pelaaja);
    }

    void LuoPalikka(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject taso = PhysicsObject.CreateStaticObject(leveys, korkeus);
        taso.Position = paikka;
        taso.Shape = Shape.Rectangle;
        taso.Image = Taso1;
        //taso.Color = Color.Black;
        Add(taso);
    }
    void LuoPalikka2(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject taso2 = PhysicsObject.CreateStaticObject(leveys, korkeus);
        taso2.Position = paikka;
        taso2.Shape = Shape.Rectangle;
        taso2.Image = Taso2; 
        //taso2.Color = Color.Black;
        Add(taso2);
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
   
    void Luosableye(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject sableye = new PhysicsObject(11, 14);
        sableye.IgnoresCollisionResponse = false;
        sableye.Position = paikka;
        sableye.Image = Sableye;
        sableye.IgnoresGravity = false;
        sableye.CanRotate = false;
        sableye.Tag = "sableye";
        Add(sableye, 1);
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
    
    void Tormaasableye(PhysicsObject pelaaja, PhysicsObject sableye)
    {

        MessageDisplay.Add("Sableye");

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

        ClearAll();

        Level.Background.CreateGradient(Color.DarkBlue, Color.Black);

        MultiSelectWindow valikko = new MultiSelectWindow("Siinä se oli, Loppu",
"Yritä uudestaa.", "Lopeta");
        valikko.ItemSelected += PainettiinValikonNappia;
        Add(valikko);

        
    }
    void LuoAikaLaskuri()
    {
        Timer aikaLaskuri = new Timer();

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

        ClearAll ();

        Level.Background.CreateGradient(Color.DarkBlue, Color.Black);

        MultiSelectWindow valikko = new MultiSelectWindow("Aika loppui",
"Yritä uudestaa.", "Lopeta");
        valikko.ItemSelected += PainettiinValikonNappia;
        Add(valikko);


       
    }
    void PainettiinValikonNappia(int valinta)
    {
       
        switch (valinta)
        {
            case 0:
                //AloitaPeli();
        LuoKentta();
        LisaaNappaimet();
        
                break;
            case 1:
                Exit();
                break;
        }
    }


    public Image Tausta1 { get; set; }
}