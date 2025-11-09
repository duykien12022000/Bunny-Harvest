using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterContainer", menuName = "Data/CharacterContainer")]
public class CharacterSO : ScriptableObject
{
    [SerializeField] Character[] characters;
    public int GetLenght() { return characters.Length; }
    [ContextMenu("GetIndex")]
    void GetIndex()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            int index = i;
            characters[i].id = index;
            characters[i].radish = 1000 * index;
        }
    }
    public Character GetCharacter(int id)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].id == id)
                return characters[i];

        }
        Debug.Log($"Fuck you! Character {id} doesn't exist!");
        return null;
    }
}
[Serializable]
public class Character
{
    public int id;
    public int radish;
    public int isBought
    {
        set { PlayerPrefs.SetInt($"Skin_{id}_Bought", value); }
        get { return PlayerPrefs.GetInt($"Skin_{id}_Bought", 0); }
    }
    public GameObject prefabs;
}
