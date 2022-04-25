namespace Sod.Model.Tasks.Types
{
    public class PersistedStateUpdateTask : SatelTask
    {

        public PersistedStateUpdateTask(string storageKey, bool[] values)
        {
            StorageKey = storageKey;
            Values = values;
        }
            
     
        public string StorageKey { get; }
        public bool[] Values { get; }    
    }
}