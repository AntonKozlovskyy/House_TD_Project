using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width * 0.5f - 50, Screen.height * 0.5f - 50, 100, 100), "Restart"))
            Application.LoadLevel("Test");
    }
}
