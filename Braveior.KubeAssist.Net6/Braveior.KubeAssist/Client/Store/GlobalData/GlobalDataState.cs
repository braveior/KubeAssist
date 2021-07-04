using Braveior.KubeAssist.Services.Models;

namespace Braveior.KubeAssist.Store.LoadData
{
	public class GlobalDataState
	{
		public string KNamespace { get; }


		public GlobalDataState(string knamespace)
		{
			KNamespace = knamespace;
		}
	}
}
