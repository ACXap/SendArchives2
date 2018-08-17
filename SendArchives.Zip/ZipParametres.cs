using System.Collections.Generic;

namespace SendArchives.Zip
{
    public class ZipParametres
    {
        public List<string> CollectionFiles { get; set; }
        public string NameArchive { get; set; }
        public string Password { get; set; }
        public int SizePart { get; set; }
        public string PathFolderForArchive { get; set; }
    }
}