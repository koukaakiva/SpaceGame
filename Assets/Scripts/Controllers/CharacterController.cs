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
            GameTime.onGameTick += UpdateState;
            foreach (Need need in character.needs){
                //GameTime.onGameTick += need.Tick;
                GameTime.RunEventAtTime(need.deplete, need.current);
            }
        }

        void Update(){

        }

        void UpdateState(float deltaTime){
            character.setState(character.state.Tick(deltaTime));
        }
    }
}
