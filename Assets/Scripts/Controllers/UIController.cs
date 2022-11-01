using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using UnityEngine.UI;
using TMPro;

namespace Controllers {
    public class UIController : MonoBehaviour {

        private CharacterController characterController;
        private Image image;
        private Character character;
        public Sprite happySprite;
        public Sprite deadSprite;
        private TextMeshProUGUI tmp;

        void Awake(){
            image = transform.Find("Face").GetComponent<Image>();
            image.sprite = happySprite;
            characterController = GameObject.FindObjectOfType<CharacterController>();
            tmp = transform.Find("GameTimeDisplay").GetComponent<TextMeshProUGUI>();
        }

        void Start(){
            character = characterController.character;
            character.needs[0].onNeedDepleted += onNeedDepleted;
        }

        void Update(){
            tmp.SetText(GameTime.gameTime.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        void onNeedDepleted(Need need){
            image.sprite = deadSprite;
            character.needs[0].onNeedDepleted -= onNeedDepleted;
        }
    }
}