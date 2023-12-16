using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIControl {


    void OnSelected();
    void OnFingerUp();
    void OnDeSelected();
    void OnSwiped();
    void OnTapped();
    void OnHeld();
}
