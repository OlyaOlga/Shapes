using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml.Serialization;
using Shapes.Annotations;

namespace Shapes
{
    public class Model:
        INotifyPropertyChanged
    {
        private Pentagon currentPentagon = new Pentagon();
        private int count;
        private ObservableCollection<Pentagon> _pentagons;
        
        public ObservableCollection<Pentagon> pentagons
        {
            get { return _pentagons; }
            set
            {
                _pentagons = value;
                OnPropertyChanged(nameof(pentagons));
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        }

        public void AddVertexToPentagon(Point vertex, SolidColorBrush color)
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
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Serialize(string fileName)
        {
            using (var stream = File.Open(fileName, FileMode.OpenOrCreate))
            {
                stream.SetLength(0);
                XamlWriter.Save(this, stream);
            }
        }

        public static Model Deserialize(string fileName)
        {
            Model result=null;
            using (var stream = File.OpenRead(fileName))
            {
                result = (XamlReader.Load(stream) as Model);
            }
            return result;
        }
    }
}
