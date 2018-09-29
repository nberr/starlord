using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* @author: Ken Oshima
 * From http://answers.unity3d.com/questions/1095047/detect-mouse-events-for-ui-canvas.html
 */

public class ImageTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
  
   private bool onImage = false;

   public float yOffset = 60;
   public GameObject tooltip;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
      if (onImage) {
         tooltip.transform.position = Input.mousePosition + new Vector3(0, yOffset, 0);
      }
	}

   public void OnPointerEnter(PointerEventData eventData)
   {
      onImage = true;
      tooltip.SetActive (true);
   }

   public void OnPointerExit(PointerEventData eventData)
   {
      onImage = false;
      tooltip.SetActive (false);
   }
}
