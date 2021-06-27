using Braveior.KubeAssist.Services.Models;


namespace Braveior.KubeAssist.Store.LoadData
{
    public class LoadDataFetchDataResultAction
    {
        public string KNamespace { get; }


        public LoadDataFetchDataResultAction(string knamespace)
        {
            KNamespace = knamespace;
        }
    }
}
