using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    private Spawnpoint spawn;
    public Transform[] backgrounds;     //list of back and foregrounds to be parallaxed
    private float[] parallaxScalesX;     //proportion of the camera's X movement to move the backgrounds by
    private float[] parallaxScalesY;     //proportion of the camera's Y movement to move the backgrounds by
    public float smoothing = 1f;        // how smooth the parallax is going to be. Set above 0.

    private Transform cam;              //reference to main camera's transform
    private Vector3 previousCamPos;     //position of the camera in previous frame

    //is called before start(). Great for references.
    void Awake()
    {
        //set up camera reference 
        cam = Camera.main.transform;
    }
    // Start is called before the first frame update. Use for initialization
    void Start()
    {
        //The previous frame had the current frame's camera position
        previousCamPos = cam.position;

        //assigning corresponding parallaxScales
        parallaxScalesX = new float[backgrounds.Length];
        parallaxScalesY = new float[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; i++) {
            parallaxScalesX[i] = backgrounds[i].position.z * -1;
            parallaxScalesY[i] = backgrounds[i].position.z * -1;
            parallaxScalesY[i] = parallaxScalesX[i] / 4;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //for each background
        for (int i = 0; i < backgrounds.Length; i++) {
            /*the parallax is the opposite of the camera movement 
            because the previous frame multiplied by the scale */
            float parallaxX = (previousCamPos.x - cam.position.x) * parallaxScalesX[i];
            float parallaxY = (previousCamPos.y - cam.position.y) * parallaxScalesY[i];

            /*set a target x position which is the 
            current position + the parallax*/
            float backgroundTargetPosX = backgrounds[i].position.x + parallaxX;
            float backgroundTargetPosY = backgrounds[i].position.y + parallaxY;

            /* create a target position which is 
             * the background's current position 
             * with it's target x position */
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, backgrounds[i].position.z);
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }
        //set the previousCamPos to the camera's position at the end of the frame
        previousCamPos = cam.position;
    }
}
