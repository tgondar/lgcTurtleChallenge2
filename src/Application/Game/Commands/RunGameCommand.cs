using Domain.Entities;
using Domain.Enums;
using MediatR;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Game.Commands
{
    public class RunGameCommand : IRequest<Board>
    {
        public RunGameCommand(string settingsFile, string movementsFile)
        {
            SettingsFile = settingsFile;
            MovementsFile = movementsFile;
        }

        public string SettingsFile { get; set; }
        public string MovementsFile { get; set; }
    }

    public class RunGameCommandHandler : IRequestHandler<RunGameCommand, Board>
    {
        private bool _gameOver = false;

        public Task<Board> Handle(RunGameCommand request, CancellationToken cancellationToken)
        {
            Board board = new();

            if (!ValidationGate(board, request.SettingsFile, request.MovementsFile)) return Task.FromResult(board);

            ReadSettingsFile(board, request.SettingsFile);

            ReadMovementsFile(board, request.MovementsFile);

            RunGameOnBoard(board);

            return Task.FromResult(board);
        }

        private void RunGameOnBoard(Board board)
        {
            int movs = 0;
            foreach (var item in board.Movements)
            {
                switch (item)
                {
                    case "m":
                        Move(board);
                        break;
                    case "r":
                        Rotate(board);
                        break;
                    default:
                        board.GameInfo.Add($"Well... [{item}] is not a valid movement, but ok..");
                        break;
                }

                movs++;

                if (_gameOver)
                {
                    break;
                }
            }

            if (movs == board.Movements.Count)
            {
                board.GameInfo.Add($"Turtle stopped, all movements were executed...");
            }
        }

        private void Rotate(Board board)
        {
            switch (board.Settings.TurtlePosition.Direction)
            {
                case DirectionEnum.north:
                    board.Settings.TurtlePosition.Direction = DirectionEnum.east;
                    break;
                case DirectionEnum.east:
                    board.Settings.TurtlePosition.Direction = DirectionEnum.south;
                    break;
                case DirectionEnum.south:
                    board.Settings.TurtlePosition.Direction = DirectionEnum.west;
                    break;
                case DirectionEnum.west:
                    board.Settings.TurtlePosition.Direction = DirectionEnum.north;
                    break;
                default:
                    break;
            }

            board.GameInfo.Add($"Turtle rotated to: {board.Settings.TurtlePosition.Direction}");
        }

        private void Move(Board board)
        {
            switch (board.Settings.TurtlePosition.Direction)
            {
                case DirectionEnum.north:
                    board.Settings.TurtlePosition.Y--;
                    break;
                case DirectionEnum.east:
                    board.Settings.TurtlePosition.X++;
                    break;
                case DirectionEnum.south:
                    board.Settings.TurtlePosition.Y++;
                    break;
                case DirectionEnum.west:
                    board.Settings.TurtlePosition.X--;
                    break;
                default:
                    break;
            }

            board.GameInfo.Add($"Turtle moved to position: {board.Settings.TurtlePosition.ToString()}");

            CheckIfTurtleIsOutsideBoard(board);

            CheckForMines(board);

            CheckForTheExit(board);
        }

        private void CheckIfTurtleIsOutsideBoard(Board board)
        {
            if (board.Settings.TurtlePosition.X < 0
                || board.Settings.TurtlePosition.X > board.Settings.BoardSize.X
                || board.Settings.TurtlePosition.Y < 0
                || board.Settings.TurtlePosition.Y > board.Settings.BoardSize.Y)
            {
                board.GameInfo.Add(":( Turtle as moved outside the board!");
                _gameOver = true;
            }
        }

        private void CheckForMines(Board board)
        {
            if (board.Settings.Mines.Any(x => x.X == board.Settings.TurtlePosition.X
            && x.Y == board.Settings.TurtlePosition.Y))
            {
                board.GameInfo.Add(":( Turtle stepped on a mine!");
                _gameOver = true;
            }
        }

        private void CheckForTheExit(Board board)
        {
            if (board.Settings.TurtlePosition.X == board.Settings.Exit.X
                && board.Settings.TurtlePosition.Y == board.Settings.Exit.Y)
            {
                board.GameInfo.Add(":) Turtle arrived at the water!!!");
                _gameOver = true;
            }
        }


        private static bool ValidationGate(Board board, string settingsFile, string MovementFile)
        {
            if (!File.Exists(settingsFile))
            {
                board.GameInfo.Add("SettingsFile doesn't exist, please upload.");
                return false;
            }

            if (!File.Exists(MovementFile))
            {
                board.GameInfo.Add("MovementsFile doesn't exist, please upload.");
                return false;
            }

            return true;
        }

        private static void ReadSettingsFile(Board board, string file)
        {
            if (File.Exists(file))
            {
                var json = File.ReadAllText(file);

                JsonSerializerOptions options = new()
                {
                    Converters =
                {
                    new JsonStringEnumConverter()
                }
                };

                board.Settings = JsonSerializer.Deserialize<Settings>(json, options);
            }
            else
            {
                board.GameInfo.Add("");
            }
        }
        private static void ReadMovementsFile(Board board, string file)
        {
            if (File.Exists(file))
            {
                var json = File.ReadAllText(file);

                JsonSerializerOptions options = new()
                {
                    Converters =
                {
                    new JsonStringEnumConverter()
                }
                };

                board.Movements = JsonSerializer.Deserialize<List<string>>(json, options);
            }
            else
            {
                board.GameInfo.Add("SettingsFile doesn't exist, please upload.");
            }
        }
    }
}
