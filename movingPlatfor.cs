/*
INFO:

https://corgi-engine-docs.moremountains.com/moving-platforms.html

Creating a moving platform
Start by dragging a sprite in your scene, or a 3D model
Select your newly created game object, and set its layer to “MovingPlatforms”
Add a non trigger BoxCollider2D to it
Add a Rigidbody2D component to it, set its BodyType to Kinematic
Add a MovingPlatform component to your object.
You’re done! All that’s left to do is tweaking the MovingPlatform’s component’s inspector, 
typically you’ll want to start by adding a few entries in its PathElements list, which 
determines the points the platform will move through

/*