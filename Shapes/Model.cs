using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class Model:
        INotifyPropertyChanged
    {
        private Pentagon currentPentagon;
        private int count;
        public ObservableCollection<Pentagon> pentagons { get; set; }

        public Pentagon CurrentPentagon
        {
            get { return currentPentagon; }
            set
            {
                currentPentagon = value;
                OnPropertyChanged("CurrentPentagon");
            }
        }

        public Model()
        {
            pentagons = new ObservableCollection<Pentagon>();
            CurrentPentagon = new Pentagon();
        }

        public void AddVertexToPentagon(Point vertex, Brush color)
        {
            CurrentPentagon.Add(vertex);
            if (CurrentPentagon.Count == 5)
            {
                CurrentPentagon.Name = $"Pentagon [{count++}]";
                CurrentPentagon.Color = color;
                pentagons.Add(CurrentPentagon);
                CurrentPentagon = new Pentagon();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
