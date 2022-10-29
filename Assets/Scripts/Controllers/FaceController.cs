using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using UnityEngine.UI;

namespace Controllers {
    public class FaceController : MonoBehaviour {

        private CharacterController characterController;
        private Image image;
        private Character character;
        public Sprite happySprite;
        public Sprite deadSprite;

        void Awake(){
            image = GetComponent<Image>();
            image.sprite = happySprite;
            characterController = GameObject.FindObjectOfType<CharacterController>();
        }

        void Start(){
            character = characterController.character;
            character.needs[0].onNeedDepleted += onNeedDepleted;
        }

        void onNeedDepleted(Need need){
            image.sprite = deadSprite;
            character.needs[0].onNeedDepleted -= onNeedDepleted;
        }
    }
}