using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Interfaces;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT02_DisplayWOutput
    {
        private IOutput output;
        private IDisplay display;

        [SetUp]
        public void Setup()
        {
            output = new Output();
            display = new Display(output);
        }

        [Test]
        public void DisplayShowTimeOutputTimeTest()
        {
            string console;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            display.ShowTime(1,2);
            console = stringWriter.ToString();
            Assert.That(console, Is.EqualTo("Display shows: 01:02\r\n"));
        }
        [Test]
        public void DisplayShowPowerOutputPowerTest()
        {
            string console;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            display.ShowPower(1);
            console = stringWriter.ToString();
            Assert.That(console, Is.EqualTo("Display shows: 1 W\r\n"));
        }
    }
}