# Project Implementation

Landscape generation:
- Using diamond-square algorithm
- Each iteration adds a smaller random value to the landscape to produce a 
  more realistic

Landscape colouring:
- Based on the generated landscape, set top 20% to white as snow, bottom 
  30% to yellow as sand

Sea generation:
- Similar implementation in workshop for sea waves

Collision:
- Use physic material to prevent the collision

Shader:
- PhongShader: Modified based on the phong shader from the workshop
- SeaPhongShader: Based on the previous one, adjusted for the sea wave

## Feedback
Score: 8.5/10

Good job overall! Diamond square looks largely correct, however the terrain doesn't look a lot like a landscape (i.e. there are lots of 'mounds'). This is probably due to not using an exponential fall-off in the random variation on each iteration. The terrain is also quite shiny (particularly the green) - specular lighting should be toned down a lot more for such a surface. Finally, the waves are a bit high - it would probably help to reduce their amplitude.
