# Quidditch

## CPSC-565-Assignment-2

This project simulates a simplified version of the game of quidditch from Harry Potter. In the game, wizards ride brooms and chase an object called a snitch. This snitch moves quickly and randomly. Each time a player "catches" the snitch, they score a point for their team. Catches that follow eachother are worth two points. The first team to reach 100 points wins the game.

Unity3D engine is used to create the simulation and implement the algorithms that dictate the behaviour of the players and the movement of the snitch.

<img width="586" alt="Screen Shot 2021-02-26 at 8 11 11 PM" src="https://user-images.githubusercontent.com/50717419/109373890-cb735100-786e-11eb-8f57-4b8b187b5529.png">


The two competing teams are differentiated by their colour with red representing Slytherin and green for Gryffindor. 

The behaviour of each individual player is dependant upon six unique traits:
- Weight
- Max velocity
- Aggressiveness
- Max exhaustion
- Disctraction
- Laziness

Each team has a different mean and standard deviation for each trait. These traits are 
