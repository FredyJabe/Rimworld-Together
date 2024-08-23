namespace GameServer
{
    [Serializable]
    public class SiteFile
    {
        public int tile;

        public string owner;

        public string type;
        
        public int limit;

        public byte[] workerData;

        public bool isFromFaction;

        public string factionName;
    }
}
