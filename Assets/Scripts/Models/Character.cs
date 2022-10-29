using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Models {
    public class Character {
        public CharacterState state {get; private set;}
        public string designator {get; private set;}
        // public BehaviourMatrix behaviourMatrix {get; private set;}
        // public Plan currentPlan {get; private set;}
        // public Action currentAction {get; private set;}
        // public Body body {get; private set;}
        // public Mind mind {get; private set;}
        public List<Need> needs {get; private set;}
        // public List<Skill> skills {get; private set;}
        // public List<Ability> abilities {get; private set;}
        // public List<Datum> knowlege {get; private set;}
        // public Dictionary<EquipmentSlot, Equipable> equipment {get; private set;}

        public Character(CharacterData data){
            state = new CharacterIdle(this);
            designator = data.designator;
            needs = new List<Need>{new Need("Hunger", 100, 100, 1)};
        }

        public void setState(CharacterState state){
            this.state = state;
        }
    }
}