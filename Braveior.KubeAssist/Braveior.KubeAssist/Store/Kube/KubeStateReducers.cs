using Fluxor;
using Braveior.KubeAssist.Services.Models;
namespace Braveior.KubeAssist.Store.Kube
{
	public class KubeStateReducers
	{
		[ReducerMethod]
		public static KubeStateState ReduceAction(KubeStateState state, KubeFetchDataAction action) =>
	new KubeStateState(
		kubeState: null
		);

		[ReducerMethod]
		public static KubeStateState ReduceResultAction(KubeStateState state, KubeFetchDataResultAction action) =>
			new KubeStateState(
				kubeState: action.KubeState);
	}
}
