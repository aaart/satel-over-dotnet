namespace Sod.Model.Tasks.Types
{
    public class PersistaedStateUpdateTask : SatelTask
    {

        public PersistaedStateUpdateTask(string storageKey, bool[] values)
        {
            StorageKey = storageKey;
            Values = values;
        }
            
     
        public string StorageKey { get; }
        public bool[] Values { get; }    
    }
}