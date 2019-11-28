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
    public class IT03_Light_With_Output
    {
        private IOutput _output;
        private ILight _sut;

        [SetUp]
        public void Setup()
        {
            _output = new Output();
            _sut = new Light(_output);
        }

        [Test]
        public void LightOnOutputLightOnTest()
        {
            string console;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            _sut.TurnOn();
            console = stringWriter.ToString();
            Assert.That(console, Is.EqualTo("Light is turned on\r\n"));
        }
        [Test]
        public void LightOffWhileOffOutputNothingTest()
        {
            string console;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            _sut.TurnOff();
            console = stringWriter.ToString();
            Assert.That(console, Is.EqualTo(""));
        }
        [Test]
        public void LightOffWhileOnOutputLightOffTest()
        {
            _sut.TurnOn();
            string console;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            _sut.TurnOff();
            console = stringWriter.ToString();
            Assert.That(console, Is.EqualTo("Light is turned off\r\n"));
        }
    }
}