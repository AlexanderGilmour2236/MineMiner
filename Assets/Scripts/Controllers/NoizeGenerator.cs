using UnityEngine;
using System.Collections;

// Create a texture and fill it with Perlin noise.
// Try varying the xOrg, yOrg and scale values in the inspector
// while in Play mode to see the effect they have on the noise.

public class NoizeGenerator
{
    // The origin of the sampled area in the plane.
    public float xOrg;
    public float yOrg;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    public float scale = 1.0F;
    
    public void CalcNoise(Texture2D texture2D, int seed)
    {
        Color[] pix = new Color[texture2D.width * texture2D.height];
        
        // For each pixel in the texture...
        float y = 0.0F;

        while (y < texture2D.height)
        {
            float x = 0.0F;
            while (x < texture2D.width)
            {
                float xCoord = xOrg + x / texture2D.width * scale;
                float yCoord = yOrg + y / texture2D.height * scale;
                
                float sample = Mathf.PerlinNoise(xCoord + seed, yCoord + seed);
                pix[(int)y * texture2D.width + (int)x] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }

        // Copy the pixel data to the texture and load it into the GPU.
        texture2D.SetPixels(pix);
        texture2D.Apply();
    }
}