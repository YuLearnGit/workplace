using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DragDrop
{
    public class DragDropAdorner : Adorner
    {
        public DragDropAdorner(UIElement parent)
            : base(parent)
        {
            IsHitTestVisible = false; // Seems Adorner is hit test visible?
            mDraggedElement = parent as FrameworkElement;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (mDraggedElement != null)
            {
                Win32.POINT screenPos = new Win32.POINT();
                if (Win32.GetCursorPos(ref screenPos))
                {
                    Point pos = PointFromScreen(new Point(screenPos.X, screenPos.Y));
                    Rect rect = new Rect(pos.X, pos.Y, mDraggedElement.ActualWidth, mDraggedElement.ActualHeight);
                    drawingContext.PushOpacity(1.0);
                    Brush highlight = mDraggedElement.TryFindResource(SystemColors.HighlightBrushKey) as Brush;
                    if (highlight != null)
                        drawingContext.DrawRectangle(highlight, new Pen(Brushes.Transparent, 0), rect);
                    drawingContext.DrawRectangle(new VisualBrush(mDraggedElement),
                        new Pen(Brushes.Transparent, 0), rect);
                    drawingContext.Pop();
                }
            }
        }

        FrameworkElement mDraggedElement = null;
    }

    public static class Win32
    {
        public struct POINT { public Int32 X; public Int32 Y; }

        // During drag-and-drop operations, the position of the mouse cannot be 
        // reliably determined through GetPosition. This is because control of 
        // the mouse (possibly including capture) is held by the originating 
        // element of the drag until the drop is completed, with much of the 
        // behavior controlled by underlying Win32 calls. As a workaround, you 
        // might need to use Win32 externals such as GetCursorPos.
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref POINT point);
    }
}
