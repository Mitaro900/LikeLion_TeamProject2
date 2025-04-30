using UnityEngine;

[System.Serializable]
public class StringIntPair
{
    public string Key;
    public int Value;

    public StringIntPair(string key, int value)
    {
        Key = key;
        Value = value;
    }
}

[System.Serializable]
public class StringFloatPair
{
    public string Key;
    public float Value;

    public StringFloatPair(string key, float value)
    {
        Key = key;
        Value = value;
    }

    //public StringFloatPair GetKey(float value) { return Value.Equals(value) ? this : null; }

    //public string GetKey() { return Key; }

    //public StringFloatPair GetValue(string key) { return Key.Equals(key) ? this : null; }

    //public float GetValue() { return Value; }
}

[System.Serializable]
public class GameObjectIntPair
{
    public GameObject Key;
    public int Value;

    public GameObjectIntPair(GameObject key, int value)
    {
        Key = key;
        Value = value;
    }
}