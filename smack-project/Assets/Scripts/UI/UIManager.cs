using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UIManager {
    public static void updateText(Text textObj, string content) {
        textObj.text = content;
    }
}
