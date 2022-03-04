namespace Sod.Model.Tasks.Types
{
    public class StorageUpdateTask : SatelTask
    {

        public StorageUpdateTask(string storageKey, bool[] values)
        {
            StorageKey = storageKey;
            Values = values;
        }
            
     
        public string StorageKey { get; }
        public bool[] Values { get; }    
    }
}