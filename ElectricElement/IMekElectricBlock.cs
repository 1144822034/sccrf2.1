using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mekiasm
{
    interface IMekElectricBlock
    {
        //创建元素
        MekElectricElement createMekElement(SubsystemMekElectricSystem subsystemElectricity, int value, int x, int y, int z);


    }
}
