using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Messaging;

[ExecuteInEditMode]
public class FogEffect : MonoBehaviour {

    // Width and height of the texture in pixels.
    public int pixWidth;
    public int pixHeight;

    // The origin of the sampled area in the plane.
    public float xOrg;
    public float yOrg;

    // How fast the fog moves relative to the player
    public float MovementDilation;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    public float scale = 1.0F;

    private Texture2D noiseTex;
    private Color[] pix;
    private Renderer rend;

	public float intensity;
	private Material material;

    private float timer = 0;

    // Creates a private material used to the effect
    void Awake ()
	{
        // Set up the texture and a Color array to hold pixels during processing.
        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
		material = new Material( Shader.Find("Hidden/FogShader") );
	}

    void setIntensity()
    {
        bool inNZ = GameObject.FindGameObjectWithTag("Player").GetComponent<WakingSight>().inNZ;

        if (inNZ && intensity < 1)
        {
            intensity = Mathf.Min(1.0f, intensity + Time.deltaTime);
        }
        else if (!inNZ && intensity > 0)
        {
            intensity = Mathf.Max(0.0f, intensity - Time.deltaTime);
        }
    }

    void CalcNoise()
    {
        // For each pixel in the texture...
        float y = 0.0F;

        while (y < noiseTex.height)
        {
            float x = 0.0F;
            while (x < noiseTex.width)
            {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;

                float sample = perlin3D(xCoord, yCoord, timer);
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }

    private float perlin3D(float x, float y, float z)
    {
        return (Mathf.PerlinNoise(x, y)
            + Mathf.PerlinNoise(x, z)
            + Mathf.PerlinNoise(y, x)
            + Mathf.PerlinNoise(y, z)) / 3;
            //+ Mathf.PerlinNoise(z, x)
            //+ Mathf.PerlinNoise(z, y)); / 6;
    }

    void Update()
    {
        timer += Time.deltaTime;
        CalcNoise();
        setIntensity();
    }

    float lerp (float x)
    {
        return x * x * x * (x * (x * 6 - 15) + 10);
    }
	
	// Postprocess the image
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		if (intensity == 0)
		{
			Graphics.Blit (source, destination);
			return;
		}

		material.SetFloat("_bwBlend", lerp(intensity));
        material.SetTexture("_FogTex", noiseTex);
		Graphics.Blit (source, destination, material);
	}
}