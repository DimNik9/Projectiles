using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] SettingsPopup settingsPopup;
    [SerializeField] GameObject cannon;
    private bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        settingsPopup.Close();
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isOpen)               //Settings open by pressing Tab key
        {
            settingsPopup.Open(); 
            isOpen = true;
            cannon.GetComponent<LaunchProjectile>().enabled = false;   //Disable projectile launching when settings are open
        }else if (Input.GetKeyDown(KeyCode.Tab) && isOpen)
        {
            settingsPopup.Close();
            isOpen = false;
            cannon.GetComponent<LaunchProjectile>().enabled = true;   //Enable projectile launching when settings are closed again
        }
        }
    }

