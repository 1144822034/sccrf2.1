using Game;
using GameEntitySystem;
using System.Collections.Generic;
using TemplatesDatabase;
using Engine;
using System;

namespace Mekiasm
{
    //buff系统
    public class ComponentBuff:Component,IUpdateable
    {
        public ComponentPlayer componentPlayer;
        public List<mProgessWidget> mProgessWidgets = new List<mProgessWidget>();
        public StackPanelWidget buffView = new StackPanelWidget() { Direction=LayoutDirection.Horizontal,HorizontalAlignment=WidgetAlignment.Center};
        public int UpdateOrder => 999;
        public double lastTime = 0;
        public int maxtime = 20;
        public List<Buff> buffList = new List<Buff>();
        public float baseSpeed = 0f;
        public SubsystemTime subsystemTime;
        public class Buff {
            public int TotalTime;
            public int RemainTime;
            public BuffTYpe bufftype;
            public Func<bool> Task;
            public Action taskEnd;
        }        
        public enum BuffTYpe {
            N0, 
            N1, 
            HealthNumUp,
            MoveSpeedUp,
            N4,
            DenfendNumUP,
            N6,
            N7,
            N8,
            N9,
            N10,
            N11
        }
        public Buff makeBuff(int tTime,BuffTYpe buffTYpe,Func<bool> task=null,Action endT=null) {
            Buff buff = new Buff() {TotalTime=tTime,RemainTime=tTime,bufftype=buffTYpe,Task=task,taskEnd=endT};
            return buff;
        }
        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            componentPlayer = Entity.FindComponent<ComponentPlayer>();
            componentPlayer.ComponentGui.ControlsContainerWidget.Children.Add(buffView);
            subsystemTime = Project.FindSubsystem<SubsystemTime>();
            buffList.Add(makeBuff(maxtime,BuffTYpe.MoveSpeedUp,()=> {
                baseSpeed = componentPlayer.ComponentLocomotion.WalkSpeed;
                componentPlayer.ComponentLocomotion.WalkSpeed = baseSpeed * 2f;
                return true;
            },()=> { componentPlayer.ComponentLocomotion.WalkSpeed = baseSpeed; }));
        }
        public void circleTask() {
            buffList.ForEach((item)=> {
                --item.RemainTime;
                if (item.Task != null) if (!item.Task()) buffList.Remove(item);
                if (item.RemainTime <= 0) { item.taskEnd?.Invoke(); buffList.Remove(item); }
            });
        }
        public void Update(float dt)
        {
            buffView.Children.Clear();
            for(int i= 0; i < buffList.Count; i++) {
                BuffView abuffView = new BuffView();
                abuffView.setData(buffList[i]);
                buffView.Children.Add(abuffView);
            }
            if (subsystemTime.PeriodicGameTimeEvent(1,0)) {
                circleTask();
            }
        }
    }
}
