using Fluxor;
using Braveior.KubeAssist.Services.Models;
namespace Braveior.KubeAssist.Store.LoadData
{
	public class LoadDataReducers
	{
		[ReducerMethod]
		public static LoadDataState ReduceAction(LoadDataState state, LoadDataFetchDataAction action) =>
	new LoadDataState(
		knamespace: null
		);

		[ReducerMethod]
		public static LoadDataState ReduceResultAction(LoadDataState state, LoadDataFetchDataResultAction action) =>
			new LoadDataState(
				knamespace: action.KNamespace);
	}
}
