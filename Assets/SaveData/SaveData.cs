using System.Collections.Generic;

public class SaveData
{
    public const int FlagNotSet = -11112;
    public Dictionary<string, int> Flags { get; set; } = new Dictionary<string, int>();

    public void SetFlag(string flagName, int value)
    {
        if (Flags.ContainsKey(flagName))
        {
            Flags[flagName] = value;
        }
        else
        {
            Flags.Add(flagName, value);
        }
    }

    public int GetFlag(string flagName)
    {
        int value;

        if (Flags.TryGetValue(flagName, out value))
        {
            return value;
        }

        return FlagNotSet;
    }
}
