﻿// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Gameplay.Generators.BaseMapGenerator
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Engine.Items;
using djack.RogueSurvivor.Engine.MapObjects;
using System;
using System.Drawing;

namespace djack.RogueSurvivor.Gameplay.Generators
{
  internal abstract class BaseMapGenerator : MapGenerator
  {
    private static readonly string[] MALE_SKINS = new string[5]
    {
      "Actors\\Decoration\\male_skin1",
      "Actors\\Decoration\\male_skin2",
      "Actors\\Decoration\\male_skin3",
      "Actors\\Decoration\\male_skin4",
      "Actors\\Decoration\\male_skin5"
    };
    private static readonly string[] MALE_HEADS = new string[8]
    {
      "Actors\\Decoration\\male_hair1",
      "Actors\\Decoration\\male_hair2",
      "Actors\\Decoration\\male_hair3",
      "Actors\\Decoration\\male_hair4",
      "Actors\\Decoration\\male_hair5",
      "Actors\\Decoration\\male_hair6",
      "Actors\\Decoration\\male_hair7",
      "Actors\\Decoration\\male_hair8"
    };
    private static readonly string[] MALE_TORSOS = new string[5]
    {
      "Actors\\Decoration\\male_shirt1",
      "Actors\\Decoration\\male_shirt2",
      "Actors\\Decoration\\male_shirt3",
      "Actors\\Decoration\\male_shirt4",
      "Actors\\Decoration\\male_shirt5"
    };
    private static readonly string[] MALE_LEGS = new string[5]
    {
      "Actors\\Decoration\\male_pants1",
      "Actors\\Decoration\\male_pants2",
      "Actors\\Decoration\\male_pants3",
      "Actors\\Decoration\\male_pants4",
      "Actors\\Decoration\\male_pants5"
    };
    private static readonly string[] MALE_SHOES = new string[3]
    {
      "Actors\\Decoration\\male_shoes1",
      "Actors\\Decoration\\male_shoes2",
      "Actors\\Decoration\\male_shoes3"
    };
    private static readonly string[] MALE_EYES = new string[6]
    {
      "Actors\\Decoration\\male_eyes1",
      "Actors\\Decoration\\male_eyes2",
      "Actors\\Decoration\\male_eyes3",
      "Actors\\Decoration\\male_eyes4",
      "Actors\\Decoration\\male_eyes5",
      "Actors\\Decoration\\male_eyes6"
    };
    private static readonly string[] FEMALE_SKINS = new string[5]
    {
      "Actors\\Decoration\\female_skin1",
      "Actors\\Decoration\\female_skin2",
      "Actors\\Decoration\\female_skin3",
      "Actors\\Decoration\\female_skin4",
      "Actors\\Decoration\\female_skin5"
    };
    private static readonly string[] FEMALE_HEADS = new string[7]
    {
      "Actors\\Decoration\\female_hair1",
      "Actors\\Decoration\\female_hair2",
      "Actors\\Decoration\\female_hair3",
      "Actors\\Decoration\\female_hair4",
      "Actors\\Decoration\\female_hair5",
      "Actors\\Decoration\\female_hair6",
      "Actors\\Decoration\\female_hair7"
    };
    private static readonly string[] FEMALE_TORSOS = new string[4]
    {
      "Actors\\Decoration\\female_shirt1",
      "Actors\\Decoration\\female_shirt2",
      "Actors\\Decoration\\female_shirt3",
      "Actors\\Decoration\\female_shirt4"
    };
    private static readonly string[] FEMALE_LEGS = new string[5]
    {
      "Actors\\Decoration\\female_pants1",
      "Actors\\Decoration\\female_pants2",
      "Actors\\Decoration\\female_pants3",
      "Actors\\Decoration\\female_pants4",
      "Actors\\Decoration\\female_pants5"
    };
    private static readonly string[] FEMALE_SHOES = new string[3]
    {
      "Actors\\Decoration\\female_shoes1",
      "Actors\\Decoration\\female_shoes2",
      "Actors\\Decoration\\female_shoes3"
    };
    private static readonly string[] FEMALE_EYES = new string[6]
    {
      "Actors\\Decoration\\female_eyes1",
      "Actors\\Decoration\\female_eyes2",
      "Actors\\Decoration\\female_eyes3",
      "Actors\\Decoration\\female_eyes4",
      "Actors\\Decoration\\female_eyes5",
      "Actors\\Decoration\\female_eyes6"
    };
    private static readonly string[] BIKER_HEADS = new string[3]
    {
      "Actors\\Decoration\\biker_hair1",
      "Actors\\Decoration\\biker_hair2",
      "Actors\\Decoration\\biker_hair3"
    };
    private static readonly string[] BIKER_LEGS = new string[1]
    {
      "Actors\\Decoration\\biker_pants"
    };
    private static readonly string[] BIKER_SHOES = new string[1]
    {
      "Actors\\Decoration\\biker_shoes"
    };
    private static readonly string[] CHARGUARD_HEADS = new string[1]
    {
      "Actors\\Decoration\\charguard_hair"
    };
    private static readonly string[] CHARGUARD_LEGS = new string[1]
    {
      "Actors\\Decoration\\charguard_pants"
    };
    private static readonly string[] DOG_SKINS = new string[3]
    {
      "Actors\\Decoration\\dog_skin1",
      "Actors\\Decoration\\dog_skin2",
      "Actors\\Decoration\\dog_skin3"
    };
    private static readonly string[] MALE_FIRST_NAMES = new string[139]
    {
      "Alan",
      "Albert",
      "Alex",
      "Alexander",
      "Andrew",
      "Andy",
      "Anton",
      "Anthony",
      "Ashley",
      "Axel",
      "Ben",
      "Bill",
      "Bob",
      "Brad",
      "Brandon",
      "Brian",
      "Bruce",
      "Caine",
      "Carl",
      "Carlton",
      "Charlie",
      "Clark",
      "Cody",
      "Cris",
      "Cristobal",
      "Dan",
      "Danny",
      "Dave",
      "David",
      "Dirk",
      "Don",
      "Donovan",
      "Doug",
      "Dustin",
      "Ed",
      "Eddy",
      "Edward",
      "Elias",
      "Elie",
      "Elmer",
      "Elton",
      "Eric",
      "Eugene",
      "Francis",
      "Frank",
      "Fred",
      "Garry",
      "Georges",
      "Greg",
      "Guy",
      "Gordon",
      "Hank",
      "Harold",
      "Harvey",
      "Henry",
      "Hubert",
      "Indy",
      "Jack",
      "Jake",
      "James",
      "Jarvis",
      "Jason",
      "Jeff",
      "Jeffrey",
      "Jeremy",
      "Jessie",
      "Jesus",
      "Jim",
      "John",
      "Johnny",
      "Jonas",
      "Joseph",
      "Julian",
      "Karl",
      "Keith",
      "Ken",
      "Larry",
      "Lars",
      "Lee",
      "Lennie",
      "Lewis",
      "Mark",
      "Mathew",
      "Max",
      "Michael",
      "Mickey",
      "Mike",
      "Mitch",
      "Ned",
      "Neil",
      "Nick",
      "Norman",
      "Oliver",
      "Orlando",
      "Oscar",
      "Pablo",
      "Patrick",
      "Pete",
      "Peter",
      "Phil",
      "Philip",
      "Preston",
      "Quentin",
      "Randy",
      "Rick",
      "Rob",
      "Ron",
      "Ross",
      "Robert",
      "Roberto",
      "Rudy",
      "Ryan",
      "Sam",
      "Samuel",
      "Saul",
      "Scott",
      "Shane",
      "Shaun",
      "Stan",
      "Stanley",
      "Stephen",
      "Steve",
      "Stuart",
      "Ted",
      "Tim",
      "Toby",
      "Tom",
      "Tommy",
      "Tony",
      "Travis",
      "Trevor",
      "Ulrich",
      "Val",
      "Vince",
      "Vincent",
      "Vinnie",
      "Walter",
      "Wayne",
      "Xavier"
    };
    private static readonly string[] FEMALE_FIRST_NAMES = new string[109]
    {
      "Abigail",
      "Amanda",
      "Ali",
      "Alice",
      "Alicia",
      "Alison",
      "Amy",
      "Angela",
      "Ann",
      "Annie",
      "Audrey",
      "Belinda",
      "Beth",
      "Brenda",
      "Carla",
      "Carolin",
      "Carrie",
      "Cassie",
      "Cherie",
      "Cheryl",
      "Claire",
      "Connie",
      "Cris",
      "Crissie",
      "Christina",
      "Dana",
      "Debbie",
      "Deborah",
      "Debrah",
      "Diana",
      "Dona",
      "Elayne",
      "Eleonor",
      "Elizabeth",
      "Ester",
      "Felicia",
      "Fiona",
      "Fran",
      "Gina",
      "Ginger",
      "Gloria",
      "Grace",
      "Helen",
      "Helena",
      "Hilary",
      "Holy",
      "Ingrid",
      "Isabela",
      "Jackie",
      "Jennifer",
      "Jess",
      "Jill",
      "Joana",
      "Kate",
      "Kathleen",
      "Kathy",
      "Katrin",
      "Kim",
      "Kira",
      "Leonor",
      "Leslie",
      "Linda",
      "Lindsay",
      "Lisa",
      "Liz",
      "Lorraine",
      "Lucia",
      "Lucy",
      "Maggie",
      "Margareth",
      "Maria",
      "Mary",
      "Mary-Ann",
      "Marylin",
      "Michelle",
      "Millie",
      "Molly",
      "Monica",
      "Nancy",
      "Ophelia",
      "Paquita",
      "Page",
      "Patricia",
      "Patty",
      "Paula",
      "Rachel",
      "Raquel",
      "Regina",
      "Roberta",
      "Ruth",
      "Sabrina",
      "Samantha",
      "Sandra",
      "Sarah",
      "Sofia",
      "Sue",
      "Susan",
      "Tabatha",
      "Tanya",
      "Teresa",
      "Tess",
      "Tifany",
      "Tori",
      "Veronica",
      "Victoria",
      "Vivian",
      "Wendy",
      "Winona",
      "Zora"
    };
    private static readonly string[] LAST_NAMES = new string[52]
    {
      "Anderson",
      "Austin",
      "Bent",
      "Black",
      "Bradley",
      "Brown",
      "Bush",
      "Carpenter",
      "Carter",
      "Collins",
      "Cordell",
      "Dobbs",
      "Engels",
      "Finch",
      "Ford",
      "Forrester",
      "Gates",
      "Hewlett",
      "Holtz",
      "Irvin",
      "Jones",
      "Kennedy",
      "Lambert",
      "Lesaint",
      "Lee",
      "Lewis",
      "McAllister",
      "Malory",
      "McGready",
      "Norton",
      "O'Brien",
      "Oswald",
      "Patterson",
      "Paul",
      "Pitt",
      "Quinn",
      "Ramirez",
      "Reeves",
      "Rockwell",
      "Rogers",
      "Robertson",
      "Sanchez",
      "Smith",
      "Stevens",
      "Steward",
      "Tarver",
      "Taylor",
      "Ulrich",
      "Vance",
      "Washington",
      "Walters",
      "White"
    };
    private static string[] CARS = new string[4]
    {
      "MapObjects\\car1",
      "MapObjects\\car2",
      "MapObjects\\car3",
      "MapObjects\\car4"
    };
    protected readonly RogueGame m_Game;

