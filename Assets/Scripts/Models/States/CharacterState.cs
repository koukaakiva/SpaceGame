using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Models {
    public abstract class CharacterState {
        public abstract CharacterState Tick(float deltaTime);
    }
}