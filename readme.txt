Project Implementation

Landscape generation: [Jin Han]
    - Using diamond-square algorithm
    - Each iteration adds a smaller random value to the landscape to produce a 
      more realistic

Landscape colouring: [Jin Han]
    - Based on the generated landscape, set top 20% to white as snow, bottom 
      30% to yellow as sand

Sea generation: [Jin Han]
    - Similar implementation in workshop for sea waves

Collision: [Wenqing Xue]
    - Use physic material to prevent the collision

Shader: [Wenqing Xue]
    - PhongShader: Modified based on the phong shader from the workshop
    - SeaPhongShader: Based on the previous one, adjusted for the sea wave
