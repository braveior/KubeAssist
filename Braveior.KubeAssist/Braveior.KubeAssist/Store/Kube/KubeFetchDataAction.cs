using Braveior.KubeAssist.Services.Models;

namespace Braveior.KubeAssist.Store.Kube
{
    public class KubeFetchDataAction
    {
        public KubeState KubeState;
        public KubeFetchDataAction(KubeState kubeState)
        {
            KubeState = kubeState;
        }
    }
}
