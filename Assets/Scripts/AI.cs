using System.Collections.Generic;
using UnityEngine;

public static class AI
{
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }

    public static int GetRandomValue(List<int> lstValues)
    {
        int index = Random.Range(0, lstValues.Count);
        return lstValues[index];
    }

    public static int GetValue(Board board)
    {
        Direction[] directions = (Direction[])System.Enum.GetValues(typeof(Direction));
        List<int> lstValues = board.GetAvailableColumns();
        Slot[,] slots = board.Slots;
        int[] columnCounter = board.ColumnCounter;
        int selectedIndex = 0;
        int higherScore = -1;
        int criticalIndex = -1;

        for (int i = 0; i < lstValues.Count; i++)
        {
            int x = lstValues[i];
            int y = columnCounter[x];
            int score = 0;
            Dictionary<Direction, DirectionInfo> directionDic = new Dictionary<Direction, DirectionInfo>();

            for (int j = 0; j < directions.Length; j++)
            {
                int col = x;
                int row = y;
                Slot slot = Slot.Empty;
                int count = 0;
                MoveCoordinates(ref col, ref row, directions[j]);

                if (ValidateDirection(col, row, slots))
                {
                    slot = slots[col, row];
                    count = DirectionCount(slots, slot, directions[j], col, row, 0);
                }

                DirectionInfo dirInfo = new DirectionInfo(slot, count);
                score += GiveScore(dirInfo);
                directionDic.Add(directions[j], dirInfo);
            }

            if (CheckCritical(directionDic, Direction.Up, Direction.Down, 3) ||
                CheckCritical(directionDic, Direction.Left, Direction.Right, 3) ||
                CheckCritical(directionDic, Direction.UpLeft, Direction.DownRight, 3) ||
                CheckCritical(directionDic, Direction.DownLeft, Direction.UpRight, 3))
                return x;

            if (CheckCritical(directionDic, Direction.Up, Direction.Down, 2) ||
                CheckCritical(directionDic, Direction.Left, Direction.Right, 2) ||
                CheckCritical(directionDic, Direction.UpLeft, Direction.DownRight, 2) ||
                CheckCritical(directionDic, Direction.DownLeft, Direction.UpRight, 2))
            {
                criticalIndex = x;
            }

            if (score > higherScore)
            {
                higherScore = score;
                selectedIndex = x;
            }
        }

        if (criticalIndex >= 0) return criticalIndex;

        return higherScore >= 0 ? selectedIndex : GetRandomValue(lstValues);
    }

    private static bool ValidateDirection(int col, int row, Slot[,] slots)
    {
        return (col < slots.GetLength(0) &&
                col >= 0 &&
                row < slots.GetLength(1) &&
                row >= 0);
    }

    private static void MoveCoordinates(ref int col, ref int row, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                row++;
                break;
            case Direction.Down:
                row--;
                break;
            case Direction.Left:
                col--;
                break;
            case Direction.Right:
                col++;
                break;
            case Direction.UpLeft:
                row++;
                col--;
                break;
            case Direction.UpRight:
                row++;
                col++;
                break;
            case Direction.DownLeft:
                row--;
                col--;
                break;
            case Direction.DownRight:
                row--;
                col++;
                break;
        }
    }

    private static int DirectionCount(Slot[,] slots, Slot currentSlot, Direction direction, int col, int row, int count)
    {
        if (!ValidateDirection(col, row, slots) || currentSlot != slots[col, row]) return count;

        count++;
        MoveCoordinates(ref col, ref row, direction);

        return DirectionCount(slots, currentSlot, direction, col, row, count);
    }

    private static bool CheckCritical(Dictionary<Direction, DirectionInfo> directions, Direction direction1, Direction direction2, int criticalCount)
    {
        int count = 0;

        if (directions[direction1].count >= criticalCount &&
            directions[direction1].slot != Slot.Empty) return true;

        count += directions[direction1].count;

        if (directions[direction2].count >= criticalCount &&
            directions[direction2].slot != Slot.Empty) return true;

        count += directions[direction2].count;

        if (count >= criticalCount &&
            directions[direction1].slot != Slot.Empty &&
            directions[direction1].slot == directions[direction2].slot) return true;

        return false;
    }

    private static int GiveScore(DirectionInfo directionInfo)
    {
        switch (directionInfo.slot)
        {
            case Slot.Empty:
                return directionInfo.count;
            case Slot.Player2:
                return directionInfo.count * 2;
            default:
                return 0;
        }
    }

    private struct DirectionInfo
    {
        public Slot slot;
        public int count;

        public DirectionInfo(Slot slot, int count)
        {
            this.slot = slot;
            this.count = count;
        }
    }
}
