using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
 [TestFixture]
        public class IT07_CookControllerWUI
        {
            private CookController cookController;
            private IUserInterface userInterface;
            private IOutput output;
            private IDisplay display;
            private IButton powerButton;
            private IButton timeButton;
            private IButton startCancelButton;
            private IDoor door;
            private ILight light;
            private IPowerTube powerTube;
            private ITimer timer;

            [SetUp]
            public void SetUp()
            { 
                output = Substitute.For<IOutput>();
                light = Substitute.For<ILight>();
                powerButton = Substitute.For<IButton>();
                timeButton = Substitute.For<IButton>();
                startCancelButton = Substitute.For<IButton>(); 
                door = Substitute.For<IDoor>();
                timer = new MicrowaveOvenClasses.Boundary.Timer();
                display = new Display(output);
                powerTube = new PowerTube(output);
                cookController = new CookController(timer, display, powerTube);
                userInterface = new UserInterface(powerButton, timeButton, startCancelButton, door, display, light, cookController);
                cookController.UI = userInterface;
            }
            [Test]
            public void OnTimeExpiredOutputsCorrect()
            {
                powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
                timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
                startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
                cookController.OnTimerExpired(this, EventArgs.Empty);
                output.Received().OutputLine("Display cleared");
                //TODO: burde jeg tjekke for "Light turned off" istedet
            }
        }
}