    protected BaseMapGenerator(RogueGame game)
      : base(game.Rules)
    {
      this.m_Game = game;
    }

    public void DressCivilian(DiceRoller roller, Actor actor)
    {
      if (actor.Model.DollBody.IsMale)
        this.DressCivilian(roller, actor, BaseMapGenerator.MALE_EYES, BaseMapGenerator.MALE_SKINS, BaseMapGenerator.MALE_HEADS, BaseMapGenerator.MALE_TORSOS, BaseMapGenerator.MALE_LEGS, BaseMapGenerator.MALE_SHOES);
      else
        this.DressCivilian(roller, actor, BaseMapGenerator.FEMALE_EYES, BaseMapGenerator.FEMALE_SKINS, BaseMapGenerator.FEMALE_HEADS, BaseMapGenerator.FEMALE_TORSOS, BaseMapGenerator.FEMALE_LEGS, BaseMapGenerator.FEMALE_SHOES);
    }

    public void SkinNakedHuman(DiceRoller roller, Actor actor)
    {
      if (actor.Model.DollBody.IsMale)
        this.SkinNakedHuman(roller, actor, BaseMapGenerator.MALE_EYES, BaseMapGenerator.MALE_SKINS, BaseMapGenerator.MALE_HEADS);
      else
        this.SkinNakedHuman(roller, actor, BaseMapGenerator.FEMALE_EYES, BaseMapGenerator.FEMALE_SKINS, BaseMapGenerator.FEMALE_HEADS);
    }

