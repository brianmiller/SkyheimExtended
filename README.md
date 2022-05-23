# SkyheimExtended 

SkyheimExtended simply intercepts the 'skyheim.SkheimMana.UpdateMana' method and injects custom code replacing Skyheim's original code.

Features:
  * Mana Regen and Max Mana scales with player level with configurable scaling factors.
  * Statically assign Mana regen and Max Mana values (instead of scaling)
    *  Regen and Max scale factors can be set independently.
<br><br>

By default, SkyheimExtended is in scaling mode. Default scaling factors are set to:
  * Mana Regen (regenScaleFactor): 1.25
    * Formula is: playerLevel * regenScaleFactor + 3
  * Max Mana (manaScaleFactor): 0.1
    * Formula is: playerLevel * manaScaleFactor + 100
<br><br>

If scaling is disabled (scaleWithLevel = false), then the "Static" values are used. SkyheimExtended sets these values to the following by default:
  * Mana Regen (manaRegen): 7
  * Max Mana (maxMana): 125
<br><br>

 Note: Skyheim's default values are:
  * Mana Regen (manaRegen): 3
  * Max Mana (maxMana): 100
<br><br>
