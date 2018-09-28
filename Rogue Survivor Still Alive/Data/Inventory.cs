// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.Inventory
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;
using System.Collections.Generic;

namespace djack.RogueSurvivor.Data
{
  [Serializable]
  internal class Inventory
  {
    private List<Item> m_Items;
    private int m_MaxCapacity;

    public IEnumerable<Item> Items
    {
      get
      {
        return (IEnumerable<Item>) this.m_Items;
      }
    }

    public int CountItems
    {
      get
      {
        return this.m_Items.Count;
      }
    }

    public Item this[int index]
    {
      get
      {
        if (index < 0 || index >= this.m_Items.Count)
          return (Item) null;
        return this.m_Items[index];
      }
    }

    public int MaxCapacity
    {
      get
      {
        return this.m_MaxCapacity;
      }
      set
      {
        this.m_MaxCapacity = value;
      }
    }

    public bool IsEmpty
    {
      get
      {
        return this.m_Items.Count == 0;
      }
    }

    public bool IsFull
    {
      get
      {
        return this.m_Items.Count >= this.m_MaxCapacity;
      }
    }

    public Item TopItem
    {
      get
      {
        if (this.m_Items.Count == 0)
          return (Item) null;
        return this.m_Items[this.m_Items.Count - 1];
      }
    }

    public Item BottomItem
    {
      get
      {
        if (this.m_Items.Count == 0)
          return (Item) null;
        return this.m_Items[0];
      }
    }

    public Inventory(int maxCapacity)
    {
      if (maxCapacity < 0)
        throw new ArgumentOutOfRangeException("maxCapacity < 0");
      this.m_MaxCapacity = maxCapacity;
      this.m_Items = new List<Item>(1);
    }

    public bool AddAll(Item it)
    {
      if (it == null)
        throw new ArgumentNullException(nameof (it));
      int stackedQuantity;
      List<Item> itemsStackableWith = this.GetItemsStackableWith(it, out stackedQuantity);
      if (stackedQuantity == it.Quantity)
      {
        int quantity = it.Quantity;
        foreach (Item to in itemsStackableWith)
        {
          int addThis = Math.Min(to.Model.StackingLimit - to.Quantity, quantity);
          this.AddToStack(it, addThis, to);
          quantity -= addThis;
          if (quantity <= 0)
            break;
        }
        return true;
      }
      if (this.IsFull)
        return false;
      this.m_Items.Add(it);
      return true;
    }

    public bool AddAsMuchAsPossible(Item it, out int quantityAdded)
    {
      if (it == null)
        throw new ArgumentNullException(nameof (it));
      int quantity = it.Quantity;
      int stackedQuantity;
      List<Item> itemsStackableWith = this.GetItemsStackableWith(it, out stackedQuantity);
      if (itemsStackableWith != null)
      {
        quantityAdded = 0;
        foreach (Item to in itemsStackableWith)
        {
          int stack = this.AddToStack(it, it.Quantity - quantityAdded, to);
          quantityAdded += stack;
        }
        if (quantityAdded < it.Quantity)
        {
          it.Quantity -= quantityAdded;
          if (!this.IsFull)
          {
            this.m_Items.Add(it);
            quantityAdded = quantity;
          }
        }
        else
          it.Quantity = 0;
        return true;
      }
      if (this.IsFull)
      {
        quantityAdded = 0;
        return false;
      }
      quantityAdded = it.Quantity;
      this.m_Items.Add(it);
      return true;
    }

    public bool CanAddAtLeastOne(Item it)
    {
      if (it == null)
        throw new ArgumentNullException(nameof (it));
      if (!this.IsFull)
        return true;
      int stackedQuantity;
      return this.GetItemsStackableWith(it, out stackedQuantity) != null;
    }

    public void RemoveAllQuantity(Item it)
    {
      this.m_Items.Remove(it);
    }

    public void Consume(Item it)
    {
      if (--it.Quantity > 0)
        return;
      this.m_Items.Remove(it);
    }

    private int AddToStack(Item from, int addThis, Item to)
    {
      int num = 0;
      for (; addThis > 0 && to.Quantity < to.Model.StackingLimit; --addThis)
      {
        ++to.Quantity;
        ++num;
      }
      return num;
    }

    private List<Item> GetItemsStackableWith(Item it, out int stackedQuantity)
    {
      stackedQuantity = 0;
      if (!it.Model.IsStackable)
        return (List<Item>) null;
      List<Item> objList = (List<Item>) null;
      foreach (Item obj in this.m_Items)
      {
        if (obj.Model == it.Model && obj.CanStackMore && !obj.IsEquipped)
        {
          if (objList == null)
            objList = new List<Item>(this.m_Items.Count);
          objList.Add(obj);
          int val2 = obj.Model.StackingLimit - obj.Quantity;
          int num = Math.Min(it.Quantity - stackedQuantity, val2);
          stackedQuantity += num;
          if (stackedQuantity == it.Quantity)
            break;
        }
      }
      return objList;
    }

    private Item GetBestDestackable(Item it)
    {
      if (!it.Model.IsStackable)
        return (Item) null;
      Item obj1 = (Item) null;
      foreach (Item obj2 in this.m_Items)
      {
        if (obj2.Model == it.Model && (obj1 == null || obj2.Quantity < obj1.Quantity))
          obj1 = obj2;
      }
      return obj1;
    }

    public bool Contains(Item it)
    {
      return this.m_Items.Contains(it);
    }

    public Item GetFirstByModel(ItemModel model)
    {
      foreach (Item obj in this.m_Items)
      {
        if (obj.Model == model)
          return obj;
      }
      return (Item) null;
    }

    public bool HasItemOfType(Type tt)
    {
      return this.GetFirstByType(tt) != null;
    }

    public Item GetFirstByType(Type tt)
    {
      foreach (Item obj in this.m_Items)
      {
        if (obj.GetType() == tt)
          return obj;
      }
      return (Item) null;
    }

    public List<_T_> GetItemsByType<_T_>() where _T_ : Item
    {
      List<_T_> tList = (List<_T_>) null;
      Type type = typeof (_T_);
      foreach (Item obj in this.m_Items)
      {
        if (obj.GetType() == type)
        {
          if (tList == null)
            tList = new List<_T_>(this.m_Items.Count);
          tList.Add(obj as _T_);
        }
      }
      return tList;
    }

    public Item GetFirstMatching(Predicate<Item> fn)
    {
      foreach (Item obj in this.m_Items)
      {
        if (fn(obj))
          return obj;
      }
      return (Item) null;
    }

    public int CountItemsMatching(Predicate<Item> fn)
    {
      int num = 0;
      foreach (Item obj in this.m_Items)
      {
        if (fn(obj))
          ++num;
      }
      return num;
    }

    public bool HasItemMatching(Predicate<Item> fn)
    {
      foreach (Item obj in this.m_Items)
      {
        if (fn(obj))
          return true;
      }
      return false;
    }
  }
}
