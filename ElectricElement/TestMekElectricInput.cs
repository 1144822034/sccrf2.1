using Engine;
using Game;
namespace Mekiasm
{
    public class TestMekElectricInput : MekElectricElement
    {
        public ComponentBaseEnergy component;
        //模拟

        public TestMekElectricInput(SubsystemMekElectricSystem subsystemMekElectricSystem, int x, int y, int z,Mode _mode):base(subsystemMekElectricSystem,x,y,z) {
            mode = _mode;
        }
        public override bool Simulate()
        {
            if (component == null) component = findComponent<ComponentBaseEnergy>();
            foreach (MekElectricElement mekElectricElement in Connections) {
                if (mekElectricElement != null) {
                    if (mekElectricElement.power > 0 && mekElectricElement.mode != Mode.INPUT) {
                        component.quantity += getAvaiablePower(component.quantity,32,component.maxquantity,mekElectricElement.power);
                    }
                }
            }
            return true;
        }
    }
}
