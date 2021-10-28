# Please read!

Do not use this as a code quality / style reference for DOTS. This is highly experimental code, more akin to a prototype and may not be suited for a production environment. 

# WIP

This is still a work in progress, as I fight my way up the DOTS learning curve. 

You can follow the progress here:

https://trello.com/b/MGJLUwrf/thats-ridiculous

# That's Ridiculous

[![PreviewGif](Docs/preview.gif)](https://youtu.be/JDszL8i2Vpo)

TR is a game where you are control a Wizard whose responsibility is keeping an Airport running amidst an Alien Goo attack.

In this first person game, you shoot cubes that destroy the goo, all while the goo destroys all planes and structures it touches.

Keep planes landing and taking off, and you'll rack up points like it's nobody's business.

Allow the goo to destroy antennas, lights, and other structures, and planes will move slower and appear less frequently, resulting in less points.

Let the main structures fall or fail 10 landings / take offs in a row and it's game over.

# But that's ridiculous!

Yes it is!

# How it works

This game runs on Unity's new DOTS, Data Oriented Technology Stack. Most of it consist of tens of thousands of entities being simulated and manipulated in parallel.

Currently, the game starts with 18k+ active entities. The player has a cube shooting weapon that shoots hundreds of cubes every few seconds, able to charge it up to 2000 cubes in a single shot!

This uses Unity.Physics, Unity.Rendering.Hybrid, Unity.Entities and other DOTS related packages.

The player uses traditional MonoBehaviour, CharacterController, resulting in a hybrid approach to Unity ECS.

# License

This code is licensed under MIT, but uses a couple of graphic assets from the Unity asset store.

I cannot distribute these assets, and they follow their own licenses.

They are available in their store pages:
- https://assetstore.unity.com/packages/3d/environments/industrial/simple-airport-cartoon-assets-37604
- (Free Asset) https://assetstore.unity.com/packages/2d/textures-materials/sky/farland-skies-cloudy-crown-60004

Wand asset is CC0, made by `dummyfish`: https://opengameart.org/content/rpg-item-collection-1