    public void DressCivilian(DiceRoller roller, Actor actor, string[] eyes, string[] skins, string[] heads, string[] torsos, string[] legs, string[] shoes)
    {
      actor.Doll.RemoveAllDecorations();
      actor.Doll.AddDecoration(DollPart.EYES, eyes[roller.Roll(0, eyes.Length)]);
      actor.Doll.AddDecoration(DollPart.SKIN, skins[roller.Roll(0, skins.Length)]);
      actor.Doll.AddDecoration(DollPart.HEAD, heads[roller.Roll(0, heads.Length)]);
      actor.Doll.AddDecoration(DollPart.TORSO, torsos[roller.Roll(0, torsos.Length)]);
      actor.Doll.AddDecoration(DollPart.LEGS, legs[roller.Roll(0, legs.Length)]);
      actor.Doll.AddDecoration(DollPart.FEET, shoes[roller.Roll(0, shoes.Length)]);
    }

    public void SkinNakedHuman(DiceRoller roller, Actor actor, string[] eyes, string[] skins, string[] heads)
    {
      actor.Doll.RemoveAllDecorations();
      actor.Doll.AddDecoration(DollPart.EYES, eyes[roller.Roll(0, eyes.Length)]);
      actor.Doll.AddDecoration(DollPart.SKIN, skins[roller.Roll(0, skins.Length)]);
      actor.Doll.AddDecoration(DollPart.HEAD, heads[roller.Roll(0, heads.Length)]);
    }

    public void SkinDog(DiceRoller roller, Actor actor)
    {
      actor.Doll.RemoveAllDecorations();
      actor.Doll.AddDecoration(DollPart.SKIN, BaseMapGenerator.DOG_SKINS[roller.Roll(0, BaseMapGenerator.DOG_SKINS.Length)]);
    }

    public void DressArmy(DiceRoller roller, Actor actor)
    {
      actor.Doll.RemoveAllDecorations();
      actor.Doll.AddDecoration(DollPart.SKIN, BaseMapGenerator.MALE_SKINS[roller.Roll(0, BaseMapGenerator.MALE_SKINS.Length)]);
      actor.Doll.AddDecoration(DollPart.HEAD, "Actors\\Decoration\\army_helmet");
      actor.Doll.AddDecoration(DollPart.TORSO, "Actors\\Decoration\\army_shirt");
      actor.Doll.AddDecoration(DollPart.LEGS, "Actors\\Decoration\\army_pants");
      actor.Doll.AddDecoration(DollPart.FEET, "Actors\\Decoration\\army_shoes");
    }

    public void DressPolice(DiceRoller roller, Actor actor)
    {
      actor.Doll.RemoveAllDecorations();
      actor.Doll.AddDecoration(DollPart.EYES, BaseMapGenerator.MALE_EYES[roller.Roll(0, BaseMapGenerator.MALE_EYES.Length)]);
      actor.Doll.AddDecoration(DollPart.SKIN, BaseMapGenerator.MALE_SKINS[roller.Roll(0, BaseMapGenerator.MALE_SKINS.Length)]);
      actor.Doll.AddDecoration(DollPart.HEAD, BaseMapGenerator.MALE_HEADS[roller.Roll(0, BaseMapGenerator.MALE_HEADS.Length)]);
      actor.Doll.AddDecoration(DollPart.HEAD, "Actors\\Decoration\\police_hat");
      actor.Doll.AddDecoration(DollPart.TORSO, "Actors\\Decoration\\police_uniform");
      actor.Doll.AddDecoration(DollPart.LEGS, "Actors\\Decoration\\police_pants");
      actor.Doll.AddDecoration(DollPart.FEET, "Actors\\Decoration\\police_shoes");
    }

