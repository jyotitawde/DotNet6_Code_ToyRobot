using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyRobot.Engine.Enums;
using ToyRobot.Engine.Handlers;
using ToyRobot.Engine.Models;

namespace ToyRobot.Engine.Test
{
    public class CommandHandlerTest
    {



        [Theory]
        [InlineData("PLACE 1,2,NORTH")]
        [InlineData("MoVe")]
        [InlineData("left")]
        [InlineData("righT")]
        [InlineData("Report")]
        public void IsValidCommand(string cmd)
        {
            //Arrange
            var tableSurface = new TableSurface(6, 6);
            var robot = new Robot();
            robot.IsPlaced = false;
            var commandHandler = new CommandHandler(robot, tableSurface);

            //command
            var result = commandHandler.ExecuteCommand(cmd);

            //Assert
            Assert.IsNotType<ArgumentException>(result);
        }


        [Theory]
        [InlineData("")]
        [InlineData("a a a")]
        [InlineData("PLACE x,y,NORTH")]
        public void IsNotValidCommand(string cmd)
        {
            //Arrange
            var tableSurface = new TableSurface(6, 6);
            var robot = new Robot();
            robot.IsPlaced = false;
            var commandHandler = new CommandHandler(robot, tableSurface);

            //command
            var argumentException =  
                Assert.Throws<ArgumentException>(() => 
                    commandHandler.ExecuteCommand(cmd));

            //Assert
            Assert.IsType<ArgumentException>(argumentException);
        }

        [Fact]
        public void IsValidPlaceCommand()
        {
            //Arrange
            var tableSurface = new TableSurface(6, 6);
            var robot = new Robot();
            robot.IsPlaced = false;
            var commandHandler = new CommandHandler(robot, tableSurface);

            //Act
            commandHandler.ExecuteCommand("PLACE 1,2,EAST");
            
            //Assert
            Assert.NotNull(robot.Location);
        }

        [Fact]
        public void IsNotValidPlaceCommand()
        {
            //Arrange
            var tableSurface = new TableSurface(6, 6);
            var robot = new Robot();
            robot.IsPlaced = false;
            var commandHandler = new CommandHandler(robot, tableSurface);

            //command
            commandHandler.ExecuteCommand("PLACE 2,6,EAST");

            //Assert
            Assert.Null(robot.Location);
        }

        [Fact]
        public void IsValidReport()
        {
            //Arrange
            var tableSurface = new TableSurface(6, 6);
            var robot = new Robot();
            robot.IsPlaced = false;
            var commandHandler = new CommandHandler(robot, tableSurface);

            //command
            commandHandler.ExecuteCommand("PLACE 2,1,EAST");
            commandHandler.ExecuteCommand("MOVE");
            commandHandler.ExecuteCommand("LEFT");
            commandHandler.ExecuteCommand("MOVE");
            commandHandler.ExecuteCommand("Right");
            commandHandler.ExecuteCommand("Right");
            commandHandler.ExecuteCommand("Move");
            commandHandler.ExecuteCommand("Move");
            commandHandler.ExecuteCommand("Report");

            //Assert
            Assert.Equal("Output: 3,0,SOUTH", commandHandler.GetOutput());
        }

        [Fact]
        public void IsNotValidMove()
        {
            //Arrange
            var tableSurface = new TableSurface(6, 6);
            var robot = new Robot();
            robot.IsPlaced = false;
            var commandHandler = new CommandHandler(robot, tableSurface);

            //command
            commandHandler.ExecuteCommand("PLACE 2,1,EAST");
            commandHandler.ExecuteCommand("MOVE");
            commandHandler.ExecuteCommand("MOVE");
            commandHandler.ExecuteCommand("MOVE");

            //all move command beyond this line will be discarded
            commandHandler.ExecuteCommand("MOVE");
            commandHandler.ExecuteCommand("MOVE");
            commandHandler.ExecuteCommand("MOVE");

            //Assert
            Assert.NotEqual("Output: 8,1,EAST", commandHandler.GetOutput());
        }

        [Fact]
        public void IsValidPlaceWithoutDirection()
        {
            //Arrange
            var tableSurface = new TableSurface(6, 6);
            var robot = new Robot();
            robot.IsPlaced = false;
            var commandHandler = new CommandHandler(robot, tableSurface);

            //command
            commandHandler.ExecuteCommand("PLACE 2,1,EAST");
            commandHandler.ExecuteCommand("MOVE");
            commandHandler.ExecuteCommand("LEFT");
            commandHandler.ExecuteCommand("MOVE");
            commandHandler.ExecuteCommand("PLACE 4,2");
            commandHandler.ExecuteCommand("MOVE");
            commandHandler.ExecuteCommand("REPORT");

            //Assert
            Assert.Equal("Output: 4,3,NORTH", commandHandler.GetOutput());
        }
    }
}
