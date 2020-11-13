using UnityEngine;

public class NonDestroyable : MonoBehaviour
{
    // Start is called before the first frame update
    public bool exist;
    void Awake()
    {
        this.exist = false;
        GameObject[] fManager = GameObject.FindGameObjectsWithTag("TrackManager");
        if (fManager.Length > 1 && !this.exist)
        {
            Destroy(fManager[0]);
            fManager[0] = this.gameObject;
            fManager[1] = null;
            this.exist = true;
            Debug.Log("one");
        } else if (fManager.Length == 1 && !fManager[0].GetComponent<NonDestroyable>().exist)
        {
            this.exist = true;
            Debug.Log("one");
        }
      

        if (this.exist)
        {
            Debug.Log(exist.ToString());
            DontDestroyOnLoad(this.gameObject);
        }

    }

}
