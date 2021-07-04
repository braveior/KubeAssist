using Braveior.KubeAssist.Services.Models;

namespace Braveior.KubeAssist.Store.LoadData
{
    public class GlobalDataFetchDataAction
    {
        public string KNamespace;
        public GlobalDataFetchDataAction(string knamespace)
        {
            KNamespace = knamespace;
        }
    }
}