    public void DressBiker(DiceRoller roller, Actor actor)
    {
      actor.Doll.RemoveAllDecorations();
      actor.Doll.AddDecoration(DollPart.EYES, BaseMapGenerator.MALE_EYES[roller.Roll(0, BaseMapGenerator.MALE_EYES.Length)]);
      actor.Doll.AddDecoration(DollPart.SKIN, BaseMapGenerator.MALE_SKINS[roller.Roll(0, BaseMapGenerator.MALE_SKINS.Length)]);
      actor.Doll.AddDecoration(DollPart.HEAD, BaseMapGenerator.BIKER_HEADS[roller.Roll(0, BaseMapGenerator.BIKER_HEADS.Length)]);
      actor.Doll.AddDecoration(DollPart.LEGS, BaseMapGenerator.BIKER_LEGS[roller.Roll(0, BaseMapGenerator.BIKER_LEGS.Length)]);
      actor.Doll.AddDecoration(DollPart.FEET, BaseMapGenerator.BIKER_SHOES[roller.Roll(0, BaseMapGenerator.BIKER_SHOES.Length)]);
    }

    public void DressGangsta(DiceRoller roller, Actor actor)
    {
      actor.Doll.RemoveAllDecorations();
      actor.Doll.AddDecoration(DollPart.EYES, BaseMapGenerator.MALE_EYES[roller.Roll(0, BaseMapGenerator.MALE_EYES.Length)]);
      actor.Doll.AddDecoration(DollPart.SKIN, BaseMapGenerator.MALE_SKINS[roller.Roll(0, BaseMapGenerator.MALE_SKINS.Length)]);
      actor.Doll.AddDecoration(DollPart.TORSO, "Actors\\Decoration\\gangsta_shirt");
      actor.Doll.AddDecoration(DollPart.HEAD, BaseMapGenerator.MALE_HEADS[roller.Roll(0, BaseMapGenerator.MALE_HEADS.Length)]);
      actor.Doll.AddDecoration(DollPart.HEAD, "Actors\\Decoration\\gangsta_hat");
      actor.Doll.AddDecoration(DollPart.LEGS, "Actors\\Decoration\\gangsta_pants");
      actor.Doll.AddDecoration(DollPart.FEET, BaseMapGenerator.MALE_SHOES[roller.Roll(0, BaseMapGenerator.MALE_SHOES.Length)]);
    }

    public void DressCHARGuard(DiceRoller roller, Actor actor)
    {
      actor.Doll.RemoveAllDecorations();
      actor.Doll.AddDecoration(DollPart.EYES, BaseMapGenerator.MALE_EYES[roller.Roll(0, BaseMapGenerator.MALE_EYES.Length)]);
      actor.Doll.AddDecoration(DollPart.SKIN, BaseMapGenerator.MALE_SKINS[roller.Roll(0, BaseMapGenerator.MALE_SKINS.Length)]);
      actor.Doll.AddDecoration(DollPart.HEAD, BaseMapGenerator.CHARGUARD_HEADS[roller.Roll(0, BaseMapGenerator.CHARGUARD_HEADS.Length)]);
      actor.Doll.AddDecoration(DollPart.LEGS, BaseMapGenerator.CHARGUARD_LEGS[roller.Roll(0, BaseMapGenerator.CHARGUARD_LEGS.Length)]);
    }

    public void DressBlackOps(DiceRoller roller, Actor actor)
    {
      actor.Doll.RemoveAllDecorations();
      actor.Doll.AddDecoration(DollPart.EYES, BaseMapGenerator.MALE_EYES[roller.Roll(0, BaseMapGenerator.MALE_EYES.Length)]);
      actor.Doll.AddDecoration(DollPart.SKIN, BaseMapGenerator.MALE_SKINS[roller.Roll(0, BaseMapGenerator.MALE_SKINS.Length)]);
      actor.Doll.AddDecoration(DollPart.TORSO, "Actors\\Decoration\\blackop_suit");
    }

    public string RandomSkin(DiceRoller roller, bool isMale)
    {
      string[] strArray = isMale ? BaseMapGenerator.MALE_SKINS : BaseMapGenerator.FEMALE_SKINS;
      return strArray[roller.Roll(0, strArray.Length)];
    }

    public void GiveNameToActor(DiceRoller roller, Actor actor)
    {
      if (actor.Model.DollBody.IsMale)
        this.GiveNameToActor(roller, actor, BaseMapGenerator.MALE_FIRST_NAMES, BaseMapGenerator.LAST_NAMES);
      else
        this.GiveNameToActor(roller, actor, BaseMapGenerator.FEMALE_FIRST_NAMES, BaseMapGenerator.LAST_NAMES);
    }

    public void GiveNameToActor(DiceRoller roller, Actor actor, string[] firstNames, string[] lastNames)
    {
      actor.IsProperName = true;
      string str = firstNames[roller.Roll(0, firstNames.Length)] + " " + lastNames[roller.Roll(0, lastNames.Length)];
      actor.Name = str;
    }

    public void GiveRandomSkillsToActor(DiceRoller roller, Actor actor, int count)
    {
      for (int index = 0; index < count; ++index)
        this.GiveRandomSkillToActor(roller, actor);
    }

    public void GiveRandomSkillToActor(DiceRoller roller, Actor actor)
    {
      Skills.IDs skillID = !actor.Model.Abilities.IsUndead ? Skills.RollLiving(roller) : Skills.RollUndead(roller);
      this.GiveStartingSkillToActor(actor, skillID);
    }

    public void GiveStartingSkillToActor(Actor actor, Skills.IDs skillID)
    {
      if (actor.Sheet.SkillTable.GetSkillLevel((int) skillID) >= Skills.MaxSkillLevel(skillID))
        return;
      actor.Sheet.SkillTable.AddOrIncreaseSkill((int) skillID);
      this.RecomputeActorStartingStats(actor);
    }

