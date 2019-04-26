using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwitcher : MonoBehaviour
{

    public GameObject PanelToOpen;
    public GameObject PanelToClose;

    public void OpenPanel()
    {

        PanelToOpen.SetActive(true);
        PanelToClose.SetActive(false);


      
    }
 
}
