using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Shapes
{
    public class DragBehavior
    {
        public static readonly DependencyProperty IsDragProperty = DependencyProperty.RegisterAttached("Drag", typeof(bool), typeof(DragBehavior), new PropertyMetadata(false, OnDragChanged));

        private Point elementStartPosition2;

        private Point mouseStartPosition2;

        public static DragBehavior Instance { get; set; } = new DragBehavior();

        public static List<UIElement> UiElements { get; set; } = new List<UIElement>();

        public static List<bool> Bools { get; set; } = new List<bool>();

        public TranslateTransform Transform { get; set; } = new TranslateTransform();

        public static bool GetDrag(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDragProperty);
        }

        public static void SetDrag(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDragProperty, value);
        }

        private static void OnDragChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var element = (UIElement)sender;
            var isDrag = (bool)e.NewValue;
            
            if (isDrag)
            {
                element.MouseLeftButtonDown += Instance.ElementOnMouseLeftButtonDown;
                element.MouseLeftButtonUp += Instance.ElementOnMouseLeftButtonUp;
                element.MouseMove += Instance.ElementOnMouseMove;
            }
            else
            {
                element.MouseLeftButtonDown -= Instance.ElementOnMouseLeftButtonDown;
                element.MouseLeftButtonUp -= Instance.ElementOnMouseLeftButtonUp;
                element.MouseMove -= Instance.ElementOnMouseMove;
            }
        }

        private void ElementOnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var parent = Application.Current.MainWindow;
            mouseStartPosition2 = mouseButtonEventArgs.GetPosition(parent);
            ((UIElement)sender).CaptureMouse();
            var p = sender as Polygon;
            Startcoll = new PointCollection();
            foreach (var point in p.Points)
            {
                Startcoll.Add(new Point(point.X, point.Y));
            }
        }

        private void ElementOnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            ((UIElement)sender).ReleaseMouseCapture();
            elementStartPosition2.X = Transform.X;
            elementStartPosition2.Y = Transform.Y;
        }

        public PointCollection Startcoll { get; set; }

        private void ElementOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            var parent = Application.Current.MainWindow;
            var mousePos = mouseEventArgs.GetPosition(parent);
            var diff = mousePos - mouseStartPosition2;
            if (!((UIElement)sender).IsMouseCaptured)
            {
                return;
            }
            
            var p = sender as Polygon;
            PointCollection newEdges = new PointCollection();
            Debug.Assert(p != null, "p != null");
            for (int index = 0; index < p.Points.Count; index++)
            {
                var point = p.Points[index];
                newEdges.Add(new Point(Startcoll[index].X  +diff.X, Startcoll[index].Y+ diff.Y));
                Console.WriteLine(p.Points[index].X);
            }
            p.Points = newEdges;
            var bindingExpression = BindingOperations.GetBindingExpression(sender as Polygon, Polygon.PointsProperty);
            bindingExpression?.UpdateSource();
        }
    }
}