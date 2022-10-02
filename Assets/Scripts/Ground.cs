using UnityEngine;

public class Ground : MonoBehaviour
{
    public MultiPlayer player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Map")
        {
            player.isGround = true;
        }
    }
}
