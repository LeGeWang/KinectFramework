using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    // Start Flag to Initial Game
    public bool StartFlag = false;

    // coordination scale
    private Vector3 horizonVector;
    private Vector3 verticalVector;

	// Use this for initialization
	void Start () {
        KinectManager manager = KinectManager.Instance;
        KinectGestures.GestureData gestureData = 
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void CheckForGesture(uint userId)
    {

    }
}