    public void RecomputeActorStartingStats(Actor actor)
    {
      actor.HitPoints = this.m_Rules.ActorMaxHPs(actor);
      actor.StaminaPoints = this.m_Rules.ActorMaxSTA(actor);
      actor.FoodPoints = this.m_Rules.ActorMaxFood(actor);
      actor.SleepPoints = this.m_Rules.ActorMaxSleep(actor);
      actor.Sanity = this.m_Rules.ActorMaxSanity(actor);
      if (actor.Inventory == null)
        return;
      actor.Inventory.MaxCapacity = this.m_Rules.ActorMaxInv(actor);
    }

    protected DoorWindow MakeObjWoodenDoor()
    {
      DoorWindow doorWindow = new DoorWindow("wooden door", "MapObjects\\wooden_door_closed", "MapObjects\\wooden_door_open", "MapObjects\\wooden_door_broken", 40);
      doorWindow.GivesWood = true;
      return doorWindow;
    }

    protected DoorWindow MakeObjHospitalDoor()
    {
      DoorWindow doorWindow = new DoorWindow("door", "MapObjects\\hospital_door_closed", "MapObjects\\hospital_door_open", "MapObjects\\hospital_door_broken", 40);
      doorWindow.GivesWood = true;
      return doorWindow;
    }

    protected DoorWindow MakeObjCharDoor()
    {
      return new DoorWindow("CHAR door", "MapObjects\\dark_door_closed", "MapObjects\\dark_door_open", "MapObjects\\dark_door_broken", 160);
    }

    protected DoorWindow MakeObjGlassDoor()
    {
      DoorWindow doorWindow = new DoorWindow("glass door", "MapObjects\\glass_door_closed", "MapObjects\\glass_door_open", "MapObjects\\glass_door_broken", 10);
      doorWindow.IsMaterialTransparent = true;
      doorWindow.BreaksWhenFiredThrough = true;
      return doorWindow;
    }

    protected DoorWindow MakeObjIronDoor()
    {
      DoorWindow doorWindow = new DoorWindow("iron door", "MapObjects\\iron_door_closed", "MapObjects\\iron_door_open", "MapObjects\\iron_door_broken", 320);
      doorWindow.IsAn = true;
      return doorWindow;
    }

    protected DoorWindow MakeObjWindow()
    {
      DoorWindow doorWindow = new DoorWindow("window", "MapObjects\\window_closed", "MapObjects\\window_open", "MapObjects\\window_broken", 10);
      doorWindow.IsWindow = true;
      doorWindow.IsMaterialTransparent = true;
      doorWindow.GivesWood = true;
      doorWindow.BreaksWhenFiredThrough = true;
      return doorWindow;
    }

    protected MapObject MakeObjFence(string fenceImageID)
    {
      return new MapObject("fence", fenceImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, 400)
      {
        IsMaterialTransparent = true,
        JumpLevel = 1,
        GivesWood = true,
        StandOnFovBonus = true
      };
    }

    protected MapObject MakeObjIronFence(string fenceImageID)
    {
      return new MapObject("iron fence", fenceImageID)
      {
        IsMaterialTransparent = true,
        IsAn = true
      };
    }

    protected MapObject MakeObjIronGate(string gateImageID)
    {
      return new MapObject("iron gate", gateImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, 800)
      {
        IsMaterialTransparent = true,
        IsAn = true
      };
    }

    public Fortification MakeObjSmallFortification(string imageID)
    {
      Fortification fortification = new Fortification("small fortification", imageID, 20);
      fortification.IsMaterialTransparent = true;
      fortification.GivesWood = true;
      fortification.IsMovable = true;
      fortification.Weight = 4;
      fortification.JumpLevel = 1;
      return fortification;
    }

    public Fortification MakeObjLargeFortification(string imageID)
    {
      Fortification fortification = new Fortification("large fortification", imageID, 40);
      fortification.GivesWood = true;
      return fortification;
    }

    protected MapObject MakeObjTree(string treeImageID)
    {
      return new MapObject("tree", treeImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, 400)
      {
        GivesWood = true
      };
    }

    protected MapObject MakeObjWreckedCar(DiceRoller roller)
    {
      return this.MakeObjWreckedCar(BaseMapGenerator.CARS[roller.Roll(0, BaseMapGenerator.CARS.Length)]);
    }

    protected MapObject MakeObjWreckedCar(string carImageID)
    {
      return new MapObject("wrecked car", carImageID)
      {
        BreakState = MapObject.Break.BROKEN,
        IsMaterialTransparent = true,
        JumpLevel = 1,
        IsMovable = true,
        Weight = 100,
        StandOnFovBonus = true
      };
    }

    protected MapObject MakeObjShelf(string shelfImageID)
    {
      return new MapObject("shelf", shelfImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, 40)
      {
        IsContainer = true,
        GivesWood = true,
        IsMovable = true,
        Weight = 6
      };
    }

    protected MapObject MakeObjBench(string benchImageID)
    {
      return new MapObject("bench", benchImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, 80)
      {
        IsMaterialTransparent = true,
        JumpLevel = 1,
        IsCouch = true,
        GivesWood = true
      };
    }

    protected MapObject MakeObjIronBench(string benchImageID)
    {
      return new MapObject("iron bench", benchImageID)
      {
        IsMaterialTransparent = true,
        JumpLevel = 1,
        IsCouch = true,
        IsAn = true
      };
    }

    protected MapObject MakeObjBed(string bedImageID)
    {
      return new MapObject("bed", bedImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, 80)
      {
        IsMaterialTransparent = true,
        IsWalkable = true,
        IsCouch = true,
        GivesWood = true,
        IsMovable = true,
        Weight = 6
      };
    }

    protected MapObject MakeObjWardrobe(string wardrobeImageID)
    {
      return new MapObject("wardrobe", wardrobeImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, 240)
      {
        IsMaterialTransparent = false,
        IsContainer = true,
        GivesWood = true,
        IsMovable = true,
        Weight = 10
      };
    }

