using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CasualScreens : MonoBehaviour
{
    public void TransitFromMainMenu()
    {
        SceneManager.LoadScene("CasualGame");
    }
}
