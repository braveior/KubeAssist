using Fluxor;

namespace Braveior.KubeAssist.Store.LoadData
{
	public class GlobalDataFeature : Feature<GlobalDataState>
	{
        public override string GetName() => "LoadDataState";
        protected override GlobalDataState GetInitialState() =>
            new GlobalDataState(
                knamespace: null
               );
    }
}
