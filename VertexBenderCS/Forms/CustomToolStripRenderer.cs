using System.Windows.Forms;

namespace VertexBenderCS
{
    //public class MainMenuRenderer : ToolStripProfessionalRenderer
    //{

    //    public MainMenuRenderer(ProfessionalColorTable table) : base(table)
    //    {

    //    }

    //    protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
    //    {
    //        //Rectangle rc = new Rectangle(Point.Empty, e.Item.Size + new Size(5000,20));
    //        //using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 39, 39, 39)))
    //        //    e.Graphics.FillRectangle(brush, rc);
    //    }
    //}

    // toolstrip borderları buglı olduğu için bunu kullandım.
    public class CustomToolStripRenderer : ToolStripSystemRenderer
    {
        public CustomToolStripRenderer() { }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            //base.OnRenderToolStripBorder(e);
        }
    }


}
