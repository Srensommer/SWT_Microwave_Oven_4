using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT01_PowerTube_With_Output
    {
        private IOutput _output;
        private IPowerTube _sut;

        [SetUp]
        public void Setup()
        {
            _output = new Output();
            _sut = new PowerTube(_output);
        }

        [Test]
        public void TurnOn20Output20Test()
        {
            string console;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            _sut.TurnOn(20);
            console = stringWriter.ToString();
            Assert.That(console, Is.EqualTo("PowerTube works with 20 %\r\n"));
        }
        [Test]
        public void TurnOffOutputNothingTest()
        {
            string console;
            StringWriter stringWriter = new StringWriter();
            _sut.TurnOff();
            console = stringWriter.ToString();
            Assert.That(console, Is.EqualTo(""));
        }
        [Test]
        public void TurnOn20TurnOffOutput20ThenTurnedOffTest()
        {
            string console;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            _sut.TurnOn(20);
            _sut.TurnOff();
            console = stringWriter.ToString();
            Assert.That(console, Is.EqualTo("PowerTube works with 20 %\r\nPowerTube turned off\r\n"));
        }
    }
}