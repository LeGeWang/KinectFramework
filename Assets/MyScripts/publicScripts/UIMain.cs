using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIMain : MonoBehaviour {

    Button button_1;
    Button button_2;

    GameObject cube;

    void Start()
    {
        button_1 = GameObject.Find("Button_1").GetComponent<Button>();
        button_2 = GameObject.Find("Button_2").GetComponent<Button>();
        cube = GameObject.Find("Cube");

        EventTriggerListener.Get(button_1.gameObject).onEnter = OnButtonEnter;
        EventTriggerListener.Get(button_2.gameObject).onEnter = OnButtonEnter;
        EventTriggerListener.Get(cube).onEnter = OnButtonEnter;

        EventTriggerListener.Get(button_1.gameObject).onExit = OnButtonExit;
        EventTriggerListener.Get(button_2.gameObject).onExit = OnButtonExit;
        EventTriggerListener.Get(cube).onExit = OnButtonExit;

        EventTriggerListener.Get(button_1.gameObject).onClick = OnButtonClick;
        EventTriggerListener.Get(button_2.gameObject).onClick = OnButtonClick;

        EventTriggerListener.Get(button_1.gameObject).onDrag = OnButtonDragEnter;
        EventTriggerListener.Get(button_2.gameObject).onDrag = OnButtonDragEnter;
    }

    private void OnButtonEnter(GameObject go)
    {
        Debug.Log(go.name + " Enter");
    }

    private void OnButtonExit(GameObject go)
    {
        if (go == button_1.gameObject)
            Debug.Log("Button 1 Exit");

        if (go == button_2.gameObject)
            Debug.Log("Button 2 Exit");
    }

    private void OnButtonClick(GameObject go)
    {
        if (go == button_1.gameObject)
            Debug.Log("Button 1 Clicked");

        if (go == button_2.gameObject)
            Debug.Log("Button 2 Clicked");
    }

    private void OnButtonDragEnter(GameObject go)
    {
        if (go == button_1.gameObject)
            Debug.Log("Button 1 Drag Enter");

        if (go == button_2.gameObject)
            Debug.Log("Button 2 Drag Enter");
    }
}
