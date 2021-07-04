using Braveior.KubeAssist.Services.Models;


namespace Braveior.KubeAssist.Store.LoadData
{
    public class GlobalDataFetchDataResultAction
    {
        public string KNamespace { get; }


        public GlobalDataFetchDataResultAction(string knamespace)
        {
            KNamespace = knamespace;
        }
    }
}
