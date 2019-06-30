using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace WPFBehavior.Client
{
    /// <summary>
    /// Behavior类使用类型参数
    /// 可使用该类型参数将行为限制到特定的元素，或使用UIElement或FrameworkElement
    /// </summary>
    public class DragInCanvasBehavior:Behavior<UIElement>
    {
        /// <summary>
        /// 跟踪画布中该元素的位置
        /// </summary>
        private Canvas canvas;
        /// <summary>
        /// 跟踪元素何时被拖动
        /// </summary>
        private bool isDragging = false;
        /// <summary>
        /// 当单击元素时，记录单击的确切位置
        /// </summary>
        private Point mouseOffset;
        protected override void OnAttached()
        {
            //监听事件
            this.AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            this.AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            this.AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;
        }
        void AssociatedObject_MouseLeftButtonDown (object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //根据可视化树帮助类不断向上级查找，知道找到Canvas控件
            if (canvas != null)    
                canvas = (Canvas)VisualTreeHelper.GetParent(this.AssociatedObject);
            isDragging = true;
            mouseOffset = e.GetPosition(AssociatedObject);
            AssociatedObject.CaptureMouse();
        }
        /// <summary>
        /// 当元素处于拖动模式并移动鼠标时，重新定位元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(isDragging)
            {
                Point point = e.GetPosition(canvas);
                AssociatedObject.SetValue(Canvas.TopProperty,point.Y-mouseOffset.Y);
                AssociatedObject.SetValue(Canvas.LeftProperty, point.X - mouseOffset.X);
            }
        }
        /// <summary>
        /// 当释放鼠标键时，结束拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AssociatedObject_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(isDragging)
            {
                AssociatedObject.ReleaseMouseCapture();
                isDragging = false;
            }
        }
    }
}
