﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Shapes.Annotations;

namespace Shapes
{
    public class Pentagon:
        INotifyPropertyChanged
    {
        private PointCollection points ;
        private Brush _color;
        private int _radius;

        public string Name { get; set; }

        public int Radius
        {
            get { return _radius; }
            set
            {
                _radius = value; 
                OnPropertyChanged(nameof(Radius));
            }
        }

        public Brush Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        public bool CanDrag { get; set; }

        public PointCollection Points
        {
            get
            {
                return points;
            }
            set
            {
                points = value;
                OnPropertyChanged("Points");
            }
        }

        public Pentagon()
        {
            Points = new PointCollection();
        }

        public void Add(Point p)
        {
            Points.Add(p);
        }

        public int Count
        {
            get { return Points.Count; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
