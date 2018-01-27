# Asteroid Assault MonoGame CSharp

Please visit: http://www.monogame.net/2017/03/01/monogame-3-6/

Download and install MonoGame for Visual Studio.

Download the complete project with solution and placement art, build and enjoy!

NOTES:
- This is a 2D top-down space shooter.
- The best feature here is the implementation of the collision detection and response (elastic collisions with rebound).
Circle and Square collisions are available and can be assigned to game objects such as the player, the enemy ships and the asteroids.
- Implementation of a simple particle system to create random explosions.
- Sound effects are played for enemy and player shots as well as explosion sounds.
- Game was organized around a TitleScreen, Playing, PlayerDead and GameOver states that flow into one another correctly.
- Simple AI was implemented. Enemy ships follow a path composed of a series of waypoints to move around the screen.
The paths are all in a list that each enemy when spawned can randomly pick one. Enemies have a chance to fire at player.
- "Housekeeping" was implemented. Each asteroid, enemy and shot sprites are destroyed when they go offscreen.
