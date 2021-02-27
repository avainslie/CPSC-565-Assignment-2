# Quidditch

## CPSC-565-Assignment-2

This project simulates a simplified version of the game of quidditch from Harry Potter. In the game, wizards ride brooms and chase an object called a snitch. This snitch moves quickly and randomly. Each time a player "catches" the snitch, they score a point for their team. "Catches" that follow eachother are worth two points. The first team to reach 100 points wins the game.

Unity3D engine is used to create the simulation and implement the algorithms that dictate the behaviour of the players and the movement of the snitch.

<img width="586" alt="Screen Shot 2021-02-26 at 8 11 11 PM" src="https://user-images.githubusercontent.com/50717419/109373890-cb735100-786e-11eb-8f57-4b8b187b5529.png">
<em>An image of the snitch</em>

The two competing teams are differentiated by their colour with red representing Slytherin and green for Gryffindor. 

Players start on their side of the field before beginning to fly after the snitch.

<img width="689" alt="Screen Shot 2021-02-26 at 9 49 58 PM" src="https://user-images.githubusercontent.com/50717419/109375786-966dfb00-787c-11eb-9d67-8bec508a68bc.png">
<em>Players at starting positions</em>

When a player becomes unconscious, they stop moving, fall to the ground, and return to their teams starting side. They must rest for 10 seconds before entering the game play again. The image below shows some unconscious players.

<img width="388" alt="Screen Shot 2021-02-26 at 9 14 52 PM" src="https://user-images.githubusercontent.com/50717419/109375884-7db21500-787d-11eb-8527-b93f0d10995a.png">
<em>Unconscious players</em>

Players movement is based off a balance of avoiding colliding into objects that are not the snitch while also chasing the snitch.
Players of opposing teams collide differently than players that share the same team, with the former taking aggressiveness into consideration and being more likely to lead to a player becoming unconscious. 

Players also exert themselves as they move. If a players exhaustion reaches their maximum, they will become unconscious. In order to avoid this, players will rest for a little bit when they start getting close to being over tired. 

The behaviour of each individual player is dependant upon six unique traits:
- Weight: How heavy the player is. Affects the acceleration of the player.
- Max velocity: How fast players can go.
- Aggressiveness: How likely a player is to "win" in a collision with another player and not become unconscious. 
- Max exhaustion: How tired the player can get before becoming unconscious. Affects when a player decides to rest.
- Disctraction: How focused the player is. More distracted players have a higher chance of being distracted and going a random direction everyonce in a while.
- Laziness: Affects how much force the player will have to move towards the snitch. Lazier players use less force.

These traits are implemented by selecting a random value from a normal distribution based on the teams unique means and standard deviations for each trait. As such, each player has it's own unique traits and can behave in a slightly different way than other players. All together, these players create unique emergent team behaviours which affect which team will win the game of quidditch. 

### Example game play

![game](https://user-images.githubusercontent.com/50717419/109377114-6f1c2b80-7886-11eb-9b63-6df57ce09e7b.gif)

![above](https://user-images.githubusercontent.com/50717419/109377135-95da6200-7886-11eb-8000-05c1636938e0.gif)


## Quick Start

You must have Unity 2019 or above installed to run this project. Please visit https://unity.com/ for more information.

1. Clone this repository to your local machine
2. Open the project in the Unity editor and select the Play button.

## Future Directions

Aside from improving and correcting some of the faults with this version of the simulations implementation, there are a few other modifications that could be done to increase the value of this project. One example of this is that assets could be implemented to add nicer visuals to the project and further traits could be added to the players such as communication and team spirit. These would produce interesting emergent behaviours and could be implemented in a variety of different ways. 


