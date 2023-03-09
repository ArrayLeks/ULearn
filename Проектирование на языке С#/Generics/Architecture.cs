using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Generics.Robots
{
    public interface IRobotAI<out T>
    {
        T GetCommand();
    }

    public class ShooterAI : IRobotAI<ShooterCommand>
    {
        int counter = 1;

        public ShooterCommand GetCommand()
        {
            return ShooterCommand.ForCounter(counter++) ;
        }
    }

    public class BuilderAI : IRobotAI<BuilderCommand>
    {
        int counter = 1;

        public BuilderCommand GetCommand()
        {
            return BuilderCommand.ForCounter(counter++);
        }
    }

    public interface IDevice<in T>
    {
        string ExecuteCommand(T command);
    }

    public class Mover : IDevice<IMoveCommand>
    {
        public string ExecuteCommand(IMoveCommand command)
        {
            return $"MOV {command.Destination.X}, {command.Destination.Y}";
        }
    }

    public class ShooterMover : IDevice<IShooterMoveCommand>
    {
        public string ExecuteCommand(IShooterMoveCommand command)
        {
            var hide = command.ShouldHide ? "YES" : "NO";
            return $"MOV {command.Destination.X}, {command.Destination.Y}, USE COVER {hide}";
        }
    }

    public class Robot<T>
    {
        private readonly IRobotAI<T> ai;
        private readonly IDevice<T> device;

        public Robot(IRobotAI<T> ai, IDevice<T> executor)
        {
            this.ai = ai;
            this.device = executor;
        }

        public IEnumerable<string> Start(int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                var command = ai.GetCommand();
                if (command == null)
                    break;
                yield return device.ExecuteCommand(command);
            }
        }

        public static Robot<T> Create<TCommand>(IRobotAI<T> ai, IDevice<T> executor)
        {
            return new Robot<T>(ai, executor);
        }
    }

    public static class Robot
    { 
        public static Robot<T> Create<T>(IRobotAI<T> ai, IDevice<T> executor)
        {
            return new Robot<T>(ai, executor);
        }
    }
}