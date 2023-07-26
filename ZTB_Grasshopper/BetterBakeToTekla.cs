using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;
using Rhino;

namespace ZTB_Grasshopper
{
    public class BetterBakeToTekla : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the BetterBakeToTekla class.
        /// </summary>
        public BetterBakeToTekla()
          : base("BetterBakeToTekla", "BBtT",
              "Description",
              "ZTB", "Utility")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Component", "C", "Tekla components to bake", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Run","R","True to bake",GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        private bool _resetted;
        private GH_Document.SolutionEndEventHandler _solutionEndHandle;
        private readonly RhinoDoc RhinoDocument;

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            System.Object Component = null;
            DA.GetData(0, ref Component);

            bool run = false;
            DA.GetData(1, ref run);
            string command = "Bake to Tekla";
            
            if (!run)
            {
                // Print("Set 'Run' to 'True' to execute the command");
                _resetted = true;
                return;
            }
            if (!_resetted)
            {
                //Print("Toggle 'Run' to execute the command");
                return;
            }
            GH_Document GrasshopperDocument = Grasshopper.Instances.ActiveCanvas.Document;
            var commandStart = command.Trim().ToUpper();


            var comp = this;
            var input = comp.Params.Input[comp.Params.IndexOfInputParam("Component")];

            var menuItemsToClick = new List<ToolStripMenuItem>();

            foreach (var source in input.Sources)
            {
                var menu = new ToolStripDropDown();

                var sourceComponent = comp.OnPingDocument().FindComponent(source.Attributes.GetTopLevel.DocObject.InstanceGuid);
                if (sourceComponent != null)
                {
                    (sourceComponent as IGH_Component).AppendMenuItems(menu);
                }
                else if (source is IGH_Param)
                {
                    (source as IGH_Param).AppendMenuItems(menu);
                }

                foreach (var item in menu.Items)
                {
                    if (item is ToolStripMenuItem && (item as ToolStripMenuItem).Text.ToUpper().StartsWith(commandStart))
                    {
                        menuItemsToClick.Add(item as ToolStripMenuItem);
                        break; // only add the first match
                    }
                }
            }

            _solutionEndHandle = delegate (System.Object sender, GH_SolutionEventArgs e)
            {
                // first get rid of our handler again            
                GrasshopperDocument.SolutionEnd -= _solutionEndHandle;

                RhinoApp.InvokeOnUiThread((MethodInvoker)
                  (
                  () =>
                  {
                      foreach (var item in menuItemsToClick)
                          item.PerformClick();
                  }
                  )
                  );
            };

            comp.OnPingDocument().SolutionEnd += _solutionEndHandle;

            //Print("Command executed");

            _resetted = false;
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("EED18630-C127-4A72-80CC-16985605F3F6"); }
        }
    }
}