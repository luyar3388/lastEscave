using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyBoardController : MonoBehaviour
{
    public string String = "";
    public GameObject TextField;
    public bool Shift = false;
    public InputField Field;
    // Start is called before the first frame update
    void Start()
    {
        String = "";
        Field = GameObject.Find("InputField").GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        TextField.GetComponent<TextMeshProUGUI>().text = String;
    }

    public void InputKey(string a)
    {
        if(!Shift) String += a;
        else String += a.ToUpper();
    }

    public void InputShift(GameObject go)
    {
        Shift = !Shift;
        if (Shift) go.GetComponent<Image>().color = new Color(0, 0, 255);
        else go.GetComponent<Image>().color = new Color(0,0,0);
    }

    public void CompleteKey()
    {
        Field.text = String;
        Destroy(gameObject);
    }

    public void CancleKey()
    {
        Destroy(gameObject);
    }
}
