using Sod.Model.CommonTypes;

namespace Sod.Model.Tools;

public static class IO
{
    public static List<BinaryIOState> ExtractIOChanges(bool[] persistedState, bool[] satelState)
    {
        var changes = new List<BinaryIOState>();
        for (int i = 0; i < persistedState.Length; i++)
        {
            if (persistedState[i] != satelState[i])
            {
                // indexing at integra starts from 1.
                changes.Add(new BinaryIOState { Index = i + 1, Value = satelState[i] });
            }
        }

        return changes;
    }
}