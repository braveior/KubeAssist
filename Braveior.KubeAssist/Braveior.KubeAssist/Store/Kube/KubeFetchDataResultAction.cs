using Braveior.KubeAssist.Services.Models;


namespace Braveior.KubeAssist.Store.Kube
{
    public class KubeFetchDataResultAction
    {
        public KubeState KubeState { get; }


        public KubeFetchDataResultAction(KubeState kubeState)
        {
            KubeState = kubeState;
        }
    }
}
