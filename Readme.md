# Cars Extended

<!-- [![](https://img.shields.io/github/v/release/MMike17/ArtOfRally_CarsExtended?label=Download)](https://github.com/MMike17/ArtOfRally_CarsExtended/releases/latest) -->
<!-- ![](https://img.shields.io/badge/Game%20Version-v1.5.5-blue) -->

A mod for Art of Rally that adds a framework to mod new cars into the game.


## Disclaimer

`⚠️This project is unfinished and is not worked on anymore⚠️`

I have run out of ideas on how to fix it and the work that has already been done to get to this point was grueling.\
If you want to take a look at this mod and try to fix it you are very much wellcome to.\
If you find it hard to debug this project, especially when the game is thrown in an `EventManager` spawn loop, I would suggest to check the _Unity logs_ available at this path :
- Windows:
	- "C:\Users\\[username]\AppData\LocalLow\Funselektor Labs\Art of Rally\Player.log"
- Mac:
	- ~/Library/Logs/Funselektor Labs\Art of Rally/Player.log (you need to un-hide the Library folder in your home directory)
- Linux:
	- ~/.config/unity3d/Funselektor Labs\Art of Rally/Player.log


## Vision

This mod was intended to be a **framework** other mod creators could use to add custom cars to the game.\
The cars in Art of Rally are made of a lot of moving parts and several systems intersect to load a fully functionnal car.\
Thus this mod aimed to manage all the necessary workarounds, tweaks and fixes to integrate the custom cars seamlessly into the game.\
A mod using this framework would provide the necessary informations, settings and assets to a single function call and the framework would manage the entirety of the logic from there.


## Current features

### Injection

Currently another mod can call `ExtraCarManager.InjectCar(CarInfos carInfos, ModEntry sourceMod, string assetBundleName)` to inject a new car.

This method takes :

- A `CarInfos` object.
	- A `Car` object.
		- The car's name (string).
		- The car's name index (int) (usually the length of the group it's part of / **Group 2** => `CarManager.SixtiesCarList`).
		- The car's prefab name (string).
		- A `Livery` object (not supported yet).
		- A `CarClass` value.
		- A `CarStats` object (partially supported).
			- The unlock year (int) (can be 0).
			- The unlock save key (string) (can be null).
			- The bonus livery unlock year (for bonus livery ?) (can be 0).
			- The bonus livery unlock save key (string) (can be null).
			- The engine's description (string) ("[engine type] - [horse power]").
			- The gear count (int).
			- A `CarSpecs.Tranny` value (transmission type).
			- A `CarSpecs.EngineAspiration` value.
			- A `CarSpecs.Country` value.
			- A "lore" key (string) (description/trivia of the car) (unsupported).
			- An "odometer" stats key (string) (total distance traveled) (unsupported).
			- A "podiums" stats key (string) (total podiums) (unsupported).
			- An "events" stats key (string) (total races) (unsupported).
		- A flag describing if the game is part of a DLC (probably set to 0, to be investigated).
	- A `CarInfos.EngineOrientation` value.
	- The width of the wheels.
	- The radius of the wheels.
	- A `CarInfos.SourceCar` value (used for sounds / game's car from which we want to copy the sounds).
- The reference to the current `ModEntry` (provided through the mod's entry point).
- The name of the asset bundle containing the new car's assets.

The AssetBundle containing the car's assets needs to be placed next to the mod's dll for this framework to load it.


### Assets

Currently the only asset needed is a Prefab configured a certain way and containing the model of the new car.\
The car prefab needs to have the following structure :

- "Car_[short car name without spaces]" empty object
	- "Body" (contains the 3D models as children except for the wheels)
	- "Collider" (MeshCollider with collider for the body)
	- "Wing_Back" (where you would put a wing / this can be adjusted for gameplay) empty object
	- "Wing_Front" (around front bumper / this can be adjusted for gameplay) empty object
	- "Wheels" empty object
	    - "WheelFL" (center of each wheel) empty object
			- "WheelFL" empty object
				- "BrakeRotorFL" (brake disc model)
				- "w0" (wheel model)
        	- "Caliper" empty object
        		- "brake caliper" (model)
    	- "WheelFR" (center of each wheel) empty object
			- (similar to previous wheel)
    	- "WheelRL" (center of each wheel) empty object
			- (similar to previous wheel)
    	- "WheelRR" (center of each wheel) empty object
			- (similar to previous wheel)
	- "PrefabSpawns" empty object
	    - "Lights" empty object
	        - "Brakelights" empty object
	            - "BrakeLight L" (empty object just in front of breaklight)
	            - "BrakeLight R" (empty object just in front of breaklight)
	        - "Headlights" empty object
	            - "Headlights" (position of the light cone / slightly angled down in front of windshield above bonnet) empty object
	            - "AuxHeadlights" (optional) empty object
	                - "Light1" (broken version ?) empty object
	                	- "headlight_stick_left1" (copy of the light bulbs models)
	                - "headlight_stick_left1" (light bulbs models)
	                - "headlight_stick_right1" (light bulbs models)
	            - "StockHeadlights" (empty object)
	                - "headlights_stick_front" (light bulbs models)
	        - "Backfire" empty object
	            - "Backfire L" (pos where backfire effect spawns / foward orients effect) empty object
	        - "Nightlight" (1/1.5 above the car) empty object
	    - "Particles" empty object
	        - "Dust" (where dust spawns / center of car slightly back) empty object
	        - "Exhaust1" (where exhaust effect spawns) empty object
	        - "EngineSteam" (where engine fires spawns) empty object
	        - "Ambient Effects" (above engine) empty object
	    - "ReverbZones" (center of car) empty object
	- "CheekyHashtag" (not supported) empty object
	- "CameraTarget" (position at general center) empty object
	- "Shadow" (Cube / managed by script / set scale to 0 ?) empty object

_The new car is spawned instead of the currently selected car (for testing purposes)._\
A patch overrides `PlayerManager.CreateCar` to spawn the new car which is then configured by `CarSpawner.ConfigureCar`.\
This method crawls the newly spawned car's hierarchy and places all necessary components.

A series of patches (defined in `CarPatcher`) configure each car component during initialization.\
The method `Setup.LoadSetup` then loads all the information regarding the current car class to configure the different components.

## Bugs

### Current bugs

- The spawned car behaves in a very jittery manner and wheels rarely touch the ground, making it absolutely unplayable.
	- The reason for this jitteriness is unknown but is probably linked to the `Wheel` component.
- Skidmarks do not have any textures, this would indicate the `SkidmarksManager` isn't configured properly.
- It seems some prefabs used by some car components fail to load.


### Known bugs

- Any failure to correctly configure a car will throw the game into an infinite loop of `EventManager` creation which will repeatedly spawn and destroy the car prefab causing the game to slow to a crawl and eventually crash.
- Incorrect setup of the car's `Rigidbody` will cause its mass to be 0 and the `Wheel` components will throw the car at an unfathomable speed causing a loop of car resets.


## Target features

Here is a list of the features this mod was designed to provide but couldn't be added :

- Livery support
	- Possible external livery support (through the game's system).
- Full integration with the game's car/livery unlock system.
- Full integration with the game's car stats system (lore, odometer, podium and events stats).
- On-demand custom sound injection, to override the sound replacement with provided sound clips.


## Possible problems

- `CarMaterialManager.LoadLiveryAndDirtTextures` will need a patch to load the desired textures from the asset bundle.
- `LiveryManager.GetStandardLiveryPath` will need a patch to load the livery from the asset bundle.
- `CarChooserManager.Awake` will need a patch to spawn the custom cars in the car selection menu (otherwise the game will default to the first car of this class).
- The `CarStats` loading and localization systems will need to be patched to provide the correct car stats and description to the car selection menu.

<!--  -->

<!-- #### Launcher Support -->

<!-- ![](https://img.shields.io/badge/Steam-Supprted-green) -->
<!-- ![](https://img.shields.io/badge/Epic-Untested-yellow) -->
<!-- ![](https://img.shields.io/badge/GOG-Untested-yellow) -->

<!-- #### Platform Support -->

<!-- ![](https://img.shields.io/badge/Windows-Supprted-green) -->
<!-- ![](https://img.shields.io/badge/Linux-Untested-yellow) -->
<!-- ![](https://img.shields.io/badge/OS%2FX-Untested-yellow) -->
<!-- ![](https://img.shields.io/badge/PlayStation-Untested-yellow) -->
<!-- ![](https://img.shields.io/badge/XBox-Untested-yellow) -->
<!-- ![](https://img.shields.io/badge/Switch-Untested-yellow) -->

<!-- ## Requirements -->

<!-- This mod requires the "..." that you can find [here](https://github.com/MMike17/CarsExtended).\ -->
<!-- Latest release [![](https://img.shields.io/github/v/release/MMike17/?label=Real%20car%20names)](https://github.com/MMike17/CarsExtended/releases/latest) -->

<!-- ## Usage -->

<!-- Press Ctrl + F10 to open the mod manager menu.\ -->
<!-- Adjust settings to select [...] you want.\ -->
<!-- By default, the mod [...]. -->

<!-- - **<settingName>** : will [...]. -->

<!-- Disabling the mod in the manager will [...] by default. -->

<!-- ## Disclaimer -->

<!-- [...] -->

<!-- ## Installation -->

<!-- Follow the [installation guide](https://www.nexusmods.com/site/mods/21/) of the Unity Mod Manager.\ -->
<!-- Then simply download the [latest release](https://github.com/MMike17/CarsExtended/releases/latest) and drop it into the mod manager's mods page. -->

<!-- ## Showcase -->

<!-- ![](Screenshots/.png) -->

<!-- ## Acknowledgments -->

<!-- [...] -->