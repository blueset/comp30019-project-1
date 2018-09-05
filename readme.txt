Project Implementation

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
