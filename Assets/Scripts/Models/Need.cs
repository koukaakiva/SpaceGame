using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Models {
    public class Need {
        public string designator;
        public float maximum;
        public float current;
        public float depletionRate;
        public Action<Need> onNeedDepleted;

        public Need(string designator, float maximum, float current, float depletionRate){
            this.designator = designator;
            this.maximum = maximum;
            this.current = current;
            this.depletionRate = depletionRate;
        }

        public void Tick(float deltaTime){
            current -= depletionRate * deltaTime;
            Debug.Log(current);
            if(current <= 0f){
                current = 0f;
                onNeedDepleted?.Invoke(this);
            }
        }
    }
}