using Game;
using Engine;
using System;
using GameEntitySystem;
namespace Mekiasm
{
    public class MekElectricElement:IElectricItem
    {
        public Point3 point;//当前点
        public bool visited;//是否浏览过
        public MekElectricElement[] Connections = new MekElectricElement[6];//附近六个方块的配置
        public bool isNeedSimulate = true;//是否需要模拟
        public int Step = 0;//执行到第几帧了
        public Entity entity;
        public int power = 0;//电量
        public SubsystemMekElectricSystem SubsystemMekElectricSystem;
        public enum Mode { OUTPUT, INPUT, INPUTOUTPUT, WIRE };
        public Mode mode;//设备当前模式        
        public MekElectricElement(SubsystemMekElectricSystem subsystemMekElectricSystem, int x,int y,int z) {
            point = new Point3(x,y,z);
            SubsystemMekElectricSystem = subsystemMekElectricSystem;
        }
        public T findComponent<T> () where T : class{
            return SubsystemMekElectricSystem.Project.FindSubsystem<SubsystemBlockEntities>().GetBlockEntity(point.X, point.Y, point.Z).Entity.FindComponent<T>();
        }
        /// <summary>
        /// 从用电设备取数值
        /// </summary>
        /// <param name="now">当前值</param>
        /// <param name="total">来源总计</param>
        /// <param name="each">每次取多少</param>
        /// <param name="max">最大容量</param>
        /// <returns></returns>
        public int getAvaiablePower(int now,int each,int max,int total) {
            int remain = max - now;//获取剩余要取多少
            if (remain >= each)
            {
                if (total > each) return each;
                else return 0;
            }
            else if (remain < each) return 0;
            return 0;
        }
        public virtual bool Simulate()
        {
            return false;
        }
    }
}
