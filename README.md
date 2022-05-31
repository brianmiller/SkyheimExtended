# SkyheimExtended 

SkyheimExtended simply intercepts the 'skyheim.SkheimMana.UpdateMana' method and injects custom code replacing Skyheim's original code.

Features:
  * Statically assign Mana Regen and Max Mana values (instead of scaling)
  * Dynamically scale Mana Regen and Max Mana values with the player's Skyheim magic skill level. Configurable scaling factors are:
    *  Regen and Max scale factors can be configured by skill. E.g.:
      *  frostManaScaleFactor
      *  frostRegenScaleFactor
      *  fireManaScaleFactor
      *  fireRegenScaleFactor
      *  natureManaScaleFactor
      *  natureRegenScaleFactor
      *  holyManaScaleFactor
      *  holyRegenScaleFactor
<br>

By default, SkyheimExtended is in scaling mode. Default scaling factors are set to:
  * Mana Regen (regenScaleFactor): 1.35
    * Formula is: skillLevel * <skillType>RegenScaleFactor + 3
  * Max Mana (manaScaleFactor): 0.05
    * Formula is: skillLevel * <skillType>ManaScaleFactor + 100
<br><br><br>

If scaling is disabled (scaleWithLevel = false), then the "Static" values are used. SkyheimExtended sets these values to the following by default:
  * Mana Regen (manaRegen): 7
  * Max Mana (maxMana): 125
<br><br><br>

 Note: Skyheim's default values are:
  * Mana Regen (manaRegen): 3
  * Max Mana (maxMana): 100


#### Dynamic Scaling
 ![debugDynamicValues](https://user-images.githubusercontent.com/342276/171280214-b38ca0cd-0352-4794-8730-8b8a1e059d67.png)
 
#### Static Assignment
 ![debugStaticValues](https://user-images.githubusercontent.com/342276/171280225-51b75330-6537-44d5-af74-ee3fcf8d01cf.png)

