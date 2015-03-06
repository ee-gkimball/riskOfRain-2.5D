using UnityEngine;
using System.Collections;

public class Pixellator : MonoBehaviour 
{	
	public float refresh = 0.01667f;
	
	// Update is called once per frame
	void Start (){
		InvokeRepeating( "SimulateRenderTexure", 0f, refresh );
	}
	
	void SimulateRenderTexure(){		
		GetComponent<Renderer>().material.mainTexture = RenderTextureFree.Capture (new Rect (0, 0, Screen.width*0.5f, Screen.height*0.5f), 0,0);
	}
}