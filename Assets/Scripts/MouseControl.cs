using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
   
   //http://answers.unity3d.com/questions/566519/camerascreentoworldpoint-in-perspective.html
   static public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z) {
      Ray ray = UnityEngine.Camera.main.ScreenPointToRay(screenPosition);
      Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
      float distance;
      xy.Raycast(ray, out distance);
      return ray.GetPoint(distance);
   }
}
