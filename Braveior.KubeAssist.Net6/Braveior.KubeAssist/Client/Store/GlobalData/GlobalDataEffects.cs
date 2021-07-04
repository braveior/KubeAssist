using Braveior.KubeAssist.Services.Models;
using Fluxor;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Braveior.KubeAssist.Store.LoadData
{
	public class GlobalDataEffects
	{


		public GlobalDataEffects()
		{

		}

		[EffectMethod]
		public async Task HandleAction(GlobalDataFetchDataAction action, IDispatcher dispatcher)
		{
			dispatcher.Dispatch(new GlobalDataFetchDataResultAction(action.KNamespace));
		}
        
    }
}