    protected MapObject MakeObjDrawer(string drawerImageID)
    {
      return new MapObject("drawer", drawerImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, 40)
      {
        IsMaterialTransparent = true,
        IsContainer = true,
        GivesWood = true,
        IsMovable = true,
        Weight = 6
      };
    }

    protected MapObject MakeObjTable(string tableImageID)
    {
      return new MapObject("table", tableImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, 40)
      {
        IsMaterialTransparent = true,
        JumpLevel = 1,
        GivesWood = true,
        IsMovable = true,
        Weight = 2
      };
    }

    protected MapObject MakeObjChair(string chairImageID)
    {
      return new MapObject("chair", chairImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, 13)
      {
        IsMaterialTransparent = true,
        JumpLevel = 1,
        GivesWood = true,
        IsMovable = true,
        Weight = 1
      };
    }

    protected MapObject MakeObjNightTable(string nightTableImageID)
    {
      return new MapObject("night table", nightTableImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, 13)
      {
        IsMaterialTransparent = true,
        JumpLevel = 1,
        GivesWood = true,
        IsMovable = true,
        Weight = 1
      };
    }

    protected MapObject MakeObjFridge(string fridgeImageID)
    {
      return new MapObject("fridge", fridgeImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, 240)
      {
        IsContainer = true,
        IsMovable = true,
        Weight = 10
      };
    }

    protected MapObject MakeObjJunk(string junkImageID)
    {
      return new MapObject("junk", junkImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, 40)
      {
        IsPlural = true,
        IsMaterialTransparent = true,
        IsMovable = true,
        GivesWood = true,
        Weight = 6
      };
    }

    protected MapObject MakeObjBarrels(string barrelsImageID)
    {
      return new MapObject("barrels", barrelsImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, 80)
      {
        IsPlural = true,
        IsMaterialTransparent = true,
        IsMovable = true,
        GivesWood = true,
        Weight = 10
      };
    }

    protected PowerGenerator MakeObjPowerGenerator(string offImageID, string onImageID)
    {
      return new PowerGenerator("power generator", offImageID, onImageID);
    }

    public MapObject MakeObjBoard(string imageID, string[] text)
    {
      return (MapObject) new Board("board", imageID, text);
    }

    public void DecorateOutsideWalls(Map map, Rectangle rect, Func<int, int, string> decoFn)
    {
      for (int left = rect.Left; left < rect.Right; ++left)
      {
        for (int top = rect.Top; top < rect.Bottom; ++top)
        {
          Tile tileAt = map.GetTileAt(left, top);
          if (!tileAt.Model.IsWalkable && !tileAt.IsInside)
          {
            string imageID = decoFn(left, top);
            if (imageID != null)
              tileAt.AddDecoration(imageID);
          }
        }
      }
    }

    public Item MakeItemBandages()
    {
      ItemMedicine itemMedicine = new ItemMedicine((ItemModel) this.m_Game.GameItems.BANDAGE);
      itemMedicine.Quantity = this.m_Rules.Roll(1, this.m_Game.GameItems.BANDAGE.StackingLimit);
      return (Item) itemMedicine;
    }

    public Item MakeItemMedikit()
    {
      return (Item) new ItemMedicine((ItemModel) this.m_Game.GameItems.MEDIKIT);
    }

    public Item MakeItemPillsSTA()
    {
      ItemMedicine itemMedicine = new ItemMedicine((ItemModel) this.m_Game.GameItems.PILLS_STA);
      itemMedicine.Quantity = this.m_Rules.Roll(1, this.m_Game.GameItems.PILLS_STA.StackingLimit);
      return (Item) itemMedicine;
    }

    public Item MakeItemPillsSLP()
    {
      ItemMedicine itemMedicine = new ItemMedicine((ItemModel) this.m_Game.GameItems.PILLS_SLP);
      itemMedicine.Quantity = this.m_Rules.Roll(1, this.m_Game.GameItems.PILLS_SLP.StackingLimit);
      return (Item) itemMedicine;
    }

    public Item MakeItemPillsSAN()
    {
      ItemMedicine itemMedicine = new ItemMedicine((ItemModel) this.m_Game.GameItems.PILLS_SAN);
      itemMedicine.Quantity = this.m_Rules.Roll(1, this.m_Game.GameItems.PILLS_SAN.StackingLimit);
      return (Item) itemMedicine;
    }

    public Item MakeItemPillsAntiviral()
    {
      ItemMedicine itemMedicine = new ItemMedicine((ItemModel) this.m_Game.GameItems.PILLS_ANTIVIRAL);
      itemMedicine.Quantity = this.m_Rules.Roll(1, this.m_Game.GameItems.PILLS_ANTIVIRAL.StackingLimit);
      return (Item) itemMedicine;
    }

    public Item MakeItemGroceries()
    {
      int turnCounter = this.m_Game.Session.WorldTime.TurnCounter;
      int max = 720 * this.m_Game.GameItems.GROCERIES.BestBeforeDays;
      int num = this.m_Rules.Roll(max / 2, max);
      return (Item) new ItemFood((ItemModel) this.m_Game.GameItems.GROCERIES, turnCounter + num);
    }

    public Item MakeItemCannedFood()
    {
      ItemFood itemFood = new ItemFood((ItemModel) this.m_Game.GameItems.CANNED_FOOD);
      itemFood.Quantity = this.m_Rules.Roll(1, this.m_Game.GameItems.CANNED_FOOD.StackingLimit);
      return (Item) itemFood;
    }

