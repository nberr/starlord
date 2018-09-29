using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwapper : MonoBehaviour {
   public Sprite altSprite;
   
   private SpriteRenderer sr;
   private Sprite originalSprite;
   
   // Use this for initialization
   void Start () {
      sr = GetComponent<SpriteRenderer>();
      originalSprite = sr.sprite;
   }

   // Update is called once per frame
   void Update () {
      if (GameControl.instance.godmode) {
         sr.sprite = altSprite;
      } else {
         sr.sprite = originalSprite;
      }
   }
}
