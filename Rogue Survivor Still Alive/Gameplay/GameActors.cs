// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Gameplay.GameActors
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Gameplay.AI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace djack.RogueSurvivor.Gameplay
{
  internal class GameActors : ActorModelDB
  {
    private static readonly Verb VERB_PUNCH = new Verb("punch", "punches");
    private ActorModel[] m_Models = new ActorModel[27];
    public int UNDEAD_FOOD = 2880;
    public int HUMAN_HUN = 1440;
    public int HUMAN_SLP = 1800;
    public int HUMAN_SAN = 2880;
    public int HUMAN_INVENTORY = 7;
    public int DOG_HUN = 1440;
    public int DOG_SLP = 1800;
    public int DOG_INVENTORY = 1;
    private const int NO_INVENTORY = 0;
    private const int NO_FOOD = 0;
    private const int NO_SLEEP = 0;
    private const int NO_SANITY = 0;
    private const int NO_SMELL = 0;
    private const int NO_AUDIO = 0;
    private GameActors.ActorData DATA_SKELETON;
    private GameActors.ActorData DATA_RED_EYED_SKELETON;
    private GameActors.ActorData DATA_RED_SKELETON;
    private GameActors.ActorData DATA_ZOMBIE;
    private GameActors.ActorData DATA_DARK_EYED_ZOMBIE;
    private GameActors.ActorData DATA_DARK_ZOMBIE;
    private GameActors.ActorData DATA_MALE_ZOMBIFIED;
    private GameActors.ActorData DATA_FEMALE_ZOMBIFIED;
    private GameActors.ActorData DATA_MALE_NEOPHYTE;
    private GameActors.ActorData DATA_FEMALE_NEOPHYTE;
    private GameActors.ActorData DATA_MALE_DISCIPLE;
    private GameActors.ActorData DATA_FEMALE_DISCIPLE;
    private GameActors.ActorData DATA_ZM;
    private GameActors.ActorData DATA_ZP;
    private GameActors.ActorData DATA_ZL;
    private GameActors.ActorData DATA_RAT_ZOMBIE;
    private GameActors.ActorData DATA_SEWERS_THING;
    private GameActors.ActorData DATA_MALE_CIVILIAN;
    private GameActors.ActorData DATA_FEMALE_CIVILIAN;
    private GameActors.ActorData DATA_FERAL_DOG;
    private GameActors.ActorData DATA_POLICEMAN;
    private GameActors.ActorData DATA_CHAR_GUARD;
    private GameActors.ActorData DATA_NATGUARD;
    private GameActors.ActorData DATA_BIKER_MAN;
    private GameActors.ActorData DATA_GANGSTA_MAN;
    private GameActors.ActorData DATA_BLACKOPS_MAN;
    private GameActors.ActorData DATA_JASON_MYERS;

    public override ActorModel this[int id]
    {
      get
      {
        return this.m_Models[id];
      }
    }

    public ActorModel this[GameActors.IDs id]
    {
      get
      {
        return this[(int) id];
      }
      private set
      {
        this.m_Models[(int) id] = value;
        this.m_Models[(int) id].ID = (int) id;
      }
    }

    public ActorModel Skeleton
    {
      get
      {
        return this[GameActors.IDs._FIRST];
      }
    }

    public ActorModel Red_Eyed_Skeleton
    {
      get
      {
        return this[GameActors.IDs.UNDEAD_RED_EYED_SKELETON];
      }
    }

    public ActorModel Red_Skeleton
    {
      get
      {
        return this[GameActors.IDs.UNDEAD_RED_SKELETON];
      }
    }

    public ActorModel Zombie
    {
      get
      {
        return this[GameActors.IDs.UNDEAD_ZOMBIE];
      }
    }

    public ActorModel DarkEyedZombie
    {
      get
      {
        return this[GameActors.IDs.UNDEAD_DARK_EYED_ZOMBIE];
      }
    }

    public ActorModel DarkZombie
    {
      get
      {
        return this[GameActors.IDs.UNDEAD_DARK_ZOMBIE];
      }
    }

    public ActorModel ZombieMaster
    {
      get
      {
        return this[GameActors.IDs.UNDEAD_ZOMBIE_MASTER];
      }
    }

    public ActorModel ZombieLord
    {
      get
      {
        return this[GameActors.IDs.UNDEAD_ZOMBIE_LORD];
      }
    }

    public ActorModel ZombiePrince
    {
      get
      {
        return this[GameActors.IDs.UNDEAD_ZOMBIE_PRINCE];
      }
    }

    public ActorModel MaleZombified
    {
      get
      {
        return this[GameActors.IDs.UNDEAD_MALE_ZOMBIFIED];
      }
    }

    public ActorModel FemaleZombified
    {
      get
      {
        return this[GameActors.IDs.UNDEAD_FEMALE_ZOMBIFIED];
      }
    }

    public ActorModel MaleNeophyte
    {
      get
      {
        return this[GameActors.IDs.UNDEAD_MALE_NEOPHYTE];
      }
    }

    public ActorModel FemaleNeophyte
    {
      get
      {
        return this[GameActors.IDs.UNDEAD_FEMALE_NEOPHYTE];
      }
    }

    public ActorModel MaleDisciple
    {
      get
      {
        return this[GameActors.IDs.UNDEAD_MALE_DISCIPLE];
      }
    }

    public ActorModel FemaleDisciple
    {
      get
      {
        return this[GameActors.IDs.UNDEAD_FEMALE_DISCIPLE];
      }
    }

    public ActorModel RatZombie
    {
      get
      {
        return this[GameActors.IDs.UNDEAD_RAT_ZOMBIE];
      }
    }

    public ActorModel SewersThing
    {
      get
      {
        return this[GameActors.IDs.SEWERS_THING];
      }
    }

    public ActorModel MaleCivilian
    {
      get
      {
        return this[GameActors.IDs.MALE_CIVILIAN];
      }
    }

    public ActorModel FemaleCivilian
    {
      get
      {
        return this[GameActors.IDs.FEMALE_CIVILIAN];
      }
    }

    public ActorModel FeralDog
    {
      get
      {
        return this[GameActors.IDs.FERAL_DOG];
      }
    }

    public ActorModel CHARGuard
    {
      get
      {
        return this[GameActors.IDs.CHAR_GUARD];
      }
    }

    public ActorModel NationalGuard
    {
      get
      {
        return this[GameActors.IDs.ARMY_NATIONAL_GUARD];
      }
    }

    public ActorModel BikerMan
    {
      get
      {
        return this[GameActors.IDs.BIKER_MAN];
      }
    }

    public ActorModel GangstaMan
    {
      get
      {
        return this[GameActors.IDs.GANGSTA_MAN];
      }
    }

    public ActorModel Policeman
    {
      get
      {
        return this[GameActors.IDs.POLICEMAN];
      }
    }

    public ActorModel BlackOps
    {
      get
      {
        return this[GameActors.IDs.BLACKOPS_MAN];
      }
    }

    public ActorModel JasonMyers
    {
      get
      {
        return this[GameActors.IDs.JASON_MYERS];
      }
    }

    public GameActors()
    {
      Models.Actors = (ActorModelDB) this;
    }

    public void CreateModels()
    {
      int num1 = 0;
      string imageID1 = "Actors\\skeleton";
      string name1 = this.DATA_SKELETON.NAME;
      string plural1 = this.DATA_SKELETON.PLURAL;
      int score1 = this.DATA_SKELETON.SCORE;
      DollBody body1 = new DollBody(true, this.DATA_SKELETON.SPD);
      Abilities abilities1 = new Abilities();
      abilities1.IsUndead = true;
      ActorSheet startingSheet1 = new ActorSheet(this.DATA_SKELETON.HP, this.DATA_SKELETON.STA, 0, 0, 0, new Attack(AttackKind.PHYSICAL, new Verb("claw"), this.DATA_SKELETON.ATK, this.DATA_SKELETON.DMG), new Defence(this.DATA_SKELETON.DEF, this.DATA_SKELETON.PRO_HIT, this.DATA_SKELETON.PRO_SHOT), this.DATA_SKELETON.FOV, 0, 0, 0);
      Type defaultController1 = typeof (SkeletonAI);
      this[(GameActors.IDs) num1] = new ActorModel(imageID1, name1, plural1, score1, body1, abilities1, startingSheet1, defaultController1)
      {
        FlavorDescription = this.DATA_SKELETON.FLAVOR
      };
      int num2 = 1;
      string imageID2 = "Actors\\red_eyed_skeleton";
      string name2 = this.DATA_RED_EYED_SKELETON.NAME;
      string plural2 = this.DATA_RED_EYED_SKELETON.PLURAL;
      int score2 = this.DATA_RED_EYED_SKELETON.SCORE;
      DollBody body2 = new DollBody(true, this.DATA_RED_EYED_SKELETON.SPD);
      Abilities abilities2 = new Abilities();
      abilities2.IsUndead = true;
      ActorSheet startingSheet2 = new ActorSheet(this.DATA_RED_EYED_SKELETON.HP, this.DATA_RED_EYED_SKELETON.STA, 0, 0, 0, new Attack(AttackKind.PHYSICAL, new Verb("claw"), this.DATA_RED_EYED_SKELETON.ATK, this.DATA_RED_EYED_SKELETON.DMG), new Defence(this.DATA_RED_EYED_SKELETON.DEF, this.DATA_RED_EYED_SKELETON.PRO_HIT, this.DATA_RED_EYED_SKELETON.PRO_SHOT), this.DATA_RED_EYED_SKELETON.FOV, 0, 0, 0);
      Type defaultController2 = typeof (SkeletonAI);
      this[(GameActors.IDs) num2] = new ActorModel(imageID2, name2, plural2, score2, body2, abilities2, startingSheet2, defaultController2)
      {
        FlavorDescription = this.DATA_RED_EYED_SKELETON.FLAVOR
      };
      int num3 = 2;
      string imageID3 = "Actors\\red_skeleton";
      string name3 = this.DATA_RED_SKELETON.NAME;
      string plural3 = this.DATA_RED_SKELETON.PLURAL;
      int score3 = this.DATA_RED_SKELETON.SCORE;
      DollBody body3 = new DollBody(true, this.DATA_RED_SKELETON.SPD);
      Abilities abilities3 = new Abilities();
      abilities3.IsUndead = true;
      ActorSheet startingSheet3 = new ActorSheet(this.DATA_RED_SKELETON.HP, this.DATA_RED_SKELETON.STA, 0, 0, 0, new Attack(AttackKind.PHYSICAL, new Verb("claw"), this.DATA_RED_SKELETON.ATK, this.DATA_RED_SKELETON.DMG), new Defence(this.DATA_RED_SKELETON.DEF, this.DATA_RED_SKELETON.PRO_HIT, this.DATA_RED_SKELETON.PRO_SHOT), this.DATA_RED_SKELETON.FOV, 0, 0, 0);
      Type defaultController3 = typeof (SkeletonAI);
      this[(GameActors.IDs) num3] = new ActorModel(imageID3, name3, plural3, score3, body3, abilities3, startingSheet3, defaultController3)
      {
        FlavorDescription = this.DATA_RED_SKELETON.FLAVOR
      };
      int num4 = 3;
      string imageID4 = "Actors\\zombie";
      string name4 = this.DATA_ZOMBIE.NAME;
      string plural4 = this.DATA_ZOMBIE.PLURAL;
      int score4 = this.DATA_ZOMBIE.SCORE;
      DollBody body4 = new DollBody(true, this.DATA_ZOMBIE.SPD);
      Abilities abilities4 = new Abilities();
      abilities4.IsUndead = true;
      abilities4.IsRotting = true;
      abilities4.CanZombifyKilled = true;
      abilities4.CanBashDoors = true;
      abilities4.CanBreakObjects = true;
      abilities4.ZombieAI_Explore = true;
      abilities4.AI_CanUseAIExits = true;
      ActorSheet startingSheet4 = new ActorSheet(this.DATA_ZOMBIE.HP, this.DATA_ZOMBIE.STA, this.UNDEAD_FOOD, 0, 0, new Attack(AttackKind.PHYSICAL, new Verb("bite"), this.DATA_ZOMBIE.ATK, this.DATA_ZOMBIE.DMG), new Defence(this.DATA_ZOMBIE.DEF, this.DATA_ZOMBIE.PRO_HIT, this.DATA_ZOMBIE.PRO_SHOT), this.DATA_ZOMBIE.FOV, 0, this.DATA_ZOMBIE.SMELL, 0);
      Type defaultController4 = typeof (ZombieAI);
      this[(GameActors.IDs) num4] = new ActorModel(imageID4, name4, plural4, score4, body4, abilities4, startingSheet4, defaultController4)
      {
        FlavorDescription = this.DATA_ZOMBIE.FLAVOR
      };
      int num5 = 4;
      string imageID5 = "Actors\\dark_eyed_zombie";
      string name5 = this.DATA_DARK_EYED_ZOMBIE.NAME;
      string plural5 = this.DATA_DARK_EYED_ZOMBIE.PLURAL;
      int score5 = this.DATA_DARK_EYED_ZOMBIE.SCORE;
      DollBody body5 = new DollBody(true, this.DATA_DARK_EYED_ZOMBIE.SPD);
      Abilities abilities5 = new Abilities();
      abilities5.IsUndead = true;
      abilities5.IsRotting = true;
      abilities5.CanZombifyKilled = true;
      abilities5.CanBashDoors = true;
      abilities5.CanBreakObjects = true;
      abilities5.ZombieAI_Explore = true;
      abilities5.AI_CanUseAIExits = true;
      ActorSheet startingSheet5 = new ActorSheet(this.DATA_DARK_EYED_ZOMBIE.HP, this.DATA_DARK_EYED_ZOMBIE.STA, this.UNDEAD_FOOD, 0, 0, new Attack(AttackKind.PHYSICAL, new Verb("bite"), this.DATA_DARK_EYED_ZOMBIE.ATK, this.DATA_DARK_EYED_ZOMBIE.DMG), new Defence(this.DATA_DARK_EYED_ZOMBIE.DEF, this.DATA_DARK_EYED_ZOMBIE.PRO_HIT, this.DATA_DARK_EYED_ZOMBIE.PRO_SHOT), this.DATA_DARK_EYED_ZOMBIE.FOV, 0, this.DATA_DARK_EYED_ZOMBIE.SMELL, 0);
      Type defaultController5 = typeof (ZombieAI);
      this[(GameActors.IDs) num5] = new ActorModel(imageID5, name5, plural5, score5, body5, abilities5, startingSheet5, defaultController5)
      {
        FlavorDescription = this.DATA_DARK_EYED_ZOMBIE.FLAVOR
      };
      int num6 = 5;
      string imageID6 = "Actors\\dark_zombie";
      string name6 = this.DATA_DARK_ZOMBIE.NAME;
      string plural6 = this.DATA_DARK_ZOMBIE.PLURAL;
      int score6 = this.DATA_DARK_ZOMBIE.SCORE;
      DollBody body6 = new DollBody(true, this.DATA_DARK_ZOMBIE.SPD);
      Abilities abilities6 = new Abilities();
      abilities6.IsUndead = true;
      abilities6.IsRotting = true;
      abilities6.CanZombifyKilled = true;
      abilities6.CanBashDoors = true;
      abilities6.CanBreakObjects = true;
      abilities6.ZombieAI_Explore = true;
      abilities6.AI_CanUseAIExits = true;
      ActorSheet startingSheet6 = new ActorSheet(this.DATA_DARK_ZOMBIE.HP, this.DATA_DARK_ZOMBIE.STA, this.UNDEAD_FOOD, 0, 0, new Attack(AttackKind.PHYSICAL, new Verb("bite"), this.DATA_DARK_ZOMBIE.ATK, this.DATA_DARK_ZOMBIE.DMG), new Defence(this.DATA_DARK_ZOMBIE.DEF, this.DATA_DARK_ZOMBIE.PRO_HIT, this.DATA_DARK_ZOMBIE.PRO_SHOT), this.DATA_DARK_ZOMBIE.FOV, 0, this.DATA_DARK_ZOMBIE.SMELL, 0);
      Type defaultController6 = typeof (ZombieAI);
      this[(GameActors.IDs) num6] = new ActorModel(imageID6, name6, plural6, score6, body6, abilities6, startingSheet6, defaultController6)
      {
        FlavorDescription = this.DATA_DARK_ZOMBIE.FLAVOR
      };
      int num7 = 9;
      // ISSUE: variable of the null type
      __Null local1 = null;
      string name7 = this.DATA_MALE_ZOMBIFIED.NAME;
      string plural7 = this.DATA_MALE_ZOMBIFIED.PLURAL;
      int score7 = this.DATA_MALE_ZOMBIFIED.SCORE;
      DollBody body7 = new DollBody(true, this.DATA_MALE_ZOMBIFIED.SPD);
      Abilities abilities7 = new Abilities();
      abilities7.IsUndead = true;
      abilities7.IsRotting = true;
      abilities7.CanZombifyKilled = true;
      abilities7.CanBashDoors = true;
      abilities7.CanBreakObjects = true;
      abilities7.ZombieAI_Explore = true;
      abilities7.AI_CanUseAIExits = true;
      ActorSheet startingSheet7 = new ActorSheet(this.DATA_MALE_ZOMBIFIED.HP, this.DATA_MALE_ZOMBIFIED.STA, this.UNDEAD_FOOD, 0, 0, new Attack(AttackKind.PHYSICAL, new Verb("bite"), this.DATA_MALE_ZOMBIFIED.ATK, this.DATA_MALE_ZOMBIFIED.DMG), new Defence(this.DATA_MALE_ZOMBIFIED.DEF, this.DATA_MALE_ZOMBIFIED.PRO_HIT, this.DATA_MALE_ZOMBIFIED.PRO_SHOT), this.DATA_MALE_ZOMBIFIED.FOV, 0, this.DATA_MALE_ZOMBIFIED.SMELL, 0);
      Type defaultController7 = typeof (ZombieAI);
      this[(GameActors.IDs) num7] = new ActorModel((string) local1, name7, plural7, score7, body7, abilities7, startingSheet7, defaultController7)
      {
        FlavorDescription = this.DATA_MALE_ZOMBIFIED.FLAVOR
      };
      int num8 = 10;
      // ISSUE: variable of the null type
      __Null local2 = null;
      string name8 = this.DATA_FEMALE_ZOMBIFIED.NAME;
      string plural8 = this.DATA_FEMALE_ZOMBIFIED.PLURAL;
      int score8 = this.DATA_FEMALE_ZOMBIFIED.SCORE;
      DollBody body8 = new DollBody(true, this.DATA_FEMALE_ZOMBIFIED.SPD);
      Abilities abilities8 = new Abilities();
      abilities8.IsUndead = true;
      abilities8.IsRotting = true;
      abilities8.CanZombifyKilled = true;
      abilities8.CanBashDoors = true;
      abilities8.CanBreakObjects = true;
      abilities8.ZombieAI_Explore = true;
      abilities8.AI_CanUseAIExits = true;
      ActorSheet startingSheet8 = new ActorSheet(this.DATA_FEMALE_ZOMBIFIED.HP, this.DATA_FEMALE_ZOMBIFIED.STA, this.UNDEAD_FOOD, 0, 0, new Attack(AttackKind.PHYSICAL, new Verb("bite"), this.DATA_FEMALE_ZOMBIFIED.ATK, this.DATA_FEMALE_ZOMBIFIED.DMG), new Defence(this.DATA_FEMALE_ZOMBIFIED.DEF, this.DATA_FEMALE_ZOMBIFIED.PRO_HIT, this.DATA_FEMALE_ZOMBIFIED.PRO_SHOT), this.DATA_FEMALE_ZOMBIFIED.FOV, 0, this.DATA_FEMALE_ZOMBIFIED.SMELL, 0);
      Type defaultController8 = typeof (ZombieAI);
      this[(GameActors.IDs) num8] = new ActorModel((string) local2, name8, plural8, score8, body8, abilities8, startingSheet8, defaultController8)
      {
        FlavorDescription = this.DATA_FEMALE_ZOMBIFIED.FLAVOR
      };
      int num9 = 11;
      string imageID7 = "Actors\\male_neophyte";
      string name9 = this.DATA_MALE_NEOPHYTE.NAME;
      string plural9 = this.DATA_MALE_NEOPHYTE.PLURAL;
      int score9 = this.DATA_MALE_NEOPHYTE.SCORE;
      DollBody body9 = new DollBody(true, this.DATA_MALE_NEOPHYTE.SPD);
      Abilities abilities9 = new Abilities();
      abilities9.IsUndead = true;
      abilities9.IsRotting = true;
      abilities9.CanZombifyKilled = true;
      abilities9.CanBashDoors = true;
      abilities9.CanBreakObjects = true;
      abilities9.CanPush = true;
      abilities9.ZombieAI_AssaultBreakables = true;
      abilities9.ZombieAI_Explore = true;
      abilities9.AI_CanUseAIExits = true;
      ActorSheet startingSheet9 = new ActorSheet(this.DATA_MALE_NEOPHYTE.HP, this.DATA_MALE_NEOPHYTE.STA, this.UNDEAD_FOOD, 0, 0, new Attack(AttackKind.PHYSICAL, new Verb("bite"), this.DATA_MALE_NEOPHYTE.ATK, this.DATA_MALE_NEOPHYTE.DMG), new Defence(this.DATA_MALE_NEOPHYTE.DEF, this.DATA_MALE_NEOPHYTE.PRO_HIT, this.DATA_MALE_NEOPHYTE.PRO_SHOT), this.DATA_MALE_NEOPHYTE.FOV, 0, this.DATA_MALE_NEOPHYTE.SMELL, 0);
      Type defaultController9 = typeof (ZombieAI);
      this[(GameActors.IDs) num9] = new ActorModel(imageID7, name9, plural9, score9, body9, abilities9, startingSheet9, defaultController9)
      {
        FlavorDescription = this.DATA_MALE_NEOPHYTE.FLAVOR
      };
      int num10 = 12;
      string imageID8 = "Actors\\female_neophyte";
      string name10 = this.DATA_FEMALE_NEOPHYTE.NAME;
      string plural10 = this.DATA_FEMALE_NEOPHYTE.PLURAL;
      int score10 = this.DATA_FEMALE_NEOPHYTE.SCORE;
      DollBody body10 = new DollBody(true, this.DATA_FEMALE_NEOPHYTE.SPD);
      Abilities abilities10 = new Abilities();
      abilities10.IsUndead = true;
      abilities10.IsRotting = true;
      abilities10.CanZombifyKilled = true;
      abilities10.CanBashDoors = true;
      abilities10.CanBreakObjects = true;
      abilities10.CanPush = true;
      abilities10.ZombieAI_AssaultBreakables = true;
      abilities10.ZombieAI_Explore = true;
      abilities10.AI_CanUseAIExits = true;
      ActorSheet startingSheet10 = new ActorSheet(this.DATA_FEMALE_NEOPHYTE.HP, this.DATA_FEMALE_NEOPHYTE.STA, this.UNDEAD_FOOD, 0, 0, new Attack(AttackKind.PHYSICAL, new Verb("bite"), this.DATA_FEMALE_NEOPHYTE.ATK, this.DATA_FEMALE_NEOPHYTE.DMG), new Defence(this.DATA_FEMALE_NEOPHYTE.DEF, this.DATA_FEMALE_NEOPHYTE.PRO_HIT, this.DATA_FEMALE_NEOPHYTE.PRO_SHOT), this.DATA_FEMALE_NEOPHYTE.FOV, 0, this.DATA_FEMALE_NEOPHYTE.SMELL, 0);
      Type defaultController10 = typeof (ZombieAI);
      this[(GameActors.IDs) num10] = new ActorModel(imageID8, name10, plural10, score10, body10, abilities10, startingSheet10, defaultController10)
      {
        FlavorDescription = this.DATA_FEMALE_NEOPHYTE.FLAVOR
      };
      int num11 = 13;
      string imageID9 = "Actors\\male_disciple";
      string name11 = this.DATA_MALE_DISCIPLE.NAME;
      string plural11 = this.DATA_MALE_DISCIPLE.PLURAL;
      int score11 = this.DATA_MALE_DISCIPLE.SCORE;
      DollBody body11 = new DollBody(true, this.DATA_MALE_DISCIPLE.SPD);
      Abilities abilities11 = new Abilities();
      abilities11.IsUndead = true;
      abilities11.IsRotting = true;
      abilities11.CanZombifyKilled = true;
      abilities11.CanBashDoors = true;
      abilities11.CanBreakObjects = true;
      abilities11.CanPush = true;
      abilities11.ZombieAI_AssaultBreakables = true;
      abilities11.ZombieAI_Explore = true;
      abilities11.AI_CanUseAIExits = true;
      ActorSheet startingSheet11 = new ActorSheet(this.DATA_MALE_DISCIPLE.HP, this.DATA_MALE_DISCIPLE.STA, this.UNDEAD_FOOD, 0, 0, new Attack(AttackKind.PHYSICAL, new Verb("bite"), this.DATA_MALE_DISCIPLE.ATK, this.DATA_MALE_DISCIPLE.DMG), new Defence(this.DATA_MALE_DISCIPLE.DEF, this.DATA_MALE_DISCIPLE.PRO_HIT, this.DATA_MALE_DISCIPLE.PRO_SHOT), this.DATA_MALE_DISCIPLE.FOV, 0, this.DATA_MALE_DISCIPLE.SMELL, 0);
      Type defaultController11 = typeof (ZombieAI);
      this[(GameActors.IDs) num11] = new ActorModel(imageID9, name11, plural11, score11, body11, abilities11, startingSheet11, defaultController11)
      {
        FlavorDescription = this.DATA_MALE_DISCIPLE.FLAVOR
      };
      int num12 = 14;
      string imageID10 = "Actors\\female_disciple";
      string name12 = this.DATA_FEMALE_DISCIPLE.NAME;
      string plural12 = this.DATA_FEMALE_DISCIPLE.PLURAL;
      int score12 = this.DATA_FEMALE_DISCIPLE.SCORE;
      DollBody body12 = new DollBody(true, this.DATA_FEMALE_DISCIPLE.SPD);
      Abilities abilities12 = new Abilities();
      abilities12.IsUndead = true;
      abilities12.IsRotting = true;
      abilities12.CanZombifyKilled = true;
      abilities12.CanBashDoors = true;
      abilities12.CanBreakObjects = true;
      abilities12.CanPush = true;
      abilities12.ZombieAI_AssaultBreakables = true;
      abilities12.ZombieAI_Explore = true;
      abilities12.AI_CanUseAIExits = true;
      ActorSheet startingSheet12 = new ActorSheet(this.DATA_FEMALE_DISCIPLE.HP, this.DATA_FEMALE_DISCIPLE.STA, this.UNDEAD_FOOD, 0, 0, new Attack(AttackKind.PHYSICAL, new Verb("bite"), this.DATA_FEMALE_DISCIPLE.ATK, this.DATA_FEMALE_DISCIPLE.DMG), new Defence(this.DATA_FEMALE_DISCIPLE.DEF, this.DATA_FEMALE_DISCIPLE.PRO_HIT, this.DATA_FEMALE_DISCIPLE.PRO_SHOT), this.DATA_FEMALE_DISCIPLE.FOV, 0, this.DATA_FEMALE_DISCIPLE.SMELL, 0);
      Type defaultController12 = typeof (ZombieAI);
      this[(GameActors.IDs) num12] = new ActorModel(imageID10, name12, plural12, score12, body12, abilities12, startingSheet12, defaultController12)
      {
        FlavorDescription = this.DATA_FEMALE_DISCIPLE.FLAVOR
      };
      int num13 = 6;
      string imageID11 = "Actors\\zombie_master";
      string name13 = this.DATA_ZM.NAME;
      string plural13 = this.DATA_ZM.PLURAL;
      int score13 = this.DATA_ZM.SCORE;
      DollBody body13 = new DollBody(true, this.DATA_ZM.SPD);
      Abilities abilities13 = new Abilities();
      abilities13.IsUndead = true;
      abilities13.IsUndeadMaster = true;
      abilities13.IsRotting = true;
      abilities13.CanZombifyKilled = true;
      abilities13.CanBashDoors = true;
      abilities13.CanBreakObjects = true;
      abilities13.CanUseMapObjects = true;
      abilities13.CanJump = true;
      abilities13.CanJumpStumble = true;
      abilities13.CanPush = true;
      abilities13.ZombieAI_Explore = true;
      abilities13.AI_CanUseAIExits = true;
      ActorSheet startingSheet13 = new ActorSheet(this.DATA_ZM.HP, this.DATA_ZM.STA, this.UNDEAD_FOOD, 0, 0, new Attack(AttackKind.PHYSICAL, new Verb("bite"), this.DATA_ZM.ATK, this.DATA_ZM.DMG), new Defence(this.DATA_ZM.DEF, this.DATA_ZM.PRO_HIT, this.DATA_ZM.PRO_SHOT), this.DATA_ZM.FOV, 0, this.DATA_ZM.SMELL, 0);
      Type defaultController13 = typeof (ZombieAI);
      this[(GameActors.IDs) num13] = new ActorModel(imageID11, name13, plural13, score13, body13, abilities13, startingSheet13, defaultController13)
      {
        FlavorDescription = this.DATA_ZM.FLAVOR
      };
      int num14 = 7;
      string imageID12 = "Actors\\zombie_lord";
      string name14 = this.DATA_ZL.NAME;
      string plural14 = this.DATA_ZL.PLURAL;
      int score14 = this.DATA_ZL.SCORE;
      DollBody body14 = new DollBody(true, this.DATA_ZL.SPD);
      Abilities abilities14 = new Abilities();
      abilities14.IsUndead = true;
      abilities14.IsUndeadMaster = true;
      abilities14.IsRotting = true;
      abilities14.CanZombifyKilled = true;
      abilities14.CanBashDoors = true;
      abilities14.CanBreakObjects = true;
      abilities14.CanUseMapObjects = true;
      abilities14.CanJump = true;
      abilities14.CanJumpStumble = true;
      abilities14.CanPush = true;
      abilities14.ZombieAI_AssaultBreakables = true;
      abilities14.ZombieAI_Explore = true;
      abilities14.AI_CanUseAIExits = true;
      ActorSheet startingSheet14 = new ActorSheet(this.DATA_ZL.HP, this.DATA_ZL.STA, this.UNDEAD_FOOD, 0, 0, new Attack(AttackKind.PHYSICAL, new Verb("bite"), this.DATA_ZL.ATK, this.DATA_ZL.DMG), new Defence(this.DATA_ZL.DEF, this.DATA_ZL.PRO_HIT, this.DATA_ZL.PRO_SHOT), this.DATA_ZL.FOV, 0, this.DATA_ZL.SMELL, 0);
      Type defaultController14 = typeof (ZombieAI);
      this[(GameActors.IDs) num14] = new ActorModel(imageID12, name14, plural14, score14, body14, abilities14, startingSheet14, defaultController14)
      {
        FlavorDescription = this.DATA_ZL.FLAVOR
      };
      int num15 = 8;
      string imageID13 = "Actors\\zombie_prince";
      string name15 = this.DATA_ZP.NAME;
      string plural15 = this.DATA_ZP.PLURAL;
      int score15 = this.DATA_ZP.SCORE;
      DollBody body15 = new DollBody(true, this.DATA_ZP.SPD);
      Abilities abilities15 = new Abilities();
      abilities15.IsUndead = true;
      abilities15.IsUndeadMaster = true;
      abilities15.IsRotting = true;
      abilities15.CanZombifyKilled = true;
      abilities15.CanBashDoors = true;
      abilities15.CanBreakObjects = true;
      abilities15.CanUseMapObjects = true;
      abilities15.CanJump = true;
      abilities15.CanJumpStumble = true;
      abilities15.CanPush = true;
      abilities15.ZombieAI_AssaultBreakables = true;
      abilities15.ZombieAI_Explore = true;
      abilities15.AI_CanUseAIExits = true;
      ActorSheet startingSheet15 = new ActorSheet(this.DATA_ZP.HP, this.DATA_ZP.STA, this.UNDEAD_FOOD, 0, 0, new Attack(AttackKind.PHYSICAL, new Verb("bite"), this.DATA_ZP.ATK, this.DATA_ZP.DMG), new Defence(this.DATA_ZP.DEF, this.DATA_ZP.PRO_HIT, this.DATA_ZP.PRO_SHOT), this.DATA_ZP.FOV, 0, this.DATA_ZP.SMELL, 0);
      Type defaultController15 = typeof (ZombieAI);
      this[(GameActors.IDs) num15] = new ActorModel(imageID13, name15, plural15, score15, body15, abilities15, startingSheet15, defaultController15)
      {
        FlavorDescription = this.DATA_ZP.FLAVOR
      };
      int num16 = 15;
      string imageID14 = "Actors\\rat_zombie";
      string name16 = this.DATA_RAT_ZOMBIE.NAME;
      string plural16 = this.DATA_RAT_ZOMBIE.PLURAL;
      int score16 = this.DATA_RAT_ZOMBIE.SCORE;
      DollBody body16 = new DollBody(true, this.DATA_RAT_ZOMBIE.SPD);
      Abilities abilities16 = new Abilities();
      abilities16.IsUndead = true;
      abilities16.IsSmall = true;
      abilities16.AI_CanUseAIExits = true;
      ActorSheet startingSheet16 = new ActorSheet(this.DATA_RAT_ZOMBIE.HP, this.DATA_RAT_ZOMBIE.STA, 0, 0, 0, new Attack(AttackKind.PHYSICAL, new Verb("bite"), this.DATA_RAT_ZOMBIE.ATK, this.DATA_RAT_ZOMBIE.DMG), new Defence(this.DATA_RAT_ZOMBIE.DEF, this.DATA_RAT_ZOMBIE.PRO_HIT, this.DATA_RAT_ZOMBIE.PRO_SHOT), this.DATA_RAT_ZOMBIE.FOV, 0, this.DATA_RAT_ZOMBIE.SMELL, 0);
      Type defaultController16 = typeof (RatAI);
      this[(GameActors.IDs) num16] = new ActorModel(imageID14, name16, plural16, score16, body16, abilities16, startingSheet16, defaultController16)
      {
        FlavorDescription = this.DATA_RAT_ZOMBIE.FLAVOR
      };
      int num17 = 25;
      string imageID15 = "Actors\\sewers_thing";
      string name17 = this.DATA_SEWERS_THING.NAME;
      string plural17 = this.DATA_SEWERS_THING.PLURAL;
      int score17 = this.DATA_SEWERS_THING.SCORE;
      DollBody body17 = new DollBody(true, this.DATA_SEWERS_THING.SPD);
      Abilities abilities17 = new Abilities();
      abilities17.IsUndead = true;
      abilities17.CanBashDoors = true;
      abilities17.CanBreakObjects = true;
      ActorSheet startingSheet17 = new ActorSheet(this.DATA_SEWERS_THING.HP, this.DATA_SEWERS_THING.STA, 0, 0, 0, new Attack(AttackKind.PHYSICAL, new Verb("bite"), this.DATA_SEWERS_THING.ATK, this.DATA_SEWERS_THING.DMG), new Defence(this.DATA_SEWERS_THING.DEF, this.DATA_SEWERS_THING.PRO_HIT, this.DATA_SEWERS_THING.PRO_SHOT), this.DATA_SEWERS_THING.FOV, 0, this.DATA_SEWERS_THING.SMELL, 0);
      Type defaultController17 = typeof (SewersThingAI);
      this[(GameActors.IDs) num17] = new ActorModel(imageID15, name17, plural17, score17, body17, abilities17, startingSheet17, defaultController17)
      {
        FlavorDescription = this.DATA_SEWERS_THING.FLAVOR
      };
      int num18 = 16;
      // ISSUE: variable of the null type
      __Null local3 = null;
      string name18 = this.DATA_MALE_CIVILIAN.NAME;
      string plural18 = this.DATA_MALE_CIVILIAN.PLURAL;
      int score18 = this.DATA_MALE_CIVILIAN.SCORE;
      DollBody body18 = new DollBody(true, this.DATA_MALE_CIVILIAN.SPD);
      Abilities abilities18 = new Abilities();
      abilities18.HasInventory = true;
      abilities18.HasToEat = true;
      abilities18.HasToSleep = true;
      abilities18.HasSanity = true;
      abilities18.CanTalk = true;
      abilities18.CanUseMapObjects = true;
      abilities18.CanBreakObjects = true;
      abilities18.CanJump = true;
      abilities18.CanTire = true;
      abilities18.CanRun = true;
      abilities18.CanUseItems = true;
      abilities18.CanTrade = true;
      abilities18.CanBarricade = true;
      abilities18.CanPush = true;
      abilities18.IsIntelligent = true;
      abilities18.AI_CanUseAIExits = true;
      ActorSheet startingSheet18 = new ActorSheet(this.DATA_MALE_CIVILIAN.HP, this.DATA_MALE_CIVILIAN.STA, this.HUMAN_HUN, this.HUMAN_SLP, this.HUMAN_SAN, new Attack(AttackKind.PHYSICAL, GameActors.VERB_PUNCH, this.DATA_MALE_CIVILIAN.ATK, this.DATA_MALE_CIVILIAN.DMG), new Defence(this.DATA_MALE_CIVILIAN.DEF, this.DATA_MALE_CIVILIAN.PRO_HIT, this.DATA_MALE_CIVILIAN.PRO_SHOT), this.DATA_MALE_CIVILIAN.FOV, this.DATA_MALE_CIVILIAN.AUDIO, 0, this.HUMAN_INVENTORY);
      // ISSUE: variable of the null type
      __Null local4 = null;
      this[(GameActors.IDs) num18] = new ActorModel((string) local3, name18, plural18, score18, body18, abilities18, startingSheet18, (Type) local4)
      {
        FlavorDescription = this.DATA_MALE_CIVILIAN.FLAVOR
      };
      int num19 = 17;
      // ISSUE: variable of the null type
      __Null local5 = null;
      string name19 = this.DATA_FEMALE_CIVILIAN.NAME;
      string plural19 = this.DATA_FEMALE_CIVILIAN.PLURAL;
      int score19 = this.DATA_FEMALE_CIVILIAN.SCORE;
      DollBody body19 = new DollBody(false, this.DATA_FEMALE_CIVILIAN.SPD);
      Abilities abilities19 = new Abilities();
      abilities19.HasInventory = true;
      abilities19.HasToEat = true;
      abilities19.HasToSleep = true;
      abilities19.HasSanity = true;
      abilities19.CanTalk = true;
      abilities19.CanUseMapObjects = true;
      abilities19.CanBreakObjects = true;
      abilities19.CanJump = true;
      abilities19.CanTire = true;
      abilities19.CanRun = true;
      abilities19.CanUseItems = true;
      abilities19.CanTrade = true;
      abilities19.CanBarricade = true;
      abilities19.CanPush = true;
      abilities19.IsIntelligent = true;
      abilities19.AI_CanUseAIExits = true;
      ActorSheet startingSheet19 = new ActorSheet(this.DATA_FEMALE_CIVILIAN.HP, this.DATA_FEMALE_CIVILIAN.STA, this.HUMAN_HUN, this.HUMAN_SLP, this.HUMAN_SAN, new Attack(AttackKind.PHYSICAL, GameActors.VERB_PUNCH, this.DATA_FEMALE_CIVILIAN.ATK, this.DATA_FEMALE_CIVILIAN.DMG), new Defence(this.DATA_FEMALE_CIVILIAN.DEF, this.DATA_FEMALE_CIVILIAN.PRO_HIT, this.DATA_FEMALE_CIVILIAN.PRO_SHOT), this.DATA_FEMALE_CIVILIAN.FOV, this.DATA_FEMALE_CIVILIAN.AUDIO, 0, this.HUMAN_INVENTORY);
      // ISSUE: variable of the null type
      __Null local6 = null;
      this[(GameActors.IDs) num19] = new ActorModel((string) local5, name19, plural19, score19, body19, abilities19, startingSheet19, (Type) local6)
      {
        FlavorDescription = this.DATA_FEMALE_CIVILIAN.FLAVOR
      };
      int num20 = 19;
      // ISSUE: variable of the null type
      __Null local7 = null;
      string name20 = this.DATA_CHAR_GUARD.NAME;
      string plural20 = this.DATA_CHAR_GUARD.PLURAL;
      int score20 = this.DATA_CHAR_GUARD.SCORE;
      DollBody body20 = new DollBody(true, this.DATA_CHAR_GUARD.SPD);
      Abilities abilities20 = new Abilities();
      abilities20.HasInventory = true;
      abilities20.CanUseMapObjects = true;
      abilities20.CanBreakObjects = true;
      abilities20.CanJump = true;
      abilities20.CanTire = true;
      abilities20.CanRun = true;
      abilities20.CanUseItems = true;
      abilities20.HasToSleep = true;
      abilities20.HasSanity = true;
      abilities20.CanTalk = true;
      abilities20.CanPush = true;
      abilities20.CanBarricade = true;
      abilities20.IsIntelligent = true;
      ActorSheet startingSheet20 = new ActorSheet(this.DATA_CHAR_GUARD.HP, this.DATA_CHAR_GUARD.STA, this.HUMAN_HUN, this.HUMAN_SLP, this.HUMAN_SAN, new Attack(AttackKind.PHYSICAL, GameActors.VERB_PUNCH, this.DATA_CHAR_GUARD.ATK, this.DATA_CHAR_GUARD.DMG), new Defence(this.DATA_CHAR_GUARD.DEF, this.DATA_CHAR_GUARD.PRO_HIT, this.DATA_CHAR_GUARD.PRO_SHOT), this.DATA_CHAR_GUARD.FOV, this.DATA_CHAR_GUARD.AUDIO, 0, this.HUMAN_INVENTORY);
      Type defaultController18 = typeof (CHARGuardAI);
      this[(GameActors.IDs) num20] = new ActorModel((string) local7, name20, plural20, score20, body20, abilities20, startingSheet20, defaultController18)
      {
        FlavorDescription = this.DATA_CHAR_GUARD.FLAVOR
      };
      int num21 = 20;
      // ISSUE: variable of the null type
      __Null local8 = null;
      string name21 = this.DATA_NATGUARD.NAME;
      string plural21 = this.DATA_NATGUARD.PLURAL;
      int score21 = this.DATA_NATGUARD.SCORE;
      DollBody body21 = new DollBody(true, this.DATA_NATGUARD.SPD);
      Abilities abilities21 = new Abilities();
      abilities21.HasInventory = true;
      abilities21.CanUseMapObjects = true;
      abilities21.CanBreakObjects = true;
      abilities21.CanJump = true;
      abilities21.CanTire = true;
      abilities21.CanRun = true;
      abilities21.CanUseItems = true;
      abilities21.CanTalk = true;
      abilities21.HasToSleep = true;
      abilities21.HasSanity = true;
      abilities21.CanPush = true;
      abilities21.CanBarricade = true;
      abilities21.IsIntelligent = true;
      ActorSheet startingSheet21 = new ActorSheet(this.DATA_NATGUARD.HP, this.DATA_NATGUARD.STA, this.HUMAN_HUN, this.HUMAN_SLP, this.HUMAN_SAN, new Attack(AttackKind.PHYSICAL, GameActors.VERB_PUNCH, this.DATA_NATGUARD.ATK, this.DATA_NATGUARD.DMG), new Defence(this.DATA_NATGUARD.DEF, this.DATA_NATGUARD.PRO_HIT, this.DATA_NATGUARD.PRO_SHOT), this.DATA_NATGUARD.FOV, this.DATA_NATGUARD.AUDIO, 0, this.HUMAN_INVENTORY);
      Type defaultController19 = typeof (SoldierAI);
      this[(GameActors.IDs) num21] = new ActorModel((string) local8, name21, plural21, score21, body21, abilities21, startingSheet21, defaultController19)
      {
        FlavorDescription = this.DATA_NATGUARD.FLAVOR
      };
      int num22 = 21;
      // ISSUE: variable of the null type
      __Null local9 = null;
      string name22 = this.DATA_BIKER_MAN.NAME;
      string plural22 = this.DATA_BIKER_MAN.PLURAL;
      int score22 = this.DATA_BIKER_MAN.SCORE;
      DollBody body22 = new DollBody(true, this.DATA_BIKER_MAN.SPD);
      Abilities abilities22 = new Abilities();
      abilities22.HasInventory = true;
      abilities22.CanUseMapObjects = true;
      abilities22.CanBreakObjects = true;
      abilities22.CanJump = true;
      abilities22.CanTire = true;
      abilities22.CanRun = true;
      abilities22.CanUseItems = true;
      abilities22.HasToEat = true;
      abilities22.HasToSleep = true;
      abilities22.HasSanity = true;
      abilities22.CanTalk = true;
      abilities22.CanPush = true;
      abilities22.CanBarricade = true;
      abilities22.CanTrade = true;
      abilities22.IsIntelligent = true;
      abilities22.AI_NotInterestedInRangedWeapons = true;
      ActorSheet startingSheet22 = new ActorSheet(this.DATA_BIKER_MAN.HP, this.DATA_BIKER_MAN.STA, this.HUMAN_HUN, this.HUMAN_SLP, this.HUMAN_SAN, new Attack(AttackKind.PHYSICAL, GameActors.VERB_PUNCH, this.DATA_BIKER_MAN.ATK, this.DATA_BIKER_MAN.DMG), new Defence(this.DATA_BIKER_MAN.DEF, this.DATA_BIKER_MAN.PRO_HIT, this.DATA_BIKER_MAN.PRO_SHOT), this.DATA_BIKER_MAN.FOV, this.DATA_BIKER_MAN.AUDIO, 0, this.HUMAN_INVENTORY);
      Type defaultController20 = typeof (GangAI);
      this[(GameActors.IDs) num22] = new ActorModel((string) local9, name22, plural22, score22, body22, abilities22, startingSheet22, defaultController20)
      {
        FlavorDescription = this.DATA_BIKER_MAN.FLAVOR
      };
      int num23 = 23;
      // ISSUE: variable of the null type
      __Null local10 = null;
      string name23 = this.DATA_GANGSTA_MAN.NAME;
      string plural23 = this.DATA_GANGSTA_MAN.PLURAL;
      int score23 = this.DATA_GANGSTA_MAN.SCORE;
      DollBody body23 = new DollBody(true, this.DATA_GANGSTA_MAN.SPD);
      Abilities abilities23 = new Abilities();
      abilities23.HasInventory = true;
      abilities23.CanUseMapObjects = true;
      abilities23.CanBreakObjects = true;
      abilities23.CanJump = true;
      abilities23.CanTire = true;
      abilities23.CanRun = true;
      abilities23.CanUseItems = true;
      abilities23.HasToEat = true;
      abilities23.HasToSleep = true;
      abilities23.HasSanity = true;
      abilities23.CanTalk = true;
      abilities23.CanPush = true;
      abilities23.CanBarricade = true;
      abilities23.CanTrade = true;
      abilities23.IsIntelligent = true;
      ActorSheet startingSheet23 = new ActorSheet(this.DATA_GANGSTA_MAN.HP, this.DATA_GANGSTA_MAN.STA, this.HUMAN_HUN, this.HUMAN_SLP, this.HUMAN_SAN, new Attack(AttackKind.PHYSICAL, GameActors.VERB_PUNCH, this.DATA_GANGSTA_MAN.ATK, this.DATA_GANGSTA_MAN.DMG), new Defence(this.DATA_GANGSTA_MAN.DEF, this.DATA_GANGSTA_MAN.PRO_HIT, this.DATA_GANGSTA_MAN.PRO_SHOT), this.DATA_GANGSTA_MAN.FOV, this.DATA_GANGSTA_MAN.AUDIO, 0, this.HUMAN_INVENTORY);
      Type defaultController21 = typeof (GangAI);
      this[(GameActors.IDs) num23] = new ActorModel((string) local10, name23, plural23, score23, body23, abilities23, startingSheet23, defaultController21)
      {
        FlavorDescription = this.DATA_GANGSTA_MAN.FLAVOR
      };
      int num24 = 22;
      // ISSUE: variable of the null type
      __Null local11 = null;
      string name24 = this.DATA_POLICEMAN.NAME;
      string plural24 = this.DATA_POLICEMAN.PLURAL;
      int score24 = this.DATA_POLICEMAN.SCORE;
      DollBody body24 = new DollBody(true, this.DATA_POLICEMAN.SPD);
      Abilities abilities24 = new Abilities();
      abilities24.HasInventory = true;
      abilities24.HasToEat = true;
      abilities24.HasToSleep = true;
      abilities24.HasSanity = true;
      abilities24.CanTalk = true;
      abilities24.CanUseMapObjects = true;
      abilities24.CanBreakObjects = true;
      abilities24.CanJump = true;
      abilities24.CanTire = true;
      abilities24.CanRun = true;
      abilities24.CanUseItems = true;
      abilities24.CanTrade = true;
      abilities24.CanBarricade = true;
      abilities24.CanPush = true;
      abilities24.AI_CanUseAIExits = true;
      abilities24.IsLawEnforcer = true;
      abilities24.IsIntelligent = true;
      ActorSheet startingSheet24 = new ActorSheet(this.DATA_POLICEMAN.HP, this.DATA_POLICEMAN.STA, this.HUMAN_HUN, this.HUMAN_SLP, this.HUMAN_SAN, new Attack(AttackKind.PHYSICAL, GameActors.VERB_PUNCH, this.DATA_POLICEMAN.ATK, this.DATA_POLICEMAN.DMG), new Defence(this.DATA_POLICEMAN.DEF, this.DATA_POLICEMAN.PRO_HIT, this.DATA_POLICEMAN.PRO_SHOT), this.DATA_POLICEMAN.FOV, this.DATA_POLICEMAN.AUDIO, 0, this.HUMAN_INVENTORY);
      // ISSUE: variable of the null type
      __Null local12 = null;
      this[(GameActors.IDs) num24] = new ActorModel((string) local11, name24, plural24, score24, body24, abilities24, startingSheet24, (Type) local12)
      {
        FlavorDescription = this.DATA_POLICEMAN.FLAVOR
      };
      int num25 = 24;
      // ISSUE: variable of the null type
      __Null local13 = null;
      string name25 = this.DATA_BLACKOPS_MAN.NAME;
      string plural25 = this.DATA_BLACKOPS_MAN.PLURAL;
      int score25 = this.DATA_BLACKOPS_MAN.SCORE;
      DollBody body25 = new DollBody(true, this.DATA_BLACKOPS_MAN.SPD);
      Abilities abilities25 = new Abilities();
      abilities25.HasInventory = true;
      abilities25.CanUseMapObjects = true;
      abilities25.CanBreakObjects = true;
      abilities25.CanJump = true;
      abilities25.CanTire = true;
      abilities25.CanRun = true;
      abilities25.CanUseItems = true;
      abilities25.CanTalk = true;
      abilities25.HasToSleep = true;
      abilities25.HasSanity = true;
      abilities25.CanPush = true;
      abilities25.CanBarricade = true;
      abilities25.IsIntelligent = true;
      ActorSheet startingSheet25 = new ActorSheet(this.DATA_BLACKOPS_MAN.HP, this.DATA_BLACKOPS_MAN.STA, this.HUMAN_HUN, this.HUMAN_SLP, this.HUMAN_SAN, new Attack(AttackKind.PHYSICAL, GameActors.VERB_PUNCH, this.DATA_BLACKOPS_MAN.ATK, this.DATA_BLACKOPS_MAN.DMG), new Defence(this.DATA_BLACKOPS_MAN.DEF, this.DATA_BLACKOPS_MAN.PRO_HIT, this.DATA_BLACKOPS_MAN.PRO_SHOT), this.DATA_BLACKOPS_MAN.FOV, this.DATA_BLACKOPS_MAN.AUDIO, 0, this.HUMAN_INVENTORY);
      Type defaultController22 = typeof (SoldierAI);
      this[(GameActors.IDs) num25] = new ActorModel((string) local13, name25, plural25, score25, body25, abilities25, startingSheet25, defaultController22)
      {
        FlavorDescription = this.DATA_BLACKOPS_MAN.FLAVOR
      };
      GameActors.ActorData dataFeralDog = this.DATA_FERAL_DOG;
      int num26 = 18;
      // ISSUE: variable of the null type
      __Null local14 = null;
      string name26 = dataFeralDog.NAME;
      string plural26 = dataFeralDog.PLURAL;
      int score26 = dataFeralDog.SCORE;
      DollBody body26 = new DollBody(true, dataFeralDog.SPD);
      Abilities abilities26 = new Abilities();
      abilities26.HasInventory = true;
      abilities26.HasToEat = true;
      abilities26.HasToSleep = true;
      abilities26.CanBreakObjects = true;
      abilities26.CanJump = true;
      abilities26.CanTire = true;
      abilities26.CanRun = true;
      abilities26.AI_CanUseAIExits = true;
      ActorSheet startingSheet26 = new ActorSheet(dataFeralDog.HP, dataFeralDog.STA, this.DOG_HUN, this.DOG_SLP, 0, new Attack(AttackKind.PHYSICAL, new Verb("bite"), dataFeralDog.ATK, dataFeralDog.DMG), new Defence(dataFeralDog.DEF, dataFeralDog.PRO_HIT, dataFeralDog.PRO_SHOT), dataFeralDog.FOV, dataFeralDog.AUDIO, dataFeralDog.SMELL, this.DOG_INVENTORY);
      Type defaultController23 = typeof (FeralDogAI);
      this[(GameActors.IDs) num26] = new ActorModel((string) local14, name26, plural26, score26, body26, abilities26, startingSheet26, defaultController23)
      {
        FlavorDescription = dataFeralDog.FLAVOR
      };
      int num27 = 26;
      // ISSUE: variable of the null type
      __Null local15 = null;
      string name27 = this.DATA_JASON_MYERS.NAME;
      string plural27 = this.DATA_JASON_MYERS.PLURAL;
      int score27 = this.DATA_JASON_MYERS.SCORE;
      DollBody body27 = new DollBody(true, this.DATA_JASON_MYERS.SPD);
      Abilities abilities27 = new Abilities();
      abilities27.HasInventory = true;
      abilities27.CanUseMapObjects = true;
      abilities27.CanBreakObjects = true;
      abilities27.CanJump = true;
      abilities27.CanTire = true;
      abilities27.CanRun = true;
      abilities27.CanUseItems = true;
      abilities27.HasToEat = false;
      abilities27.HasToSleep = false;
      abilities27.CanTalk = true;
      abilities27.CanPush = true;
      abilities27.CanBarricade = true;
      abilities27.AI_CanUseAIExits = true;
      ActorSheet startingSheet27 = new ActorSheet(this.DATA_JASON_MYERS.HP, this.DATA_JASON_MYERS.STA, this.HUMAN_HUN, this.HUMAN_SLP, 0, new Attack(AttackKind.PHYSICAL, GameActors.VERB_PUNCH, this.DATA_JASON_MYERS.ATK, this.DATA_JASON_MYERS.DMG), new Defence(this.DATA_JASON_MYERS.DEF, this.DATA_JASON_MYERS.PRO_HIT, this.DATA_JASON_MYERS.PRO_SHOT), this.DATA_JASON_MYERS.FOV, this.DATA_JASON_MYERS.AUDIO, 0, this.HUMAN_INVENTORY);
      Type defaultController24 = typeof (InsaneHumanAI);
      this[(GameActors.IDs) num27] = new ActorModel((string) local15, name27, plural27, score27, body27, abilities27, startingSheet27, defaultController24)
      {
        FlavorDescription = this.DATA_JASON_MYERS.FLAVOR
      };
    }

    public bool LoadFromCSV(IRogueUI ui, string path)
    {
      this.Notify(ui, "loading file...");
      List<string> stringList = new List<string>();
      bool flag = true;
      using (StreamReader streamReader = File.OpenText(path))
      {
        while (!streamReader.EndOfStream)
        {
          string str = streamReader.ReadLine();
          if (flag)
            flag = false;
          else
            stringList.Add(str);
        }
        streamReader.Close();
      }
      this.Notify(ui, "parsing CSV...");
      CSVTable toTable = new CSVParser().ParseToTable(stringList.ToArray(), 16);
      this.Notify(ui, "reading data...");
      this.DATA_SKELETON = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs._FIRST);
      this.DATA_RED_EYED_SKELETON = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.UNDEAD_RED_EYED_SKELETON);
      this.DATA_RED_SKELETON = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.UNDEAD_RED_SKELETON);
      this.DATA_ZOMBIE = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.UNDEAD_ZOMBIE);
      this.DATA_DARK_EYED_ZOMBIE = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.UNDEAD_DARK_EYED_ZOMBIE);
      this.DATA_DARK_ZOMBIE = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.UNDEAD_DARK_ZOMBIE);
      this.DATA_MALE_ZOMBIFIED = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.UNDEAD_MALE_ZOMBIFIED);
      this.DATA_FEMALE_ZOMBIFIED = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.UNDEAD_FEMALE_ZOMBIFIED);
      this.DATA_MALE_NEOPHYTE = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.UNDEAD_MALE_NEOPHYTE);
      this.DATA_FEMALE_NEOPHYTE = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.UNDEAD_FEMALE_NEOPHYTE);
      this.DATA_MALE_DISCIPLE = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.UNDEAD_MALE_DISCIPLE);
      this.DATA_FEMALE_DISCIPLE = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.UNDEAD_FEMALE_DISCIPLE);
      this.DATA_ZM = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.UNDEAD_ZOMBIE_MASTER);
      this.DATA_ZL = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.UNDEAD_ZOMBIE_LORD);
      this.DATA_ZP = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.UNDEAD_ZOMBIE_PRINCE);
      this.DATA_RAT_ZOMBIE = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.UNDEAD_RAT_ZOMBIE);
      this.DATA_SEWERS_THING = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.SEWERS_THING);
      this.DATA_MALE_CIVILIAN = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.MALE_CIVILIAN);
      this.DATA_FEMALE_CIVILIAN = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.FEMALE_CIVILIAN);
      this.DATA_FERAL_DOG = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.FERAL_DOG);
      this.DATA_POLICEMAN = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.POLICEMAN);
      this.DATA_CHAR_GUARD = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.CHAR_GUARD);
      this.DATA_NATGUARD = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.ARMY_NATIONAL_GUARD);
      this.DATA_BIKER_MAN = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.BIKER_MAN);
      this.DATA_GANGSTA_MAN = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.GANGSTA_MAN);
      this.DATA_BLACKOPS_MAN = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.BLACKOPS_MAN);
      this.DATA_JASON_MYERS = this.GetDataFromCSVTable(ui, toTable, GameActors.IDs.JASON_MYERS);
      this.CreateModels();
      this.Notify(ui, "done!");
      return true;
    }

    private CSVLine FindLineForModel(CSVTable table, GameActors.IDs modelID)
    {
      foreach (CSVLine line in table.Lines)
      {
        if (line[0].ParseText() == modelID.ToString())
          return line;
      }
      return (CSVLine) null;
    }

    private GameActors.ActorData GetDataFromCSVTable(IRogueUI ui, CSVTable table, GameActors.IDs modelID)
    {
      CSVLine lineForModel = this.FindLineForModel(table, modelID);
      if (lineForModel == null)
        throw new InvalidOperationException(string.Format("model {0} not found", (object) modelID.ToString()));
      try
      {
        return GameActors.ActorData.FromCSVLine(lineForModel);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException(string.Format("invalid data format for model {0}; exception : {1}", (object) modelID.ToString(), (object) ex.ToString()));
      }
    }

    private void Notify(IRogueUI ui, string stage)
    {
      ui.UI_Clear(Color.Black);
      ui.UI_DrawStringBold(Color.White, "Loading actors data : " + stage, 0, 0, new Color?());
      ui.UI_Repaint();
    }

    public bool IsZombifiedBranch(ActorModel m)
    {
      if (m != this.MaleZombified && m != this.FemaleZombified && (m != this.MaleNeophyte && m != this.FemaleNeophyte) && m != this.MaleDisciple)
        return m == this.FemaleDisciple;
      return true;
    }

    public bool IsZMBranch(ActorModel m)
    {
      if (m != this.ZombieMaster && m != this.ZombieLord)
        return m == this.ZombiePrince;
      return true;
    }

    public bool IsSkeletonBranch(ActorModel m)
    {
      if (m != this.Skeleton && m != this.Red_Eyed_Skeleton)
        return m == this.Red_Skeleton;
      return true;
    }

    public bool IsShamblerBranch(ActorModel m)
    {
      if (m != this.Zombie && m != this.DarkEyedZombie)
        return m == this.DarkZombie;
      return true;
    }

    public bool IsRatBranch(ActorModel m)
    {
      return m == this.RatZombie;
    }

    public enum IDs
    {
      UNDEAD_SKELETON = 0,
      _FIRST = 0,
      UNDEAD_RED_EYED_SKELETON = 1,
      UNDEAD_RED_SKELETON = 2,
      UNDEAD_ZOMBIE = 3,
      UNDEAD_DARK_EYED_ZOMBIE = 4,
      UNDEAD_DARK_ZOMBIE = 5,
      UNDEAD_ZOMBIE_MASTER = 6,
      UNDEAD_ZOMBIE_LORD = 7,
      UNDEAD_ZOMBIE_PRINCE = 8,
      UNDEAD_MALE_ZOMBIFIED = 9,
      UNDEAD_FEMALE_ZOMBIFIED = 10, // 0x0000000A
      UNDEAD_MALE_NEOPHYTE = 11, // 0x0000000B
      UNDEAD_FEMALE_NEOPHYTE = 12, // 0x0000000C
      UNDEAD_MALE_DISCIPLE = 13, // 0x0000000D
      UNDEAD_FEMALE_DISCIPLE = 14, // 0x0000000E
      UNDEAD_RAT_ZOMBIE = 15, // 0x0000000F
      MALE_CIVILIAN = 16, // 0x00000010
      FEMALE_CIVILIAN = 17, // 0x00000011
      FERAL_DOG = 18, // 0x00000012
      CHAR_GUARD = 19, // 0x00000013
      ARMY_NATIONAL_GUARD = 20, // 0x00000014
      BIKER_MAN = 21, // 0x00000015
      POLICEMAN = 22, // 0x00000016
      GANGSTA_MAN = 23, // 0x00000017
      BLACKOPS_MAN = 24, // 0x00000018
      SEWERS_THING = 25, // 0x00000019
      JASON_MYERS = 26, // 0x0000001A
      _COUNT = 27, // 0x0000001B
    }

    public struct ActorData
    {
      public const int COUNT_FIELDS = 16;

      public string NAME { get; set; }

      public string PLURAL { get; set; }

      public int SPD { get; set; }

      public int HP { get; set; }

      public int STA { get; set; }

      public int ATK { get; set; }

      public int DMG { get; set; }

      public int DEF { get; set; }

      public int PRO_HIT { get; set; }

      public int PRO_SHOT { get; set; }

      public int FOV { get; set; }

      public int AUDIO { get; set; }

      public int SMELL { get; set; }

      public int SCORE { get; set; }

      public string FLAVOR { get; set; }

      public static GameActors.ActorData FromCSVLine(CSVLine line)
      {
        return new GameActors.ActorData()
        {
          NAME = line[1].ParseText(),
          PLURAL = line[2].ParseText(),
          SPD = line[3].ParseInt(),
          HP = line[4].ParseInt(),
          STA = line[5].ParseInt(),
          ATK = line[6].ParseInt(),
          DMG = line[7].ParseInt(),
          DEF = line[8].ParseInt(),
          PRO_HIT = line[9].ParseInt(),
          PRO_SHOT = line[10].ParseInt(),
          FOV = line[11].ParseInt(),
          AUDIO = line[12].ParseInt(),
          SMELL = line[13].ParseInt(),
          SCORE = line[14].ParseInt(),
          FLAVOR = line[15].ParseText()
        };
      }
    }
  }
}
