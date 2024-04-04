using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

public class IHM_ImageEditor : ImageEditor {

    Texture2D ihmap;

    public IHM_ImageEditor(int Size){
        ihmap = new Texture2D(Size, Size);
    }



    public void ResizeCanvas(int Size){
        ihmap.Reinitialize(Size,Size);
    }

}
