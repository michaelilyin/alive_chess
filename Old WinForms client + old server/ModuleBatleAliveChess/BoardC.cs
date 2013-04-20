namespace ModuleBatleAliveChess
{
    public class BoardC
    {
        public static ulong[,] BitFaceMove = new ulong[7, 64];

        public static ulong[,] BitFaceAttack = new ulong[7, 64];

        public static object[,] FaceMove = new object[7, 64];

        public static ulong[,] CollisionMask = new ulong[64, 64];//столкновения
        public static void InitializeBoardC()
        {
            InitializeBits();
            InitializeCastleMask();
            InitializeBitMoves();
            InitializeBitAttacks();
            InitializeMoves();
            InitializeCollisionMask();
        }


        public static void InitializeBits()//инициализация клеток поля
        {
            ulong b = 1;
            for (int i = 0; i < 64; i++, b <<= 1)
            {
                Bit[i] = b;
            }
        }



        public static void InitializeCastleMask()//место начального располажения армий
        {
            CastleMask[0] = Bit[0] | Bit[7];
            CastleMask[1] = Bit[56] | Bit[63];
        }


        public static void InitializeMoves()
        {
            byte[] temp = new byte[27];

            for (int face = 0; face < 7; face++)
            {
                for (int from = 0; from < 64; from++)
                {
                    ulong mask = BitFaceMove[face, from]; //Get mask for face

                    int count = 0;
                    for (byte to = 0; to < 64; to++)
                    {
                        if ((mask & ((ulong)1 << to)) != 0)
                        {
                            temp[count] = to;
                            count++;
                        }
                    }

                    FaceMove[face, from] = new byte[count];
                    for (int i = 0; i < count; i++)
                    {
                        ((byte[])FaceMove[face, from])[i] = temp[i];
                    }
                }
            }
        }


        public static void InitializeBitMoves()
        {
            WPawnMoves();
            KnightMoves();
            RookMoves();
            BishopMoves();
            QueenMoves();//After rook & bishop
            KingMoves();
            BPawnMoves();
        }


        public static void InitializeBitAttacks()
        {
            int f;
            for (f = 0; f < 7; f++)
            {
                for (int s = 0; s < 64; s++)
                {
                    BitFaceAttack[f, s] = BitFaceMove[f, s];
                }
            }

            for (int s = 0; s < 64; s++)
            {
                BitFaceAttack[5, s] &= ~BoardC.Col[s % 8];
            }

            for (int s = 0; s < 64; s++)
            {
                BitFaceAttack[6, s] &= ~BoardC.Col[s % 8];
            }

            BitFaceAttack[4, 4] &= ~(BoardC.Bit[6] | BoardC.Bit[2]);
            BitFaceAttack[4, 60] &= ~(BoardC.Bit[58] | BoardC.Bit[62]);
        }


        public static void WPawnMoves()
        {
            for (int i = 0; i < 64; i++)
            {
                ulong res = 0;

                int x, y;
                x = i % 8;
                y = i / 8;
                if (i >= 8 && i < 56)
                {

                    res |= Bit[x + (y + 1) * 8];
                    if (y == 1)
                        res |= Bit[x + (y + 2) * 8];
                    if (x > 0)
                        res |= Bit[x - 1 + (y + 1) * 8];
                    if (x < 7)
                        res |= Bit[x + 1 + (y + 1) * 8];
                }

                BitFaceMove[5, i] = res;
            }
        }


        public static void KnightMoves()
        {
            int[] coords = new int[16]
			{
				2,  1,
				2, -1,
				-2,  1,
				-2, -1,
				1,  2,
				1, -2,
				-1,  2,
				-1, -2,
			};

            for (int i = 0; i < 64; i++)
            {
                ulong res = 0;

                int x, y;
                x = i % 8;
                y = i / 8;

                for (int j = 0; j < 8; j++)
                {
                    if (x + coords[j * 2] >= 0 && x + coords[j * 2] <= 7 &&
                        y + coords[j * 2 + 1] >= 0 && y + coords[j * 2 + 1] <= 7)
                    {
                        res |= Bit[x + coords[j * 2] + (y + coords[j * 2 + 1]) * 8];
                    }
                }

                BitFaceMove[0, i] = res;
            }
        }


        public static void BishopMoves()
        {
            for (int i = 0; i < 64; i++)
            {
                ulong res = 0;

                int x, y;
                x = i % 8;
                y = i / 8;

                for (int j = 1; j < 8; j++)
                {
                    if (x + j >= 0 && x + j <= 7 &&
                        y + j >= 0 && y + j <= 7)
                    {
                        res |= Bit[x + j + (y + j) * 8];
                    }

                    if (x + j >= 0 && x + j <= 7 &&
                        y - j >= 0 && y - j <= 7)
                    {
                        res |= Bit[x + j + (y - j) * 8];
                    }

                    if (x - j >= 0 && x - j <= 7 &&
                        y + j >= 0 && y + j <= 7)
                    {
                        res |= Bit[x - j + (y + j) * 8];
                    }

                    if (x - j >= 0 && x - j <= 7 &&
                        y - j >= 0 && y - j <= 7)
                    {
                        res |= Bit[x - j + (y - j) * 8];
                    }
                }


                BitFaceMove[3, i] = res;
            }
        }


        public static void RookMoves()
        {
            for (int i = 0; i < 64; i++)
            {
                ulong res = 0;

                int x, y;
                x = i % 8;
                y = i / 8;

                for (int j = 1; j < 8; j++)
                {
                    if (x + j >= 0 && x + j <= 7)
                    {
                        res |= Bit[x + j + y * 8];
                    }

                    if (x - j >= 0 && x - j <= 7)
                    {
                        res |= Bit[x - j + y * 8];
                    }

                    if (y + j >= 0 && y + j <= 7)
                    {
                        res |= Bit[x + (y + j) * 8];
                    }

                    if (y - j >= 0 && y - j <= 7)
                    {
                        res |= Bit[x + (y - j) * 8];
                    }
                }

                BitFaceMove[2, i] = res;
            }
        }


        public static void QueenMoves()
        {
            for (int i = 0; i < 64; i++)
            {
                ulong res;

                res = BitFaceMove[2, i];
                res |= BitFaceMove[3, i];

                BitFaceMove[1, i] = res;
            }
        }


        public static void KingMoves()
        {
            int[] coords = new int[16]
			{
				1,  1,
				1, -1,
				1,  0,
				-1,  1,
				-1, -1,
				-1,  0,
				0,  1,
				0, -1,
			};

            for (int i = 0; i < 64; i++)
            {
                ulong res = 0;

                int x, y;
                x = i % 8;
                y = i / 8;

                for (int j = 0; j < 8; j++)
                {
                    if (x + coords[j * 2] >= 0 && x + coords[j * 2] <= 7 &&
                        y + coords[j * 2 + 1] >= 0 && y + coords[j * 2 + 1] <= 7)
                    {
                        res |= Bit[x + coords[j * 2] + (y + coords[j * 2 + 1]) * 8];
                    }
                }

                if (i == 4) res |= Bit[2] | Bit[6]; //White Castling

                if (i == 60) res |= Bit[58] | Bit[62]; //Black Castling

                BitFaceMove[4, i] = res;
            }
        }


        public static void BPawnMoves()
        {
            GlobalC Globa = new GlobalC();
            for (int i = 0; i < 64; i++)
            {
                ulong res = 0;

                int x, y;
                x = i % 8;
                y = i / 8;
                if (i >= 8 && i < 56)
                {

                    res |= Bit[x + (y - 1) * 8];
                    if (y == 6)
                        res |= Bit[x + (y - 2) * 8];
                    if (x > 0)
                        res |= Bit[x - 1 + (y - 1) * 8];
                    if (x < 7)
                        res |= Bit[x + 1 + (y - 1) * 8];
                }

                BitFaceMove[Globa.BPawn, i] = res;
            }
        }


        public static void InitializeCollisionMask()
        {
            int count = 0;
            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    ulong res = 0;
                    if ((BitFaceMove[1, i] & Bit[j]) > 0)
                    {
                        int c, d;
                        res = 0;

                        c = i / 8 - j / 8;
                        if (c > 0) c = -1;
                        else if (c < 0) c = 1;
                        else c = 0;

                        d = i % 8 - j % 8;
                        if (d > 0) d = -1;
                        else if (d < 0) d = 1;
                        else d = 0;

                        int m, n;
                        for (m = i / 8 + c, n = i % 8 + d; m != j / 8 || n != j % 8; m += c, n += d)
                            res |= Bit[m * 8 + n];
                    }

                    CollisionMask[i, j] = res;
                    if (res > 0) count++;
                }
            }
        }




        //Holds the value for a certain face
        //Держит ценность для определенного лица
        public static int[] FaceValue = new int[8]
		{
			300,
			905,
			505,
			315,
			20000,
			100,
			100,
			0
		};


        public static ulong[] Bit = new ulong[64];


        public static ulong[] Row = new ulong[8]
		{
			// Row, 0, 0
			0x00000000000000FF,//0
			0x000000000000FF00,//1
			0x0000000000FF0000,//2
			0x00000000FF000000,//3
			0x000000FF00000000,//4
			0x0000FF0000000000,//5
			0x00FF000000000000,//6
			0xFF00000000000000,//7
		};


        public static ulong[] Col = new ulong[8]
		{
			// Col, 0, 0
			0x0101010101010101,//0
			0x0202020202020202,//1
			0x0404040404040404,//2
			0x0808080808080808,//3
			0x1010101010101010,//4
			0x2020202020202020,//5
			0x4040404040404040,//6
			0x8080808080808080,//7
		};


        //For masking with OddMoves
        public static ulong[] CastleMask =
		{
			0, 0
		};


        //For masking with OddMoves
        public static ulong[] PassantMask =
		{
			Row[2],
			Row[5]
		};


        //For masking any promotion.
        public static ulong PromotionMask = Row[0] | Row[7];


        public static void SetMiddleGamePositionBonus()
        {
            GlobalC Globa = new GlobalC();
            int[,] MiddleGameBonus = new int[7, 64]
			{
				{
					// FacePositionBonus, 1 Knight
				   -20,-10,-10,-10,-10,-10,-10,-20,//R1
				   -10,  0,  0,  3,  3,  0,  0,-10,//R2
				   -10,  6,  5,  5,  5,  5,  6,-10,//R3
				   -10,  6,  5, 10, 10,  5,  6,-10,//R4
				   -10,  6,  5, 10, 10,  5,  6,-10,//R5
				   -10,  6,  5,  5,  5,  5,  6,-10,//R6
				   -10,  0,  0,  3,  3,  0,  0,-10,//R7
				   -20,-10,-10,-10,-10,-10,-10,-20,//R8
				},

				{
					// FacePositionBonus, 2 Queen
					0,  0,  0,  0,  0,  0,  0,  0,//R1
					0,  0,  0,  0,  0,  0,  0,  0,//R2
					0,  0,  0,  0,  0,  0,  0,  0,//R3
					0,  0,  0,  0,  0,  0,  0,  0,//R0
					0,  0,  0,  0,  0,  0,  0,  0,//R0
					0,  0,  0,  0,  0,  0,  0,  0,//R6
					0,  0,  0,  0,  0,  0,  0,  0,//R0
					0,  0,  0,  0,  0,  0,  0,  0,//R8
				},

				{
					// FacePositionBonus, 3 Rook
					0,  0,  0,  0,  0,  0,  0,  0,//R1
					3,  4,  5,  5,  5,  5,  4,  3,//R2
					0,  0,  0,  0,  0,  0,  0,  0,//R3
					0,  0,  0,  1,  1,  0,  0,  0,//R4
					0,  0,  0,  1,  1,  0,  0,  0,//R5
					0,  0,  0,  0,  0,  0,  0,  0,//R6
					3,  4,  5,  5,  5,  5,  4,  3,//R7
					0,  0,  0,  0,  0,  0,  0,  0,//R8
				},

				{
					// FacePositionBonus, 4 Bishop
					-5, -5, -5, -5, -5, -5, -5, -5,//R1
					-5,  5,  5,  5,  5,  5,  5, -5,//R2
					-5,  5,  8,  9,  9,  8,  5, -5,//R3
					-5,  5,  9,  9,  9,  9,  5, -5,//R-5
					-5,  5,  9,  9,  9,  9,  5, -5,//R5
					-5,  5,  8,  9,  9,  8,  5, -5,//R6
					-5,  5,  5,  5,  5,  5,  5, -5,//R7
					-5, -5, -5, -5, -5, -5, -5, -5,//R8
				},

				{
					// FacePositionBonus, 5 King
					  5,  5,  5,  0,  0,  5,  5,  5,//R1
					-15,-15,-15,-15,-15,-15,-15,-15,//R2
					-10,-10,-10,-10,-10,-10,-10,-10,//R3
					-20,-20,-25,-35,-35,-25,-20,-20,//R4
					-20,-20,-25,-35,-35,-25,-20,-20,//R5
					-15,-15,-15,-15,-15,-15,-15,-15,//R6
					 -5, -5, -5, -5, -5, -5, -5, -5,//R7
					  5,  5,  5,  0,  0,  5,  5,  5,//R8
				},

				{
					// FacePositionBonus, 0 WPawn
					0,  0,  0,  0,  0,  0,  0,  0,//R1
					0,  0,  0, -4, -4,  0,  0,  0,//R2
					1,  1,  2,  4,  4,  2,  1,  1,//R3
					2,  2,  6,  8,  8,  6,  2,  2,//R4
					3,  3,  9, 12, 12,  9,  3,  3,//R5
					4,  4, 12, 16, 16, 12,  4,  4,//R6
					5, 10, 15, 20, 20, 15, 10,  5,//R7
					0,  0,  0,  0,  0,  0,  0,  0,//R8
			},

				{
					// FacePositionBonus, 6 BPawn, as WPawn but mirrored
					0,  0,  0,  0,  0,  0,  0,  0,//R1
					5, 10, 15, 20, 20, 15, 10,  5,//R2
					4,  4, 12, 16, 16, 12,  4,  4,//R3
					3,  3,  9, 12, 12,  9,  3,  3,//R4
					2,  2,  6,  8,  8,  6,  2,  2,//R5
					1,  1,  2,  4,  4,  2,  1,  1,//R6
					0,  0,  0, -4, -4,  0,  0,  0,//R7
					0,  0,  0,  0,  0,  0,  0,  0,//R8
				}
			};

            for (int f = 0; f < 7; f++)
            {
                for (int i = 0; i < 64; i++)
                {
                    FacePositionBonus[f, i] = MiddleGameBonus[f, i];
                }
            }
        }


        public static void SetEndGamePositionBonus()
        {
            int[,] MiddleGameBonus = new int[7, 64]//СРЕДНЯЯ премия игры
			{
				{
					// FacePositionBonus, 1 Knight
					-20,-10,-10,-10,-10,-10,-10,-20,//R1
					-10,  0,  0,  3,  3,  0,  0,-10,//R2
					-10,  6,  5,  5,  5,  5,  6,-10,//R3
					-10,  6,  5, 10, 10,  5,  6,-10,//R4
					-10,  6,  5, 10, 10,  5,  6,-10,//R5
					-10,  6,  5,  5,  5,  5,  6,-10,//R6
					-10,  0,  0,  3,  3,  0,  0,-10,//R7
					-20,-10,-10,-10,-10,-10,-10,-20,//R8
			},

				{
					// FacePositionBonus, 2 Queen
					0,  0,  0,  0,  0,  0,  0,  0,//R1
					0,  0,  0,  0,  0,  0,  0,  0,//R2
					0,  0,  0,  0,  0,  0,  0,  0,//R3
					0,  0,  0,  0,  0,  0,  0,  0,//R0
					0,  0,  0,  0,  0,  0,  0,  0,//R0
					0,  0,  0,  0,  0,  0,  0,  0,//R6
					0,  0,  0,  0,  0,  0,  0,  0,//R0
					0,  0,  0,  0,  0,  0,  0,  0,//R8
			},

				{
					// FacePositionBonus, 3 RookD:\Учёба\Курсовая\AC\AC\AliveChessLibrary\Entities\Batle\Engine.cs
					0,  0,  0,  0,  0,  0,  0,  0,//R1
					3,  3,  3,  3,  3,  3,  3,  3,//R2
					0,  0,  0,  0,  0,  0,  0,  0,//R3
					0,  0,  0,  0,  0,  0,  0,  0,//R4
					0,  0,  0,  0,  0,  0,  0,  0,//R5
					0,  0,  0,  0,  0,  0,  0,  0,//R6
					3,  3,  3,  3,  3,  3,  3,  3,//R7
					0,  0,  0,  0,  0,  0,  0,  0,//R8
			},

				{
					// FacePositionBonus, 4 Bishop
					-5, -5, -5, -5, -5, -5, -5, -5,//R1
					-5,  5,  5,  5,  5,  5,  5, -5,//R2
					-5,  5,  8,  9,  9,  8,  5, -5,//R3
					-5,  5,  9,  9,  9,  9,  5, -5,//R-5
					-5,  5,  9,  9,  9,  9,  5, -5,//R5
					-5,  5,  8,  9,  9,  8,  5, -5,//R6
					-5,  5,  5,  5,  5,  5,  5, -5,//R7
					-5, -5, -5, -5, -5, -5, -5, -5,//R8
			},

				{
					// FacePositionBonus, 5 King
				  -10, -5,  0,  0,  0,  0, -5,-10,//R1
				   -5,  2,  5,  5,  5,  5,  2, -5,//R2
					0,  5, 10, 10, 10, 10,  5,  0,//R3
					0,  5, 15, 18, 18, 15,  5,  0,//R4
					0,  5, 15, 18, 18, 15,  5,  0,//R5
					0,  5, 10, 10, 10, 10,  5,  0,//R6
				   -5,  2,  5,  5,  5,  5,  2, -5,//R7
				  -10, -5,  0,  0,  0,  0, -5,-10,//R8
			},

				{
					// FacePositionBonus, 0 WPawn
					0,  0,  0,  0,  0,  0,  0,  0,//R1
					0,  0,  0, -4, -4,  0,  0,  0,//R2
					1,  1,  2,  4,  4,  2,  1,  1,//R3
					2,  2,  6,  8,  8,  6,  2,  2,//R4
					3,  3,  9, 12, 12,  9,  3,  3,//R5
					4,  4, 12, 16, 16, 12,  4,  4,//R6
					9, 15, 18, 25, 25, 18, 15,  9,//R7
					0,  0,  0,  0,  0,  0,  0,  0,//R8
			},

				{
					// FacePositionBonus, 6 BPawn, as WPawn but mirrored
					0,  0,  0,  0,  0,  0,  0,  0,//R1
					9, 15, 18, 25, 25, 18, 15,  9,//R2
					4,  4, 12, 16, 16, 12,  4,  4,//R3
					3,  3,  9, 12, 12,  9,  3,  3,//R4
					2,  2,  6,  8,  8,  6,  2,  2,//R5
					1,  1,  2,  4,  4,  2,  1,  1,//R6
					0,  0,  0, -4, -4,  0,  0,  0,//R7
					0,  0,  0,  0,  0,  0,  0,  0,//R8
			}
			};

            for (int f = 0; f < 7; f++)
            {
                for (int i = 0; i < 64; i++)
                {
                    FacePositionBonus[f, i] = MiddleGameBonus[f, i];
                }
            }
        }

        public static int[,] FacePositionBonus = new int[7, 64];
    }
}

