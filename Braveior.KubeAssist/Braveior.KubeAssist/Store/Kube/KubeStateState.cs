using Braveior.KubeAssist.Services.Models;

namespace Braveior.KubeAssist.Store.Kube
{
	public class KubeStateState
	{
		public KubeState KubeState { get; }


		public KubeStateState(KubeState kubeState)
		{
			KubeState = kubeState;
		}
	}
}
