using Xunit;
using System.Linq;
using Newtonsoft.Json;
using Elements;
using Hypar.Functions.Execution.Local;
using Elements.Serialization.glTF;
using Elements.Serialization.JSON;


namespace MultiResidence.tests
{
    public class MultiResidenceTests
    {
        [Fact]
        public void MultiResidenceTest()
        {
            var inStore = new FileModelStore<MultiResidenceInputs>("../../../../", true);
            var inputs = new MultiResidenceInputs(new Hypar.Functions.Execution.InputData("../../../../parcel.csv"),
                                                  60, "", "", "", "", "");

            var model = new Model();
            var outputs = MultiResidence.Execute(model, inputs);
            System.IO.File.WriteAllText("../../../../MultiResidence.json", model.ToJson());
            var outStore = new FileModelStore<Hypar.Functions.Execution.ArgsBase>("../../../../", true);
            model.ToGlTF("../../../../MultiResidence.glb");
        }
    }
}
