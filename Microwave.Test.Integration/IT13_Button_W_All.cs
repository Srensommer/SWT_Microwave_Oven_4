using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT13_Button_W_All
    {
        private IOutput output;
        private IPowerTube powerTube;
        private IDisplay display;
        private ILight light;
        private ITimer timer1;
        private CookController cookController;
        private IUserInterface userInterface;
        private IButton powerButton, startCancelButton, timeButton;
        private IDoor door;


        [SetUp]
        public void Setup()
        {
            output = Substitute.For<IOutput>();
            powerTube = new PowerTube(output);
            display = new Display(output);
            light = new Light(output);
            timer1 = new Timer();
            cookController = new CookController(timer1, display, powerTube);
            door = new Door();
            powerButton = new Button();
            startCancelButton = new Button();
            timeButton = new Button();
            userInterface = new UserInterface(powerButton, timeButton, startCancelButton, door, display, light, cookController);
            cookController.UI = userInterface;
        }

        [Test]
        public void PressPowerButtonOncePrint50()
        {
            powerButton.Press();
            output.Received().OutputLine(Arg.Is<string>(x => x == "Display shows: 50 W"));
        }

        [Test]
        public void PressPowerButtonTwicePrint100()
        {
            powerButton.Press();
            powerButton.Press();
            output.Received().OutputLine(Arg.Is<string>(x => x == "Display shows: 100 W"));
        }


        [Test]
        public void PressTimeButtonOncePrint0100()
        {
            powerButton.Press();
            timeButton.Press();
            output.Received().OutputLine(Arg.Is<string>(x => x == "Display shows: 01:00"));
        }

        [Test]
        public void PressTimeButtonTwicePrint0200()
        {
            powerButton.Press();
            timeButton.Press();
            timeButton.Press();
            output.Received().OutputLine(Arg.Is<string>(x => x == "Display shows: 02:00"));
        }

        [Test] public void PressStartCancelButtonToStartCookingAfterPowerAndTimerPressed()
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            timer1.Expired += (sender, args) => pause.Set();

            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();


            output.Received().OutputLine(Arg.Is<string>(x => x == "Light is turned on"));
            output.Received().OutputLine(Arg.Is<string>(x => x == "PowerTube works with 7 %"));
            output.Received().OutputLine(Arg.Is<string>(x => x == "Display shows: 01:00"));
            output.ClearReceivedCalls();

            Thread.Sleep(1000*70);
            //Assert.That((pause.WaitOne(1000 * 125))); //60 sec bør være nok

           
            output.Received().OutputLine(Arg.Is<string>(x => x == "Display shows: 00:59"));
            output.Received().OutputLine(Arg.Is<string>(x => x == "Display shows: 00:01"));
            output.Received().OutputLine(Arg.Is<string>(x => x == "Display shows: 00:00"));
            output.Received().OutputLine(Arg.Is<string>(x => x == "PowerTube turned off"));
            output.Received().OutputLine(Arg.Is<string>(x => x == "Display cleared"));
            output.Received().OutputLine(Arg.Is<string>(x => x == "Light is turned off"));
        }

        [Test]
        public void PressStartCancelButtonToStopCooking()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            startCancelButton.Press();

            output.Received().OutputLine(Arg.Is<string>(x => x == "Display cleared"));
            output.Received().OutputLine(Arg.Is<string>(x => x == "Light is turned off"));
            output.Received().OutputLine(Arg.Is<string>(x => x == "PowerTube turned off"));
        }


        [Test]
        public void PressStartCancelButtonInPowerStateGoBackToReadyState()
        {
            powerButton.Press();
            startCancelButton.Press();

            output.Received().OutputLine(Arg.Is<string>(x => x == "Display cleared"));
        }
    }
}