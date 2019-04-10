using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostEffectScript : MonoBehaviour
{
    public Material material;
    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        //src is the newly rendered scene that you would normally send to the monitor.
        //We are intercepting this so that we can do more with it before moving on.


        Graphics.Blit(source, destination, material);

    }
}