    public Item MakeItemCrowbar()
    {
      ItemMeleeWeapon itemMeleeWeapon = new ItemMeleeWeapon((ItemModel) this.m_Game.GameItems.CROWBAR);
      itemMeleeWeapon.Quantity = this.m_Rules.Roll(1, this.m_Game.GameItems.CROWBAR.StackingLimit);
      return (Item) itemMeleeWeapon;
    }

    public Item MakeItemBaseballBat()
    {
      return (Item) new ItemMeleeWeapon((ItemModel) this.m_Game.GameItems.BASEBALLBAT);
    }

    public Item MakeItemCombatKnife()
    {
      return (Item) new ItemMeleeWeapon((ItemModel) this.m_Game.GameItems.COMBAT_KNIFE);
    }

    public Item MakeItemTruncheon()
    {
      return (Item) new ItemMeleeWeapon((ItemModel) this.m_Game.GameItems.TRUNCHEON);
    }

    public Item MakeItemGolfClub()
    {
      return (Item) new ItemMeleeWeapon((ItemModel) this.m_Game.GameItems.GOLFCLUB);
    }

    public Item MakeItemIronGolfClub()
    {
      return (Item) new ItemMeleeWeapon((ItemModel) this.m_Game.GameItems.IRON_GOLFCLUB);
    }

    public Item MakeItemHugeHammer()
    {
      return (Item) new ItemMeleeWeapon((ItemModel) this.m_Game.GameItems.HUGE_HAMMER);
    }

    public Item MakeItemSmallHammer()
    {
      return (Item) new ItemMeleeWeapon((ItemModel) this.m_Game.GameItems.SMALL_HAMMER);
    }

    public Item MakeItemJasonMyersAxe()
    {
      ItemMeleeWeapon itemMeleeWeapon = new ItemMeleeWeapon((ItemModel) this.m_Game.GameItems.UNIQUE_JASON_MYERS_AXE);
      itemMeleeWeapon.IsUnique = true;
      return (Item) itemMeleeWeapon;
    }

    public Item MakeItemShovel()
    {
      return (Item) new ItemMeleeWeapon((ItemModel) this.m_Game.GameItems.SHOVEL);
    }

    public Item MakeItemShortShovel()
    {
      return (Item) new ItemMeleeWeapon((ItemModel) this.m_Game.GameItems.SHORT_SHOVEL);
    }

    public ItemBarricadeMaterial MakeItemWoodenPlank()
    {
      return new ItemBarricadeMaterial((ItemModel) this.m_Game.GameItems.WOODENPLANK);
    }

    public Item MakeItemHuntingCrossbow()
    {
      return (Item) new ItemRangedWeapon((ItemModel) this.m_Game.GameItems.HUNTING_CROSSBOW);
    }

    public Item MakeItemBoltsAmmo()
    {
      return (Item) new ItemAmmo((ItemModel) this.m_Game.GameItems.AMMO_BOLTS);
    }

    public Item MakeItemHuntingRifle()
    {
      return (Item) new ItemRangedWeapon((ItemModel) this.m_Game.GameItems.HUNTING_RIFLE);
    }

    public Item MakeItemLightRifleAmmo()
    {
      return (Item) new ItemAmmo((ItemModel) this.m_Game.GameItems.AMMO_LIGHT_RIFLE);
    }

    public Item MakeItemPistol()
    {
      return (Item) new ItemRangedWeapon((ItemModel) this.m_Game.GameItems.PISTOL);
    }

    public Item MakeItemKoltRevolver()
    {
      return (Item) new ItemRangedWeapon((ItemModel) this.m_Game.GameItems.KOLT_REVOLVER);
    }

    public Item MakeItemRandomPistol()
    {
      if (!this.m_Game.Rules.RollChance(50))
        return this.MakeItemKoltRevolver();
      return this.MakeItemPistol();
    }

    public Item MakeItemLightPistolAmmo()
    {
      return (Item) new ItemAmmo((ItemModel) this.m_Game.GameItems.AMMO_LIGHT_PISTOL);
    }

    public Item MakeItemShotgun()
    {
      return (Item) new ItemRangedWeapon((ItemModel) this.m_Game.GameItems.SHOTGUN);
    }

    public Item MakeItemShotgunAmmo()
    {
      return (Item) new ItemAmmo((ItemModel) this.m_Game.GameItems.AMMO_SHOTGUN);
    }

    public Item MakeItemCHARLightBodyArmor()
    {
      return (Item) new ItemBodyArmor((ItemModel) this.m_Game.GameItems.CHAR_LT_BODYARMOR);
    }

    public Item MakeItemBikerGangJacket(GameGangs.IDs gangId)
    {
      if (gangId == GameGangs.IDs.BIKER_HELLS_SOULS)
        return (Item) new ItemBodyArmor((ItemModel) this.m_Game.GameItems.HELLS_SOULS_JACKET);
      if (gangId == GameGangs.IDs.BIKER_FREE_ANGELS)
        return (Item) new ItemBodyArmor((ItemModel) this.m_Game.GameItems.FREE_ANGELS_JACKET);
      throw new ArgumentException("unhandled biker gang");
    }

    public Item MakeItemPoliceJacket()
    {
      return (Item) new ItemBodyArmor((ItemModel) this.m_Game.GameItems.POLICE_JACKET);
    }

    public Item MakeItemPoliceRiotArmor()
    {
      return (Item) new ItemBodyArmor((ItemModel) this.m_Game.GameItems.POLICE_RIOT);
    }

    public Item MakeItemHunterVest()
    {
      return (Item) new ItemBodyArmor((ItemModel) this.m_Game.GameItems.HUNTER_VEST);
    }

    public Item MakeItemCellPhone()
    {
      return (Item) new ItemTracker((ItemModel) this.m_Game.GameItems.CELL_PHONE);
    }

