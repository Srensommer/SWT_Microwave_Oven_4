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
    public class IT01_PowerTubeWOutput
    {
        private IOutput output;
        private PowerTube powerTube;

        [SetUp]
        public void Setup()
        {
            output = new Output();
            //output = Substitute.For<Output>();
            powerTube = new PowerTube(output);
        }

        [Test]
        public void TurnOn20Output20Test()
        {
            string console;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            powerTube.TurnOn(20);
            console = stringWriter.ToString();
            Assert.That(console, Is.EqualTo("PowerTube works with 20 %\r\n"));
        }
        [Test]
        public void TurnOffOutputNothingTest()
        {
            string console;
            StringWriter stringWriter = new StringWriter();
            powerTube.TurnOff();
            console = stringWriter.ToString();
            Assert.That(console, Is.EqualTo(""));
        }
        [Test]
        public void TurnOn20TurnOffOutput20ThenTurnedOffTest()
        {
            string console;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            powerTube.TurnOn(20);
            powerTube.TurnOff();
            console = stringWriter.ToString();
            Assert.That(console, Is.EqualTo("PowerTube works with 20 %\r\nPowerTube turned off\r\n"));
        }



        //TODO: Denne og de to næste tests melder tilbage uanset input, at testen er gået godt... (husk at bruge substitute til disse)
        [Test]
        public void TurnOn30Output30Test()
        {
            //TODO: Bruger en rigtig output pt.. Burde vi bruge en stub output, som disse gør??
            powerTube.TurnOn(30);
            output.Received().OutputLine("PowerTube works with 30 %\r\n");
        }
        [Test]
        public void TurnOffOutputNothingTest2()
        {
            powerTube.TurnOff();
            output.Received().OutputLine("");
        }
        [Test]
        public void TurnOn30TurnOffOutputTest()
        {
            powerTube.TurnOn(30);
            powerTube.TurnOff();
            output.Received().OutputLine("PowerTube works with 30 %\r\nPowerTube turned off\r\n");
        }
    }
}