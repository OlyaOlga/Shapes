﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using Shapes.Annotations;

namespace Shapes
{
    class ViewModelCanvas :
        INotifyPropertyChanged
    {
        private void _changeColor()
        {
            Color newColor = new Color();
            newColor.B = (byte) BColorValue;
            newColor.G = (byte) GColorValue;
            newColor.R = (byte) RColorValue;
            newColor.A = 255;
            CurrentColor = new SolidColorBrush(newColor);
        }

        public int _BColorValue;
        public int _GColorValue;
        public int _RColorValue;
        private Model _model;

        private void ShowMessageBox(object param)
        {
            if (!model.pentagons.Any(s => s.CanDrag))
            {
                Point mouseCoordinate = new Point(Mouse.GetPosition(param as IInputElement).X,
                    Mouse.GetPosition(param as IInputElement).Y);
                model.AddVertexToPentagon(mouseCoordinate, CurrentColor);
            }
        }

        public Model model
        {
            get { return _model; }
            set
            {
                _model = value;
                OnPropertyChanged("model");
            }
        }

        public ViewModelCanvas()
        {
            OutputMessageBoxCommand = new AbstractCommand(ShowMessageBox);
            model = new Model();
            PentagonMenuItemClick = new AbstractCommand(MenuItemClick);
            OpenCommand = new AbstractCommand(Open);
            SaveCommand = new AbstractCommand(Save);
            NewCommand = new AbstractCommand(New);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public AbstractCommand OutputMessageBoxCommand { get; set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private SolidColorBrush _currentColor = Brushes.Black;

        public SolidColorBrush CurrentColor
        {
            get { return _currentColor; }
            set
            {
                _currentColor = value;
                OnPropertyChanged(nameof(CurrentColor));
            }
        }

        public ICommand OpenCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand NewCommand { get; set; }

        public int BColorValue
        {
            get { return _BColorValue; }
            set
            {
                _BColorValue = value;
                _changeColor();
                OnPropertyChanged("BColorValue");
            }
        }

        public int GColorValue
        {
            get { return _GColorValue; }
            set
            {
                _GColorValue = value;
                _changeColor();
                OnPropertyChanged("GColorValue");
            }
        }

        public int RColorValue
        {
            get { return _RColorValue; }
            set
            {
                _RColorValue = value;
                _changeColor();
                OnPropertyChanged("RColorValue");
            }
        }

        public AbstractCommand PentagonMenuItemClick { get; set; }

        private void MenuItemClick(object parameter)
        {
            var p = parameter as Pentagon;
            EnableClickedItem(p);

            OnPropertyChanged(nameof(model.pentagons));
        }

        private void EnableClickedItem(Pentagon p)
        {
            foreach (var item in _model.pentagons)
            {
                if (p != item)
                {
                    item.CanDrag = false;
                    item.Radius = 0;
                }
                else
                {
                    item.Radius = (item.Radius + 15)%30;
                }
            }
        }

        private void Open(object parametr)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "XML (*.xml)|*.xml";
            var confirm = dialog.ShowDialog();
            if (confirm ?? false)
            {
                model = Model.Deserialize(dialog.FileName);
            }
        }

        private void Save(object parametr)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "XML (*.xml)|*.xml";
            var confirm = dialog.ShowDialog();
            if (confirm ?? false)
            {
                model.Serialize(dialog.FileName);
            }
        }

        private void New(object parametr)
        {
            model = new Model();

        }
    }
}
