// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.Attack
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Data
{
  [Serializable]
  internal struct Attack
  {
    [NonSerialized]
    public static readonly Attack BLANK = new Attack(AttackKind.PHYSICAL, new Verb("<blank>"), 0, 0, 0, 0);

    public AttackKind Kind { get; private set; }

    public Verb Verb { get; private set; }

    public int HitValue { get; private set; }

    public int DamageValue { get; private set; }

    public int StaminaPenalty { get; private set; }

    public int Range { get; private set; }

    public int EfficientRange
    {
      get
      {
        return this.Range / 2;
      }
    }

    public Attack(AttackKind kind, Verb verb, int hitValue, int damageValue, int staminaPenalty, int range)
    {
      this = new Attack();
      if (verb == null)
        throw new ArgumentNullException(nameof (verb));
      this.Kind = kind;
      this.Verb = verb;
      this.HitValue = hitValue;
      this.DamageValue = damageValue;
      this.StaminaPenalty = staminaPenalty;
      this.Range = range;
    }

    public Attack(AttackKind kind, Verb verb, int hitValue, int damageValue)
    {
      this = new Attack(kind, verb, hitValue, damageValue, 0, 0);
    }

    public Attack(AttackKind kind, Verb verb, int hitValue, int damageValue, int staminaPenalty)
    {
      this = new Attack(kind, verb, hitValue, damageValue, staminaPenalty, 0);
    }
  }
}
