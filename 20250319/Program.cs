using System;

namespace _20250319
{
    class Program
    {
        struct Position
        {
            public int x;
            public int y;
        }
        static void Main(string[] args)
        {
            bool gameClear = false;
            Position playerPos;
            playerPos.x = 0;
            playerPos.y = 0;

            Position goalPos;
            Position mapColor;
            

            char[,] map;


            // 게임 준비
            Start(ref playerPos, out goalPos, out mapColor, out map);
            while (gameClear == false)
            {
                // 출력
                Render(playerPos, goalPos, map);
                // 입력
                ConsoleKey key = Input();
                // 처리
                Update(key, ref playerPos, goalPos, mapColor, map, ref gameClear);
            }
            End();
            // 게임 종료

        }

        static void Start(ref Position playerPos, out Position goalPos, out Position mapColor, out char[,] map)
        {
            Console.CursorVisible = false;
            // 플레이어 초기 위치 설정
            playerPos.x = 1;
            playerPos.y = 1;

            goalPos.x = 6;
            goalPos.y = 13;

            mapColor.x = 6;
            mapColor.y = 2;


            map = new char[15, 8]
            {
                { '■', '■', '■', '■', '■', '■', '■', '■'},
                { '■', ' ', ' ', ' ', ' ', ' ', ' ', '■'},
                { '■', 'x', 'x', ' ', 'x', 'x', ' ', '■'},
                { '■', '◇', 'x', ' ', ' ', ' ', ' ', '■'},
                { '■', ' ', 'x', 'x', ' ', 'x', 'x', '■'},
                { '■', ' ', 'x', 'x', ' ', ' ', ' ', '■'},
                { '■', ' ', ' ', '◆', ' ', 'x', ' ', '■'},
                { '■', ' ', 'x', 'x', 'x', 'x', ' ', '■'},
                { '■', ' ', ' ', ' ', 'x', ' ', ' ', '■'},
                { '■', 'x', 'x', ' ', 'x', ' ', 'x', '■'},
                { '■', ' ', ' ', ' ', ' ', ' ', 'x', '■'},
                { '■', ' ', 'x', 'x', ' ', 'x', ' ', '■'},
                { '■', ' ', ' ', ' ', ' ', ' ', ' ', '■'},
                { '■', ' ', 'x', 'x', 'x', 'x', ' ', '■'},
                { '■', '■', '■', '■', '■', '■', '■', '■'}
            };
        }
        static void PrintPlayer(Position playerPos)
        {
            // 플레이어 위치
            Console.SetCursorPosition(playerPos.x, playerPos.y);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write('▼');
            Console.ResetColor();
        }

        static void PrintMap(char[,] map)
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] == 'x')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write('x');
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(map[y, x]);
                    }
                }
                Console.WriteLine();
            }
        }

        static void Goal(Position goalPos)
        {
            Console.SetCursorPosition(goalPos.x, goalPos.y);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write('G');
            Console.ResetColor();
        }


        static void Render(Position playerPos, Position goalPos, char[,] map)
        {
            Console.Clear();
            // 플레이어 위치 출력
            PrintMap(map);
            Goal(goalPos);
            PrintPlayer(playerPos);
        }


        static ConsoleKey Input()
        {
            // 플레이어 위치 입력
            ConsoleKey input = Console.ReadKey(true).Key;
            return input;
        }

        static void Move(ConsoleKey key, ref Position playerPos, char[,] map)
        {
            Position targetPos;
            Position overPos;

            // 플레이어 위치 처리
            switch (key)
            {
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow: 
                    targetPos.x = playerPos.x - 1;
                    targetPos.y = playerPos.y;
                    overPos.x = playerPos.x - 2;
                    overPos.y = playerPos.y;
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    targetPos.x = playerPos.x + 1;
                    targetPos.y = playerPos.y;
                    overPos.x = playerPos.x + 2;
                    overPos.y = playerPos.y;
                    break;
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    targetPos.x = playerPos.x;
                    targetPos.y = playerPos.y - 1;
                    overPos.x = playerPos.x;
                    overPos.y = playerPos.y - 2;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    targetPos.x = playerPos.x;
                    targetPos.y = playerPos.y + 1;
                    overPos.x = playerPos.x;
                    overPos.y = playerPos.y + 2;
                    break;
                default:
                    return;
            }
            // 움직이는 방향에 박스가 있을 때
            if (map[targetPos.y, targetPos.x] == '◆')
            {
                // 그 다음 위치에 골이 있을 때
                if (map[overPos.y, overPos.x] == '◇')
                {
                    // 1. 골 위치에 골박스로
                    map[overPos.y, overPos.x] = '◈';

                    map[targetPos.y, targetPos.x] = ' ';

                    playerPos.x = targetPos.x;
                    playerPos.y = targetPos.y;
                }
                // 그 다음 위치에 빈칸이 있을 때
                else if (map[overPos.y, overPos.x] == ' ')
                {   // 1. 빈칸 위치에 박스를
                    map[overPos.y, overPos.x] = '◆';

                    // 2. 박스 위치를 빈칸으로
                    map[targetPos.y, targetPos.x] = ' ';
                    // 3. 플레이어를 움직이려는 위치로
                    playerPos.x = targetPos.x;
                    playerPos.y = targetPos.y;
                }
            }

            // 움직이는 방향이 빈칸일 때
            else if (map[targetPos.y, targetPos.x] == ' ')
            {
                playerPos.x = targetPos.x;
                playerPos.y = targetPos.y;
            }
            // 움직이는 방향에 M이 있을 때
            else if (map[targetPos.y, targetPos.x] == 'M')
            {
                playerPos.x = targetPos.x;
                playerPos.y = targetPos.y;
            }

            // 움직이는 방향에 x가 있을 때
            else if (map[targetPos.y, targetPos.x] == 'x')
            {
                playerPos.x = 1;
                playerPos.y = 1;
            }


            // 움직이는 방향에 벽이 있을 때
            else if (map[targetPos.y, targetPos.x] == '■')
            {

            }

            else
            {

            }



        }

        static void Update(ConsoleKey key, ref Position playerPos, Position goalPos, Position mapColor, char[,] map, ref bool GameClear)
        {
            Move(key, ref playerPos, map);
            bool color = CheckMap(playerPos, mapColor);
            if (color)
            {

            }
            bool isClear = CheckGameClear(playerPos, goalPos, map);
            if (isClear)
            {
                GameClear = true;
            }
        }
        static bool CheckMap(Position playerPos, Position mapColor)
        {
            bool cover = (playerPos.x == mapColor.x && playerPos.y == mapColor.y);
            return cover;
        }


        static bool CheckGameClear(Position playerPos, Position goalPos, char[,]map)
        {
            int goalcount = 0;
            // 클리어 조건 빈칸이 없고 골 지점으로 가기
            foreach (char tile in map)
            {
                if(tile == '◇')
                {
                    goalcount++;
                    break;
                }
            }
            if (goalcount == 0)
            {
                bool success = (playerPos.x == goalPos.x && playerPos.y == goalPos.y);
                return success;
            }
            else
            {
                return false;
            }
        }

        static void End()
        {
            Console.Clear();
            Console.WriteLine("끝났당");
        }


    }
}
