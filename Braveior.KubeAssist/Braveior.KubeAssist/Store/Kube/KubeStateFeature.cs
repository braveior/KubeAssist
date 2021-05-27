using Fluxor;

namespace Braveior.KubeAssist.Store.Kube
{
	public class KubeStateFeature : Feature<KubeStateState>
	{
        public override string GetName() => "KubeState";
        protected override KubeStateState GetInitialState() =>
            new KubeStateState(
                kubeState: null
               );
    }
}
