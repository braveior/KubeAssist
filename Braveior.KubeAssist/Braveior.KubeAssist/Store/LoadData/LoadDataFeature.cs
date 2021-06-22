using Fluxor;

namespace Braveior.KubeAssist.Store.LoadData
{
	public class LoadDataFeature : Feature<LoadDataState>
	{
        public override string GetName() => "LoadDataState";
        protected override LoadDataState GetInitialState() =>
            new LoadDataState(
                knamespace: null
               );
    }
}
