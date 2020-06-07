using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSocket : MonoBehaviour {

    public PlayerControllerThird player;
    public AdvCameraController cam;
    public MediaShare media;
    public NPCControllerThird[] npcs;
    public BinController trashCan;
    public Canvas canvas;
    public UIController uiController;
    public PopUpController popUpController;

    // LEVEL SPECIFIC
    public int timeStart;
    public int timeMaximum;


    //TESTING
    public Toggle hostileToggle;

}
