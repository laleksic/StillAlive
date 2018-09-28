// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Gameplay.GameItems
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Engine.Items;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace djack.RogueSurvivor.Gameplay
{
  internal class GameItems : ItemModelDB
  {
    private ItemModel[] m_Models = new ItemModel[69];
    private GameItems.MedecineData DATA_MEDICINE_BANDAGE;
    private GameItems.MedecineData DATA_MEDICINE_MEDIKIT;
    private GameItems.MedecineData DATA_MEDICINE_PILLS_STA;
    private GameItems.MedecineData DATA_MEDICINE_PILLS_SLP;
    private GameItems.MedecineData DATA_MEDICINE_PILLS_SAN;
    private GameItems.MedecineData DATA_MEDICINE_PILLS_ANTIVIRAL;
    private GameItems.FoodData DATA_FOOD_ARMY_RATION;
    private GameItems.FoodData DATA_FOOD_GROCERIES;
    private GameItems.FoodData DATA_FOOD_CANNED_FOOD;
    private GameItems.MeleeWeaponData DATA_MELEE_CROWBAR;
    private GameItems.MeleeWeaponData DATA_MELEE_BASEBALLBAT;
    private GameItems.MeleeWeaponData DATA_MELEE_COMBAT_KNIFE;
    private GameItems.MeleeWeaponData DATA_MELEE_UNIQUE_JASON_MYERS_AXE;
    private GameItems.MeleeWeaponData DATA_MELEE_GOLFCLUB;
    private GameItems.MeleeWeaponData DATA_MELEE_HUGE_HAMMER;
    private GameItems.MeleeWeaponData DATA_MELEE_SMALL_HAMMER;
    private GameItems.MeleeWeaponData DATA_MELEE_IRON_GOLFCLUB;
    private GameItems.MeleeWeaponData DATA_MELEE_SHOVEL;
    private GameItems.MeleeWeaponData DATA_MELEE_SHORT_SHOVEL;
    private GameItems.MeleeWeaponData DATA_MELEE_TRUNCHEON;
    private GameItems.MeleeWeaponData DATA_MELEE_IMPROVISED_CLUB;
    private GameItems.MeleeWeaponData DATA_MELEE_IMPROVISED_SPEAR;
    private GameItems.MeleeWeaponData DATA_MELEE_UNIQUE_FAMU_FATARU_KATANA;
    private GameItems.MeleeWeaponData DATA_MELEE_UNIQUE_BIGBEAR_BAT;
    private GameItems.MeleeWeaponData DATA_MELEE_UNIQUE_ROGUEDJACK_KEYBOARD;
    private GameItems.RangedWeaponData DATA_RANGED_ARMY_PISTOL;
    private GameItems.RangedWeaponData DATA_RANGED_ARMY_RIFLE;
    private GameItems.RangedWeaponData DATA_RANGED_HUNTING_CROSSBOW;
    private GameItems.RangedWeaponData DATA_RANGED_HUNTING_RIFLE;
    private GameItems.RangedWeaponData DATA_RANGED_KOLT_REVOLVER;
    private GameItems.RangedWeaponData DATA_RANGED_PISTOL;
    private GameItems.RangedWeaponData DATA_RANGED_PRECISION_RIFLE;
    private GameItems.RangedWeaponData DATA_RANGED_SHOTGUN;
    private GameItems.RangedWeaponData DATA_UNIQUE_SANTAMAN_SHOTGUN;
    private GameItems.RangedWeaponData DATA_UNIQUE_HANS_VON_HANZ_PISTOL;
    private GameItems.ExplosiveData DATA_EXPLOSIVE_GRENADE;
    private GameItems.BarricadingMaterialData DATA_BAR_WOODEN_PLANK;
    private GameItems.ArmorData DATA_ARMOR_ARMY;
    private GameItems.ArmorData DATA_ARMOR_CHAR;
    private GameItems.ArmorData DATA_ARMOR_HELLS_SOULS_JACKET;
    private GameItems.ArmorData DATA_ARMOR_FREE_ANGELS_JACKET;
    private GameItems.ArmorData DATA_ARMOR_POLICE_JACKET;
    private GameItems.ArmorData DATA_ARMOR_POLICE_RIOT;
    private GameItems.ArmorData DATA_ARMOR_HUNTER_VEST;
    private GameItems.TrackerData DATA_TRACKER_BLACKOPS_GPS;
    private GameItems.TrackerData DATA_TRACKER_CELL_PHONE;
    private GameItems.TrackerData DATA_TRACKER_ZTRACKER;
    private GameItems.TrackerData DATA_TRACKER_POLICE_RADIO;
    private GameItems.SprayPaintData DATA_SPRAY_PAINT1;
    private GameItems.SprayPaintData DATA_SPRAY_PAINT2;
    private GameItems.SprayPaintData DATA_SPRAY_PAINT3;
    private GameItems.SprayPaintData DATA_SPRAY_PAINT4;
    private GameItems.LightData DATA_LIGHT_FLASHLIGHT;
    private GameItems.LightData DATA_LIGHT_BIG_FLASHLIGHT;
    private GameItems.ScentSprayData DATA_SCENT_SPRAY_STENCH_KILLER;
    private GameItems.TrapData DATA_TRAP_EMPTY_CAN;
    private GameItems.TrapData DATA_TRAP_BEAR_TRAP;
    private GameItems.TrapData DATA_TRAP_SPIKES;
    private GameItems.TrapData DATA_TRAP_BARBED_WIRE;
    private GameItems.EntData DATA_ENT_BOOK;
    private GameItems.EntData DATA_ENT_MAGAZINE;

    public override ItemModel this[int id]
    {
      get
      {
        return this.m_Models[id];
      }
    }

    public ItemModel this[GameItems.IDs id]
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

    public ItemMedicineModel BANDAGE
    {
      get
      {
        return this[GameItems.IDs._FIRST] as ItemMedicineModel;
      }
    }

    public ItemMedicineModel MEDIKIT
    {
      get
      {
        return this[GameItems.IDs.MEDICINE_MEDIKIT] as ItemMedicineModel;
      }
    }

    public ItemMedicineModel PILLS_STA
    {
      get
      {
        return this[GameItems.IDs.MEDICINE_PILLS_STA] as ItemMedicineModel;
      }
    }

    public ItemMedicineModel PILLS_SLP
    {
      get
      {
        return this[GameItems.IDs.MEDICINE_PILLS_SLP] as ItemMedicineModel;
      }
    }

    public ItemMedicineModel PILLS_SAN
    {
      get
      {
        return this[GameItems.IDs.MEDICINE_PILLS_SAN] as ItemMedicineModel;
      }
    }

    public ItemMedicineModel PILLS_ANTIVIRAL
    {
      get
      {
        return this[GameItems.IDs.MEDICINE_PILLS_ANTIVIRAL] as ItemMedicineModel;
      }
    }

    public ItemFoodModel ARMY_RATION
    {
      get
      {
        return this[GameItems.IDs.FOOD_ARMY_RATION] as ItemFoodModel;
      }
    }

    public ItemFoodModel GROCERIES
    {
      get
      {
        return this[GameItems.IDs.FOOD_GROCERIES] as ItemFoodModel;
      }
    }

    public ItemFoodModel CANNED_FOOD
    {
      get
      {
        return this[GameItems.IDs.FOOD_CANNED_FOOD] as ItemFoodModel;
      }
    }

    public ItemMeleeWeaponModel CROWBAR
    {
      get
      {
        return this[GameItems.IDs.MELEE_CROWBAR] as ItemMeleeWeaponModel;
      }
    }

    public ItemMeleeWeaponModel BASEBALLBAT
    {
      get
      {
        return this[GameItems.IDs.MELEE_BASEBALLBAT] as ItemMeleeWeaponModel;
      }
    }

    public ItemMeleeWeaponModel COMBAT_KNIFE
    {
      get
      {
        return this[GameItems.IDs.MELEE_COMBAT_KNIFE] as ItemMeleeWeaponModel;
      }
    }

    public ItemMeleeWeaponModel UNIQUE_JASON_MYERS_AXE
    {
      get
      {
        return this[GameItems.IDs.UNIQUE_JASON_MYERS_AXE] as ItemMeleeWeaponModel;
      }
    }

    public ItemMeleeWeaponModel GOLFCLUB
    {
      get
      {
        return this[GameItems.IDs.MELEE_GOLFCLUB] as ItemMeleeWeaponModel;
      }
    }

    public ItemMeleeWeaponModel HUGE_HAMMER
    {
      get
      {
        return this[GameItems.IDs.MELEE_HUGE_HAMMER] as ItemMeleeWeaponModel;
      }
    }

    public ItemMeleeWeaponModel SMALL_HAMMER
    {
      get
      {
        return this[GameItems.IDs.MELEE_SMALL_HAMMER] as ItemMeleeWeaponModel;
      }
    }

    public ItemMeleeWeaponModel IRON_GOLFCLUB
    {
      get
      {
        return this[GameItems.IDs.MELEE_IRON_GOLFCLUB] as ItemMeleeWeaponModel;
      }
    }

    public ItemMeleeWeaponModel SHOVEL
    {
      get
      {
        return this[GameItems.IDs.MELEE_SHOVEL] as ItemMeleeWeaponModel;
      }
    }

    public ItemMeleeWeaponModel SHORT_SHOVEL
    {
      get
      {
        return this[GameItems.IDs.MELEE_SHORT_SHOVEL] as ItemMeleeWeaponModel;
      }
    }

    public ItemMeleeWeaponModel TRUNCHEON
    {
      get
      {
        return this[GameItems.IDs.MELEE_TRUNCHEON] as ItemMeleeWeaponModel;
      }
    }

    public ItemMeleeWeaponModel IMPROVISED_CLUB
    {
      get
      {
        return this[GameItems.IDs.MELEE_IMPROVISED_CLUB] as ItemMeleeWeaponModel;
      }
    }

    public ItemMeleeWeaponModel IMPROVISED_SPEAR
    {
      get
      {
        return this[GameItems.IDs.MELEE_IMPROVISED_SPEAR] as ItemMeleeWeaponModel;
      }
    }

    public ItemMeleeWeaponModel UNIQUE_FAMU_FATARU_KATANA
    {
      get
      {
        return this[GameItems.IDs.UNIQUE_FAMU_FATARU_KATANA] as ItemMeleeWeaponModel;
      }
    }

    public ItemMeleeWeaponModel UNIQUE_BIGBEAR_BAT
    {
      get
      {
        return this[GameItems.IDs.UNIQUE_BIGBEAR_BAT] as ItemMeleeWeaponModel;
      }
    }

    public ItemMeleeWeaponModel UNIQUE_ROGUEDJACK_KEYBOARD
    {
      get
      {
        return this[GameItems.IDs.UNIQUE_ROGUEDJACK_KEYBOARD] as ItemMeleeWeaponModel;
      }
    }

    public ItemRangedWeaponModel ARMY_PISTOL
    {
      get
      {
        return this[GameItems.IDs.RANGED_ARMY_PISTOL] as ItemRangedWeaponModel;
      }
    }

    public ItemRangedWeaponModel ARMY_RIFLE
    {
      get
      {
        return this[GameItems.IDs.RANGED_ARMY_RIFLE] as ItemRangedWeaponModel;
      }
    }

    public ItemRangedWeaponModel HUNTING_CROSSBOW
    {
      get
      {
        return this[GameItems.IDs.RANGED_HUNTING_CROSSBOW] as ItemRangedWeaponModel;
      }
    }

    public ItemRangedWeaponModel HUNTING_RIFLE
    {
      get
      {
        return this[GameItems.IDs.RANGED_HUNTING_RIFLE] as ItemRangedWeaponModel;
      }
    }

    public ItemRangedWeaponModel KOLT_REVOLVER
    {
      get
      {
        return this[GameItems.IDs.RANGED_KOLT_REVOLVER] as ItemRangedWeaponModel;
      }
    }

    public ItemRangedWeaponModel PISTOL
    {
      get
      {
        return this[GameItems.IDs.RANGED_PISTOL] as ItemRangedWeaponModel;
      }
    }

    public ItemRangedWeaponModel PRECISION_RIFLE
    {
      get
      {
        return this[GameItems.IDs.RANGED_PRECISION_RIFLE] as ItemRangedWeaponModel;
      }
    }

    public ItemRangedWeaponModel SHOTGUN
    {
      get
      {
        return this[GameItems.IDs.RANGED_SHOTGUN] as ItemRangedWeaponModel;
      }
    }

    public ItemRangedWeaponModel UNIQUE_SANTAMAN_SHOTGUN
    {
      get
      {
        return this[GameItems.IDs.UNIQUE_SANTAMAN_SHOTGUN] as ItemRangedWeaponModel;
      }
    }

    public ItemRangedWeaponModel UNIQUE_HANS_VON_HANZ_PISTOL
    {
      get
      {
        return this[GameItems.IDs.UNIQUE_HANS_VON_HANZ_PISTOL] as ItemRangedWeaponModel;
      }
    }

    public ItemAmmoModel AMMO_LIGHT_PISTOL
    {
      get
      {
        return this[GameItems.IDs.AMMO_LIGHT_PISTOL] as ItemAmmoModel;
      }
    }

    public ItemAmmoModel AMMO_HEAVY_PISTOL
    {
      get
      {
        return this[GameItems.IDs.AMMO_HEAVY_PISTOL] as ItemAmmoModel;
      }
    }

    public ItemAmmoModel AMMO_LIGHT_RIFLE
    {
      get
      {
        return this[GameItems.IDs.AMMO_LIGHT_RIFLE] as ItemAmmoModel;
      }
    }

    public ItemAmmoModel AMMO_HEAVY_RIFLE
    {
      get
      {
        return this[GameItems.IDs.AMMO_HEAVY_RIFLE] as ItemAmmoModel;
      }
    }

    public ItemAmmoModel AMMO_SHOTGUN
    {
      get
      {
        return this[GameItems.IDs.AMMO_SHOTGUN] as ItemAmmoModel;
      }
    }

    public ItemAmmoModel AMMO_BOLTS
    {
      get
      {
        return this[GameItems.IDs.AMMO_BOLTS] as ItemAmmoModel;
      }
    }

    public ItemGrenadeModel GRENADE
    {
      get
      {
        return this[GameItems.IDs.EXPLOSIVE_GRENADE] as ItemGrenadeModel;
      }
    }

    public ItemGrenadePrimedModel GRENADE_PRIMED
    {
      get
      {
        return this[GameItems.IDs.EXPLOSIVE_GRENADE_PRIMED] as ItemGrenadePrimedModel;
      }
    }

    public ItemBarricadeMaterialModel WOODENPLANK
    {
      get
      {
        return this[GameItems.IDs.BAR_WOODEN_PLANK] as ItemBarricadeMaterialModel;
      }
    }

    public ItemBodyArmorModel ARMY_BODYARMOR
    {
      get
      {
        return this[GameItems.IDs.ARMOR_ARMY_BODYARMOR] as ItemBodyArmorModel;
      }
    }

    public ItemBodyArmorModel CHAR_LT_BODYARMOR
    {
      get
      {
        return this[GameItems.IDs.ARMOR_CHAR_LIGHT_BODYARMOR] as ItemBodyArmorModel;
      }
    }

    public ItemBodyArmorModel HELLS_SOULS_JACKET
    {
      get
      {
        return this[GameItems.IDs.ARMOR_HELLS_SOULS_JACKET] as ItemBodyArmorModel;
      }
    }

    public ItemBodyArmorModel FREE_ANGELS_JACKET
    {
      get
      {
        return this[GameItems.IDs.ARMOR_FREE_ANGELS_JACKET] as ItemBodyArmorModel;
      }
    }

    public ItemBodyArmorModel POLICE_JACKET
    {
      get
      {
        return this[GameItems.IDs.ARMOR_POLICE_JACKET] as ItemBodyArmorModel;
      }
    }

    public ItemBodyArmorModel POLICE_RIOT
    {
      get
      {
        return this[GameItems.IDs.ARMOR_POLICE_RIOT] as ItemBodyArmorModel;
      }
    }

    public ItemBodyArmorModel HUNTER_VEST
    {
      get
      {
        return this[GameItems.IDs.ARMOR_HUNTER_VEST] as ItemBodyArmorModel;
      }
    }

    public ItemTrackerModel BLACKOPS_GPS
    {
      get
      {
        return this[GameItems.IDs.TRACKER_BLACKOPS] as ItemTrackerModel;
      }
    }

    public ItemTrackerModel CELL_PHONE
    {
      get
      {
        return this[GameItems.IDs.TRACKER_CELL_PHONE] as ItemTrackerModel;
      }
    }

    public ItemTrackerModel ZTRACKER
    {
      get
      {
        return this[GameItems.IDs.TRACKER_ZTRACKER] as ItemTrackerModel;
      }
    }

    public ItemTrackerModel POLICE_RADIO
    {
      get
      {
        return this[GameItems.IDs.TRACKER_POLICE_RADIO] as ItemTrackerModel;
      }
    }

    public ItemSprayPaintModel SPRAY_PAINT1
    {
      get
      {
        return this[GameItems.IDs.SPRAY_PAINT1] as ItemSprayPaintModel;
      }
    }

    public ItemSprayPaintModel SPRAY_PAINT2
    {
      get
      {
        return this[GameItems.IDs.SPRAY_PAINT2] as ItemSprayPaintModel;
      }
    }

    public ItemSprayPaintModel SPRAY_PAINT3
    {
      get
      {
        return this[GameItems.IDs.SPRAY_PAINT3] as ItemSprayPaintModel;
      }
    }

    public ItemSprayPaintModel SPRAY_PAINT4
    {
      get
      {
        return this[GameItems.IDs.SPRAY_PAINT4] as ItemSprayPaintModel;
      }
    }

    public ItemLightModel FLASHLIGHT
    {
      get
      {
        return this[GameItems.IDs.LIGHT_FLASHLIGHT] as ItemLightModel;
      }
    }

    public ItemLightModel BIG_FLASHLIGHT
    {
      get
      {
        return this[GameItems.IDs.LIGHT_BIG_FLASHLIGHT] as ItemLightModel;
      }
    }

    public ItemModel STENCH_KILLER
    {
      get
      {
        return this[GameItems.IDs.SCENT_SPRAY_STENCH_KILLER];
      }
    }

    public ItemModel EMPTY_CAN
    {
      get
      {
        return this[GameItems.IDs.TRAP_EMPTY_CAN];
      }
    }

    public ItemModel BEAR_TRAP
    {
      get
      {
        return this[GameItems.IDs.TRAP_BEAR_TRAP];
      }
    }

    public ItemModel SPIKES
    {
      get
      {
        return this[GameItems.IDs.TRAP_SPIKES];
      }
    }

    public ItemModel BARBED_WIRE
    {
      get
      {
        return this[GameItems.IDs.TRAP_BARBED_WIRE];
      }
    }

    public ItemModel BOOK
    {
      get
      {
        return this[GameItems.IDs.ENT_BOOK];
      }
    }

    public ItemModel MAGAZINE
    {
      get
      {
        return this[GameItems.IDs.ENT_MAGAZINE];
      }
    }

    public ItemModel UNIQUE_SUBWAY_BADGE
    {
      get
      {
        return this[GameItems.IDs.UNIQUE_SUBWAY_BADGE];
      }
    }

    public GameItems()
    {
      Models.Items = (ItemModelDB) this;
    }

    private bool StartsWithVowel(string name)
    {
      if (name[0] != 'a' && name[0] != 'A' && (name[0] != 'e' && name[0] != 'E') && (name[0] != 'i' && name[0] != 'I' && name[0] != 'y'))
        return name[0] == 'Y';
      return true;
    }

    private bool CheckPlural(string name, string plural)
    {
      return name == plural;
    }

    public void CreateModels()
    {
      int num1 = 0;
      ItemMedicineModel itemMedicineModel1 = new ItemMedicineModel(this.DATA_MEDICINE_BANDAGE.NAME, this.DATA_MEDICINE_BANDAGE.PLURAL, "Items\\item_bandages", this.DATA_MEDICINE_BANDAGE.HEALING, this.DATA_MEDICINE_BANDAGE.STAMINABOOST, this.DATA_MEDICINE_BANDAGE.SLEEPBOOST, this.DATA_MEDICINE_BANDAGE.INFECTIONCURE, this.DATA_MEDICINE_BANDAGE.SANITYCURE);
      itemMedicineModel1.IsPlural = true;
      itemMedicineModel1.IsStackable = true;
      itemMedicineModel1.StackingLimit = this.DATA_MEDICINE_BANDAGE.STACKINGLIMIT;
      itemMedicineModel1.FlavorDescription = this.DATA_MEDICINE_BANDAGE.FLAVOR;
      this[(GameItems.IDs) num1] = (ItemModel) itemMedicineModel1;
      int num2 = 1;
      ItemMedicineModel itemMedicineModel2 = new ItemMedicineModel(this.DATA_MEDICINE_MEDIKIT.NAME, this.DATA_MEDICINE_MEDIKIT.PLURAL, "Items\\item_medikit", this.DATA_MEDICINE_MEDIKIT.HEALING, this.DATA_MEDICINE_MEDIKIT.STAMINABOOST, this.DATA_MEDICINE_MEDIKIT.SLEEPBOOST, this.DATA_MEDICINE_MEDIKIT.INFECTIONCURE, this.DATA_MEDICINE_MEDIKIT.SANITYCURE);
      itemMedicineModel2.FlavorDescription = this.DATA_MEDICINE_MEDIKIT.FLAVOR;
      this[(GameItems.IDs) num2] = (ItemModel) itemMedicineModel2;
      int num3 = 2;
      ItemMedicineModel itemMedicineModel3 = new ItemMedicineModel(this.DATA_MEDICINE_PILLS_STA.NAME, this.DATA_MEDICINE_PILLS_STA.PLURAL, "Items\\item_pills_green", this.DATA_MEDICINE_PILLS_STA.HEALING, this.DATA_MEDICINE_PILLS_STA.STAMINABOOST, this.DATA_MEDICINE_PILLS_STA.SLEEPBOOST, this.DATA_MEDICINE_PILLS_STA.INFECTIONCURE, this.DATA_MEDICINE_PILLS_STA.SANITYCURE);
      itemMedicineModel3.IsPlural = true;
      itemMedicineModel3.IsStackable = true;
      itemMedicineModel3.StackingLimit = this.DATA_MEDICINE_PILLS_STA.STACKINGLIMIT;
      itemMedicineModel3.FlavorDescription = this.DATA_MEDICINE_PILLS_STA.FLAVOR;
      this[(GameItems.IDs) num3] = (ItemModel) itemMedicineModel3;
      int num4 = 3;
      ItemMedicineModel itemMedicineModel4 = new ItemMedicineModel(this.DATA_MEDICINE_PILLS_SLP.NAME, this.DATA_MEDICINE_PILLS_SLP.PLURAL, "Items\\item_pills_blue", this.DATA_MEDICINE_PILLS_SLP.HEALING, this.DATA_MEDICINE_PILLS_SLP.STAMINABOOST, this.DATA_MEDICINE_PILLS_SLP.SLEEPBOOST, this.DATA_MEDICINE_PILLS_SLP.INFECTIONCURE, this.DATA_MEDICINE_PILLS_SLP.SANITYCURE);
      itemMedicineModel4.IsPlural = true;
      itemMedicineModel4.IsStackable = true;
      itemMedicineModel4.StackingLimit = this.DATA_MEDICINE_PILLS_SLP.STACKINGLIMIT;
      itemMedicineModel4.FlavorDescription = this.DATA_MEDICINE_PILLS_SLP.FLAVOR;
      this[(GameItems.IDs) num4] = (ItemModel) itemMedicineModel4;
      int num5 = 4;
      ItemMedicineModel itemMedicineModel5 = new ItemMedicineModel(this.DATA_MEDICINE_PILLS_SAN.NAME, this.DATA_MEDICINE_PILLS_SAN.PLURAL, "Items\\item_pills_san", this.DATA_MEDICINE_PILLS_SAN.HEALING, this.DATA_MEDICINE_PILLS_SAN.STAMINABOOST, this.DATA_MEDICINE_PILLS_SAN.SLEEPBOOST, this.DATA_MEDICINE_PILLS_SAN.INFECTIONCURE, this.DATA_MEDICINE_PILLS_SAN.SANITYCURE);
      itemMedicineModel5.IsPlural = true;
      itemMedicineModel5.IsStackable = true;
      itemMedicineModel5.StackingLimit = this.DATA_MEDICINE_PILLS_SAN.STACKINGLIMIT;
      itemMedicineModel5.FlavorDescription = this.DATA_MEDICINE_PILLS_SAN.FLAVOR;
      this[(GameItems.IDs) num5] = (ItemModel) itemMedicineModel5;
      int num6 = 5;
      ItemMedicineModel itemMedicineModel6 = new ItemMedicineModel(this.DATA_MEDICINE_PILLS_ANTIVIRAL.NAME, this.DATA_MEDICINE_PILLS_ANTIVIRAL.PLURAL, "Items\\item_pills_antiviral", this.DATA_MEDICINE_PILLS_ANTIVIRAL.HEALING, this.DATA_MEDICINE_PILLS_ANTIVIRAL.STAMINABOOST, this.DATA_MEDICINE_PILLS_ANTIVIRAL.SLEEPBOOST, this.DATA_MEDICINE_PILLS_ANTIVIRAL.INFECTIONCURE, this.DATA_MEDICINE_PILLS_ANTIVIRAL.SANITYCURE);
      itemMedicineModel6.IsPlural = true;
      itemMedicineModel6.IsStackable = true;
      itemMedicineModel6.StackingLimit = this.DATA_MEDICINE_PILLS_ANTIVIRAL.STACKINGLIMIT;
      itemMedicineModel6.FlavorDescription = this.DATA_MEDICINE_PILLS_ANTIVIRAL.FLAVOR;
      this[(GameItems.IDs) num6] = (ItemModel) itemMedicineModel6;
      int num7 = 6;
      ItemFoodModel itemFoodModel1 = new ItemFoodModel(this.DATA_FOOD_ARMY_RATION.NAME, this.DATA_FOOD_ARMY_RATION.PLURAL, "Items\\item_army_ration", this.DATA_FOOD_ARMY_RATION.NUTRITION, this.DATA_FOOD_ARMY_RATION.BESTBEFORE);
      itemFoodModel1.IsAn = this.StartsWithVowel(this.DATA_FOOD_ARMY_RATION.NAME);
      itemFoodModel1.IsPlural = this.CheckPlural(this.DATA_FOOD_ARMY_RATION.NAME, this.DATA_FOOD_ARMY_RATION.PLURAL);
      itemFoodModel1.StackingLimit = this.DATA_FOOD_ARMY_RATION.STACKINGLIMIT;
      itemFoodModel1.FlavorDescription = this.DATA_FOOD_ARMY_RATION.FLAVOR;
      this[(GameItems.IDs) num7] = (ItemModel) itemFoodModel1;
      int num8 = 7;
      ItemFoodModel itemFoodModel2 = new ItemFoodModel(this.DATA_FOOD_GROCERIES.NAME, this.DATA_FOOD_GROCERIES.PLURAL, "Items\\item_groceries", this.DATA_FOOD_GROCERIES.NUTRITION, this.DATA_FOOD_GROCERIES.BESTBEFORE);
      itemFoodModel2.IsAn = this.StartsWithVowel(this.DATA_FOOD_GROCERIES.NAME);
      itemFoodModel2.IsPlural = this.CheckPlural(this.DATA_FOOD_GROCERIES.NAME, this.DATA_FOOD_GROCERIES.PLURAL);
      itemFoodModel2.StackingLimit = this.DATA_FOOD_GROCERIES.STACKINGLIMIT;
      itemFoodModel2.FlavorDescription = this.DATA_FOOD_GROCERIES.FLAVOR;
      this[(GameItems.IDs) num8] = (ItemModel) itemFoodModel2;
      int num9 = 8;
      ItemFoodModel itemFoodModel3 = new ItemFoodModel(this.DATA_FOOD_CANNED_FOOD.NAME, this.DATA_FOOD_CANNED_FOOD.PLURAL, "Items\\item_canned_food", this.DATA_FOOD_CANNED_FOOD.NUTRITION, this.DATA_FOOD_CANNED_FOOD.BESTBEFORE);
      itemFoodModel3.IsAn = this.StartsWithVowel(this.DATA_FOOD_CANNED_FOOD.NAME);
      itemFoodModel3.IsPlural = this.CheckPlural(this.DATA_FOOD_CANNED_FOOD.NAME, this.DATA_FOOD_CANNED_FOOD.PLURAL);
      itemFoodModel3.StackingLimit = this.DATA_FOOD_CANNED_FOOD.STACKINGLIMIT;
      itemFoodModel3.IsStackable = true;
      itemFoodModel3.FlavorDescription = this.DATA_FOOD_CANNED_FOOD.FLAVOR;
      this[(GameItems.IDs) num9] = (ItemModel) itemFoodModel3;
      int num10 = 9;
      ItemMeleeWeaponModel meleeWeaponModel1 = new ItemMeleeWeaponModel(this.DATA_MELEE_BASEBALLBAT.NAME, this.DATA_MELEE_BASEBALLBAT.PLURAL, "Items\\item_baseballbat", new Attack(AttackKind.PHYSICAL, new Verb("smash", "smashes"), this.DATA_MELEE_BASEBALLBAT.ATK, this.DATA_MELEE_BASEBALLBAT.DMG, this.DATA_MELEE_BASEBALLBAT.STA));
      meleeWeaponModel1.EquipmentPart = DollPart._FIRST;
      meleeWeaponModel1.IsStackable = this.DATA_MELEE_BASEBALLBAT.STACKINGLIMIT > 1;
      meleeWeaponModel1.StackingLimit = this.DATA_MELEE_BASEBALLBAT.STACKINGLIMIT;
      meleeWeaponModel1.FlavorDescription = this.DATA_MELEE_BASEBALLBAT.FLAVOR;
      meleeWeaponModel1.IsFragile = this.DATA_MELEE_BASEBALLBAT.ISFRAGILE;
      this[(GameItems.IDs) num10] = (ItemModel) meleeWeaponModel1;
      int num11 = 10;
      ItemMeleeWeaponModel meleeWeaponModel2 = new ItemMeleeWeaponModel(this.DATA_MELEE_COMBAT_KNIFE.NAME, this.DATA_MELEE_COMBAT_KNIFE.PLURAL, "Items\\item_combat_knife", new Attack(AttackKind.PHYSICAL, new Verb("stab", "stabs"), this.DATA_MELEE_COMBAT_KNIFE.ATK, this.DATA_MELEE_COMBAT_KNIFE.DMG, this.DATA_MELEE_COMBAT_KNIFE.STA));
      meleeWeaponModel2.EquipmentPart = DollPart._FIRST;
      meleeWeaponModel2.IsStackable = true;
      meleeWeaponModel2.StackingLimit = this.DATA_MELEE_COMBAT_KNIFE.STACKINGLIMIT;
      meleeWeaponModel2.FlavorDescription = this.DATA_MELEE_COMBAT_KNIFE.FLAVOR;
      meleeWeaponModel2.IsFragile = this.DATA_MELEE_COMBAT_KNIFE.ISFRAGILE;
      this[(GameItems.IDs) num11] = (ItemModel) meleeWeaponModel2;
      int num12 = 11;
      ItemMeleeWeaponModel meleeWeaponModel3 = new ItemMeleeWeaponModel(this.DATA_MELEE_CROWBAR.NAME, this.DATA_MELEE_CROWBAR.PLURAL, "Items\\item_crowbar", new Attack(AttackKind.PHYSICAL, new Verb("strike"), this.DATA_MELEE_CROWBAR.ATK, this.DATA_MELEE_CROWBAR.DMG, this.DATA_MELEE_CROWBAR.STA));
      meleeWeaponModel3.EquipmentPart = DollPart._FIRST;
      meleeWeaponModel3.IsStackable = true;
      meleeWeaponModel3.StackingLimit = this.DATA_MELEE_CROWBAR.STACKINGLIMIT;
      meleeWeaponModel3.FlavorDescription = this.DATA_MELEE_CROWBAR.FLAVOR;
      meleeWeaponModel3.IsFragile = this.DATA_MELEE_CROWBAR.ISFRAGILE;
      this[(GameItems.IDs) num12] = (ItemModel) meleeWeaponModel3;
      int num13 = 12;
      ItemMeleeWeaponModel meleeWeaponModel4 = new ItemMeleeWeaponModel(this.DATA_MELEE_UNIQUE_JASON_MYERS_AXE.NAME, this.DATA_MELEE_UNIQUE_JASON_MYERS_AXE.PLURAL, "Items\\item_jason_myers_axe", new Attack(AttackKind.PHYSICAL, new Verb("slash", "slashes"), this.DATA_MELEE_UNIQUE_JASON_MYERS_AXE.ATK, this.DATA_MELEE_UNIQUE_JASON_MYERS_AXE.DMG, this.DATA_MELEE_UNIQUE_JASON_MYERS_AXE.STA));
      meleeWeaponModel4.EquipmentPart = DollPart._FIRST;
      meleeWeaponModel4.IsProper = true;
      meleeWeaponModel4.FlavorDescription = this.DATA_MELEE_UNIQUE_JASON_MYERS_AXE.FLAVOR;
      meleeWeaponModel4.IsUnbreakable = true;
      this[(GameItems.IDs) num13] = (ItemModel) meleeWeaponModel4;
      int num14 = 15;
      ItemMeleeWeaponModel meleeWeaponModel5 = new ItemMeleeWeaponModel(this.DATA_MELEE_GOLFCLUB.NAME, this.DATA_MELEE_GOLFCLUB.PLURAL, "Items\\item_golfclub", new Attack(AttackKind.PHYSICAL, new Verb("strike"), this.DATA_MELEE_GOLFCLUB.ATK, this.DATA_MELEE_GOLFCLUB.DMG, this.DATA_MELEE_GOLFCLUB.STA));
      meleeWeaponModel5.EquipmentPart = DollPart._FIRST;
      meleeWeaponModel5.IsStackable = this.DATA_MELEE_GOLFCLUB.STACKINGLIMIT > 1;
      meleeWeaponModel5.StackingLimit = this.DATA_MELEE_GOLFCLUB.STACKINGLIMIT;
      meleeWeaponModel5.FlavorDescription = this.DATA_MELEE_GOLFCLUB.FLAVOR;
      meleeWeaponModel5.IsFragile = this.DATA_MELEE_GOLFCLUB.ISFRAGILE;
      this[(GameItems.IDs) num14] = (ItemModel) meleeWeaponModel5;
      int num15 = 16;
      ItemMeleeWeaponModel meleeWeaponModel6 = new ItemMeleeWeaponModel(this.DATA_MELEE_IRON_GOLFCLUB.NAME, this.DATA_MELEE_IRON_GOLFCLUB.PLURAL, "Items\\item_iron_golfclub", new Attack(AttackKind.PHYSICAL, new Verb("strike"), this.DATA_MELEE_IRON_GOLFCLUB.ATK, this.DATA_MELEE_IRON_GOLFCLUB.DMG, this.DATA_MELEE_IRON_GOLFCLUB.STA));
      meleeWeaponModel6.EquipmentPart = DollPart._FIRST;
      meleeWeaponModel6.IsStackable = this.DATA_MELEE_IRON_GOLFCLUB.STACKINGLIMIT > 1;
      meleeWeaponModel6.StackingLimit = this.DATA_MELEE_IRON_GOLFCLUB.STACKINGLIMIT;
      meleeWeaponModel6.FlavorDescription = this.DATA_MELEE_IRON_GOLFCLUB.FLAVOR;
      meleeWeaponModel6.IsFragile = this.DATA_MELEE_IRON_GOLFCLUB.ISFRAGILE;
      this[(GameItems.IDs) num15] = (ItemModel) meleeWeaponModel6;
      int num16 = 13;
      ItemMeleeWeaponModel meleeWeaponModel7 = new ItemMeleeWeaponModel(this.DATA_MELEE_HUGE_HAMMER.NAME, this.DATA_MELEE_HUGE_HAMMER.PLURAL, "Items\\item_huge_hammer", new Attack(AttackKind.PHYSICAL, new Verb("smash", "smashes"), this.DATA_MELEE_HUGE_HAMMER.ATK, this.DATA_MELEE_HUGE_HAMMER.DMG, this.DATA_MELEE_HUGE_HAMMER.STA));
      meleeWeaponModel7.EquipmentPart = DollPart._FIRST;
      meleeWeaponModel7.IsStackable = this.DATA_MELEE_HUGE_HAMMER.STACKINGLIMIT > 1;
      meleeWeaponModel7.StackingLimit = this.DATA_MELEE_HUGE_HAMMER.STACKINGLIMIT;
      meleeWeaponModel7.FlavorDescription = this.DATA_MELEE_HUGE_HAMMER.FLAVOR;
      meleeWeaponModel7.IsFragile = this.DATA_MELEE_HUGE_HAMMER.ISFRAGILE;
      this[(GameItems.IDs) num16] = (ItemModel) meleeWeaponModel7;
      int num17 = 17;
      ItemMeleeWeaponModel meleeWeaponModel8 = new ItemMeleeWeaponModel(this.DATA_MELEE_SHOVEL.NAME, this.DATA_MELEE_SHOVEL.PLURAL, "Items\\item_shovel", new Attack(AttackKind.PHYSICAL, new Verb("strike"), this.DATA_MELEE_SHOVEL.ATK, this.DATA_MELEE_SHOVEL.DMG, this.DATA_MELEE_SHOVEL.STA));
      meleeWeaponModel8.EquipmentPart = DollPart._FIRST;
      meleeWeaponModel8.IsStackable = this.DATA_MELEE_SHOVEL.STACKINGLIMIT > 1;
      meleeWeaponModel8.StackingLimit = this.DATA_MELEE_SHOVEL.STACKINGLIMIT;
      meleeWeaponModel8.FlavorDescription = this.DATA_MELEE_SHOVEL.FLAVOR;
      meleeWeaponModel8.IsFragile = this.DATA_MELEE_SHOVEL.ISFRAGILE;
      this[(GameItems.IDs) num17] = (ItemModel) meleeWeaponModel8;
      int num18 = 18;
      ItemMeleeWeaponModel meleeWeaponModel9 = new ItemMeleeWeaponModel(this.DATA_MELEE_SHORT_SHOVEL.NAME, this.DATA_MELEE_SHORT_SHOVEL.PLURAL, "Items\\item_short_shovel", new Attack(AttackKind.PHYSICAL, new Verb("strike"), this.DATA_MELEE_SHORT_SHOVEL.ATK, this.DATA_MELEE_SHORT_SHOVEL.DMG, this.DATA_MELEE_SHORT_SHOVEL.STA));
      meleeWeaponModel9.EquipmentPart = DollPart._FIRST;
      meleeWeaponModel9.IsStackable = this.DATA_MELEE_SHORT_SHOVEL.STACKINGLIMIT > 1;
      meleeWeaponModel9.StackingLimit = this.DATA_MELEE_SHORT_SHOVEL.STACKINGLIMIT;
      meleeWeaponModel9.FlavorDescription = this.DATA_MELEE_SHORT_SHOVEL.FLAVOR;
      meleeWeaponModel9.IsFragile = this.DATA_MELEE_SHORT_SHOVEL.ISFRAGILE;
      this[(GameItems.IDs) num18] = (ItemModel) meleeWeaponModel9;
      int num19 = 19;
      ItemMeleeWeaponModel meleeWeaponModel10 = new ItemMeleeWeaponModel(this.DATA_MELEE_TRUNCHEON.NAME, this.DATA_MELEE_TRUNCHEON.PLURAL, "Items\\item_truncheon", new Attack(AttackKind.PHYSICAL, new Verb("strike"), this.DATA_MELEE_TRUNCHEON.ATK, this.DATA_MELEE_TRUNCHEON.DMG, this.DATA_MELEE_TRUNCHEON.STA));
      meleeWeaponModel10.EquipmentPart = DollPart._FIRST;
      meleeWeaponModel10.IsStackable = true;
      meleeWeaponModel10.StackingLimit = this.DATA_MELEE_TRUNCHEON.STACKINGLIMIT;
      meleeWeaponModel10.FlavorDescription = this.DATA_MELEE_TRUNCHEON.FLAVOR;
      meleeWeaponModel10.IsFragile = this.DATA_MELEE_TRUNCHEON.ISFRAGILE;
      this[(GameItems.IDs) num19] = (ItemModel) meleeWeaponModel10;
      GameItems.MeleeWeaponData meleeImprovisedClub = this.DATA_MELEE_IMPROVISED_CLUB;
      int num20 = 20;
      ItemMeleeWeaponModel meleeWeaponModel11 = new ItemMeleeWeaponModel(meleeImprovisedClub.NAME, meleeImprovisedClub.PLURAL, "Items\\item_improvised_club", new Attack(AttackKind.PHYSICAL, new Verb("strike"), meleeImprovisedClub.ATK, meleeImprovisedClub.DMG, meleeImprovisedClub.STA));
      meleeWeaponModel11.EquipmentPart = DollPart._FIRST;
      meleeWeaponModel11.IsStackable = meleeImprovisedClub.STACKINGLIMIT > 1;
      meleeWeaponModel11.StackingLimit = meleeImprovisedClub.STACKINGLIMIT;
      meleeWeaponModel11.FlavorDescription = meleeImprovisedClub.FLAVOR;
      meleeWeaponModel11.IsFragile = meleeImprovisedClub.ISFRAGILE;
      this[(GameItems.IDs) num20] = (ItemModel) meleeWeaponModel11;
      GameItems.MeleeWeaponData meleeImprovisedSpear = this.DATA_MELEE_IMPROVISED_SPEAR;
      int num21 = 21;
      ItemMeleeWeaponModel meleeWeaponModel12 = new ItemMeleeWeaponModel(meleeImprovisedSpear.NAME, meleeImprovisedSpear.PLURAL, "Items\\item_improvised_spear", new Attack(AttackKind.PHYSICAL, new Verb("pierce"), meleeImprovisedSpear.ATK, meleeImprovisedSpear.DMG, meleeImprovisedSpear.STA));
      meleeWeaponModel12.EquipmentPart = DollPart._FIRST;
      meleeWeaponModel12.IsStackable = meleeImprovisedSpear.STACKINGLIMIT > 1;
      meleeWeaponModel12.StackingLimit = meleeImprovisedSpear.STACKINGLIMIT;
      meleeWeaponModel12.FlavorDescription = meleeImprovisedSpear.FLAVOR;
      meleeWeaponModel12.IsFragile = meleeImprovisedSpear.ISFRAGILE;
      this[(GameItems.IDs) num21] = (ItemModel) meleeWeaponModel12;
      GameItems.MeleeWeaponData meleeSmallHammer = this.DATA_MELEE_SMALL_HAMMER;
      int num22 = 14;
      ItemMeleeWeaponModel meleeWeaponModel13 = new ItemMeleeWeaponModel(meleeSmallHammer.NAME, meleeSmallHammer.PLURAL, "Items\\item_small_hammer", new Attack(AttackKind.PHYSICAL, new Verb("smash"), meleeSmallHammer.ATK, meleeSmallHammer.DMG, meleeSmallHammer.STA));
      meleeWeaponModel13.EquipmentPart = DollPart._FIRST;
      meleeWeaponModel13.IsStackable = meleeSmallHammer.STACKINGLIMIT > 1;
      meleeWeaponModel13.StackingLimit = meleeSmallHammer.STACKINGLIMIT;
      meleeWeaponModel13.FlavorDescription = meleeSmallHammer.FLAVOR;
      meleeWeaponModel13.IsFragile = meleeSmallHammer.ISFRAGILE;
      this[(GameItems.IDs) num22] = (ItemModel) meleeWeaponModel13;
      GameItems.MeleeWeaponData famuFataruKatana = this.DATA_MELEE_UNIQUE_FAMU_FATARU_KATANA;
      int num23 = 64;
      ItemMeleeWeaponModel meleeWeaponModel14 = new ItemMeleeWeaponModel(famuFataruKatana.NAME, famuFataruKatana.PLURAL, "Items\\item_famu_fataru_katana", new Attack(AttackKind.PHYSICAL, new Verb("slash", "slashes"), famuFataruKatana.ATK, famuFataruKatana.DMG, famuFataruKatana.STA));
      meleeWeaponModel14.EquipmentPart = DollPart._FIRST;
      meleeWeaponModel14.FlavorDescription = famuFataruKatana.FLAVOR;
      meleeWeaponModel14.IsProper = true;
      meleeWeaponModel14.IsUnbreakable = true;
      this[(GameItems.IDs) num23] = (ItemModel) meleeWeaponModel14;
      GameItems.MeleeWeaponData uniqueBigbearBat = this.DATA_MELEE_UNIQUE_BIGBEAR_BAT;
      int num24 = 65;
      ItemMeleeWeaponModel meleeWeaponModel15 = new ItemMeleeWeaponModel(uniqueBigbearBat.NAME, uniqueBigbearBat.PLURAL, "Items\\item_bigbear_bat", new Attack(AttackKind.PHYSICAL, new Verb("smash", "smashes"), uniqueBigbearBat.ATK, uniqueBigbearBat.DMG, uniqueBigbearBat.STA));
      meleeWeaponModel15.EquipmentPart = DollPart._FIRST;
      meleeWeaponModel15.FlavorDescription = uniqueBigbearBat.FLAVOR;
      meleeWeaponModel15.IsProper = true;
      meleeWeaponModel15.IsUnbreakable = true;
      this[(GameItems.IDs) num24] = (ItemModel) meleeWeaponModel15;
      GameItems.MeleeWeaponData roguedjackKeyboard = this.DATA_MELEE_UNIQUE_ROGUEDJACK_KEYBOARD;
      int num25 = 66;
      ItemMeleeWeaponModel meleeWeaponModel16 = new ItemMeleeWeaponModel(roguedjackKeyboard.NAME, roguedjackKeyboard.PLURAL, "Items\\item_roguedjack_keyboard", new Attack(AttackKind.PHYSICAL, new Verb("bash", "bashes"), roguedjackKeyboard.ATK, roguedjackKeyboard.DMG, roguedjackKeyboard.STA));
      meleeWeaponModel16.EquipmentPart = DollPart._FIRST;
      meleeWeaponModel16.FlavorDescription = roguedjackKeyboard.FLAVOR;
      meleeWeaponModel16.IsProper = true;
      meleeWeaponModel16.IsUnbreakable = true;
      this[(GameItems.IDs) num25] = (ItemModel) meleeWeaponModel16;
      GameItems.RangedWeaponData rangedArmyPistol = this.DATA_RANGED_ARMY_PISTOL;
      int num26 = 22;
      ItemRangedWeaponModel rangedWeaponModel1 = new ItemRangedWeaponModel(rangedArmyPistol.NAME, rangedArmyPistol.FLAVOR, "Items\\item_army_pistol", new Attack(AttackKind.FIREARM, new Verb("shoot"), rangedArmyPistol.ATK, rangedArmyPistol.DMG, 0, rangedArmyPistol.RANGE), rangedArmyPistol.MAXAMMO, AmmoType.HEAVY_PISTOL);
      rangedWeaponModel1.EquipmentPart = DollPart._FIRST;
      rangedWeaponModel1.FlavorDescription = rangedArmyPistol.FLAVOR;
      rangedWeaponModel1.IsAn = true;
      this[(GameItems.IDs) num26] = (ItemModel) rangedWeaponModel1;
      GameItems.RangedWeaponData dataRangedArmyRifle = this.DATA_RANGED_ARMY_RIFLE;
      int num27 = 23;
      ItemRangedWeaponModel rangedWeaponModel2 = new ItemRangedWeaponModel(dataRangedArmyRifle.NAME, dataRangedArmyRifle.FLAVOR, "Items\\item_army_rifle", new Attack(AttackKind.FIREARM, new Verb("fire a salvo at", "fires a salvo at"), dataRangedArmyRifle.ATK, dataRangedArmyRifle.DMG, 0, dataRangedArmyRifle.RANGE), dataRangedArmyRifle.MAXAMMO, AmmoType.HEAVY_RIFLE);
      rangedWeaponModel2.EquipmentPart = DollPart._FIRST;
      rangedWeaponModel2.FlavorDescription = dataRangedArmyRifle.FLAVOR;
      rangedWeaponModel2.IsAn = true;
      this[(GameItems.IDs) num27] = (ItemModel) rangedWeaponModel2;
      GameItems.RangedWeaponData rangedHuntingCrossbow = this.DATA_RANGED_HUNTING_CROSSBOW;
      int num28 = 24;
      ItemRangedWeaponModel rangedWeaponModel3 = new ItemRangedWeaponModel(rangedHuntingCrossbow.NAME, rangedHuntingCrossbow.FLAVOR, "Items\\item_hunting_crossbow", new Attack(AttackKind.BOW, new Verb("shoot"), rangedHuntingCrossbow.ATK, rangedHuntingCrossbow.DMG, 0, rangedHuntingCrossbow.RANGE), rangedHuntingCrossbow.MAXAMMO, AmmoType.BOLT);
      rangedWeaponModel3.EquipmentPart = DollPart._FIRST;
      rangedWeaponModel3.FlavorDescription = rangedHuntingCrossbow.FLAVOR;
      this[(GameItems.IDs) num28] = (ItemModel) rangedWeaponModel3;
      GameItems.RangedWeaponData rangedHuntingRifle = this.DATA_RANGED_HUNTING_RIFLE;
      int num29 = 25;
      ItemRangedWeaponModel rangedWeaponModel4 = new ItemRangedWeaponModel(rangedHuntingRifle.NAME, rangedHuntingRifle.FLAVOR, "Items\\item_hunting_rifle", new Attack(AttackKind.FIREARM, new Verb("shoot"), rangedHuntingRifle.ATK, rangedHuntingRifle.DMG, 0, rangedHuntingRifle.RANGE), rangedHuntingRifle.MAXAMMO, AmmoType.LIGHT_RIFLE);
      rangedWeaponModel4.EquipmentPart = DollPart._FIRST;
      rangedWeaponModel4.FlavorDescription = rangedHuntingRifle.FLAVOR;
      this[(GameItems.IDs) num29] = (ItemModel) rangedWeaponModel4;
      GameItems.RangedWeaponData dataRangedPistol = this.DATA_RANGED_PISTOL;
      int num30 = 26;
      ItemRangedWeaponModel rangedWeaponModel5 = new ItemRangedWeaponModel(dataRangedPistol.NAME, dataRangedPistol.FLAVOR, "Items\\item_pistol", new Attack(AttackKind.FIREARM, new Verb("shoot"), dataRangedPistol.ATK, dataRangedPistol.DMG, 0, dataRangedPistol.RANGE), dataRangedPistol.MAXAMMO, AmmoType._FIRST);
      rangedWeaponModel5.EquipmentPart = DollPart._FIRST;
      rangedWeaponModel5.FlavorDescription = dataRangedPistol.FLAVOR;
      this[(GameItems.IDs) num30] = (ItemModel) rangedWeaponModel5;
      GameItems.RangedWeaponData rangedKoltRevolver = this.DATA_RANGED_KOLT_REVOLVER;
      int num31 = 27;
      ItemRangedWeaponModel rangedWeaponModel6 = new ItemRangedWeaponModel(rangedKoltRevolver.NAME, rangedKoltRevolver.FLAVOR, "Items\\item_kolt_revolver", new Attack(AttackKind.FIREARM, new Verb("shoot"), rangedKoltRevolver.ATK, rangedKoltRevolver.DMG, 0, rangedKoltRevolver.RANGE), rangedKoltRevolver.MAXAMMO, AmmoType._FIRST);
      rangedWeaponModel6.EquipmentPart = DollPart._FIRST;
      rangedWeaponModel6.FlavorDescription = rangedKoltRevolver.FLAVOR;
      this[(GameItems.IDs) num31] = (ItemModel) rangedWeaponModel6;
      GameItems.RangedWeaponData rangedPrecisionRifle = this.DATA_RANGED_PRECISION_RIFLE;
      int num32 = 28;
      ItemRangedWeaponModel rangedWeaponModel7 = new ItemRangedWeaponModel(rangedPrecisionRifle.NAME, rangedPrecisionRifle.FLAVOR, "Items\\item_precision_rifle", new Attack(AttackKind.FIREARM, new Verb("shoot"), rangedPrecisionRifle.ATK, rangedPrecisionRifle.DMG, 0, rangedPrecisionRifle.RANGE), rangedPrecisionRifle.MAXAMMO, AmmoType.HEAVY_RIFLE);
      rangedWeaponModel7.EquipmentPart = DollPart._FIRST;
      rangedWeaponModel7.FlavorDescription = rangedPrecisionRifle.FLAVOR;
      this[(GameItems.IDs) num32] = (ItemModel) rangedWeaponModel7;
      GameItems.RangedWeaponData dataRangedShotgun = this.DATA_RANGED_SHOTGUN;
      int num33 = 29;
      ItemRangedWeaponModel rangedWeaponModel8 = new ItemRangedWeaponModel(dataRangedShotgun.NAME, dataRangedShotgun.FLAVOR, "Items\\item_shotgun", new Attack(AttackKind.FIREARM, new Verb("shoot"), dataRangedShotgun.ATK, dataRangedShotgun.DMG, 0, dataRangedShotgun.RANGE), dataRangedShotgun.MAXAMMO, AmmoType.SHOTGUN);
      rangedWeaponModel8.EquipmentPart = DollPart._FIRST;
      rangedWeaponModel8.FlavorDescription = dataRangedShotgun.FLAVOR;
      this[(GameItems.IDs) num33] = (ItemModel) rangedWeaponModel8;
      GameItems.RangedWeaponData uniqueSantamanShotgun = this.DATA_UNIQUE_SANTAMAN_SHOTGUN;
      int num34 = 67;
      ItemRangedWeaponModel rangedWeaponModel9 = new ItemRangedWeaponModel(uniqueSantamanShotgun.NAME, uniqueSantamanShotgun.FLAVOR, "Items\\item_santaman_shotgun", new Attack(AttackKind.FIREARM, new Verb("shoot"), uniqueSantamanShotgun.ATK, uniqueSantamanShotgun.DMG, 0, uniqueSantamanShotgun.RANGE), uniqueSantamanShotgun.MAXAMMO, AmmoType.SHOTGUN);
      rangedWeaponModel9.EquipmentPart = DollPart._FIRST;
      rangedWeaponModel9.FlavorDescription = uniqueSantamanShotgun.FLAVOR;
      rangedWeaponModel9.IsProper = true;
      rangedWeaponModel9.IsUnbreakable = true;
      this[(GameItems.IDs) num34] = (ItemModel) rangedWeaponModel9;
      GameItems.RangedWeaponData hansVonHanzPistol = this.DATA_UNIQUE_HANS_VON_HANZ_PISTOL;
      int num35 = 68;
      ItemRangedWeaponModel rangedWeaponModel10 = new ItemRangedWeaponModel(hansVonHanzPistol.NAME, hansVonHanzPistol.FLAVOR, "Items\\item_hans_von_hanz_pistol", new Attack(AttackKind.FIREARM, new Verb("shoot"), hansVonHanzPistol.ATK, hansVonHanzPistol.DMG, 0, hansVonHanzPistol.RANGE), hansVonHanzPistol.MAXAMMO, AmmoType._FIRST);
      rangedWeaponModel10.EquipmentPart = DollPart._FIRST;
      rangedWeaponModel10.FlavorDescription = hansVonHanzPistol.FLAVOR;
      rangedWeaponModel10.IsProper = true;
      rangedWeaponModel10.IsUnbreakable = true;
      this[(GameItems.IDs) num35] = (ItemModel) rangedWeaponModel10;
      int num36 = 51;
      ItemAmmoModel itemAmmoModel1 = new ItemAmmoModel("light pistol bullets", "light pistol bullets", "Items\\item_ammo_light_pistol", AmmoType._FIRST, 20);
      itemAmmoModel1.IsPlural = true;
      itemAmmoModel1.FlavorDescription = "";
      this[(GameItems.IDs) num36] = (ItemModel) itemAmmoModel1;
      int num37 = 52;
      ItemAmmoModel itemAmmoModel2 = new ItemAmmoModel("heavy pistol bullets", "heavy pistol bullets", "Items\\item_ammo_heavy_pistol", AmmoType.HEAVY_PISTOL, 12);
      itemAmmoModel2.IsPlural = true;
      itemAmmoModel2.FlavorDescription = "";
      this[(GameItems.IDs) num37] = (ItemModel) itemAmmoModel2;
      int num38 = 53;
      ItemAmmoModel itemAmmoModel3 = new ItemAmmoModel("light rifle bullets", "light rifle bullets", "Items\\item_ammo_light_rifle", AmmoType.LIGHT_RIFLE, 14);
      itemAmmoModel3.IsPlural = true;
      itemAmmoModel3.FlavorDescription = "";
      this[(GameItems.IDs) num38] = (ItemModel) itemAmmoModel3;
      int num39 = 54;
      ItemAmmoModel itemAmmoModel4 = new ItemAmmoModel("heavy rifle bullets", "heavy rifle bullets", "Items\\item_ammo_heavy_rifle", AmmoType.HEAVY_RIFLE, 20);
      itemAmmoModel4.IsPlural = true;
      itemAmmoModel4.FlavorDescription = "";
      this[(GameItems.IDs) num39] = (ItemModel) itemAmmoModel4;
      int num40 = 55;
      ItemAmmoModel itemAmmoModel5 = new ItemAmmoModel("shotgun shells", "shotgun shells", "Items\\item_ammo_shotgun", AmmoType.SHOTGUN, 10);
      itemAmmoModel5.IsPlural = true;
      itemAmmoModel5.FlavorDescription = "";
      this[(GameItems.IDs) num40] = (ItemModel) itemAmmoModel5;
      int num41 = 56;
      ItemAmmoModel itemAmmoModel6 = new ItemAmmoModel("crossbow bolts", "crossbow bolts", "Items\\item_ammo_bolts", AmmoType.BOLT, 30);
      itemAmmoModel6.IsPlural = true;
      itemAmmoModel6.FlavorDescription = "";
      this[(GameItems.IDs) num41] = (ItemModel) itemAmmoModel6;
      GameItems.ExplosiveData explosiveGrenade = this.DATA_EXPLOSIVE_GRENADE;
      int[] damage = new int[explosiveGrenade.RADIUS + 1];
      for (int index = 0; index < explosiveGrenade.RADIUS + 1; ++index)
        damage[index] = explosiveGrenade.DMG[index];
      int num42 = 30;
      ItemGrenadeModel itemGrenadeModel = new ItemGrenadeModel(explosiveGrenade.NAME, explosiveGrenade.PLURAL, "Items\\item_grenade", explosiveGrenade.FUSE, new BlastAttack(explosiveGrenade.RADIUS, damage, true, false), "Icons\\blast", explosiveGrenade.MAXTHROW);
      itemGrenadeModel.EquipmentPart = DollPart._FIRST;
      itemGrenadeModel.IsStackable = true;
      itemGrenadeModel.StackingLimit = explosiveGrenade.STACKLINGLIMIT;
      itemGrenadeModel.FlavorDescription = explosiveGrenade.FLAVOR;
      this[(GameItems.IDs) num42] = (ItemModel) itemGrenadeModel;
      int num43 = 31;
      ItemGrenadePrimedModel grenadePrimedModel = new ItemGrenadePrimedModel("primed " + explosiveGrenade.NAME, "primed " + explosiveGrenade.PLURAL, "Items\\item_grenade_primed", this[GameItems.IDs.EXPLOSIVE_GRENADE] as ItemGrenadeModel);
      grenadePrimedModel.EquipmentPart = DollPart._FIRST;
      this[(GameItems.IDs) num43] = (ItemModel) grenadePrimedModel;
      GameItems.BarricadingMaterialData dataBarWoodenPlank = this.DATA_BAR_WOODEN_PLANK;
      int num44 = 32;
      ItemBarricadeMaterialModel barricadeMaterialModel = new ItemBarricadeMaterialModel(dataBarWoodenPlank.NAME, dataBarWoodenPlank.PLURAL, "Items\\item_wooden_plank", dataBarWoodenPlank.VALUE);
      barricadeMaterialModel.IsStackable = dataBarWoodenPlank.STACKINGLIMIT > 1;
      barricadeMaterialModel.StackingLimit = dataBarWoodenPlank.STACKINGLIMIT;
      barricadeMaterialModel.FlavorDescription = dataBarWoodenPlank.FLAVOR;
      this[(GameItems.IDs) num44] = (ItemModel) barricadeMaterialModel;
      GameItems.ArmorData dataArmorArmy = this.DATA_ARMOR_ARMY;
      int num45 = 33;
      ItemBodyArmorModel itemBodyArmorModel1 = new ItemBodyArmorModel(dataArmorArmy.NAME, dataArmorArmy.PLURAL, "Items\\item_army_bodyarmor", dataArmorArmy.PRO_HIT, dataArmorArmy.PRO_SHOT, dataArmorArmy.ENC, dataArmorArmy.WEIGHT);
      itemBodyArmorModel1.EquipmentPart = DollPart.TORSO;
      itemBodyArmorModel1.FlavorDescription = dataArmorArmy.FLAVOR;
      itemBodyArmorModel1.IsAn = this.StartsWithVowel(dataArmorArmy.NAME);
      this[(GameItems.IDs) num45] = (ItemModel) itemBodyArmorModel1;
      GameItems.ArmorData dataArmorChar = this.DATA_ARMOR_CHAR;
      int num46 = 34;
      ItemBodyArmorModel itemBodyArmorModel2 = new ItemBodyArmorModel(dataArmorChar.NAME, dataArmorChar.PLURAL, "Items\\item_CHAR_light_bodyarmor", dataArmorChar.PRO_HIT, dataArmorChar.PRO_SHOT, dataArmorChar.ENC, dataArmorChar.WEIGHT);
      itemBodyArmorModel2.EquipmentPart = DollPart.TORSO;
      itemBodyArmorModel2.FlavorDescription = dataArmorChar.FLAVOR;
      itemBodyArmorModel2.IsAn = this.StartsWithVowel(dataArmorChar.NAME);
      this[(GameItems.IDs) num46] = (ItemModel) itemBodyArmorModel2;
      GameItems.ArmorData armorData = this.DATA_ARMOR_HELLS_SOULS_JACKET;
      int num47 = 35;
      ItemBodyArmorModel itemBodyArmorModel3 = new ItemBodyArmorModel(armorData.NAME, armorData.PLURAL, "Items\\item_hells_souls_jacket", armorData.PRO_HIT, armorData.PRO_SHOT, armorData.ENC, armorData.WEIGHT);
      itemBodyArmorModel3.EquipmentPart = DollPart.TORSO;
      itemBodyArmorModel3.FlavorDescription = armorData.FLAVOR;
      itemBodyArmorModel3.IsAn = this.StartsWithVowel(armorData.NAME);
      this[(GameItems.IDs) num47] = (ItemModel) itemBodyArmorModel3;
      armorData = this.DATA_ARMOR_FREE_ANGELS_JACKET;
      int num48 = 36;
      ItemBodyArmorModel itemBodyArmorModel4 = new ItemBodyArmorModel(armorData.NAME, armorData.PLURAL, "Items\\item_free_angels_jacket", armorData.PRO_HIT, armorData.PRO_SHOT, armorData.ENC, armorData.WEIGHT);
      itemBodyArmorModel4.EquipmentPart = DollPart.TORSO;
      itemBodyArmorModel4.FlavorDescription = armorData.FLAVOR;
      itemBodyArmorModel4.IsAn = this.StartsWithVowel(armorData.NAME);
      this[(GameItems.IDs) num48] = (ItemModel) itemBodyArmorModel4;
      armorData = this.DATA_ARMOR_POLICE_JACKET;
      int num49 = 37;
      ItemBodyArmorModel itemBodyArmorModel5 = new ItemBodyArmorModel(armorData.NAME, armorData.PLURAL, "Items\\item_police_jacket", armorData.PRO_HIT, armorData.PRO_SHOT, armorData.ENC, armorData.WEIGHT);
      itemBodyArmorModel5.EquipmentPart = DollPart.TORSO;
      itemBodyArmorModel5.FlavorDescription = armorData.FLAVOR;
      itemBodyArmorModel5.IsAn = this.StartsWithVowel(armorData.NAME);
      this[(GameItems.IDs) num49] = (ItemModel) itemBodyArmorModel5;
      armorData = this.DATA_ARMOR_POLICE_RIOT;
      int num50 = 38;
      ItemBodyArmorModel itemBodyArmorModel6 = new ItemBodyArmorModel(armorData.NAME, armorData.PLURAL, "Items\\item_police_riot_armor", armorData.PRO_HIT, armorData.PRO_SHOT, armorData.ENC, armorData.WEIGHT);
      itemBodyArmorModel6.EquipmentPart = DollPart.TORSO;
      itemBodyArmorModel6.FlavorDescription = armorData.FLAVOR;
      itemBodyArmorModel6.IsAn = this.StartsWithVowel(armorData.NAME);
      this[(GameItems.IDs) num50] = (ItemModel) itemBodyArmorModel6;
      armorData = this.DATA_ARMOR_HUNTER_VEST;
      int num51 = 39;
      ItemBodyArmorModel itemBodyArmorModel7 = new ItemBodyArmorModel(armorData.NAME, armorData.PLURAL, "Items\\item_hunter_vest", armorData.PRO_HIT, armorData.PRO_SHOT, armorData.ENC, armorData.WEIGHT);
      itemBodyArmorModel7.EquipmentPart = DollPart.TORSO;
      itemBodyArmorModel7.FlavorDescription = armorData.FLAVOR;
      itemBodyArmorModel7.IsAn = this.StartsWithVowel(armorData.NAME);
      this[(GameItems.IDs) num51] = (ItemModel) itemBodyArmorModel7;
      GameItems.TrackerData trackerData = this.DATA_TRACKER_CELL_PHONE;
      int num52 = 41;
      ItemTrackerModel itemTrackerModel1 = new ItemTrackerModel(trackerData.NAME, trackerData.PLURAL, "Items\\item_cellphone", ItemTrackerModel.TrackingFlags.FOLLOWER_AND_LEADER, trackerData.BATTERIES * 30);
      itemTrackerModel1.EquipmentPart = DollPart.LEFT_HAND;
      itemTrackerModel1.FlavorDescription = trackerData.FLAVOR;
      this[(GameItems.IDs) num52] = (ItemModel) itemTrackerModel1;
      trackerData = this.DATA_TRACKER_ZTRACKER;
      int num53 = 42;
      ItemTrackerModel itemTrackerModel2 = new ItemTrackerModel(trackerData.NAME, trackerData.PLURAL, "Items\\item_ztracker", ItemTrackerModel.TrackingFlags.UNDEADS, trackerData.BATTERIES * 30);
      itemTrackerModel2.EquipmentPart = DollPart.LEFT_HAND;
      itemTrackerModel2.FlavorDescription = trackerData.FLAVOR;
      this[(GameItems.IDs) num53] = (ItemModel) itemTrackerModel2;
      trackerData = this.DATA_TRACKER_BLACKOPS_GPS;
      int num54 = 40;
      ItemTrackerModel itemTrackerModel3 = new ItemTrackerModel(trackerData.NAME, trackerData.PLURAL, "Items\\item_blackops_gps", ItemTrackerModel.TrackingFlags.BLACKOPS_FACTION, trackerData.BATTERIES * 30);
      itemTrackerModel3.EquipmentPart = DollPart.LEFT_HAND;
      itemTrackerModel3.FlavorDescription = trackerData.FLAVOR;
      this[(GameItems.IDs) num54] = (ItemModel) itemTrackerModel3;
      trackerData = this.DATA_TRACKER_POLICE_RADIO;
      int num55 = 43;
      ItemTrackerModel itemTrackerModel4 = new ItemTrackerModel(trackerData.NAME, trackerData.PLURAL, "Items\\item_police_radio", ItemTrackerModel.TrackingFlags.POLICE_FACTION, trackerData.BATTERIES * 30);
      itemTrackerModel4.EquipmentPart = DollPart.LEFT_HAND;
      itemTrackerModel4.FlavorDescription = trackerData.FLAVOR;
      this[(GameItems.IDs) num55] = (ItemModel) itemTrackerModel4;
      GameItems.SprayPaintData sprayPaintData = this.DATA_SPRAY_PAINT1;
      int num56 = 44;
      ItemSprayPaintModel itemSprayPaintModel1 = new ItemSprayPaintModel(sprayPaintData.NAME, sprayPaintData.PLURAL, "Items\\item_spraypaint", sprayPaintData.QUANTITY, "Tiles\\Decoration\\player_tag");
      itemSprayPaintModel1.EquipmentPart = DollPart.LEFT_HAND;
      itemSprayPaintModel1.FlavorDescription = sprayPaintData.FLAVOR;
      this[(GameItems.IDs) num56] = (ItemModel) itemSprayPaintModel1;
      sprayPaintData = this.DATA_SPRAY_PAINT2;
      int num57 = 45;
      ItemSprayPaintModel itemSprayPaintModel2 = new ItemSprayPaintModel(sprayPaintData.NAME, sprayPaintData.PLURAL, "Items\\item_spraypaint2", sprayPaintData.QUANTITY, "Tiles\\Decoration\\player_tag2");
      itemSprayPaintModel2.EquipmentPart = DollPart.LEFT_HAND;
      itemSprayPaintModel2.FlavorDescription = sprayPaintData.FLAVOR;
      this[(GameItems.IDs) num57] = (ItemModel) itemSprayPaintModel2;
      sprayPaintData = this.DATA_SPRAY_PAINT3;
      int num58 = 46;
      ItemSprayPaintModel itemSprayPaintModel3 = new ItemSprayPaintModel(sprayPaintData.NAME, sprayPaintData.PLURAL, "Items\\item_spraypaint3", sprayPaintData.QUANTITY, "Tiles\\Decoration\\player_tag3");
      itemSprayPaintModel3.EquipmentPart = DollPart.LEFT_HAND;
      itemSprayPaintModel3.FlavorDescription = sprayPaintData.FLAVOR;
      this[(GameItems.IDs) num58] = (ItemModel) itemSprayPaintModel3;
      sprayPaintData = this.DATA_SPRAY_PAINT4;
      int num59 = 47;
      ItemSprayPaintModel itemSprayPaintModel4 = new ItemSprayPaintModel(sprayPaintData.NAME, sprayPaintData.PLURAL, "Items\\item_spraypaint4", sprayPaintData.QUANTITY, "Tiles\\Decoration\\player_tag4");
      itemSprayPaintModel4.EquipmentPart = DollPart.LEFT_HAND;
      itemSprayPaintModel4.FlavorDescription = sprayPaintData.FLAVOR;
      this[(GameItems.IDs) num59] = (ItemModel) itemSprayPaintModel4;
      GameItems.LightData lightData = this.DATA_LIGHT_FLASHLIGHT;
      int num60 = 49;
      ItemLightModel itemLightModel1 = new ItemLightModel(lightData.NAME, lightData.PLURAL, "Items\\item_flashlight", lightData.FOV, lightData.BATTERIES * 30, "Items\\item_flashlight_out");
      itemLightModel1.EquipmentPart = DollPart.LEFT_HAND;
      itemLightModel1.FlavorDescription = lightData.FLAVOR;
      this[(GameItems.IDs) num60] = (ItemModel) itemLightModel1;
      lightData = this.DATA_LIGHT_BIG_FLASHLIGHT;
      int num61 = 50;
      ItemLightModel itemLightModel2 = new ItemLightModel(lightData.NAME, lightData.PLURAL, "Items\\item_big_flashlight", lightData.FOV, lightData.BATTERIES * 30, "Items\\item_big_flashlight_out");
      itemLightModel2.EquipmentPart = DollPart.LEFT_HAND;
      itemLightModel2.FlavorDescription = lightData.FLAVOR;
      this[(GameItems.IDs) num61] = (ItemModel) itemLightModel2;
      GameItems.ScentSprayData sprayStenchKiller = this.DATA_SCENT_SPRAY_STENCH_KILLER;
      int num62 = 48;
      ItemSprayScentModel itemSprayScentModel = new ItemSprayScentModel(sprayStenchKiller.NAME, sprayStenchKiller.PLURAL, "Items\\item_stench_killer", sprayStenchKiller.QUANTITY, Odor.PERFUME_LIVING_SUPRESSOR, sprayStenchKiller.STRENGTH * 30);
      itemSprayScentModel.EquipmentPart = DollPart.LEFT_HAND;
      itemSprayScentModel.FlavorDescription = sprayStenchKiller.FLAVOR;
      this[(GameItems.IDs) num62] = (ItemModel) itemSprayScentModel;
      GameItems.TrapData trapData1 = this.DATA_TRAP_EMPTY_CAN;
      int num63 = 57;
      ItemTrapModel itemTrapModel1 = new ItemTrapModel(trapData1.NAME, trapData1.PLURAL, "Items\\item_empty_can", trapData1.STACKING, trapData1.CHANCE, trapData1.DAMAGE, trapData1.DROP_ACTIVATE, trapData1.USE_ACTIVATE, trapData1.IS_ONE_TIME, trapData1.BREAK_CHANCE, trapData1.BLOCK_CHANCE, trapData1.BREAK_CHANCE_ESCAPE, trapData1.IS_NOISY, trapData1.NOISE_NAME, trapData1.IS_FLAMMABLE);
      itemTrapModel1.FlavorDescription = trapData1.FLAVOR;
      this[(GameItems.IDs) num63] = (ItemModel) itemTrapModel1;
      trapData1 = this.DATA_TRAP_BEAR_TRAP;
      int num64 = 58;
      ItemTrapModel itemTrapModel2 = new ItemTrapModel(trapData1.NAME, trapData1.PLURAL, "Items\\item_bear_trap", trapData1.STACKING, trapData1.CHANCE, trapData1.DAMAGE, trapData1.DROP_ACTIVATE, trapData1.USE_ACTIVATE, trapData1.IS_ONE_TIME, trapData1.BREAK_CHANCE, trapData1.BLOCK_CHANCE, trapData1.BREAK_CHANCE_ESCAPE, trapData1.IS_NOISY, trapData1.NOISE_NAME, trapData1.IS_FLAMMABLE);
      itemTrapModel2.FlavorDescription = trapData1.FLAVOR;
      this[(GameItems.IDs) num64] = (ItemModel) itemTrapModel2;
      GameItems.TrapData trapData2 = this.DATA_TRAP_SPIKES;
      int num65 = 59;
      ItemTrapModel itemTrapModel3 = new ItemTrapModel(trapData2.NAME, trapData2.PLURAL, "Items\\item_spikes", trapData2.STACKING, trapData2.CHANCE, trapData2.DAMAGE, trapData2.DROP_ACTIVATE, trapData2.USE_ACTIVATE, trapData2.IS_ONE_TIME, trapData2.BREAK_CHANCE, trapData2.BLOCK_CHANCE, trapData2.BREAK_CHANCE_ESCAPE, trapData2.IS_NOISY, trapData2.NOISE_NAME, trapData2.IS_FLAMMABLE);
      itemTrapModel3.FlavorDescription = trapData2.FLAVOR;
      this[(GameItems.IDs) num65] = (ItemModel) itemTrapModel3;
      trapData2 = this.DATA_TRAP_BARBED_WIRE;
      int num66 = 60;
      ItemTrapModel itemTrapModel4 = new ItemTrapModel(trapData2.NAME, trapData2.PLURAL, "Items\\item_barbed_wire", trapData2.STACKING, trapData2.CHANCE, trapData2.DAMAGE, trapData2.DROP_ACTIVATE, trapData2.USE_ACTIVATE, trapData2.IS_ONE_TIME, trapData2.BREAK_CHANCE, trapData2.BLOCK_CHANCE, trapData2.BREAK_CHANCE_ESCAPE, trapData2.IS_NOISY, trapData2.NOISE_NAME, trapData2.IS_FLAMMABLE);
      itemTrapModel4.FlavorDescription = trapData2.FLAVOR;
      this[(GameItems.IDs) num66] = (ItemModel) itemTrapModel4;
      GameItems.EntData entData = this.DATA_ENT_BOOK;
      int num67 = 61;
      ItemEntertainmentModel entertainmentModel1 = new ItemEntertainmentModel(entData.NAME, entData.PLURAL, "Items\\item_book", entData.VALUE, entData.BORECHANCE);
      entertainmentModel1.StackingLimit = entData.STACKING;
      entertainmentModel1.FlavorDescription = entData.FLAVOR;
      this[(GameItems.IDs) num67] = (ItemModel) entertainmentModel1;
      entData = this.DATA_ENT_MAGAZINE;
      int num68 = 62;
      ItemEntertainmentModel entertainmentModel2 = new ItemEntertainmentModel(entData.NAME, entData.PLURAL, "Items\\item_magazine", entData.VALUE, entData.BORECHANCE);
      entertainmentModel2.StackingLimit = entData.STACKING;
      entertainmentModel2.FlavorDescription = entData.FLAVOR;
      this[(GameItems.IDs) num68] = (ItemModel) entertainmentModel2;
      this[GameItems.IDs.UNIQUE_SUBWAY_BADGE] = new ItemModel("Subway Worker Badge", "Subways Worker Badges", "Items\\item_subway_badge")
      {
        DontAutoEquip = true,
        EquipmentPart = DollPart.LEFT_HAND,
        FlavorDescription = "You got yourself a new job!"
      };
      for (int index = 0; index < 69; ++index)
      {
        ItemModel itemModel = this[index];
        itemModel.IsAn = this.StartsWithVowel(itemModel.SingleName);
        itemModel.IsStackable = itemModel.StackingLimit > 1;
      }
    }

    private void Notify(IRogueUI ui, string what, string stage)
    {
      ui.UI_Clear(Color.Black);
      ui.UI_DrawStringBold(Color.White, "Loading " + what + " data : " + stage, 0, 0, new Color?());
      ui.UI_Repaint();
    }

    private CSVLine FindLineForModel(CSVTable table, GameItems.IDs modelID)
    {
      foreach (CSVLine line in table.Lines)
      {
        if (line[0].ParseText() == modelID.ToString())
          return line;
      }
      return (CSVLine) null;
    }

    private _DATA_TYPE_ GetDataFromCSVTable<_DATA_TYPE_>(IRogueUI ui, CSVTable table, Func<CSVLine, _DATA_TYPE_> fn, GameItems.IDs modelID)
    {
      CSVLine lineForModel = this.FindLineForModel(table, modelID);
      if (lineForModel == null)
        throw new InvalidOperationException(string.Format("model {0} not found", (object) modelID.ToString()));
      try
      {
        return fn(lineForModel);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException(string.Format("invalid data format for model {0}; exception : {1}", (object) modelID.ToString(), (object) ex.ToString()));
      }
    }

    private bool LoadDataFromCSV<_DATA_TYPE_>(IRogueUI ui, string path, string kind, int fieldsCount, Func<CSVLine, _DATA_TYPE_> fn, GameItems.IDs[] idsToRead, out _DATA_TYPE_[] data)
    {
      this.Notify(ui, kind, "loading file...");
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
      this.Notify(ui, kind, "parsing CSV...");
      CSVTable toTable = new CSVParser().ParseToTable(stringList.ToArray(), fieldsCount);
      this.Notify(ui, kind, "reading data...");
      data = new _DATA_TYPE_[idsToRead.Length];
      for (int index = 0; index < idsToRead.Length; ++index)
        data[index] = this.GetDataFromCSVTable<_DATA_TYPE_>(ui, toTable, fn, idsToRead[index]);
      this.Notify(ui, kind, "done!");
      return true;
    }

    public bool LoadMedicineFromCSV(IRogueUI ui, string path)
    {
      GameItems.MedecineData[] data;
      this.LoadDataFromCSV<GameItems.MedecineData>(ui, path, "medicine items", 10, new Func<CSVLine, GameItems.MedecineData>(GameItems.MedecineData.FromCSVLine), new GameItems.IDs[6]
      {
        GameItems.IDs._FIRST,
        GameItems.IDs.MEDICINE_MEDIKIT,
        GameItems.IDs.MEDICINE_PILLS_SLP,
        GameItems.IDs.MEDICINE_PILLS_STA,
        GameItems.IDs.MEDICINE_PILLS_SAN,
        GameItems.IDs.MEDICINE_PILLS_ANTIVIRAL
      }, out data);
      this.DATA_MEDICINE_BANDAGE = data[0];
      this.DATA_MEDICINE_MEDIKIT = data[1];
      this.DATA_MEDICINE_PILLS_SLP = data[2];
      this.DATA_MEDICINE_PILLS_STA = data[3];
      this.DATA_MEDICINE_PILLS_SAN = data[4];
      this.DATA_MEDICINE_PILLS_ANTIVIRAL = data[5];
      return true;
    }

    public bool LoadFoodFromCSV(IRogueUI ui, string path)
    {
      GameItems.FoodData[] data;
      this.LoadDataFromCSV<GameItems.FoodData>(ui, path, "food items", 7, new Func<CSVLine, GameItems.FoodData>(GameItems.FoodData.FromCSVLine), new GameItems.IDs[3]
      {
        GameItems.IDs.FOOD_ARMY_RATION,
        GameItems.IDs.FOOD_CANNED_FOOD,
        GameItems.IDs.FOOD_GROCERIES
      }, out data);
      this.DATA_FOOD_ARMY_RATION = data[0];
      this.DATA_FOOD_CANNED_FOOD = data[1];
      this.DATA_FOOD_GROCERIES = data[2];
      return true;
    }

    public bool LoadMeleeWeaponsFromCSV(IRogueUI ui, string path)
    {
      GameItems.MeleeWeaponData[] data;
      this.LoadDataFromCSV<GameItems.MeleeWeaponData>(ui, path, "melee weapons items", 9, new Func<CSVLine, GameItems.MeleeWeaponData>(GameItems.MeleeWeaponData.FromCSVLine), new GameItems.IDs[16]
      {
        GameItems.IDs.MELEE_BASEBALLBAT,
        GameItems.IDs.MELEE_COMBAT_KNIFE,
        GameItems.IDs.MELEE_CROWBAR,
        GameItems.IDs.MELEE_GOLFCLUB,
        GameItems.IDs.MELEE_HUGE_HAMMER,
        GameItems.IDs.MELEE_IRON_GOLFCLUB,
        GameItems.IDs.MELEE_SHOVEL,
        GameItems.IDs.MELEE_SHORT_SHOVEL,
        GameItems.IDs.MELEE_TRUNCHEON,
        GameItems.IDs.UNIQUE_JASON_MYERS_AXE,
        GameItems.IDs.MELEE_IMPROVISED_CLUB,
        GameItems.IDs.MELEE_IMPROVISED_SPEAR,
        GameItems.IDs.MELEE_SMALL_HAMMER,
        GameItems.IDs.UNIQUE_FAMU_FATARU_KATANA,
        GameItems.IDs.UNIQUE_BIGBEAR_BAT,
        GameItems.IDs.UNIQUE_ROGUEDJACK_KEYBOARD
      }, out data);
      this.DATA_MELEE_BASEBALLBAT = data[0];
      this.DATA_MELEE_COMBAT_KNIFE = data[1];
      this.DATA_MELEE_CROWBAR = data[2];
      this.DATA_MELEE_GOLFCLUB = data[3];
      this.DATA_MELEE_HUGE_HAMMER = data[4];
      this.DATA_MELEE_IRON_GOLFCLUB = data[5];
      this.DATA_MELEE_SHOVEL = data[6];
      this.DATA_MELEE_SHORT_SHOVEL = data[7];
      this.DATA_MELEE_TRUNCHEON = data[8];
      this.DATA_MELEE_UNIQUE_JASON_MYERS_AXE = data[9];
      this.DATA_MELEE_IMPROVISED_CLUB = data[10];
      this.DATA_MELEE_IMPROVISED_SPEAR = data[11];
      this.DATA_MELEE_SMALL_HAMMER = data[12];
      this.DATA_MELEE_UNIQUE_FAMU_FATARU_KATANA = data[13];
      this.DATA_MELEE_UNIQUE_BIGBEAR_BAT = data[14];
      this.DATA_MELEE_UNIQUE_ROGUEDJACK_KEYBOARD = data[15];
      return true;
    }

    public bool LoadRangedWeaponsFromCSV(IRogueUI ui, string path)
    {
      GameItems.RangedWeaponData[] data;
      this.LoadDataFromCSV<GameItems.RangedWeaponData>(ui, path, "ranged weapons items", 8, new Func<CSVLine, GameItems.RangedWeaponData>(GameItems.RangedWeaponData.FromCSVLine), new GameItems.IDs[10]
      {
        GameItems.IDs.RANGED_ARMY_PISTOL,
        GameItems.IDs.RANGED_ARMY_RIFLE,
        GameItems.IDs.RANGED_HUNTING_CROSSBOW,
        GameItems.IDs.RANGED_HUNTING_RIFLE,
        GameItems.IDs.RANGED_KOLT_REVOLVER,
        GameItems.IDs.RANGED_PISTOL,
        GameItems.IDs.RANGED_PRECISION_RIFLE,
        GameItems.IDs.RANGED_SHOTGUN,
        GameItems.IDs.UNIQUE_SANTAMAN_SHOTGUN,
        GameItems.IDs.UNIQUE_HANS_VON_HANZ_PISTOL
      }, out data);
      this.DATA_RANGED_ARMY_PISTOL = data[0];
      this.DATA_RANGED_ARMY_RIFLE = data[1];
      this.DATA_RANGED_HUNTING_CROSSBOW = data[2];
      this.DATA_RANGED_HUNTING_RIFLE = data[3];
      this.DATA_RANGED_KOLT_REVOLVER = data[4];
      this.DATA_RANGED_PISTOL = data[5];
      this.DATA_RANGED_PRECISION_RIFLE = data[6];
      this.DATA_RANGED_SHOTGUN = data[7];
      this.DATA_UNIQUE_SANTAMAN_SHOTGUN = data[8];
      this.DATA_UNIQUE_HANS_VON_HANZ_PISTOL = data[9];
      return true;
    }

    public bool LoadExplosivesFromCSV(IRogueUI ui, string path)
    {
      GameItems.ExplosiveData[] data;
      this.LoadDataFromCSV<GameItems.ExplosiveData>(ui, path, "explosives items", 14, new Func<CSVLine, GameItems.ExplosiveData>(GameItems.ExplosiveData.FromCSVLine), new GameItems.IDs[1]
      {
        GameItems.IDs.EXPLOSIVE_GRENADE
      }, out data);
      this.DATA_EXPLOSIVE_GRENADE = data[0];
      return true;
    }

    public bool LoadBarricadingMaterialFromCSV(IRogueUI ui, string path)
    {
      GameItems.BarricadingMaterialData[] data;
      this.LoadDataFromCSV<GameItems.BarricadingMaterialData>(ui, path, "barricading items", 6, new Func<CSVLine, GameItems.BarricadingMaterialData>(GameItems.BarricadingMaterialData.FromCSVLine), new GameItems.IDs[1]
      {
        GameItems.IDs.BAR_WOODEN_PLANK
      }, out data);
      this.DATA_BAR_WOODEN_PLANK = data[0];
      return true;
    }

    public bool LoadArmorsFromCSV(IRogueUI ui, string path)
    {
      GameItems.ArmorData[] data;
      this.LoadDataFromCSV<GameItems.ArmorData>(ui, path, "armors items", 8, new Func<CSVLine, GameItems.ArmorData>(GameItems.ArmorData.FromCSVLine), new GameItems.IDs[7]
      {
        GameItems.IDs.ARMOR_ARMY_BODYARMOR,
        GameItems.IDs.ARMOR_CHAR_LIGHT_BODYARMOR,
        GameItems.IDs.ARMOR_HELLS_SOULS_JACKET,
        GameItems.IDs.ARMOR_FREE_ANGELS_JACKET,
        GameItems.IDs.ARMOR_POLICE_JACKET,
        GameItems.IDs.ARMOR_POLICE_RIOT,
        GameItems.IDs.ARMOR_HUNTER_VEST
      }, out data);
      this.DATA_ARMOR_ARMY = data[0];
      this.DATA_ARMOR_CHAR = data[1];
      this.DATA_ARMOR_HELLS_SOULS_JACKET = data[2];
      this.DATA_ARMOR_FREE_ANGELS_JACKET = data[3];
      this.DATA_ARMOR_POLICE_JACKET = data[4];
      this.DATA_ARMOR_POLICE_RIOT = data[5];
      this.DATA_ARMOR_HUNTER_VEST = data[6];
      return true;
    }

    public bool LoadTrackersFromCSV(IRogueUI ui, string path)
    {
      GameItems.TrackerData[] data;
      this.LoadDataFromCSV<GameItems.TrackerData>(ui, path, "trackers items", 5, new Func<CSVLine, GameItems.TrackerData>(GameItems.TrackerData.FromCSVLine), new GameItems.IDs[4]
      {
        GameItems.IDs.TRACKER_BLACKOPS,
        GameItems.IDs.TRACKER_CELL_PHONE,
        GameItems.IDs.TRACKER_ZTRACKER,
        GameItems.IDs.TRACKER_POLICE_RADIO
      }, out data);
      this.DATA_TRACKER_BLACKOPS_GPS = data[0];
      this.DATA_TRACKER_CELL_PHONE = data[1];
      this.DATA_TRACKER_ZTRACKER = data[2];
      this.DATA_TRACKER_POLICE_RADIO = data[3];
      return true;
    }

    public bool LoadSpraypaintsFromCSV(IRogueUI ui, string path)
    {
      GameItems.SprayPaintData[] data;
      this.LoadDataFromCSV<GameItems.SprayPaintData>(ui, path, "spraypaint items", 5, new Func<CSVLine, GameItems.SprayPaintData>(GameItems.SprayPaintData.FromCSVLine), new GameItems.IDs[4]
      {
        GameItems.IDs.SPRAY_PAINT1,
        GameItems.IDs.SPRAY_PAINT2,
        GameItems.IDs.SPRAY_PAINT3,
        GameItems.IDs.SPRAY_PAINT4
      }, out data);
      this.DATA_SPRAY_PAINT1 = data[0];
      this.DATA_SPRAY_PAINT2 = data[1];
      this.DATA_SPRAY_PAINT3 = data[2];
      this.DATA_SPRAY_PAINT4 = data[3];
      return true;
    }

    public bool LoadLightsFromCSV(IRogueUI ui, string path)
    {
      GameItems.LightData[] data;
      this.LoadDataFromCSV<GameItems.LightData>(ui, path, "lights items", 6, new Func<CSVLine, GameItems.LightData>(GameItems.LightData.FromCSVLine), new GameItems.IDs[2]
      {
        GameItems.IDs.LIGHT_FLASHLIGHT,
        GameItems.IDs.LIGHT_BIG_FLASHLIGHT
      }, out data);
      this.DATA_LIGHT_FLASHLIGHT = data[0];
      this.DATA_LIGHT_BIG_FLASHLIGHT = data[1];
      return true;
    }

    public bool LoadScentspraysFromCSV(IRogueUI ui, string path)
    {
      GameItems.ScentSprayData[] data;
      this.LoadDataFromCSV<GameItems.ScentSprayData>(ui, path, "scentsprays items", 6, new Func<CSVLine, GameItems.ScentSprayData>(GameItems.ScentSprayData.FromCSVLine), new GameItems.IDs[1]
      {
        GameItems.IDs.SCENT_SPRAY_STENCH_KILLER
      }, out data);
      this.DATA_SCENT_SPRAY_STENCH_KILLER = data[0];
      return true;
    }

    public bool LoadTrapsFromCSV(IRogueUI ui, string path)
    {
      GameItems.TrapData[] data;
      this.LoadDataFromCSV<GameItems.TrapData>(ui, path, "traps items", 16, new Func<CSVLine, GameItems.TrapData>(GameItems.TrapData.FromCSVLine), new GameItems.IDs[4]
      {
        GameItems.IDs.TRAP_EMPTY_CAN,
        GameItems.IDs.TRAP_BEAR_TRAP,
        GameItems.IDs.TRAP_SPIKES,
        GameItems.IDs.TRAP_BARBED_WIRE
      }, out data);
      this.DATA_TRAP_EMPTY_CAN = data[0];
      this.DATA_TRAP_BEAR_TRAP = data[1];
      this.DATA_TRAP_SPIKES = data[2];
      this.DATA_TRAP_BARBED_WIRE = data[3];
      return true;
    }

    public bool LoadEntertainmentFromCSV(IRogueUI ui, string path)
    {
      GameItems.EntData[] data;
      this.LoadDataFromCSV<GameItems.EntData>(ui, path, "entertainment items", 7, new Func<CSVLine, GameItems.EntData>(GameItems.EntData.FromCSVLine), new GameItems.IDs[2]
      {
        GameItems.IDs.ENT_BOOK,
        GameItems.IDs.ENT_MAGAZINE
      }, out data);
      this.DATA_ENT_BOOK = data[0];
      this.DATA_ENT_MAGAZINE = data[1];
      return true;
    }

    public enum IDs
    {
      MEDICINE_BANDAGES = 0,
      _FIRST = 0,
      MEDICINE_MEDIKIT = 1,
      MEDICINE_PILLS_STA = 2,
      MEDICINE_PILLS_SLP = 3,
      MEDICINE_PILLS_SAN = 4,
      MEDICINE_PILLS_ANTIVIRAL = 5,
      FOOD_ARMY_RATION = 6,
      FOOD_GROCERIES = 7,
      FOOD_CANNED_FOOD = 8,
      MELEE_BASEBALLBAT = 9,
      MELEE_COMBAT_KNIFE = 10, // 0x0000000A
      MELEE_CROWBAR = 11, // 0x0000000B
      UNIQUE_JASON_MYERS_AXE = 12, // 0x0000000C
      MELEE_HUGE_HAMMER = 13, // 0x0000000D
      MELEE_SMALL_HAMMER = 14, // 0x0000000E
      MELEE_GOLFCLUB = 15, // 0x0000000F
      MELEE_IRON_GOLFCLUB = 16, // 0x00000010
      MELEE_SHOVEL = 17, // 0x00000011
      MELEE_SHORT_SHOVEL = 18, // 0x00000012
      MELEE_TRUNCHEON = 19, // 0x00000013
      MELEE_IMPROVISED_CLUB = 20, // 0x00000014
      MELEE_IMPROVISED_SPEAR = 21, // 0x00000015
      RANGED_ARMY_PISTOL = 22, // 0x00000016
      RANGED_ARMY_RIFLE = 23, // 0x00000017
      RANGED_HUNTING_CROSSBOW = 24, // 0x00000018
      RANGED_HUNTING_RIFLE = 25, // 0x00000019
      RANGED_PISTOL = 26, // 0x0000001A
      RANGED_KOLT_REVOLVER = 27, // 0x0000001B
      RANGED_PRECISION_RIFLE = 28, // 0x0000001C
      RANGED_SHOTGUN = 29, // 0x0000001D
      EXPLOSIVE_GRENADE = 30, // 0x0000001E
      EXPLOSIVE_GRENADE_PRIMED = 31, // 0x0000001F
      BAR_WOODEN_PLANK = 32, // 0x00000020
      ARMOR_ARMY_BODYARMOR = 33, // 0x00000021
      ARMOR_CHAR_LIGHT_BODYARMOR = 34, // 0x00000022
      ARMOR_HELLS_SOULS_JACKET = 35, // 0x00000023
      ARMOR_FREE_ANGELS_JACKET = 36, // 0x00000024
      ARMOR_POLICE_JACKET = 37, // 0x00000025
      ARMOR_POLICE_RIOT = 38, // 0x00000026
      ARMOR_HUNTER_VEST = 39, // 0x00000027
      TRACKER_BLACKOPS = 40, // 0x00000028
      TRACKER_CELL_PHONE = 41, // 0x00000029
      TRACKER_ZTRACKER = 42, // 0x0000002A
      TRACKER_POLICE_RADIO = 43, // 0x0000002B
      SPRAY_PAINT1 = 44, // 0x0000002C
      SPRAY_PAINT2 = 45, // 0x0000002D
      SPRAY_PAINT3 = 46, // 0x0000002E
      SPRAY_PAINT4 = 47, // 0x0000002F
      SCENT_SPRAY_STENCH_KILLER = 48, // 0x00000030
      LIGHT_FLASHLIGHT = 49, // 0x00000031
      LIGHT_BIG_FLASHLIGHT = 50, // 0x00000032
      AMMO_LIGHT_PISTOL = 51, // 0x00000033
      AMMO_HEAVY_PISTOL = 52, // 0x00000034
      AMMO_LIGHT_RIFLE = 53, // 0x00000035
      AMMO_HEAVY_RIFLE = 54, // 0x00000036
      AMMO_SHOTGUN = 55, // 0x00000037
      AMMO_BOLTS = 56, // 0x00000038
      TRAP_EMPTY_CAN = 57, // 0x00000039
      TRAP_BEAR_TRAP = 58, // 0x0000003A
      TRAP_SPIKES = 59, // 0x0000003B
      TRAP_BARBED_WIRE = 60, // 0x0000003C
      ENT_BOOK = 61, // 0x0000003D
      ENT_MAGAZINE = 62, // 0x0000003E
      UNIQUE_SUBWAY_BADGE = 63, // 0x0000003F
      UNIQUE_FAMU_FATARU_KATANA = 64, // 0x00000040
      UNIQUE_BIGBEAR_BAT = 65, // 0x00000041
      UNIQUE_ROGUEDJACK_KEYBOARD = 66, // 0x00000042
      UNIQUE_SANTAMAN_SHOTGUN = 67, // 0x00000043
      UNIQUE_HANS_VON_HANZ_PISTOL = 68, // 0x00000044
      _COUNT = 69, // 0x00000045
    }

    private struct MedecineData
    {
      public const int COUNT_FIELDS = 10;

      public string NAME { get; set; }

      public string PLURAL { get; set; }

      public int STACKINGLIMIT { get; set; }

      public int HEALING { get; set; }

      public int STAMINABOOST { get; set; }

      public int SLEEPBOOST { get; set; }

      public int INFECTIONCURE { get; set; }

      public int SANITYCURE { get; set; }

      public string FLAVOR { get; set; }

      public static GameItems.MedecineData FromCSVLine(CSVLine line)
      {
        return new GameItems.MedecineData()
        {
          NAME = line[1].ParseText(),
          PLURAL = line[2].ParseText(),
          HEALING = line[3].ParseInt(),
          STAMINABOOST = line[4].ParseInt(),
          SLEEPBOOST = line[5].ParseInt(),
          INFECTIONCURE = line[6].ParseInt(),
          SANITYCURE = line[7].ParseInt(),
          STACKINGLIMIT = line[8].ParseInt(),
          FLAVOR = line[9].ParseText()
        };
      }
    }

    private struct FoodData
    {
      public const int COUNT_FIELDS = 7;

      public string NAME { get; set; }

      public string PLURAL { get; set; }

      public int NUTRITION { get; set; }

      public int BESTBEFORE { get; set; }

      public int STACKINGLIMIT { get; set; }

      public string FLAVOR { get; set; }

      public static GameItems.FoodData FromCSVLine(CSVLine line)
      {
        return new GameItems.FoodData()
        {
          NAME = line[1].ParseText(),
          PLURAL = line[2].ParseText(),
          NUTRITION = (int) (1440.0 * (double) line[3].ParseFloat()),
          BESTBEFORE = line[4].ParseInt(),
          STACKINGLIMIT = line[5].ParseInt(),
          FLAVOR = line[6].ParseText()
        };
      }
    }

    private struct MeleeWeaponData
    {
      public const int COUNT_FIELDS = 9;

      public string NAME { get; set; }

      public string PLURAL { get; set; }

      public int ATK { get; set; }

      public int DMG { get; set; }

      public int STA { get; set; }

      public int STACKINGLIMIT { get; set; }

      public bool ISFRAGILE { get; set; }

      public string FLAVOR { get; set; }

      public static GameItems.MeleeWeaponData FromCSVLine(CSVLine line)
      {
        return new GameItems.MeleeWeaponData()
        {
          NAME = line[1].ParseText(),
          PLURAL = line[2].ParseText(),
          ATK = line[3].ParseInt(),
          DMG = line[4].ParseInt(),
          STA = line[5].ParseInt(),
          STACKINGLIMIT = line[6].ParseInt(),
          ISFRAGILE = line[7].ParseBool(),
          FLAVOR = line[8].ParseText()
        };
      }
    }

    private struct RangedWeaponData
    {
      public const int COUNT_FIELDS = 8;

      public string NAME { get; set; }

      public string PLURAL { get; set; }

      public int ATK { get; set; }

      public int DMG { get; set; }

      public int RANGE { get; set; }

      public int MAXAMMO { get; set; }

      public string FLAVOR { get; set; }

      public static GameItems.RangedWeaponData FromCSVLine(CSVLine line)
      {
        return new GameItems.RangedWeaponData()
        {
          NAME = line[1].ParseText(),
          PLURAL = line[2].ParseText(),
          ATK = line[3].ParseInt(),
          DMG = line[4].ParseInt(),
          RANGE = line[5].ParseInt(),
          MAXAMMO = line[6].ParseInt(),
          FLAVOR = line[7].ParseText()
        };
      }
    }

    private struct ExplosiveData
    {
      public const int COUNT_FIELDS = 14;

      public string NAME { get; set; }

      public string PLURAL { get; set; }

      public int FUSE { get; set; }

      public int MAXTHROW { get; set; }

      public int STACKLINGLIMIT { get; set; }

      public int RADIUS { get; set; }

      public int[] DMG { get; set; }

      public string FLAVOR { get; set; }

      public static GameItems.ExplosiveData FromCSVLine(CSVLine line)
      {
        return new GameItems.ExplosiveData()
        {
          NAME = line[1].ParseText(),
          PLURAL = line[2].ParseText(),
          FUSE = line[3].ParseInt(),
          MAXTHROW = line[4].ParseInt(),
          STACKLINGLIMIT = line[5].ParseInt(),
          RADIUS = line[6].ParseInt(),
          DMG = new int[6]
          {
            line[7].ParseInt(),
            line[8].ParseInt(),
            line[9].ParseInt(),
            line[10].ParseInt(),
            line[11].ParseInt(),
            line[12].ParseInt()
          },
          FLAVOR = line[13].ParseText()
        };
      }
    }

    private struct BarricadingMaterialData
    {
      public const int COUNT_FIELDS = 6;

      public string NAME { get; set; }

      public string PLURAL { get; set; }

      public int VALUE { get; set; }

      public int STACKINGLIMIT { get; set; }

      public string FLAVOR { get; set; }

      public static GameItems.BarricadingMaterialData FromCSVLine(CSVLine line)
      {
        return new GameItems.BarricadingMaterialData()
        {
          NAME = line[1].ParseText(),
          PLURAL = line[2].ParseText(),
          VALUE = line[3].ParseInt(),
          STACKINGLIMIT = line[4].ParseInt(),
          FLAVOR = line[5].ParseText()
        };
      }
    }

    private struct ArmorData
    {
      public const int COUNT_FIELDS = 8;

      public string NAME { get; set; }

      public string PLURAL { get; set; }

      public int PRO_HIT { get; set; }

      public int PRO_SHOT { get; set; }

      public int ENC { get; set; }

      public int WEIGHT { get; set; }

      public string FLAVOR { get; set; }

      public static GameItems.ArmorData FromCSVLine(CSVLine line)
      {
        return new GameItems.ArmorData()
        {
          NAME = line[1].ParseText(),
          PLURAL = line[2].ParseText(),
          PRO_HIT = line[3].ParseInt(),
          PRO_SHOT = line[4].ParseInt(),
          ENC = line[5].ParseInt(),
          WEIGHT = line[6].ParseInt(),
          FLAVOR = line[7].ParseText()
        };
      }
    }

    private struct TrackerData
    {
      public const int COUNT_FIELDS = 5;

      public string NAME { get; set; }

      public string PLURAL { get; set; }

      public int BATTERIES { get; set; }

      public string FLAVOR { get; set; }

      public static GameItems.TrackerData FromCSVLine(CSVLine line)
      {
        return new GameItems.TrackerData()
        {
          NAME = line[1].ParseText(),
          PLURAL = line[2].ParseText(),
          BATTERIES = line[3].ParseInt(),
          FLAVOR = line[4].ParseText()
        };
      }
    }

    private struct SprayPaintData
    {
      public const int COUNT_FIELDS = 5;

      public string NAME { get; set; }

      public string PLURAL { get; set; }

      public int QUANTITY { get; set; }

      public string FLAVOR { get; set; }

      public static GameItems.SprayPaintData FromCSVLine(CSVLine line)
      {
        return new GameItems.SprayPaintData()
        {
          NAME = line[1].ParseText(),
          PLURAL = line[2].ParseText(),
          QUANTITY = line[3].ParseInt(),
          FLAVOR = line[4].ParseText()
        };
      }
    }

    private struct LightData
    {
      public const int COUNT_FIELDS = 6;

      public string NAME { get; set; }

      public string PLURAL { get; set; }

      public int FOV { get; set; }

      public int BATTERIES { get; set; }

      public string FLAVOR { get; set; }

      public static GameItems.LightData FromCSVLine(CSVLine line)
      {
        return new GameItems.LightData()
        {
          NAME = line[1].ParseText(),
          PLURAL = line[2].ParseText(),
          FOV = line[3].ParseInt(),
          BATTERIES = line[4].ParseInt(),
          FLAVOR = line[5].ParseText()
        };
      }
    }

    private struct ScentSprayData
    {
      public const int COUNT_FIELDS = 6;

      public string NAME { get; set; }

      public string PLURAL { get; set; }

      public int QUANTITY { get; set; }

      public int STRENGTH { get; set; }

      public string FLAVOR { get; set; }

      public static GameItems.ScentSprayData FromCSVLine(CSVLine line)
      {
        return new GameItems.ScentSprayData()
        {
          NAME = line[1].ParseText(),
          PLURAL = line[2].ParseText(),
          QUANTITY = line[3].ParseInt(),
          STRENGTH = line[4].ParseInt(),
          FLAVOR = line[5].ParseText()
        };
      }
    }

    private struct TrapData
    {
      public const int COUNT_FIELDS = 16;

      public string NAME { get; set; }

      public string PLURAL { get; set; }

      public int STACKING { get; set; }

      public bool USE_ACTIVATE { get; set; }

      public int CHANCE { get; set; }

      public int DAMAGE { get; set; }

      public bool DROP_ACTIVATE { get; set; }

      public bool IS_ONE_TIME { get; set; }

      public int BREAK_CHANCE { get; set; }

      public int BLOCK_CHANCE { get; set; }

      public int BREAK_CHANCE_ESCAPE { get; set; }

      public bool IS_NOISY { get; set; }

      public string NOISE_NAME { get; set; }

      public bool IS_FLAMMABLE { get; set; }

      public string FLAVOR { get; set; }

      public static GameItems.TrapData FromCSVLine(CSVLine line)
      {
        return new GameItems.TrapData()
        {
          NAME = line[1].ParseText(),
          PLURAL = line[2].ParseText(),
          STACKING = line[3].ParseInt(),
          DROP_ACTIVATE = line[4].ParseBool(),
          USE_ACTIVATE = line[5].ParseBool(),
          CHANCE = line[6].ParseInt(),
          DAMAGE = line[7].ParseInt(),
          IS_ONE_TIME = line[8].ParseBool(),
          BREAK_CHANCE = line[9].ParseInt(),
          BLOCK_CHANCE = line[10].ParseInt(),
          BREAK_CHANCE_ESCAPE = line[11].ParseInt(),
          IS_NOISY = line[12].ParseBool(),
          NOISE_NAME = line[13].ParseText(),
          IS_FLAMMABLE = line[14].ParseBool(),
          FLAVOR = line[15].ParseText()
        };
      }
    }

    private struct EntData
    {
      public const int COUNT_FIELDS = 7;

      public string NAME { get; set; }

      public string PLURAL { get; set; }

      public int STACKING { get; set; }

      public int VALUE { get; set; }

      public int BORECHANCE { get; set; }

      public string FLAVOR { get; set; }

      public static GameItems.EntData FromCSVLine(CSVLine line)
      {
        return new GameItems.EntData()
        {
          NAME = line[1].ParseText(),
          PLURAL = line[2].ParseText(),
          STACKING = line[3].ParseInt(),
          VALUE = line[4].ParseInt(),
          BORECHANCE = line[5].ParseInt(),
          FLAVOR = line[6].ParseText()
        };
      }
    }
  }
}
