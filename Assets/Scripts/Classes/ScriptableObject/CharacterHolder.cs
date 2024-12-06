using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/CharacterHolder")]
public class CharacterHolder : DataHolder
{
    [System.Serializable]
    public class CharacterInheritance
    {
        public Character character,enemy;
        [Space]
        public bool hasChild = true;
        public Character child;
        [Space]
        public int needLevels;
       // public int priceMultiplayer = 1;

    }
    private static CharacterHolder _default;
    public static CharacterHolder Default => _default;

    [SerializeField] private CharacterInheritance[] characters;
    public override void Init()
    {
        _default = this;
    }
    public int Count => characters.Length;
    public Character this[int id]
    {
        get => characters[id].character;
    }
    public CharacterInheritance Get(int id) => characters[id];

public bool CheckCharacterHasChild(Character character)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            var cs = characters[i];
            if (cs.character.Specifier.Equals(character.Specifier) && cs.hasChild)
                return true;
        }
        return false;
    }
    public bool TryFindCharcterBySpecifier(string specifier, out Character character)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            var cs = characters[i];
            character = cs.character;
            if (character.Specifier.Equals(specifier))
                return true;
        }
        character = null;
        return false;
    }
    public bool TryFindCharcterByChild(Character child, out Character character)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            var cs = characters[i];
            character = cs.character;
            if (cs.hasChild && cs.child.Specifier.Equals(child.Specifier))
                return true;
        }
        character = null;
        return false;
    }

    public CharacterInheritance GetActive()
    {
        int level = LevelManager.Default.CurrentLevelCount;
        for (int i = characters.Length-1; i >= 1; i--)
        {
             var c = characters[i];
            if (c.hasChild && level >= c.needLevels)
                return c;
        }
        return characters[0];
    }
    public ulong GetMoneyForCharacter(int count = 1)
    {
        ulong money = 0;
        for (int i = 0; i < count; i++)
        {
            money +=(ulong)(
    GameData.Default.moneyForCharacter *
    Mathf.Pow(GameData.Default.moneyForCharacterMultiplier,
    PartyManager.Default.GetBuyCount()+ i));

        }


        return money;
    }
    public CharacterInheritance GetInchByTier(int tier)
    {
        int count = 0;
        var scs = characters[0];
        for (int i = 0; i < characters.Length && count < tier; i++)
        {
            var cs = characters[i];
            if (cs.hasChild && cs.child.Specifier.Equals(scs.character.Specifier))
            {
                scs = cs;
                count++;
            }
        }
        return scs;
    }
    public int GetChildCount(Character character)
    {
        int count = 0;
        for (int i = characters.Length-1; i >= 0; i--)
        {
            var cs = characters[i];
            var c = cs.character;
            if (c.Specifier.Equals(character.Specifier)&& cs.hasChild)
            {
                count++;
                character = cs.child;
            }
        }
        return Mathf.RoundToInt(Mathf.Pow(2, count));
    }
}
