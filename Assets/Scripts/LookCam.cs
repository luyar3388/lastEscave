using UnityEngine;
using TMPro;
using Photon.Pun;

public class LookCam : MonoBehaviourPunCallbacks
{
    GameObject Cam;
    MultiPlayer py;
    Vector3 startScale;
    //카메라와의 거리
    public float distance = 20;
    TextMeshPro tmp;



    private void Awake()
    {
        //솔로플레이나 디버그화면때 지움
        if (GameManagement.staticPlaymode == "soloplay" || GameManagement.staticPlaymode == null)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        tmp = gameObject.GetComponent<TextMeshPro>();
        startScale = transform.localScale;
        py = gameObject.transform.parent.GetComponent<MultiPlayer>();
        //서버에 저장되어있는 닉네임 가져와서 적용
        tmp.text = py.photonView.Owner.NickName;
        Cam = GameObject.Find("OVRCameraRig");
    }


    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(Cam.transform.position, transform.position);
        Vector3 newScale = startScale * dist / distance;
        transform.localScale = newScale;
        transform.rotation = Cam.transform.rotation;

    }
}
