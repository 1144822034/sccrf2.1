using System.Collections.Generic;
using Game;
using GameEntitySystem;
using Engine;
using TemplatesDatabase;

namespace Mekiasm
{
    public class SubsystemMekElectricSystem : SubsystemBlockBehavior, IUpdateable
    {
        //电路系统构建尝试
        public override int[] HandledBlocks => new int[] { 
        1003
        };
        public Dictionary<Point3, bool> update_points = new Dictionary<Point3, bool>();
        public Dictionary<Point3, MekElectricElement> update_elements = new Dictionary<Point3, MekElectricElement>();
        public int nowStep = 0;
        public int UpdateOrder => 20;

        //添加一个电路元件
        public void addElement(Point3 point3, MekElectricElement mekElectricElement)
        {
            if (!update_points.ContainsKey(point3))
            {
                update_points.Add(point3, true);
                update_elements.Add(point3,mekElectricElement);
            }
            updateConnections(point3);
        }
        public override bool OnUse(Vector3 start, Vector3 direction, ComponentMiner componentMiner)
        {
            return base.OnUse(start, direction, componentMiner);
        }
        public override void OnBlockAdded(int value, int oldValue, int x, int y, int z)
        {
            int id = Terrain.ExtractContents(value);
            ItemElectricBlock itemElectricBlock = BlocksManager.Blocks[id] as ItemElectricBlock;
            MekElectricElement mekElectricElement = itemElectricBlock.createMekElement(this, value, x, y, z);
            SubsystemPlayers subsystemPlayers = Project.FindSubsystem<SubsystemPlayers>();
            mekElectricElement.entity = subsystemPlayers.m_componentPlayers[0].Entity;
            if (itemElectricBlock!=null)addElement(new Point3(x,y,z),mekElectricElement);

        }
        public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
        {
            removeElement(new Point3(x,y,z));
        }
        //从某点开始更新连接点
        public void updateConnections(Point3 pointStart) {
            //从pointStart开始更新所有连接点
            update_elements[pointStart].visited = true;//设定这个点已经遍历过
            for (int i=0;i<6;i++) {
                //找到这个方向的点，并查找列表里面是否存在，如果存在，则更新这个点的Connections，同时更新下一个点的Connections
                Point3 point3 = ILibrary.getNextPointByFace(pointStart,i);
                if (update_elements.ContainsKey(point3)) {
                    update_elements[pointStart].Connections[i]=update_elements[point3];
                    if (!update_elements[point3].visited)updateConnections(point3);
                }
            }

        }
        public void removeElement(Point3 point3) {
            if (update_points.ContainsKey(point3)) {
                update_points.Remove(point3);
                update_elements.Remove(point3);
            }
            //删除与此相关的所有连接点
            foreach (KeyValuePair<Point3,MekElectricElement> item in update_elements) {
                for (int i=0;i<6;i++) {
                    if (item.Value.Connections[i] != null) {
                        if (item.Value.Connections[i].point == point3) update_elements[item.Key].Connections[i]=null;
                    }
                }
            }
        }
        public override void Load(ValuesDictionary valuesDictionary)
        {
            base.Load(valuesDictionary);
        }
        public override void Save(ValuesDictionary valuesDictionary)
        {
            base.Save(valuesDictionary);
        }
        //帧更新
        public void Update(float dt)
        {
            foreach (KeyValuePair<Point3, bool> item in update_points) {
                if (item.Value)
                {
                    if (update_elements[item.Key] == null) continue;
                    if (update_elements[item.Key].isNeedSimulate) {
                        //如果Simulate返回false则不让继续Simulate
                        if (!update_elements[item.Key].Simulate()){
                            update_elements[item.Key].isNeedSimulate=false;
                        }
                    }
                }
            }
            ++nowStep;
        }
    }
}
