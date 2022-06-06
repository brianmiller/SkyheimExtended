# SkyheimExtended 

SkyheimExtended intercepts Skyheim's method calls and injects custom code replacing Skyheim's original code, when both (Skyheim+SkyeheimExtended) mods are used.

Note:
  * This mod does not replace Skyheim's original dll.  SkyeheimExtended can only be run alongside Skyheim

Features:
  * All runes can now be configured independently from each other.
    * Configure each rune to scale mana capacity and regen rate with the respective Skyheim magic skill level
	* Configure static mana drain values for each rune. I.e., how much mana is used for each rune use
    * Note: the travel rune behaves differently.  The travel rune consumes mana when equipped, not when fired. The travel rune can also be configured


  * Statically assign Mana Regen and Max Mana values (instead of scaling)
    * If desired, scaling can be disabled (behaves like the original Skyheim) but you can still change these static values with SkyheimExtended
	  * staticManaRegen
      * staticMaxMana
      * static[RUNE]ManaDrain

  
  * You can take a look at the latest SkyheimExtended default config here: https://github.com/brianmiller/SkyheimExtended/blob/master/posixone.SkyheimExtended.cfg  



By default, SkyheimExtended is in scaling mode. Default scaling factors are set to:
  * Mana Regen (regenScaleFactor): 1.41
    * Formula is: skillLevel * scaling[RUNE]RegenFactor + 3
  * Max Mana (manaScaleFactor): 0.06
    * Formula is: skillLevel * scaling[RUNE]ManaFactor + 100


If scaling is disabled (scaleWithLevel = false), then the "Static" values are used. SkyheimExtended sets these values to the following by default:
  * Mana Regen (manaRegen): 7
  * Max Mana (maxMana): 125


 Note: Skyheim's default values are:
  * Mana Regen (manaRegen): 3
  * Max Mana (maxMana): 100

### Dynamic Scaling
 ![debugDynamicValues](https://user-images.githubusercontent.com/342276/171280214-b38ca0cd-0352-4794-8730-8b8a1e059d67.png)
 
### Static Assignment
 ![debugStaticValues](https://user-images.githubusercontent.com/342276/171280225-51b75330-6537-44d5-af74-ee3fcf8d01cf.png)
