using Braveior.KubeAssist.Services.Models;
using Fluxor;
using Nest;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Braveior.KubeAssist.Store.LoadData
{
	public class LoadDataEffects
	{


		public LoadDataEffects()
		{

		}

		[EffectMethod]
		public async Task HandleAction(LoadDataFetchDataAction action, IDispatcher dispatcher)
		{
			dispatcher.Dispatch(new LoadDataFetchDataResultAction(action.KNamespace));
		}
        
    }
}
