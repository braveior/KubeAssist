using Braveior.KubeAssist.Services.Models;

namespace Braveior.KubeAssist.Store.LoadData
{
    public class LoadDataFetchDataAction
    {
        public string KNamespace;
        public LoadDataFetchDataAction(string knamespace)
        {
            KNamespace = knamespace;
        }
    }
}
