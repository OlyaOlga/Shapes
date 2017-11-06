using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Drawing;
using System.Windows;
using System.Windows.Media;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestPentagonSerialization()
        {
            Pentagon p = new Pentagon();
            p.Add(new Point(3, 4));
            p.Add(new Point(4, 4));
            p.Add(new Point(3, 5));
            p.Color = Brushes.Coral;
            p.Serialize("serializeTest.xml");
        }

        [TestMethod]
        public void TestPentagonDeserealization()
        {
            Pentagon p = Pentagon.Deserialize("serializeTest.xml");
            Assert.IsTrue(p != null);
        }

        [TestMethod]
        public void TestModelSerialization()
        {
            Model model = new Model();
            Random rand = new Random();
            for (int i = 0; i < 5; ++i)
            {
                model.AddVertexToPentagon(new Point(rand.Next(1, 100), rand.Next(1, 100)), Brushes.Coral);
            }
            model.Serialize("collectionSerialize.xml");
        }
        
        [TestMethod]
        public void TestModelDeserealization()
        {
            Model p = Model.Deserialize("collectionSerialize.xml");
            Assert.IsTrue(p != null);
        }
    }
}
