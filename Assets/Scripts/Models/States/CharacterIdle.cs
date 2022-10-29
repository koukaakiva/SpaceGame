using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Models {
    public class CharacterIdle : CharacterState {
        private Character character;

        public CharacterIdle(Character character){
            this.character = character;
        }

        public override CharacterState Tick(float deltaTime){
            return this; //TODO finish this
        }
    }
}
