using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwitcher : MonoBehaviour
{
    internal static void OpenPanel(GameObject PanelToOpen, GameObject PanelToClose)
    {
        PanelToOpen.SetActive(true);
        PanelToClose.SetActive(false);
    }
}
