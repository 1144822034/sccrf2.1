using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mekiasm
{
    public class MekWireElement: MekElectricElement
    {
        public int MaxPower = 32;
        public MekWireElement(SubsystemMekElectricSystem subsystemMekElectricSystem, int x, int y, int z,int maxpower) : base(subsystemMekElectricSystem, x, y, z) {
            MaxPower = maxpower;
            mode = Mode.INPUTOUTPUT;
        }
        public override bool Simulate()
        {
            foreach (MekElectricElement mekElectricElement in Connections)
            {
                if (mekElectricElement != null)
                {
                    if (mekElectricElement.power > 0 && mekElectricElement.mode != Mode.INPUT)
                    {
                        //从周围的输出设备取电
                        power += getAvaiablePower(power,MaxPower,MaxPower,mekElectricElement.power);
                    }
                }
            }
            return base.Simulate();
        }
    }
}
