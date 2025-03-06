# How to start the game
You should start play mode from the **Main** scene. 

After entering play mode you will start from a simple main menu where you can select the game mode and the level. After selecting you should press **Start** to start the level.

# Technical Considerations
## Multiplayer
The game currently supports local players and simple AI players. The current structure can not support multiplayer. To support multiplayer, the networking strategy (e.g. Server-authoritive, P2P, lock-stepping) should be defined and be considered for the whole architecture of the game.
## Splash Screen
To transition between different scenes, `GameTransitionManager` is used to abstract the details of the transitions. The current implementation directly goes to the target scenes (Main Menu and Level), but it can easily be changed to first start the splash screen and after that load the target scene (or load the target scene in the background while in the splash screen)

## Addressables
Currently, I've used Resources as a quick implementation for loading quiz assets. Using Addressable generally adds more complexity to resource management. To use Addressables for remote asset management, the downloading, preloading, instantiating and unloading strategies should be defined (aside from the bundling strategy). 

The downloading can happen during splashscreen or in background. The preloading can happen when starting a level, or showing a quiz, which in both cases requires a loading screen. For Instantiating, instead of using addressable directly, a new abstraction can be provided that supports synchronous instantiating (by relying on the preloading phase). For better memory management, it should be also consider when to unload the preloaded assets.

## New Quizzes
I did not fully understand the question regarding adding new questions and answer types. I need more information about the new types first.
But if the question is about adding new questions, currently multiple JSON files for quizzes can be injected to the game (It can also be changed to have multiple quizzes in one file).
If it is about remotely adding new questions, the Addressable System can be used to download the JSON files.

## Ports and Adapters
In the level flow, the Port and Adapter pattern is used for the communication of the game logic with presentation. In my previous projects, I have used callbacks with this pattern. But in this project, for the first time, I experimented with using UniTask instead of callbacks. 

