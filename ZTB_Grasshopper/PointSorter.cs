using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ZTB_Grasshopper
{
    public class PointSorter : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public PointSorter()
          : base("PointSorter", "PS",
            "Sorting points by its coordinates",
            "ZTB", "Utility")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "Points to sort", GH_ParamAccess.list);
            pManager.AddTextParameter("First coordinate", "FC", "First coordinate to sort", GH_ParamAccess.item);
            pManager.AddTextParameter("Second coordinate", "SC", "Second coordinate to sort", GH_ParamAccess.item);
            pManager.AddTextParameter("Third coordinate", "TC", "Third coordinate to sort", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Sorted points", "SP", "Sorted points", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> points = new List<Point3d>();
            string firstCoordinate = string.Empty;
            string secondCoordinate = string.Empty;
            string thirdCoordinate = string.Empty;
            DA.GetDataList(0, points);
            DA.GetData(1, ref firstCoordinate);
            DA.GetData(2, ref secondCoordinate);
            DA.GetData(3, ref thirdCoordinate);

            Point3d point3D = new Point3d();
            List<Point3d> sortedPoints = new List<Point3d>();

            var firstPropertyInfo = point3D.GetType().GetProperty(firstCoordinate);
            if (secondCoordinate != string.Empty)
            {
                var secondPropertyInfo = point3D.GetType().GetProperty(secondCoordinate);
                if (thirdCoordinate != string.Empty)
                {
                    var thirdPropertyInfo = point3D.GetType().GetProperty(thirdCoordinate);
                    sortedPoints = points.OrderBy(p => firstPropertyInfo.GetValue(p, null)).ThenBy(p => secondPropertyInfo.GetValue(p, null)).ThenBy(p => thirdPropertyInfo.GetValue(p, null)).ToList();
                }
                else
                {
                    sortedPoints = points.OrderBy(p => firstPropertyInfo.GetValue(p, null)).ThenBy(p => secondPropertyInfo.GetValue(p, null)).ToList();
                }
            }
            else
            {
                sortedPoints = points.OrderBy(p => firstPropertyInfo.GetValue(p)).ToList();
            }

            DA.SetDataList(0, sortedPoints);

        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => null;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("4861eb6c-4053-4fd0-882b-192e6d6bc3bc");
    }
}