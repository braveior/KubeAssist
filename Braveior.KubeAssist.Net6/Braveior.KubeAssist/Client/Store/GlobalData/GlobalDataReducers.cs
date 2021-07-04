using Fluxor;
using Braveior.KubeAssist.Services.Models;
namespace Braveior.KubeAssist.Store.LoadData
{
	public class GlobalDataReducers
	{
		[ReducerMethod]
		public static GlobalDataState ReduceAction(GlobalDataState state, GlobalDataFetchDataAction action) =>
	new GlobalDataState(
		knamespace: null
		);

		[ReducerMethod]
		public static GlobalDataState ReduceResultAction(GlobalDataState state, GlobalDataFetchDataResultAction action) =>
			new GlobalDataState(
				knamespace: action.KNamespace);
	}
}
