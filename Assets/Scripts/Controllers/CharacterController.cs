using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

namespace Controllers {
    [System.Serializable]
    public class CharacterController : MonoBehaviour {

        [SerializeField] private CharacterData characterData;
        public Character character {get; protected set;}

        void Awake(){
            character = new Character(characterData);
        }

        void Update(){
            character.setState(character.state.Tick(Time.deltaTime));
            foreach (Need need in character.needs){
                need.Tick(Time.deltaTime);
            }
        }
    }
}
