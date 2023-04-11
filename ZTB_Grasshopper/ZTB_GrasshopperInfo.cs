using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace ZTB_Grasshopper
{
    public class ZTB_GrasshopperInfo : GH_AssemblyInfo
    {
        public override string Name => "PointSorter";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("ca3975c4-30b9-4645-9bf9-49fd7307933d");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}