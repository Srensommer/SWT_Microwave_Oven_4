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
    public class IT03_LightWOutput
    {
        private IOutput output;
        private ILight light;

        [SetUp]
        public void Setup()
        {
            output = new Output();
            light = new Light(output);
        }

        [Test]
        public void LightOnOutputLightOnTest()
        {
            string console;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            light.TurnOn();
            console = stringWriter.ToString();
            Assert.That(console, Is.EqualTo("Light is turned on\r\n"));
        }
        [Test]
        public void LightOffWhileOffOutputNothingTest()
        {
            string console;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            light.TurnOff();
            console = stringWriter.ToString();
            Assert.That(console, Is.EqualTo(""));
        }
        [Test]
        public void LightOffWhileOnOutputLightOffTest()
        {
            light.TurnOn();
            string console;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            light.TurnOff();
            console = stringWriter.ToString();
            Assert.That(console, Is.EqualTo("Light is turned off\r\n"));
        }
    }
}