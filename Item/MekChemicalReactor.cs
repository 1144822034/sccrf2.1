﻿using Game;
namespace Mekiasm
{
    class MekChemicalReactor:Item
    {
        public MekChemicalReactor(int d, string n) : base(d, n) { }
        public override int GetFaceTextureSlot(int face, int value)
        {
            int[] maps = new int[] { 121, 87, 81, 87, 88, 81 };
            return ILibrary.GetFaceTextureSlot(face, value, maps);
        }
        public override bool IsInteractive(SubsystemTerrain subsystemTerrain, int value)
        {
            return true;
        }
        public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
        {
            return DisplayName;
        }
    }
}