    public Item MakeItemSprayPaint()
    {
      ItemSprayPaintModel itemSprayPaintModel;
      switch (this.m_Game.Rules.Roll(0, 4))
      {
        case 0:
          itemSprayPaintModel = this.m_Game.GameItems.SPRAY_PAINT1;
          break;
        case 1:
          itemSprayPaintModel = this.m_Game.GameItems.SPRAY_PAINT2;
          break;
        case 2:
          itemSprayPaintModel = this.m_Game.GameItems.SPRAY_PAINT3;
          break;
        case 3:
          itemSprayPaintModel = this.m_Game.GameItems.SPRAY_PAINT4;
          break;
        default:
          throw new ArgumentOutOfRangeException("unhandled roll");
      }
      return (Item) new ItemSprayPaint((ItemModel) itemSprayPaintModel);
    }

    public Item MakeItemStenchKiller()
    {
      return (Item) new ItemSprayScent(this.m_Game.GameItems.STENCH_KILLER);
    }

    public Item MakeItemArmyRifle()
    {
      return (Item) new ItemRangedWeapon((ItemModel) this.m_Game.GameItems.ARMY_RIFLE);
    }

    public Item MakeItemPrecisionRifle()
    {
      return (Item) new ItemRangedWeapon((ItemModel) this.m_Game.GameItems.PRECISION_RIFLE);
    }

    public Item MakeItemHeavyRifleAmmo()
    {
      return (Item) new ItemAmmo((ItemModel) this.m_Game.GameItems.AMMO_HEAVY_RIFLE);
    }

    public Item MakeItemArmyPistol()
    {
      return (Item) new ItemRangedWeapon((ItemModel) this.m_Game.GameItems.ARMY_PISTOL);
    }

    public Item MakeItemHeavyPistolAmmo()
    {
      return (Item) new ItemAmmo((ItemModel) this.m_Game.GameItems.AMMO_HEAVY_PISTOL);
    }

    public Item MakeItemArmyBodyArmor()
    {
      return (Item) new ItemBodyArmor((ItemModel) this.m_Game.GameItems.ARMY_BODYARMOR);
    }

    public Item MakeItemArmyRation()
    {
      return (Item) new ItemFood((ItemModel) this.m_Game.GameItems.ARMY_RATION, this.m_Game.Session.WorldTime.TurnCounter + 720 * this.m_Game.GameItems.ARMY_RATION.BestBeforeDays);
    }

    public Item MakeItemFlashlight()
    {
      return (Item) new ItemLight((ItemModel) this.m_Game.GameItems.FLASHLIGHT);
    }

    public Item MakeItemBigFlashlight()
    {
      return (Item) new ItemLight((ItemModel) this.m_Game.GameItems.BIG_FLASHLIGHT);
    }

    public Item MakeItemZTracker()
    {
      return (Item) new ItemTracker((ItemModel) this.m_Game.GameItems.ZTRACKER);
    }

    public Item MakeItemBlackOpsGPS()
    {
      return (Item) new ItemTracker((ItemModel) this.m_Game.GameItems.BLACKOPS_GPS);
    }

    public Item MakeItemPoliceRadio()
    {
      return (Item) new ItemTracker((ItemModel) this.m_Game.GameItems.POLICE_RADIO);
    }

    public Item MakeItemGrenade()
    {
      ItemGrenade itemGrenade = new ItemGrenade((ItemModel) this.m_Game.GameItems.GRENADE, (ItemModel) this.m_Game.GameItems.GRENADE_PRIMED);
      itemGrenade.Quantity = this.m_Rules.Roll(1, this.m_Game.GameItems.GRENADE.StackingLimit);
      return (Item) itemGrenade;
    }

    public Item MakeItemBearTrap()
    {
      return (Item) new ItemTrap(this.m_Game.GameItems.BEAR_TRAP);
    }

    public Item MakeItemSpikes()
    {
      ItemTrap itemTrap = new ItemTrap(this.m_Game.GameItems.SPIKES);
      itemTrap.Quantity = this.m_Rules.Roll(1, this.m_Game.GameItems.BARBED_WIRE.StackingLimit);
      return (Item) itemTrap;
    }

    public Item MakeItemBarbedWire()
    {
      ItemTrap itemTrap = new ItemTrap(this.m_Game.GameItems.BARBED_WIRE);
      itemTrap.Quantity = this.m_Rules.Roll(1, this.m_Game.GameItems.BARBED_WIRE.StackingLimit);
      return (Item) itemTrap;
    }

    public Item MakeItemBook()
    {
      return (Item) new ItemEntertainment(this.m_Game.GameItems.BOOK);
    }

    public Item MakeItemMagazines()
    {
      ItemEntertainment itemEntertainment = new ItemEntertainment(this.m_Game.GameItems.MAGAZINE);
      itemEntertainment.Quantity = this.m_Rules.Roll(1, this.m_Game.GameItems.MAGAZINE.StackingLimit);
      return (Item) itemEntertainment;
    }

    protected void BarricadeDoors(Map map, Rectangle rect, int barricadeLevel)
    {
      barricadeLevel = Math.Min(80, barricadeLevel);
      for (int left = rect.Left; left < rect.Right; ++left)
      {
        for (int top = rect.Top; top < rect.Bottom; ++top)
        {
          DoorWindow mapObjectAt = map.GetMapObjectAt(left, top) as DoorWindow;
          if (mapObjectAt != null)
            mapObjectAt.BarricadePoints = barricadeLevel;
        }
      }
    }

    protected Zone MakeUniqueZone(string basename, Rectangle rect)
    {
      return new Zone(string.Format("{0}@{1}-{2}", (object) basename, (object) (rect.Left + rect.Width / 2), (object) (rect.Top + rect.Height / 2)), rect);
    }
  }
}
