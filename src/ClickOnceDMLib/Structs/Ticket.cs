using ClickOnceDMLib.Path;
using System.IO;
using System.Runtime.Serialization.Json;

namespace ClickOnceDMLib.Structs
{
    public class Ticket
    {
        public string Name;
        public Source Source
        {
            get
            {
                Source sourceInfo = new Source();
                using (FileStream stream = new FileStream(PathInfo.CombinePath(PathInfo.Ticket, this.Name), FileMode.Open))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Source));
                    sourceInfo = (Source)serializer.ReadObject(stream);
                }

                return sourceInfo;
            }
        }
    }
}
