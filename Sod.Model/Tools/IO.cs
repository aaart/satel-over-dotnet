using Sod.Model.CommonTypes;

namespace Sod.Model.Tools;

public static class IO
{
    public static List<IOState> ExtractIOChanges(bool[] persistedState, bool[] satelState)
    {
        var changes = new List<IOState>();
        for (int i = 0; i < persistedState.Length; i++)
        {
            if (persistedState[i] != satelState[i])
            {
                changes.Add(new IOState { Index = i + 1, Value = satelState[i] });
            }
        }

        return changes;
    }
}