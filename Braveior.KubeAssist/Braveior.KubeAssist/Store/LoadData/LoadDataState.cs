using Braveior.KubeAssist.Services.Models;

namespace Braveior.KubeAssist.Store.LoadData
{
	public class LoadDataState
	{
		public string KNamespace { get; }


		public LoadDataState(string knamespace)
		{
			KNamespace = knamespace;
		}
	}
}
