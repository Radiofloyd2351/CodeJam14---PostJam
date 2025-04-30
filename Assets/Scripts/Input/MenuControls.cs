using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuControls : MonoBehaviour
{
    private Characters _controls;

    public void StartUp(Characters controls)
    {
        _controls = controls;

        _controls.MenuDiving.Accept.performed += Scum;
        _controls.MenuDiving.CloseMenu.performed += ToggleMenu;
    }

    // Update is called once per frame
    void Scum(InputAction.CallbackContext ctx)
    {
        
    }
    void ToggleMenu(InputAction.CallbackContext ctx) {
        Inventory.instance.ToggleMenu();
        _controls.BasicActions.Enable();
        _controls.MenuDiving.Disable();
    }
}
