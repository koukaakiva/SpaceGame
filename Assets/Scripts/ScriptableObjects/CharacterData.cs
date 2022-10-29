using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Data/Character")]
public class CharacterData : ScriptableObject {
    public string designator;
    public SpeciesData species;
}
