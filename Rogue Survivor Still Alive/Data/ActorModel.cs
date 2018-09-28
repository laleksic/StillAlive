// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.ActorModel
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Data
{
  internal class ActorModel
  {
    private int m_ID;
    private string m_ImageID;
    private DollBody m_DollBody;
    private string m_Name;
    private string m_PluralName;
    private Abilities m_Abilities;
    private ActorSheet m_StartingSheet;
    private Type m_DefaultController;
    private string m_FlavorDescription;
    private int m_ScoreValue;
    private int m_CreatedCount;

    public int ID
    {
      get
      {
        return this.m_ID;
      }
      set
      {
        this.m_ID = value;
      }
    }

    public string ImageID
    {
      get
      {
        return this.m_ImageID;
      }
    }

    public DollBody DollBody
    {
      get
      {
        return this.m_DollBody;
      }
    }

    public string Name
    {
      get
      {
        return this.m_Name;
      }
    }

    public string PluralName
    {
      get
      {
        return this.m_PluralName;
      }
    }

    public ActorSheet StartingSheet
    {
      get
      {
        return this.m_StartingSheet;
      }
    }

    public Abilities Abilities
    {
      get
      {
        return this.m_Abilities;
      }
    }

    public Type DefaultController
    {
      get
      {
        return this.m_DefaultController;
      }
    }

    public int CreatedCount
    {
      get
      {
        return this.m_CreatedCount;
      }
    }

    public int ScoreValue
    {
      get
      {
        return this.m_ScoreValue;
      }
    }

    public string FlavorDescription
    {
      get
      {
        return this.m_FlavorDescription;
      }
      set
      {
        this.m_FlavorDescription = value;
      }
    }

    public ActorModel(string imageID, string name, string pluralName, int scoreValue, DollBody body, Abilities abilities, ActorSheet startingSheet, Type defaultController)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (abilities == null)
        throw new ArgumentNullException(nameof (abilities));
      if (startingSheet == null)
        throw new ArgumentNullException(nameof (startingSheet));
      if (defaultController != null && !defaultController.IsSubclassOf(typeof (ActorController)))
        throw new ArgumentException("defaultController is not a subclass of ActorController");
      this.m_ImageID = imageID;
      this.m_DollBody = body;
      this.m_Name = name;
      this.m_PluralName = pluralName;
      this.m_StartingSheet = startingSheet;
      this.m_Abilities = abilities;
      this.m_DefaultController = defaultController;
      this.m_ScoreValue = scoreValue;
      this.m_CreatedCount = 0;
    }

    private Actor Create(Faction faction, int spawnTime)
    {
      ++this.m_CreatedCount;
      return new Actor(this, faction, spawnTime)
      {
        Controller = this.InstanciateController()
      };
    }

    private ActorController InstanciateController()
    {
      if (this.m_DefaultController == null)
        return (ActorController) null;
      return this.m_DefaultController.GetConstructor(Type.EmptyTypes).Invoke((object[]) null) as ActorController;
    }

    public Actor CreateAnonymous(Faction faction, int spawnTime)
    {
      return this.Create(faction, spawnTime);
    }

    public Actor CreateNumberedName(Faction faction, int spawnTime)
    {
      Actor actor = this.Create(faction, spawnTime);
      actor.Name += this.m_CreatedCount.ToString();
      actor.IsProperName = true;
      return actor;
    }

    public Actor CreateNamed(Faction faction, string properName, bool isPluralName, int spawnTime)
    {
      Actor actor = this.Create(faction, spawnTime);
      actor.Name = properName;
      actor.IsProperName = true;
      actor.IsPluralName = isPluralName;
      return actor;
    }
  }
}
