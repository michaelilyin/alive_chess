using System;

namespace ModuleBatleAliveChess
{
    public struct Move
    {
        public byte from;
        public byte to;
        public byte promote;
        public int cmp;
    };

    public class BoardModFighters
    {
        //TODO: Implement compacting to make sure that the engine doesn't run out of memory
        // для проверки хватает ли памяти
        private const int MOVEMEMORY = 1000;
        GlobalC Gl = new GlobalC();


        //TODO: Activate and fix BitBoards
        //private ulong[,] myBitBoard; //BitBoard representation
        public byte[,] myBoard; //Main board represantation
        private int[,] Arm;
        private int myMaterial; //Material balance
        private int myMemoryIndex;
        private int myFullMoves;//все действия
        private ulong[] myCastlings;// мои рокировки
        private ulong[] myZobristKeys;
        private byte[] myHalfMoves;//моя половина действий
        private byte[] myPassants;//Holds the enpassant square if any
        private byte[] myCaptures;// захваты
        private byte[] myCapturePositions;
        private byte[] myKingPos;// место поражения короля
        private byte myPlayer; //Speed up for aBoardCess to myPlayer and myOpponent
        // ускорение для игрока и опонента
        private byte myOpponent;
        private ulong myEmpty; //Speed up for empty squares пустой
        private ulong myPlayerPieces; //Speed up for myPlayer pieces
        private int[] Weight = new int[16];

        public bool DrawByMoves
        {
            get
            {
                if (myHalfMoves[myMemoryIndex] >= 50) return true;

                return false;
            }
        }


        public byte Player
        {
            get
            {
                return myPlayer;
            }
        }

        public ulong ZobristKey
        {
            get
            {
                return myZobristKeys[myMemoryIndex];
            }
        }



        public void UpdateGamePhase()
        {
            if (myFullMoves < 30)
            {
                BoardC.SetMiddleGamePositionBonus();
            }
            else
            {
                BoardC.SetEndGamePositionBonus();
            }
        }


#if (DEBUG)
        public void CompareWith(BoardModFighters c)
        {
            if (myEmpty != c.myEmpty) throw new Exception("Difference in Empty");
            //if (myZobristKey != c.myZobristKey) throw new Exception("Difference in ZobristKey");
            if (myPlayerPieces != c.myPlayerPieces) throw new Exception("Difference in PiecesmyPlayer");
            //if (myCastling != c.myCastling) throw new Exception("Difference in OddMoves");
            if (myMaterial != c.myMaterial) throw new Exception("Difference in Material");
            //if (myPassantSquare != c.myPassantSquare) throw new Exception("Difference in PassantSquare");
            if (myPlayer != c.myPlayer) throw new Exception("Difference in myPlayer");
            if (myOpponent != c.myOpponent) throw new Exception("Difference in myOpponent");

            if (myKingPos[0] != c.myKingPos[0]) throw new Exception("Difference in KingPos[0]");
            if (myKingPos[1] != c.myKingPos[1]) throw new Exception("Difference in KingPos[1]");

            //for (int i = 0 ; i < GlobalC.Faces ; i++)
            //if (myBitBoard[0,i] != c.myBitBoard[0,i]) throw new Exception("Difference in BitBoard[0]");
            //for (int i = 0 ; i < GlobalC.Faces ; i++)
            //if (myBitBoard[1,i] != c.myBitBoard[1,i]) throw new Exception("Difference in BitBoard[1]");

            for (int i = 0; i < 64; i++)
                if (myBoard[0, i] != c.myBoard[0, i]) throw new Exception("Difference in Board[0]");
            for (int i = 0; i < 64; i++)
                if (myBoard[1, i] != c.myBoard[1, i]) throw new Exception("Difference in Board[1]");
        }


        public BoardModFighters(BoardModFighters c, byte[,] myBoard, int[,] Arm)
        {
            this.myBoard = myBoard;
            this.Arm = Arm;
            //myBitBoard = new ulong[GlobalC.Sides,GlobalC.Faces];
            myKingPos = new byte[Gl.Sides];

            myEmpty = c.myEmpty;
            myPlayerPieces = c.myPlayerPieces;
            myMaterial = c.myMaterial;
            myPlayer = c.myPlayer;
            myOpponent = c.myOpponent;

            myKingPos[0] = c.myKingPos[0];
            myKingPos[1] = c.myKingPos[1];

            //for (int i = 0 ; i < GlobalC.Faces ; i++)
            //myBitBoard[0,i] = c.myBitBoard[0,i];
            //for (int i = 0 ; i < GlobalC.Faces ; i++)
            //myBitBoard[1,i] = c.myBitBoard[1,i];

            for (int i = 0; i < 64; i++)
                myBoard[0, i] = c.myBoard[0, i];
            for (int i = 0; i < 64; i++)
                myBoard[1, i] = c.myBoard[1, i];
        }
#endif


        public BoardModFighters(byte[,] myBoard, int[,] Arm)
        {
            this.myBoard = myBoard;
            this.Arm = Arm;
            //myBitBoard = new ulong[GlobalC.Sides,GlobalC.Faces];
            myKingPos = new byte[Gl.Sides];

            myHalfMoves = new byte[MOVEMEMORY];
            myCastlings = new ulong[MOVEMEMORY];
            myZobristKeys = new ulong[MOVEMEMORY];
            myPassants = new byte[MOVEMEMORY];
            myCaptures = new byte[MOVEMEMORY];
            myCapturePositions = new byte[MOVEMEMORY];
        }


        public void Initialize()
        {
            SetBoardFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        }


        public void ValidateMove(byte from, byte to, byte promote)// ПРОВЕРКА ДВИЖЕНИЯ
        {
            const string NO_SOURCE_PIECE = "No player piece on source square";//нет игрока в исходном квадрате
            const string SELF_CAPTURE = "Own piece on destination square";//фигура на месте назначания
            const string ILLEGAL_MOVE = "Not a possible move for the piece";// невозможное движение для этой фигуры
            const string PIECE_COLLISION = "Pieces in the path";// столкновение фигур
            const string CASTLING_NA_MOVED = "Castling not available, either king or rook has been moved before";// рокировка не доступна
            const string CASTLING_NA_THREAT = "Castling not available, threat on a square in the path";//на пути рокировки что то есть
            const string CASTLING_NA_PIECES = "Castling not available, pieces in the path";//фигуры в движении
            const string MOVE_INTO_CHECK = "Move into check";// движение в выбранную позицию
            const string ILLEGAL_CAPTURE = "Opponent piece on destination square"; // Противник находится на клетке куда возможен ход
            const string NOT_A_CAPTURE = "No opponent piece on destination square"; // не захвачено
            const string ILLEGAL_PROMOTION = "Illegal promotion";// незаконное поощерение (подставился)


            byte face = myBoard[myPlayer, from];

            if (face == Gl.Empty) throw new Exception(NO_SOURCE_PIECE);

            if (myBoard[myPlayer, to] != Gl.Empty) throw new Exception(SELF_CAPTURE);

            if ((BoardC.CollisionMask[from, to] & ~myEmpty) != 0) throw new Exception(PIECE_COLLISION); //Check obstacles, can be done before knowing face since Knight paths are not in the collision mask.

            ulong mask = BoardC.BitFaceMove[face, from]; //Get mask for face

            if ((mask & BoardC.Bit[to]) == 0) throw new Exception(ILLEGAL_MOVE);//Validate aBoardCording to move rule

            if (face >= Gl.WPawn)
            {
                if (from % 8 != to % 8) //Diagonal move
                {
                    if (myBoard[myOpponent, to] == Gl.Empty)
                        if (to != myPassants[myMemoryIndex])
                            throw new Exception(NOT_A_CAPTURE);
                }
                else //Vertical move Вертикальное движение
                {
                    if (myBoard[myOpponent, to] != Gl.Empty) throw new Exception(ILLEGAL_CAPTURE);
                }

                if (promote != Gl.getPawn(0)) //Validate promotion (проверка поощерения)
                {
                    if (myPlayer == Gl.White)
                    {
                        if (to < 56) throw new Exception(ILLEGAL_PROMOTION);
                    }
                    else
                    {
                        if (to > 7) throw new Exception(ILLEGAL_PROMOTION);
                    }
                }
            }


            if (face == Gl.King)
            {
                if (ThreatOnSquareAfterMove(to, from, to)) throw new Exception(MOVE_INTO_CHECK);//угроза на квадрате после движения

                if (BoardC.CollisionMask[from, to] != 0) //If Castling (King moves two squares, collmask will have a bit set) король перемещается на 2 клетки и мустанавливается маска
                {
                    byte rookpos = (byte)(myPlayer * 56);
                    if (to % 8 > 4) rookpos += 7;

                    if ((myCastlings[myMemoryIndex] & BoardC.Bit[rookpos]) == 0) throw new Exception(CASTLING_NA_MOVED); //рокировка не доступна

                    if ((BoardC.CollisionMask[from, rookpos] & ~myEmpty) == 0) //If free path если длинна свободного места
                    {
                        if (ThreatOnSquare(to)) throw new Exception(CASTLING_NA_THREAT);
                        if (ThreatOnSquare(from)) throw new Exception(CASTLING_NA_THREAT);

                        if (from < to)
                        {
                            if (ThreatOnSquare((byte)(from + 1))) throw new Exception(CASTLING_NA_THREAT);
                        }
                        else
                        {
                            if (ThreatOnSquare((byte)(from - 1))) throw new Exception(CASTLING_NA_THREAT);
                        }
                    }
                    else
                        throw new Exception(CASTLING_NA_PIECES);
                }
            }
            else
            {
                if (ThreatOnSquareAfterMove(myKingPos[myPlayer], from, to)) throw new Exception(MOVE_INTO_CHECK);
                // if (ThreatOnSquareAfterMove(myKingPos[myOpponent], from, to)) throw new Exception(MOVE_INTO_CHECK);
            }
        }


        private bool ValidatePseudoMove(byte from, byte to, byte face)//проверка псевдо движения!!
        {
            if (face >= Gl.WPawn)
            {
                if (from % 8 != to % 8) //Diagonal move
                {
                    if (myBoard[myOpponent, to] == Gl.Empty)
                        if (to != myPassants[myMemoryIndex])
                            return false;
                }
                else //Vertical move
                {
                    if (myBoard[myOpponent, to] != Gl.Empty) return false;
                }
            }

            if (face == Gl.King)
            {
                if (ThreatOnSquareAfterMove(to, from, to)) return false;

                if (BoardC.CollisionMask[from, to] != 0) //If Castling (King moves two squares, collmask will have a bit set)
                {
                    byte rookpos = (byte)(myPlayer * 56);
                    if (to % 8 > 4) rookpos += 7;

                    if ((myCastlings[myMemoryIndex] & BoardC.Bit[rookpos]) == 0) return false;

                    if ((BoardC.CollisionMask[from, rookpos] & ~myEmpty) == 0) //If free path если длинна свободного пути
                    {
                        if (ThreatOnSquare(to)) return false;
                        if (ThreatOnSquare(from)) return false;

                        if (from < to)
                        {
                            if (ThreatOnSquare((byte)(from + 1))) return false;
                        }
                        else
                        {
                            if (ThreatOnSquare((byte)(from - 1))) return false;
                        }
                    }
                    else
                        return false;

                }
            }
            else
            {
                if (ThreatOnSquareAfterMove(myKingPos[myPlayer], from, to)) return false;
                //if (ThreatOnSquareAfterMove(myKingPos[myOpponent], from, to)) return false;
            }

            return true;
        }


        //TODO: Optimize this!
        private bool ThreatOnSquareAfterMove(byte square, byte from, byte to)//угроза на клетку после хода
        {
            for (byte src = 0; src < 64; src++) //Go through all (myOpponent) pieces пройти по всей доске
            {
                byte f = myBoard[myOpponent, src];

                if (f == Gl.Empty) continue;

                if (src == to) continue; //The piece is captured if the move is done клетка захвачена если движение выполнено
                if (f >= Gl.WPawn && to == myPassants[myMemoryIndex]) continue;

                ulong mask = BoardC.BitFaceAttack[f, src]; //Get mask for face

                if ((mask & BoardC.Bit[square]) != 0) //A possible move? возможно движение
                    //   The basic collision mask     &   nonempty -  rem from      + add to
                    if ((BoardC.CollisionMask[src, square] & ((~myEmpty & ~BoardC.Bit[from]) | BoardC.Bit[to])) == 0)
                        return true; //There is a threat on the square существунт угроза на клетке
            }

            return false;
        }


        //TODO: Test and put into use...проверенны и зданы в использование

        private bool CheckingAfterMove(byte square, byte from, byte to) // проверка после движения
        {
            if (BoardC.BitFaceAttack[myBoard[myPlayer, from], square] != 0) return true;

            if ((BoardC.Bit[from] & BoardC.BitFaceAttack[Gl.getQueen(0), square]) == 0) return false;

            myPlayer = myOpponent;
            myOpponent = (byte)(1 - myPlayer);

            bool res = InCheckAfterMoveFromSafe(square, from, to);

            myPlayer = myOpponent;
            myOpponent = (byte)(1 - myPlayer);

            return res;
        }


        private bool InCheckAfterMoveFromSafe(byte square, byte from, byte to)// проверка безопасности после хода
        {
            if ((BoardC.Bit[from] & BoardC.BitFaceAttack[Gl.getQueen(0), square]) == 0) return false;

            int x, y;
            byte dst;

            if (square < from)
            {
                if ((from - square) % 9 == 0)
                {
                    x = square % 8 + 1;
                    y = square / 8 + 1;
                    dst = square;
                    for (; x <= 7 && y <= 7; x++, y++)
                    {
                        dst += 9;
                        if (dst != from && myBoard[myPlayer, dst] != Gl.Empty) break;
                        if (dst == to) break;
                        if (myBoard[myOpponent, dst] != Gl.Empty)
                        {
                            if (myBoard[myOpponent, dst] == Gl.getBishop(0) || myBoard[myOpponent, dst] == Gl.getQueen(0))
                                return true;
                            else
                                break;
                        }
                    }
                }

                if ((from - square) % 8 == 0)
                {
                    y = square / 8 + 1;
                    dst = square;
                    for (; y <= 7; y++)
                    {
                        dst += 8;
                        if (dst != from && myBoard[myPlayer, dst] != Gl.Empty) break;
                        if (dst == to) break;
                        if (myBoard[myOpponent, dst] != Gl.Empty)
                        {
                            if (myBoard[myOpponent, dst] == Gl.getRook(0) || myBoard[myOpponent, dst] == Gl.getQueen(0))
                                return true;
                            else
                                break;
                        }
                    }
                }

                if ((from - square) % 7 == 0)
                {
                    x = square % 8 - 1;
                    y = square / 8 + 1;
                    dst = square;
                    for (; x >= 0 && y <= 7; x--, y++)
                    {
                        dst += 7;
                        if (dst != from && myBoard[myPlayer, dst] != Gl.Empty) break;
                        if (dst == to) break;
                        if (myBoard[myOpponent, dst] != Gl.Empty)
                        {
                            if (myBoard[myOpponent, dst] == Gl.getBishop(0) || myBoard[myOpponent, dst] == Gl.getQueen(0))
                                return true;
                            else
                                break;
                        }
                    }
                }

                x = square % 8 + 1;
                dst = square;
                for (; x <= 7; x++)
                {
                    dst++;
                    if (dst != from && myBoard[myPlayer, dst] != Gl.Empty) break;
                    if (dst == to) break;
                    if (myBoard[myOpponent, dst] != Gl.Empty)
                    {
                        if (myBoard[myOpponent, dst] == Gl.getRook(0) || myBoard[myOpponent, dst] == Gl.getQueen(0))
                            return true;
                        else
                            break;
                    }
                }
            }
            else
            {
                if ((square - from) % 9 == 0)
                {
                    x = square % 8 - 1;
                    y = square / 8 - 1;
                    dst = square;
                    for (; x >= 0 && y >= 0; x--, y--)
                    {
                        dst -= 9;
                        if (dst != from && myBoard[myPlayer, dst] != Gl.Empty) break;
                        if (dst == to) break;
                        if (myBoard[myOpponent, dst] != Gl.Empty)
                        {
                            if (myBoard[myOpponent, dst] == Gl.getBishop(0) || myBoard[myOpponent, dst] == Gl.getQueen(0))
                                return true;
                            else
                                break;
                        }
                    }
                }

                if ((square - from) % 8 == 0)
                {
                    y = square / 8 - 1;
                    dst = square;
                    for (; y >= 0; y--)
                    {
                        dst -= 8;
                        if (dst != from && myBoard[myPlayer, dst] != Gl.Empty) break;
                        if (dst == to) break;
                        if (myBoard[myOpponent, dst] != Gl.Empty)
                        {
                            if (myBoard[myOpponent, dst] == Gl.getRook(0) || myBoard[myOpponent, dst] == Gl.getQueen(0))
                                return true;
                            else
                                break;
                        }
                    }
                }

                if ((square - from) % 7 == 0)
                {
                    x = square % 8 + 1;
                    y = square / 8 - 1;
                    dst = square;
                    for (; x <= 7 && y >= 0; x++, y--)
                    {
                        dst -= 7;
                        if (dst != from && myBoard[myPlayer, dst] != Gl.Empty) break;
                        if (dst == to) break;
                        if (myBoard[myOpponent, dst] != Gl.Empty)
                        {
                            if (myBoard[myOpponent, dst] == Gl.getBishop(0) || myBoard[myOpponent, dst] == Gl.getQueen(0))
                                return true;
                            else
                                break;
                        }
                    }
                }

                x = square % 8 - 1;
                dst = square;
                for (; x >= 0; x--)
                {
                    dst--;
                    if (dst != from && myBoard[myPlayer, dst] != Gl.Empty) break;
                    if (dst == to) break;
                    if (myBoard[myOpponent, dst] != Gl.Empty)
                    {
                        if (myBoard[myOpponent, dst] == Gl.getRook(0) || myBoard[myOpponent, dst] == Gl.getQueen(0))
                            return true;
                        else
                            break;
                    }
                }
            }

            return false;
        }


        public bool InCheck()
        {
            return ThreatOnSquare(myKingPos[myPlayer]);
        }


        //TODO: Optimize this (low priority)
        private bool ThreatOnSquare(byte square)
        {
            for (byte src = 0; src < 64; src++) //Go through all (myOpponent) pieces пройти по всей доске
            {
                byte f = myBoard[myOpponent, src];

                if (f == Gl.Empty) continue;

                ulong mask = BoardC.BitFaceAttack[f, src]; //Get mask for face

                if ((mask & BoardC.Bit[square]) != 0) //A possible move?
                    if ((BoardC.CollisionMask[src, square] & ~myEmpty) == 0)
                        return true; //There is a threat on the square
            }

            return false;
        }


        public int GetMoves(Move[] moves, int p)
        {
            if (InCheck())
            {
                return GetAvoidCheckMoves(moves, p);
            }
            else
            {
                return GetAllMoves(moves, p);
            }
        }


        public int GetAvoidCheckMoves(Move[] moves, int p)
        {
            int count = p;


            byte attackers = 0;
            byte square = myKingPos[myPlayer];
            ulong threatmask = 0;
            for (byte src = 0; src < 64; src++) //Go through all (myOpponent) pieces
            {
                byte f = myBoard[myOpponent, src];

                if (f == Gl.Empty) continue;

                ulong mask = BoardC.BitFaceAttack[f, src]; //Get mask for face

                if ((mask & BoardC.Bit[square]) != 0) //A possible move?
                    if ((BoardC.CollisionMask[src, square] & ~myEmpty) == 0)
                    {
                        attackers++; //There is a threat on the king существует угроза на короля!!!
                        threatmask |= BoardC.CollisionMask[src, square] | BoardC.Bit[src];
                    }
            }


            if (attackers > 1)
            {
                int[] coords = new int[16]// координаты
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

                for (int i = 0; i < 16; i += 2)
                {
                    int x = square % 8 + coords[i];
                    int y = square / 8 + coords[i + 1];
                    if (x >= 0 && x <= 7 && y >= 0 && y <= 7)
                    {
                        byte to = (byte)(x + y * 8);
                        if (myBoard[myPlayer, to] == Gl.Empty)
                            if (!ThreatOnSquareAfterMove(to, square, to))
                            {
                                moves[count].from = square;
                                moves[count].to = to;
                                moves[count].promote = Gl.getPawn(0);
                                moves[count].cmp = 0;
                                if (myBoard[myOpponent, to] != Gl.Empty)
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 90;
                                count++;
                            }
                    }
                }

                return count - p;
            }


            for (byte from = 0; from < 64; from++)
            {
                byte face = myBoard[myPlayer, from];

                if (face == Gl.Empty) continue;

                int x, y;
                byte to;

                if (face != Gl.King)
                {
                    ulong mask = BoardC.BitFaceAttack[face, from]; //Get mask for face
                    mask |= BoardC.BitFaceMove[face, from];

                    if ((mask & threatmask) == 0 && face < Gl.WPawn) continue;
                }

                if (face == Gl.getRook(0) || face == Gl.getQueen(0))
                {
                    //+x
                    x = from % 8 + 1;
                    y = from / 8;
                    to = from;
                    for (; x <= 7; x++)
                    {
                        to++;
                        if (myBoard[myPlayer, to] != Gl.Empty) break;

                        if ((threatmask & BoardC.Bit[to]) != 0 && !ThreatOnSquareAfterMove(myKingPos[myPlayer], from, to))
                        {
                            moves[count].from = from;
                            moves[count].to = to;
                            moves[count].promote = Gl.getPawn(0);
                            moves[count].cmp = 0;
                            if (myBoard[myOpponent, to] != Gl.Empty)
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - BoardC.FaceValue[face] / 10;
                            count++;
                        }

                        if (myBoard[myOpponent, to] != Gl.Empty) break;
                    }

                    //-x
                    x = from % 8 - 1;
                    y = from / 8;
                    to = from;
                    for (; x >= 0; x--)
                    {
                        to--;
                        if (myBoard[myPlayer, to] != Gl.Empty) break;

                        if ((threatmask & BoardC.Bit[to]) != 0 && !ThreatOnSquareAfterMove(myKingPos[myPlayer], from, to))
                        {
                            moves[count].from = from;
                            moves[count].to = to;
                            moves[count].promote = Gl.getPawn(0);
                            moves[count].cmp = 0;
                            if (myBoard[myOpponent, to] != Gl.Empty)
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - BoardC.FaceValue[face] / 10;
                            count++;
                        }

                        if (myBoard[myOpponent, to] != Gl.Empty) break;
                    }

                    //+y
                    x = from % 8;
                    y = from / 8 + 1;
                    to = from;
                    for (; y <= 7; y++)
                    {
                        to += 8;
                        if (myBoard[myPlayer, to] != Gl.Empty) break;

                        if ((threatmask & BoardC.Bit[to]) != 0 && !ThreatOnSquareAfterMove(myKingPos[myPlayer], from, to))
                        {
                            moves[count].from = from;
                            moves[count].to = to;
                            moves[count].promote = Gl.getPawn(0);
                            moves[count].cmp = 0;
                            if (myBoard[myOpponent, to] != Gl.Empty)
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - BoardC.FaceValue[face] / 10;
                            count++;
                        }

                        if (myBoard[myOpponent, to] != Gl.Empty) break;
                    }

                    //-y
                    x = from % 8;
                    y = from / 8 - 1;
                    to = from;
                    for (; y >= 0; y--)
                    {
                        to -= 8;
                        if (myBoard[myPlayer, to] != Gl.Empty) break;

                        if ((threatmask & BoardC.Bit[to]) != 0 && !ThreatOnSquareAfterMove(myKingPos[myPlayer], from, to))
                        {
                            moves[count].from = from;
                            moves[count].to = to;
                            moves[count].promote = Gl.getPawn(0);
                            moves[count].cmp = 0;
                            if (myBoard[myOpponent, to] != Gl.Empty)
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - BoardC.FaceValue[face] / 10;
                            count++;
                        }

                        if (myBoard[myOpponent, to] != Gl.Empty) break;
                    }

                    if (face == Gl.getRook(0)) continue;
                }



                if (face == Gl.getBishop(0) || face == Gl.getQueen(0))
                {
                    //+x+y
                    x = from % 8 + 1;
                    y = from / 8 + 1;
                    to = from;
                    for (; x <= 7 && y <= 7; x++, y++)
                    {
                        to += 9;
                        if (myBoard[myPlayer, to] != Gl.Empty) break;

                        if ((threatmask & BoardC.Bit[to]) != 0 && !ThreatOnSquareAfterMove(myKingPos[myPlayer], from, to))
                        {
                            moves[count].from = from;
                            moves[count].to = to;
                            moves[count].promote = Gl.getPawn(0);
                            moves[count].cmp = 0;
                            if (myBoard[myOpponent, to] != Gl.Empty)
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - BoardC.FaceValue[face] / 10;
                            count++;
                        }

                        if (myBoard[myOpponent, to] != Gl.Empty) break;
                    }

                    //-x+y
                    x = from % 8 - 1;
                    y = from / 8 + 1;
                    to = from;
                    for (; x >= 0 && y <= 7; x--, y++)
                    {
                        to += 7;
                        if (myBoard[myPlayer, to] != Gl.Empty) break;

                        if ((threatmask & BoardC.Bit[to]) != 0 && !ThreatOnSquareAfterMove(myKingPos[myPlayer], from, to))
                        {
                            moves[count].from = from;
                            moves[count].to = to;
                            moves[count].promote = Gl.getPawn(0);
                            moves[count].cmp = 0;
                            if (myBoard[myOpponent, to] != Gl.Empty)
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - BoardC.FaceValue[face] / 10;
                            count++;
                        }

                        if (myBoard[myOpponent, to] != Gl.Empty) break;
                    }

                    //-y+x
                    x = from % 8 + 1;
                    y = from / 8 - 1;
                    to = from;
                    for (; y >= 0 && x <= 7; y--, x++)
                    {
                        to -= 7;
                        if (myBoard[myPlayer, to] != Gl.Empty) break;

                        if ((threatmask & BoardC.Bit[to]) != 0 && !ThreatOnSquareAfterMove(myKingPos[myPlayer], from, to))
                        {
                            moves[count].from = from;
                            moves[count].to = to;
                            moves[count].promote = Gl.getPawn(0);
                            moves[count].cmp = 0;
                            if (myBoard[myOpponent, to] != Gl.Empty)
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - BoardC.FaceValue[face] / 10;
                            count++;
                        }

                        if (myBoard[myOpponent, to] != Gl.Empty) break;
                    }

                    //-y-x
                    x = from % 8 - 1;
                    y = from / 8 - 1;
                    to = from;
                    for (; x >= 0 && y >= 0; y--, x--)
                    {
                        to -= 9;
                        if (myBoard[myPlayer, to] != Gl.Empty) break;

                        if ((threatmask & BoardC.Bit[to]) != 0 && !ThreatOnSquareAfterMove(myKingPos[myPlayer], from, to))
                        {
                            moves[count].from = from;
                            moves[count].to = to;
                            moves[count].promote = Gl.getPawn(0);
                            moves[count].cmp = 0;
                            if (myBoard[myOpponent, to] != Gl.Empty)
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - BoardC.FaceValue[face] / 10;
                            count++;
                        }

                        if (myBoard[myOpponent, to] != Gl.Empty) break;
                    }

                    continue;
                }



                if (face == Gl.BPawn)
                {
                    x = from % 8;
                    y = from / 8;
                    to = (byte)(from - 8);
                    if ((BoardC.Bit[to] & myEmpty) != 0)
                    {
                        if ((threatmask & BoardC.Bit[to]) != 0 && !ThreatOnSquareAfterMove(myKingPos[myPlayer], from, to))
                        {
                            moves[count].from = from;
                            moves[count].to = to;
                            moves[count].promote = Gl.getPawn(0);
                            moves[count].cmp = 0;
                            count++;

                            if (to <= 7)//Promotion
                            {
                                moves[count - 1].promote = Gl.getQueen(0);
                                moves[count - 1].cmp = BoardC.FaceValue[Gl.getQueen(0)] - BoardC.FaceValue[Gl.BPawn];

                                moves[count] = moves[count - 1];
                                moves[count].promote = Gl.getKnight(0);
                                moves[count].cmp = BoardC.FaceValue[Gl.getKnight(0)] - BoardC.FaceValue[Gl.BPawn];
                                count++;

                                moves[count] = moves[count - 1];
                                moves[count].promote = Gl.getRook(0);
                                moves[count].cmp = BoardC.FaceValue[Gl.getRook(0)] - BoardC.FaceValue[Gl.BPawn];
                                count++;

                                moves[count] = moves[count - 1];
                                moves[count].promote = Gl.getBishop(0);
                                moves[count].cmp = BoardC.FaceValue[Gl.getBishop(0)] - BoardC.FaceValue[Gl.BPawn];
                                count++;
                            }
                        }

                        if (y == 6)
                        {
                            to = (byte)(from - 16);
                            if ((BoardC.Bit[to] & myEmpty) != 0)
                                if ((threatmask & BoardC.Bit[to]) != 0 && !ThreatOnSquareAfterMove(myKingPos[myPlayer], from, to))
                                {
                                    moves[count].from = from;
                                    moves[count].to = to;
                                    moves[count].promote = Gl.getPawn(0);
                                    moves[count].cmp = 0;
                                    count++;
                                }
                        }
                    }


                    if (x >= 1)
                    {
                        to = (byte)(from - 9);
                        if (myBoard[myOpponent, to] != Gl.Empty || to == myPassants[myMemoryIndex])
                            if (((threatmask & BoardC.Bit[to]) != 0 || to == myPassants[myMemoryIndex]) && !ThreatOnSquareAfterMove(myKingPos[myPlayer], from, to))
                            {
                                moves[count].from = from;
                                moves[count].to = to;
                                moves[count].promote = Gl.getPawn(0);
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10;
                                count++;

                                if (to <= 7)//Promotion
                                {
                                    moves[count - 1].promote = Gl.getQueen(0);
                                    moves[count - 1].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getQueen(0)] - BoardC.FaceValue[Gl.BPawn];

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getKnight(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getKnight(0)] - BoardC.FaceValue[Gl.BPawn];
                                    count++;

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getRook(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getRook(0)] - BoardC.FaceValue[Gl.BPawn];
                                    count++;

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getBishop(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getBishop(0)] - BoardC.FaceValue[Gl.BPawn];
                                    count++;
                                }
                            }
                    }


                    if (x <= 6)
                    {
                        to = (byte)(from - 7);
                        if (myBoard[myOpponent, to] != Gl.Empty || to == myPassants[myMemoryIndex])
                            if (((threatmask & BoardC.Bit[to]) != 0 || to == myPassants[myMemoryIndex]) && !ThreatOnSquareAfterMove(myKingPos[myPlayer], from, to))
                            {
                                moves[count].from = from;
                                moves[count].to = to;
                                moves[count].promote = Gl.getPawn(0);
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10;
                                count++;

                                if (to <= 7)//Promotion
                                {
                                    moves[count - 1].promote = Gl.getQueen(0);
                                    moves[count - 1].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getQueen(0)] - BoardC.FaceValue[Gl.BPawn];

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getKnight(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getKnight(0)] - BoardC.FaceValue[Gl.BPawn];
                                    count++;

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getRook(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getRook(0)] - BoardC.FaceValue[Gl.BPawn];
                                    count++;

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getBishop(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getBishop(0)] - BoardC.FaceValue[Gl.BPawn];
                                    count++;
                                }
                            }
                    }

                    continue;
                }



                if (face == Gl.WPawn)
                {
                    x = from % 8;
                    y = from / 8;
                    to = (byte)(from + 8);
                    if ((BoardC.Bit[to] & myEmpty) != 0)
                    {
                        if ((threatmask & BoardC.Bit[to]) != 0 && !ThreatOnSquareAfterMove(myKingPos[myPlayer], from, to))
                        {
                            moves[count].from = from;
                            moves[count].to = to;
                            moves[count].promote = Gl.getPawn(0);
                            moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - BoardC.FaceValue[Gl.WPawn];
                            count++;

                            if (to >= 56)//Promotion
                            {
                                moves[count - 1].promote = Gl.getQueen(0);
                                moves[count - 1].cmp = BoardC.FaceValue[Gl.getQueen(0)] - BoardC.FaceValue[Gl.WPawn];

                                moves[count] = moves[count - 1];
                                moves[count].promote = Gl.getKnight(0);
                                moves[count].cmp = BoardC.FaceValue[Gl.getKnight(0)] - BoardC.FaceValue[Gl.WPawn];
                                count++;

                                moves[count] = moves[count - 1];
                                moves[count].promote = Gl.getRook(0);
                                moves[count].cmp = BoardC.FaceValue[Gl.getRook(0)] - BoardC.FaceValue[Gl.WPawn];
                                count++;

                                moves[count] = moves[count - 1];
                                moves[count].promote = Gl.getBishop(0);
                                moves[count].cmp = BoardC.FaceValue[Gl.getBishop(0)] - BoardC.FaceValue[Gl.WPawn];
                                count++;
                            }
                        }

                        if (y == 1)
                        {
                            to = (byte)(from + 16);
                            if ((BoardC.Bit[to] & myEmpty) != 0)
                                if ((threatmask & BoardC.Bit[to]) != 0 && !ThreatOnSquareAfterMove(myKingPos[myPlayer], from, to))
                                {
                                    moves[count].from = from;
                                    moves[count].to = to;
                                    moves[count].promote = Gl.getPawn(0);
                                    moves[count].cmp = 0;
                                    count++;
                                }
                        }
                    }


                    if (x <= 6)
                    {
                        to = (byte)(from + 9);
                        if (myBoard[myOpponent, to] != Gl.Empty || to == myPassants[myMemoryIndex])
                            if (((threatmask & BoardC.Bit[to]) != 0 || to == myPassants[myMemoryIndex]) && !ThreatOnSquareAfterMove(myKingPos[myPlayer], from, to))
                            {
                                moves[count].from = from;
                                moves[count].to = to;
                                moves[count].promote = Gl.getPawn(0);
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10;
                                count++;

                                if (to >= 56)//Promotion
                                {
                                    moves[count - 1].promote = Gl.getQueen(0);
                                    moves[count - 1].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getQueen(0)] - BoardC.FaceValue[Gl.WPawn];

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getKnight(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getKnight(0)] - BoardC.FaceValue[Gl.WPawn];
                                    count++;

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getRook(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getRook(0)] - BoardC.FaceValue[Gl.WPawn];
                                    count++;

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getBishop(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getBishop(0)] - BoardC.FaceValue[Gl.WPawn];
                                    count++;
                                }
                            }
                    }


                    if (x >= 1)
                    {
                        to = (byte)(from + 7);
                        if (myBoard[myOpponent, to] != Gl.Empty || to == myPassants[myMemoryIndex])
                            if (((threatmask & BoardC.Bit[to]) != 0 || to == myPassants[myMemoryIndex]) && !ThreatOnSquareAfterMove(myKingPos[myPlayer], from, to))
                            {
                                moves[count].from = from;
                                moves[count].to = to;
                                moves[count].promote = Gl.getPawn(0);
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10;
                                count++;

                                if (to >= 56)//Promotion
                                {
                                    moves[count - 1].promote = Gl.getQueen(0);
                                    moves[count - 1].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getQueen(0)] - BoardC.FaceValue[Gl.WPawn];

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getKnight(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getKnight(0)] - BoardC.FaceValue[Gl.WPawn];
                                    count++;

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getRook(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getRook(0)] - BoardC.FaceValue[Gl.WPawn];
                                    count++;

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getBishop(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getBishop(0)] - BoardC.FaceValue[Gl.WPawn];
                                    count++;
                                }
                            }
                    }

                    continue;
                }




                if (face == Gl.King)
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

                    for (int i = 0; i < 16; i += 2)
                    {
                        x = from % 8 + coords[i];
                        y = from / 8 + coords[i + 1];
                        if (x >= 0 && x <= 7 && y >= 0 && y <= 7)
                        {
                            to = (byte)(x + y * 8);
                            if (myBoard[myPlayer, to] == Gl.Empty)
                                if (!ThreatOnSquareAfterMove(to, from, to))
                                {
                                    moves[count].from = from;
                                    moves[count].to = to;
                                    moves[count].promote = Gl.getPawn(0);
                                    moves[count].cmp = 0;
                                    if (myBoard[myOpponent, to] != Gl.Empty)
                                        moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 90;
                                    count++;
                                }
                        }
                    }

                    continue;
                }



                if (face == Gl.getKnight(0))
                {
                    int[] coords = new int[16]
					{
						2,  1,
						2, -1,
						1,  2,
						1, -2,
						-2,  1,
						-2, -1,
						-1,  2,
						-1, -2,
					};

                    for (int i = 0; i < 16; i += 2)
                    {
                        x = from % 8 + coords[i];
                        y = from / 8 + coords[i + 1];
                        if (x >= 0 && x <= 7 && y >= 0 && y <= 7)
                        {
                            to = (byte)(x + y * 8);
                            if (myBoard[myPlayer, to] == Gl.Empty)
                                if ((threatmask & BoardC.Bit[to]) != 0 && !ThreatOnSquareAfterMove(myKingPos[myPlayer], from, to))
                                {
                                    moves[count].from = from;
                                    moves[count].to = to;
                                    moves[count].promote = Gl.getPawn(0);
                                    moves[count].cmp = 0;
                                    if (myBoard[myOpponent, to] != Gl.Empty)
                                        moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 30;
                                    count++;
                                }
                        }
                    }

                    continue;
                }
            }

            return count - p;
        }




        public int GetAllMoves(Move[] moves, int p)
        {
            int count = p;

            for (byte from = 0; from < 64; from++)
            {
                byte face = myBoard[myPlayer, from];

                if (face == Gl.Empty) continue;

                int x, y;
                byte to;



                if (face == Gl.getRook(0) || face == Gl.getQueen(0))
                {
                    //+x
                    x = from % 8 + 1;
                    y = from / 8;
                    to = from;
                    for (; x <= 7; x++)
                    {
                        to++;
                        if (myBoard[myPlayer, to] != Gl.Empty) break;

                        if (!InCheckAfterMoveFromSafe(myKingPos[myPlayer], from, to))
                        {
                            moves[count].from = from;
                            moves[count].to = to;
                            moves[count].promote = Gl.getPawn(0);
                            moves[count].cmp = 0;
                            if (myBoard[myOpponent, to] != Gl.Empty)
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - BoardC.FaceValue[face] / 10;
                            count++;
                        }
                        else break;

                        if (myBoard[myOpponent, to] != Gl.Empty) break;
                    }

                    //-x
                    x = from % 8 - 1;
                    y = from / 8;
                    to = from;
                    for (; x >= 0; x--)
                    {
                        to--;
                        if (myBoard[myPlayer, to] != Gl.Empty) break;

                        if (!InCheckAfterMoveFromSafe(myKingPos[myPlayer], from, to))
                        {
                            moves[count].from = from;
                            moves[count].to = to;
                            moves[count].promote = Gl.getPawn(0);
                            moves[count].cmp = 0;
                            if (myBoard[myOpponent, to] != Gl.Empty)
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - BoardC.FaceValue[face] / 10;
                            count++;
                        }
                        else break;

                        if (myBoard[myOpponent, to] != Gl.Empty) break;
                    }

                    //+y
                    x = from % 8;
                    y = from / 8 + 1;
                    to = from;
                    for (; y <= 7; y++)
                    {
                        to += 8;
                        if (myBoard[myPlayer, to] != Gl.Empty) break;

                        if (!InCheckAfterMoveFromSafe(myKingPos[myPlayer], from, to))
                        {
                            moves[count].from = from;
                            moves[count].to = to;
                            moves[count].promote = Gl.getPawn(0);
                            moves[count].cmp = 0;
                            if (myBoard[myOpponent, to] != Gl.Empty)
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - BoardC.FaceValue[face] / 10;
                            count++;
                        }
                        else break;


                        if (myBoard[myOpponent, to] != Gl.Empty) break;
                    }

                    //-y
                    x = from % 8;
                    y = from / 8 - 1;
                    to = from;
                    for (; y >= 0; y--)
                    {
                        to -= 8;
                        if (myBoard[myPlayer, to] != Gl.Empty) break;

                        if (!InCheckAfterMoveFromSafe(myKingPos[myPlayer], from, to))
                        {
                            moves[count].from = from;
                            moves[count].to = to;
                            moves[count].promote = Gl.getPawn(0);
                            moves[count].cmp = 0;
                            if (myBoard[myOpponent, to] != Gl.Empty)
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - BoardC.FaceValue[face] / 10;
                            count++;
                        }
                        else break;

                        if (myBoard[myOpponent, to] != Gl.Empty) break;
                    }

                    if (face == Gl.getRook(0)) continue;
                }



                if (face == Gl.getBishop(0) || face == Gl.getQueen(0))
                {
                    //+x+y
                    x = from % 8 + 1;
                    y = from / 8 + 1;
                    to = from;
                    for (; x <= 7 && y <= 7; x++, y++)
                    {
                        to += 9;
                        if (myBoard[myPlayer, to] != Gl.Empty) break;

                        if (!InCheckAfterMoveFromSafe(myKingPos[myPlayer], from, to))
                        {
                            moves[count].from = from;
                            moves[count].to = to;
                            moves[count].promote = Gl.getPawn(0);
                            moves[count].cmp = 0;
                            if (myBoard[myOpponent, to] != Gl.Empty)
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - BoardC.FaceValue[face] / 10;
                            count++;
                        }
                        else break;

                        if (myBoard[myOpponent, to] != Gl.Empty) break;
                    }

                    //-x+y
                    x = from % 8 - 1;
                    y = from / 8 + 1;
                    to = from;
                    for (; x >= 0 && y <= 7; x--, y++)
                    {
                        to += 7;
                        if (myBoard[myPlayer, to] != Gl.Empty) break;

                        if (!InCheckAfterMoveFromSafe(myKingPos[myPlayer], from, to))
                        {
                            moves[count].from = from;
                            moves[count].to = to;
                            moves[count].promote = Gl.getPawn(0);
                            moves[count].cmp = 0;
                            if (myBoard[myOpponent, to] != Gl.Empty)
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - BoardC.FaceValue[face] / 10;
                            count++;
                        }
                        else break;

                        if (myBoard[myOpponent, to] != Gl.Empty) break;
                    }

                    //-y+x
                    x = from % 8 + 1;
                    y = from / 8 - 1;
                    to = from;
                    for (; y >= 0 && x <= 7; y--, x++)
                    {
                        to -= 7;
                        if (myBoard[myPlayer, to] != Gl.Empty) break;

                        if (!InCheckAfterMoveFromSafe(myKingPos[myPlayer], from, to))
                        {
                            moves[count].from = from;
                            moves[count].to = to;
                            moves[count].promote = Gl.getPawn(0);
                            moves[count].cmp = 0;
                            if (myBoard[myOpponent, to] != Gl.Empty)
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - BoardC.FaceValue[face] / 10;
                            count++;
                        }
                        else break;

                        if (myBoard[myOpponent, to] != Gl.Empty) break;
                    }

                    //-y-x
                    x = from % 8 - 1;
                    y = from / 8 - 1;
                    to = from;
                    for (; x >= 0 && y >= 0; y--, x--)
                    {
                        to -= 9;
                        if (myBoard[myPlayer, to] != Gl.Empty) break;

                        if (!InCheckAfterMoveFromSafe(myKingPos[myPlayer], from, to))
                        {
                            moves[count].from = from;
                            moves[count].to = to;
                            moves[count].promote = Gl.getPawn(0);
                            moves[count].cmp = 0;
                            if (myBoard[myOpponent, to] != Gl.Empty)
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - BoardC.FaceValue[face] / 10;
                            count++;
                        }
                        else break;

                        if (myBoard[myOpponent, to] != Gl.Empty) break;
                    }

                    continue;
                }



                if (face == Gl.BPawn)
                {
                    x = from % 8;
                    y = from / 8;
                    to = (byte)(from - 8);
                    if ((BoardC.Bit[to] & myEmpty) != 0)
                    {
                        if (!InCheckAfterMoveFromSafe(myKingPos[myPlayer], from, to))
                        {
                            moves[count].from = from;
                            moves[count].to = to;
                            moves[count].promote = Gl.getPawn(0);
                            moves[count].cmp = 0;
                            count++;

                            if (to <= 7)//Promotion
                            {
                                moves[count - 1].promote = Gl.getQueen(0);
                                moves[count - 1].cmp = BoardC.FaceValue[Gl.getQueen(0)] - BoardC.FaceValue[Gl.BPawn];

                                moves[count] = moves[count - 1];
                                moves[count].promote = Gl.getKnight(0);
                                moves[count].cmp = BoardC.FaceValue[Gl.getKnight(0)] - BoardC.FaceValue[Gl.BPawn];
                                count++;

                                moves[count] = moves[count - 1];
                                moves[count].promote = Gl.getRook(0);
                                moves[count].cmp = BoardC.FaceValue[Gl.getRook(0)] - BoardC.FaceValue[Gl.BPawn];
                                count++;

                                moves[count] = moves[count - 1];
                                moves[count].promote = Gl.getBishop(0);
                                moves[count].cmp = BoardC.FaceValue[Gl.getBishop(0)] - BoardC.FaceValue[Gl.BPawn];
                                count++;
                            }
                        }

                        if (y == 6)
                        {
                            to = (byte)(from - 16);
                            if ((BoardC.Bit[to] & myEmpty) != 0)
                                if (!InCheckAfterMoveFromSafe(myKingPos[myPlayer], from, to))
                                {
                                    moves[count].from = from;
                                    moves[count].to = to;
                                    moves[count].promote = Gl.getPawn(0);
                                    moves[count].cmp = 0;
                                    count++;
                                }
                        }
                    }


                    if (x >= 1)
                    {
                        to = (byte)(from - 9);
                        if (myBoard[myOpponent, to] != Gl.Empty || to == myPassants[myMemoryIndex])
                            if (!InCheckAfterMoveFromSafe(myKingPos[myPlayer], from, to))
                            {
                                moves[count].from = from;
                                moves[count].to = to;
                                moves[count].promote = Gl.getPawn(0);
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10;
                                count++;

                                if (to <= 7)//Promotion
                                {
                                    moves[count - 1].promote = Gl.getQueen(0);
                                    moves[count - 1].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getQueen(0)] - BoardC.FaceValue[Gl.BPawn];

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getKnight(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getKnight(0)] - BoardC.FaceValue[Gl.BPawn];
                                    count++;

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getRook(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getRook(0)] - BoardC.FaceValue[Gl.BPawn];
                                    count++;

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getBishop(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getBishop(0)] - BoardC.FaceValue[Gl.BPawn];
                                    count++;
                                }
                            }
                    }


                    if (x <= 6)
                    {
                        to = (byte)(from - 7);
                        if (myBoard[myOpponent, to] != Gl.Empty || to == myPassants[myMemoryIndex])
                            if (!InCheckAfterMoveFromSafe(myKingPos[myPlayer], from, to))
                            {
                                moves[count].from = from;
                                moves[count].to = to;
                                moves[count].promote = Gl.getPawn(0);
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10;
                                count++;

                                if (to <= 7)//Promotion
                                {
                                    moves[count - 1].promote = Gl.getQueen(0);
                                    moves[count - 1].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getQueen(0)] - BoardC.FaceValue[Gl.BPawn];

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getKnight(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getKnight(0)] - BoardC.FaceValue[Gl.BPawn];
                                    count++;

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getRook(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getRook(0)] - BoardC.FaceValue[Gl.BPawn];
                                    count++;

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getBishop(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getBishop(0)] - BoardC.FaceValue[Gl.BPawn];
                                    count++;
                                }
                            }
                    }

                    continue;
                }



                if (face == Gl.WPawn)
                {
                    x = from % 8;
                    y = from / 8;
                    to = (byte)(from + 8);
                    if ((BoardC.Bit[to] & myEmpty) != 0)
                    {
                        if (!InCheckAfterMoveFromSafe(myKingPos[myPlayer], from, to))
                        {
                            moves[count].from = from;
                            moves[count].to = to;
                            moves[count].promote = Gl.getPawn(0);
                            moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - BoardC.FaceValue[Gl.WPawn];
                            count++;

                            if (to >= 56)//Promotion
                            {
                                moves[count - 1].promote = Gl.getQueen(0);
                                moves[count - 1].cmp = BoardC.FaceValue[Gl.getQueen(0)] - BoardC.FaceValue[Gl.WPawn];

                                moves[count] = moves[count - 1];
                                moves[count].promote = Gl.getKnight(0);
                                moves[count].cmp = BoardC.FaceValue[Gl.getKnight(0)] - BoardC.FaceValue[Gl.WPawn];
                                count++;

                                moves[count] = moves[count - 1];
                                moves[count].promote = Gl.getRook(0);
                                moves[count].cmp = BoardC.FaceValue[Gl.getRook(0)] - BoardC.FaceValue[Gl.WPawn];
                                count++;

                                moves[count] = moves[count - 1];
                                moves[count].promote = Gl.getBishop(0);
                                moves[count].cmp = BoardC.FaceValue[Gl.getBishop(0)] - BoardC.FaceValue[Gl.WPawn];
                                count++;
                            }
                        }

                        if (y == 1)
                        {
                            to = (byte)(from + 16);
                            if ((BoardC.Bit[to] & myEmpty) != 0)
                                if (!InCheckAfterMoveFromSafe(myKingPos[myPlayer], from, to))
                                {
                                    moves[count].from = from;
                                    moves[count].to = to;
                                    moves[count].promote = Gl.getPawn(0);
                                    moves[count].cmp = 0;
                                    count++;
                                }
                        }
                    }


                    if (x <= 6)
                    {
                        to = (byte)(from + 9);
                        if (myBoard[myOpponent, to] != Gl.Empty || to == myPassants[myMemoryIndex])
                            if (!InCheckAfterMoveFromSafe(myKingPos[myPlayer], from, to))
                            {
                                moves[count].from = from;
                                moves[count].to = to;
                                moves[count].promote = Gl.getPawn(0);
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10;
                                count++;

                                if (to >= 56)//Promotion
                                {
                                    moves[count - 1].promote = Gl.getQueen(0);
                                    moves[count - 1].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getQueen(0)] - BoardC.FaceValue[Gl.WPawn];

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getKnight(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getKnight(0)] - BoardC.FaceValue[Gl.WPawn];
                                    count++;

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getRook(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getRook(0)] - BoardC.FaceValue[Gl.WPawn];
                                    count++;

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getBishop(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getBishop(0)] - BoardC.FaceValue[Gl.WPawn];
                                    count++;
                                }
                            }
                    }


                    if (x >= 1)
                    {
                        to = (byte)(from + 7);
                        if (myBoard[myOpponent, to] != Gl.Empty || to == myPassants[myMemoryIndex])
                            if (!InCheckAfterMoveFromSafe(myKingPos[myPlayer], from, to))
                            {
                                moves[count].from = from;
                                moves[count].to = to;
                                moves[count].promote = Gl.getPawn(0);
                                moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10;
                                count++;

                                if (to >= 56)//Promotion
                                {
                                    moves[count - 1].promote = Gl.getQueen(0);
                                    moves[count - 1].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getQueen(0)] - BoardC.FaceValue[Gl.WPawn];

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getKnight(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getKnight(0)] - BoardC.FaceValue[Gl.WPawn];
                                    count++;

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getRook(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getRook(0)] - BoardC.FaceValue[Gl.WPawn];
                                    count++;

                                    moves[count] = moves[count - 1];
                                    moves[count].promote = Gl.getBishop(0);
                                    moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 10 + BoardC.FaceValue[Gl.getBishop(0)] - BoardC.FaceValue[Gl.WPawn];
                                    count++;
                                }
                            }
                    }

                    continue;
                }




                if (face == Gl.King)
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

                    for (int i = 0; i < 16; i += 2)
                    {
                        x = from % 8 + coords[i];
                        y = from / 8 + coords[i + 1];
                        if (x >= 0 && x <= 7 && y >= 0 && y <= 7)
                        {
                            to = (byte)(x + y * 8);
                            if (myBoard[myPlayer, to] == Gl.Empty)
                                if (!ThreatOnSquareAfterMove(to, from, to))
                                {
                                    moves[count].from = from;
                                    moves[count].to = to;
                                    moves[count].promote = Gl.getPawn(0);
                                    moves[count].cmp = 0;
                                    if (myBoard[myOpponent, to] != Gl.Empty)
                                        moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 90;
                                    count++;
                                }
                        }
                    }
                    /*
                    byte rookpos = (byte)(myPlayer * 56);
                    if (to % 8 > 4) rookpos += 7;

                    if ((myCastlings[myMemoryIndex] & BoardC.Bit[rookpos]) == 0) return false;

                    if ((BoardC.CollisionMask[from,rookpos] & ~myEmpty) == 0) //If free path
                    {
                        if (ThreatOnSquare(to)) return false;
                        if (ThreatOnSquare(from)) return false;

                        if (from < to)
                        {
                            if (ThreatOnSquare((byte)(from + 1))) return false;
                        }
                        else
                        {
                            if (ThreatOnSquare((byte)(from - 1))) return false;
                        }
                    }
                    else
                        return false;
                    */
                    if (myPlayer == Gl.White)
                    {
                        if (from == 4)
                        {
                            //if ((myEmpty & BoardC.Bit[5]) != 0)
                            if (ValidatePseudoMove(from, 6, face))
                            {
                                moves[count].from = from;
                                moves[count].to = 6;
                                moves[count].promote = Gl.getPawn(0);
                                moves[count].cmp = 0;
                                count++;
                            }

                            //if ((myEmpty & BoardC.Bit[3]) != 0)
                            if (ValidatePseudoMove(from, 2, face))
                            {
                                moves[count].from = from;
                                moves[count].to = 2;
                                moves[count].promote = Gl.getPawn(0);
                                moves[count].cmp = 0;
                                count++;
                            }
                        }
                    }
                    else
                    {
                        if (from == 60)
                        {
                            //if ((myEmpty & BoardC.Bit[61]) != 0)
                            if (ValidatePseudoMove(from, 62, face))
                            {
                                moves[count].from = from;
                                moves[count].to = 62;
                                moves[count].promote = Gl.getPawn(0);
                                moves[count].cmp = 0;
                                count++;
                            }

                            //if ((myEmpty & BoardC.Bit[59]) != 0)
                            if (ValidatePseudoMove(from, 58, face))
                            {
                                moves[count].from = from;
                                moves[count].to = 58;
                                moves[count].promote = Gl.getPawn(0);
                                moves[count].cmp = 0;
                                count++;
                            }
                        }
                    }

                    continue;
                }



                if (face == Gl.getKnight(0))
                {
                    int[] coords = new int[16]
					{
						2,  1,
						2, -1,
						1,  2,
						1, -2,
						-2,  1,
						-2, -1,
						-1,  2,
						-1, -2,
					};

                    for (int i = 0; i < 16; i += 2)
                    {
                        x = from % 8 + coords[i];
                        y = from / 8 + coords[i + 1];
                        if (x >= 0 && x <= 7 && y >= 0 && y <= 7)
                        {
                            to = (byte)(x + y * 8);
                            if (myBoard[myPlayer, to] == Gl.Empty)
                                if (!InCheckAfterMoveFromSafe(myKingPos[myPlayer], from, to))
                                {
                                    moves[count].from = from;
                                    moves[count].to = to;
                                    moves[count].promote = Gl.getPawn(0);
                                    moves[count].cmp = 0;
                                    if (myBoard[myOpponent, to] != Gl.Empty)
                                        moves[count].cmp = BoardC.FaceValue[myBoard[myOpponent, to]] - 30;
                                    count++;
                                }
                        }
                    }

                    continue;
                }
            }

            return count - p;
        }


        public void ActualMove(byte from, byte to, byte promote)
        {
            if (myPlayer == Gl.Black) myFullMoves++;
            Move move = new Move();
            move.from = from; move.to = to; move.promote = promote;
            DoMove(move);
        }


        public void DoMove(Move move)//, out byte capture, out byte capture_pos, out byte passant, out ulong oddmoves)
        {
            myMemoryIndex++;

            myHalfMoves[myMemoryIndex] = (byte)(myHalfMoves[myMemoryIndex - 1] + 1);
            myPassants[myMemoryIndex] = myPassants[myMemoryIndex - 1];
            myCastlings[myMemoryIndex] = myCastlings[myMemoryIndex - 1];
            myZobristKeys[myMemoryIndex] = myZobristKeys[myMemoryIndex - 1];

            byte face = myBoard[myPlayer, move.from];

            ulong b_from = BoardC.Bit[move.from];
            //myBitBoard[myPlayer,face] &= ~b_from;//Remove from board
            myPlayerPieces &= ~b_from;
            myEmpty |= b_from;
            myBoard[myPlayer, move.from] = Gl.Empty;//начало передвижение веса
            myZobristKeys[myMemoryIndex] ^= ZobristC.PositionKeys[myPlayer, face, move.from];


            if (myBoard[myOpponent, move.to] != Gl.Empty || (face >= Gl.WPawn && move.to == myPassants[myMemoryIndex]))//Capture?
            {
                myHalfMoves[myMemoryIndex] = 0;

                if (myBoard[myOpponent, move.to] != Gl.Empty) //Ordinary capture обыкновение захвата
                {
                    myCapturePositions[myMemoryIndex] = move.to;
                    myCaptures[myMemoryIndex] = myBoard[myOpponent, move.to];

                    if (myCaptures[myMemoryIndex] == Gl.getRook(0)) myCastlings[myMemoryIndex] &= ~BoardC.Bit[move.to]; //Clear myCastling if a rook is captured эта лодья теперь не сможнт зделать рокировку
                }
                else //En passant
                {
                    myCapturePositions[myMemoryIndex] = (byte)((move.from / 8) * 8 + move.to % 8); //Same row as capturing pawn and same column as to-square координаты от куда начнётся ход
                    if (myPlayer == Gl.White)
                        myCaptures[myMemoryIndex] = Gl.BPawn;
                    else
                        myCaptures[myMemoryIndex] = Gl.WPawn;
                    myEmpty |= BoardC.Bit[myCapturePositions[myMemoryIndex]]; //This pos will be left empty, so remove it from the board клетка занята
                }

                //myBitBoard[myOpponent,capture] &= ~BoardC.Bit[capture_pos];//Remove from myOpponent board
                myBoard[myOpponent, myCapturePositions[myMemoryIndex]] = Gl.Empty;

                myZobristKeys[myMemoryIndex] ^= ZobristC.PositionKeys[myOpponent, myCaptures[myMemoryIndex], myCapturePositions[myMemoryIndex]];

                myMaterial += BoardC.FaceValue[myCaptures[myMemoryIndex]];//Update material value
            }
            else
            {
                myCaptures[myMemoryIndex] = Gl.Empty;
                myCapturePositions[myMemoryIndex] = 64;
            }

            if (face == Gl.getRook(0)) myCastlings[myMemoryIndex] &= ~BoardC.Bit[move.from]; //Clear the castlemask if a rook moves

            if (myPassants[myMemoryIndex] != 64) myZobristKeys[myMemoryIndex] ^= ZobristC.PassantKeys[myPassants[myMemoryIndex]];
            myPassants[myMemoryIndex] = 64; //Clear passantmask

            if (face >= Gl.WPawn)
            {
                myHalfMoves[myMemoryIndex] = 0;

                //Set passant mask
                if (move.from > move.to)
                {
                    if (move.from == move.to + 16) //Two square move ХОД НА 2 КЛЕТКИ ВПЕРЁД
                        myPassants[myMemoryIndex] = (byte)(move.to + 8);
                }
                else
                {
                    if (move.to == move.from + 16) //Two square move
                        myPassants[myMemoryIndex] = (byte)(move.from + 8);
                }

                if (myPassants[myMemoryIndex] != 64) myZobristKeys[myMemoryIndex] ^= ZobristC.PassantKeys[myPassants[myMemoryIndex]];

                if (move.promote != Gl.getPawn(0))//Promotion, myEmpty and myPieces unchanged неизменны
                {
                    face = move.promote;

                    myMaterial += BoardC.FaceValue[move.promote] - BoardC.FaceValue[Gl.WPawn];
                }
            }


            ulong b_to = BoardC.Bit[move.to];
            //myBitBoard[myPlayer,face] |= b_to;//Place on board
            myPlayerPieces |= b_to;
            myEmpty &= ~b_to;
            myBoard[myPlayer, move.to] = face; //вес передвинут
            myZobristKeys[myMemoryIndex] ^= ZobristC.PositionKeys[myPlayer, face, move.to];


            if (face == Gl.King)
            {
                myCastlings[myMemoryIndex] &= ~BoardC.CastleMask[myPlayer]; //Clear the castlemask if the king moves

                myKingPos[myPlayer] = move.to; //Update KingPos helper

                if (move.from == move.to + 2 || move.to == move.from + 2)//Castling, move the rook to!
                {
                    byte rookfrom = (byte)(myPlayer * 56);
                    if (move.to % 8 > 4) rookfrom += 7;

                    byte rookto = (byte)(myPlayer * 56 + 3);
                    if (move.to % 8 > 4) rookto += 2;

                    ulong b_rookfrom = BoardC.Bit[rookfrom];
                    //myBitBoard[myPlayer,GlobalC.Rook] &= ~b_rookfrom;//Remove from board
                    myPlayerPieces &= ~b_rookfrom;
                    myEmpty |= b_rookfrom;
                    myBoard[myPlayer, rookfrom] = Gl.Empty;
                    myZobristKeys[myMemoryIndex] ^= ZobristC.PositionKeys[myPlayer, Gl.getRook(0), rookfrom];


                    ulong b_rookto = BoardC.Bit[rookto];
                    //myBitBoard[myPlayer,GlobalC.Rook] |= b_rookto;//Place on board
                    myPlayerPieces |= b_rookto;
                    myEmpty &= ~b_rookto;
                    myBoard[myPlayer, rookto] = Gl.getRook(0);
                    myZobristKeys[myMemoryIndex] ^= ZobristC.PositionKeys[myPlayer, Gl.getRook(0), rookto];
                }
            }

            if (myCastlings[myMemoryIndex] != myCastlings[myMemoryIndex - 1])
            {
                ulong temp;

                temp = myCastlings[myMemoryIndex] ^ myCastlings[myMemoryIndex - 1];

                if ((temp & BoardC.Bit[0]) != 0) myZobristKeys[myMemoryIndex] ^= ZobristC.CastlingKeys[0];
                if ((temp & BoardC.Bit[7]) != 0) myZobristKeys[myMemoryIndex] ^= ZobristC.CastlingKeys[1];
                if ((temp & BoardC.Bit[56]) != 0) myZobristKeys[myMemoryIndex] ^= ZobristC.CastlingKeys[2];
                if ((temp & BoardC.Bit[63]) != 0) myZobristKeys[myMemoryIndex] ^= ZobristC.CastlingKeys[3];
            }

            //Update the ZobristKey when switching user обновить перед сменой пользователя
            myZobristKeys[myMemoryIndex] ^= ZobristC.ColourKey;

            SwitchPlayer();//Смена игрока
        }


        public void UndoMove(Move move)//, byte capture, byte capture_pos, byte passant, ulong oddmoves)
        {
            SwitchPlayer();

            byte face = myBoard[myPlayer, move.to];

            ulong b_to = BoardC.Bit[move.to];
            //myBitBoard[myPlayer,face] &= ~b_to;//Remove from board
            myPlayerPieces &= ~b_to;
            myEmpty |= b_to;
            myBoard[myPlayer, move.to] = Gl.Empty;


            if (myCaptures[myMemoryIndex] != Gl.Empty)
            {
                ulong b_capture = BoardC.Bit[myCapturePositions[myMemoryIndex]];

                //myBitBoard[myOpponent,capture] |= b_capture;//Place on board
                myEmpty &= ~b_capture;
                myBoard[myOpponent, myCapturePositions[myMemoryIndex]] = myCaptures[myMemoryIndex];
                myMaterial -= BoardC.FaceValue[myCaptures[myMemoryIndex]];//Update material value
            }


            if (move.promote != Gl.getPawn(0)) //Promotion Occured
            {
                myMaterial -= BoardC.FaceValue[face] - BoardC.FaceValue[Gl.WPawn];
                if (myPlayer == Gl.White)
                    face = Gl.WPawn;
                else
                    face = Gl.BPawn;
            }


            ulong b_from = BoardC.Bit[move.from];
            //myBitBoard[myPlayer,face] |= b_from;//Place on board
            myPlayerPieces |= b_from;
            myEmpty &= ~b_from;
            myBoard[myPlayer, move.from] = face;


            if (face == Gl.King)
            {
                myKingPos[myPlayer] = move.from; //Update KingPos helper

                if (move.from == move.to + 2 || move.to == move.from + 2)//Castling, move the rook to!
                {
                    byte rookfrom = (byte)(myPlayer * 56);
                    if (move.to % 8 > 4) rookfrom += 7;

                    byte rookto = (byte)(myPlayer * 56 + 3);
                    if (move.to % 8 > 4) rookto += 2;

                    ulong b_rookfrom = BoardC.Bit[rookfrom];
                    //myBitBoard[myPlayer,GlobalC.Rook] |= b_rookfrom;//Place on board
                    myPlayerPieces |= b_rookfrom;
                    myEmpty &= ~b_rookfrom;
                    myBoard[myPlayer, rookfrom] = Gl.getRook(0);

                    ulong b_rookto = BoardC.Bit[rookto];
                    //myBitBoard[myPlayer,GlobalC.Rook] &= ~b_rookto;//Remove from board
                    myPlayerPieces &= ~b_rookto;
                    myEmpty |= b_rookto;
                    myBoard[myPlayer, rookto] = Gl.Empty;
                }
            }

            myMemoryIndex--;
        }


        private int PositionBonus()
        {
            int res = 0;

            //Sum PST bonus for player
            for (int i = 0; i < 64; i++)
            {
                byte face = myBoard[myPlayer, i];
                if (face == Gl.Empty) continue;
                res += BoardC.FacePositionBonus[face, i];
                if (myFullMoves < 12)
                {
                    if (face == Gl.getQueen(0) && i == 3) res -= 5;
                    if (face == Gl.getKnight(0) && i < 7) res -= 18;
                    if (face == Gl.getBishop(0) && i < 7) res -= 14;
                }
                if (face == Gl.King)
                {
                    if (i < 24)
                    {
                        byte pface1 = myBoard[myPlayer, i + 7];
                        byte pface2 = myBoard[myPlayer, i + 8];
                        byte pface3 = myBoard[myPlayer, i + 9];
                        if (pface1 == Gl.WPawn && pface2 == Gl.WPawn && pface3 == Gl.WPawn) res += 30;
                    }
                }

            }
            // Sum PST bonus for opponent
            for (int i = 0; i < 64; i++)
            {
                byte face = myBoard[myOpponent, i];
                if (face == Gl.Empty) continue;
                res -= BoardC.FacePositionBonus[face, i];
                if (myFullMoves < 12)
                {
                    if (face == Gl.getQueen(0) && i == 59) res += 5;
                    if (face == Gl.getKnight(0) && i > 56) res += 18;
                    if (face == Gl.getBishop(0) && i > 56) res += 14;
                }
                if (face == Gl.King)
                {
                    if (i > 39)
                    {
                        byte pface1 = myBoard[myPlayer, i - 7];
                        byte pface2 = myBoard[myPlayer, i - 8];
                        byte pface3 = myBoard[myPlayer, i - 9];
                        if (pface1 == Gl.BPawn && pface2 == Gl.BPawn && pface3 == Gl.BPawn) res -= 30;
                    }
                }
            }
            return res;
        }


        public int Evaluate(int alpha, int beta)
        {
            //if (myMaterial < alpha - 50 || myMaterial > beta + 50)
            //	return myMaterial;
            return myMaterial + PositionBonus();
        }


        public void SwitchPlayer()//смена игрока
        {
            myMaterial *= -1; //Change material balance to reflect myPlayer balanceИзмените материальный баланс, чтобы отразить баланс myPlayer
            myPlayer = myOpponent;
            myOpponent = (byte)(1 - myPlayer);

            myPlayerPieces = ~myEmpty & ~myPlayerPieces;
        }

        public void w()
        {
            Weight[0] = Gl.getRook(1);
            Weight[1] = Gl.getKnight(1);
            Weight[2] = Gl.getBishop(1);
            Weight[3] = Gl.getQueen(1);
            Weight[4] = 1;
            Weight[5] = Gl.getBishop(1);
            Weight[6] = Gl.getKnight(1);
            Weight[7] = Gl.getRook(1);
            for (int i = 8; i < 16; i++)
            {
                Weight[i] = 1;
            }
        }


        //TODO: Felaktig FEN accepteras men krashar i motorn vid perft, rnbqkbnr/pppp4/8/PPPP4/4pppp/8/4PPPP/RNBQKBNR b KQkq D3
        public void SetBoardFEN(string fen)
        {
            w();
            string white = "", black = "";
            for (int i = 0; i < Gl.Faces + 1; i++) white += Gl.PieceFace(Gl.White, i);
            for (int i = 0; i < Gl.Faces + 1; i++) black += Gl.PieceFace(Gl.Black, i);

            //Clear board
            for (int i = 0; i < 64; i++) myBoard[Gl.White, i] = Gl.Empty;
            for (int i = 0; i < 64; i++) myBoard[Gl.Black, i] = Gl.Empty;
            for (int i = 0; i < 64; i++) Arm[Gl.White, i] = 0;
            for (int i = 0; i < 64; i++) Arm[Gl.Black, i] = 0;
            myEmpty = ~(ulong)0;

            string[] arr = fen.Split(new char[] { '/', ' ' }, 14);

            if (arr.Length < 8)
            {
                throw new ArgumentException("Invalid FEN!");
            }

            myMemoryIndex = 0;
            myCastlings[myMemoryIndex] = BoardC.Bit[0] | BoardC.Bit[7] | BoardC.Bit[56] | BoardC.Bit[63];
            myPassants[myMemoryIndex] = 64;
            myCaptures[myMemoryIndex] = Gl.Empty;
            myCapturePositions[myMemoryIndex] = 64;


            //Read board
            int a = 0;
            int b = 15;
            for (int row = 7, i = 0; row >= 0; row--, i++)
            {
                for (int col = 0, pos = 0; pos < arr[i].Length; col++, pos++)
                {
                    if (white.IndexOf(arr[i][pos]) != -1)
                    {
                        myBoard[Gl.White, row * 8 + col] = (byte)white.IndexOf(arr[i][pos]);

                        Arm[Gl.White, row * 8 + col] = Gl.getWArm(b) * Weight[b];
                        Arm[Gl.White, row * 8 + col] = Arm[Gl.White, row * 8 + col];
                        b--;
                        myEmpty &= ~BoardC.Bit[row * 8 + col];
                    }
                    else if (black.IndexOf(arr[i][pos]) != -1)
                    {
                        myBoard[Gl.Black, row * 8 + col] = (byte)black.IndexOf(arr[i][pos]);
                        Arm[Gl.Black, row * 8 + col] = Gl.getBArm(a) * Weight[a];
                        a++;
                        myEmpty &= ~BoardC.Bit[row * 8 + col];
                    }
                    else
                        col += arr[i][pos] - '0' - 1;
                }
            }

            //myPlayer to move, default is W
            if (arr.Length >= 9)
            {
                if (Gl.SideLetter(1) == arr[8][0])
                {
                    myPlayer = Gl.Black;
                    myOpponent = Gl.White;
                }
                else
                {
                    myPlayer = Gl.White;
                    myOpponent = Gl.Black;
                }
            }

            //Kingpos
            for (int i = 0; i < 64; i++) if (myBoard[myPlayer, i] == Gl.King) myKingPos[myPlayer] = (byte)i;
            for (int i = 0; i < 64; i++) if (myBoard[myOpponent, i] == Gl.King) myKingPos[myOpponent] = (byte)i;

            //Material
            myMaterial = 0;
            for (int i = 0; i < 64; i++) myMaterial += BoardC.FaceValue[myBoard[myPlayer, i]];
            for (int i = 0; i < 64; i++) myMaterial -= BoardC.FaceValue[myBoard[myOpponent, i]];


            //Pieces
            myPlayerPieces = 0;
            for (int i = 0; i < 64; i++)
            {
                if (myBoard[myPlayer, i] != Gl.Empty) myPlayerPieces |= BoardC.Bit[i];
            }

            //Castling
            myCastlings[myMemoryIndex] = 0;
            if (arr.Length >= 10)
            {
                if (arr[9].IndexOf(Gl.PieceFace(Gl.White, Gl.King)) != -1) myCastlings[myMemoryIndex] |= BoardC.Bit[7];
                if (arr[9].IndexOf(Gl.PieceFace(Gl.White, Gl.getQueen(0))) != -1) myCastlings[myMemoryIndex] |= BoardC.Bit[0];
                if (arr[9].IndexOf(Gl.PieceFace(Gl.Black, Gl.King)) != -1) myCastlings[myMemoryIndex] |= BoardC.Bit[63];
                if (arr[9].IndexOf(Gl.PieceFace(Gl.Black, Gl.getQueen(0))) != -1) myCastlings[myMemoryIndex] |= BoardC.Bit[56];
            }

            //Passant square
            myPassants[myMemoryIndex] = 64;
            if (arr.Length >= 11 && arr[10].Length == 2)
                myPassants[myMemoryIndex] = Square(arr[10]);


            myHalfMoves[myMemoryIndex] = 0;
            if (arr.Length >= 12)
                myHalfMoves[myMemoryIndex] = (byte)Int32.Parse(arr[11]);

            myFullMoves = 1;
            if (arr.Length >= 13)
                myFullMoves = Int32.Parse(arr[12]);


            myZobristKeys[myMemoryIndex] = GetZobristKey();


            UpdateGamePhase();
        }


        public string GetBoardFEN()
        {
            string res = "";

            int empty = 0;
            for (int row = 7; row >= 0; row--)
            {
                for (int col = 0; col < 8; col++)
                {
                    if ((myEmpty & BoardC.Bit[row * 8 + col]) != 0)
                    {
                        empty++;
                    }
                    else
                    {
                        if (empty > 0)
                        {
                            res += empty.ToString();
                            empty = 0;
                        }

                        if (myBoard[Gl.White, row * 8 + col] != Gl.Empty)
                        {
                            res += Gl.PieceFace(Gl.White, myBoard[Gl.White, row * 8 + col]);
                        }
                        else
                        {
                            res += Gl.PieceFace(Gl.Black, myBoard[Gl.Black, row * 8 + col]);
                        }
                    }
                }

                if (empty > 0)
                {
                    res += empty.ToString();
                    empty = 0;
                }

                if (row != 0) res += '/';
            }

            res += ' ';

            //Side to move
            res += Gl.SideLetter(myPlayer);
            res += ' ';

            //Castling availability
            if (myCastlings[myMemoryIndex] == 0)
            {
                res += '-';
            }
            else
            {
                if ((myCastlings[myMemoryIndex] & BoardC.Bit[7]) != 0)
                    res += Gl.PieceFace(Gl.White, Gl.King);
                if ((myCastlings[myMemoryIndex] & BoardC.Bit[0]) != 0)
                    res += Gl.PieceFace(Gl.White, Gl.getQueen(0));
                if ((myCastlings[myMemoryIndex] & BoardC.Bit[63]) != 0)
                    res += Gl.PieceFace(Gl.Black, Gl.King);
                if ((myCastlings[myMemoryIndex] & BoardC.Bit[56]) != 0)
                    res += Gl.PieceFace(Gl.Black, Gl.getQueen(0));
            }
            res += ' ';


            //Passantsquare
            if (myPassants[myMemoryIndex] != 64)
                res += Square(myPassants[myMemoryIndex]);
            else
                res += '-';


            //Moves
            res += " ";
            res += myHalfMoves[myMemoryIndex].ToString();

            res += " ";
            res += myFullMoves.ToString();


            return res;
        }


        public string PrintBoard()
        {
            string result = "";

            result += " |--------|\n";

            for (sbyte r = 7; r >= 0; r--)
            {
                result += " |";
                for (byte c = 0; c < 8; c++)
                {
                    ulong b = BoardC.Bit[r * 8 + c];
                    if ((myEmpty & b) != 0)
                    {
                        result += ' ';
                    }
                    else
                    {
                        if ((myPlayerPieces & b) != 0) //Get which myPlayer owns the piece
                            result += Gl.PieceFace(myPlayer, myBoard[myPlayer, r * 8 + c]);
                        else
                            result += Gl.PieceFace(myOpponent, myBoard[myOpponent, r * 8 + c]);
                    }
                }
                result += "|\n";
            }

            result += String.Format(" |--------|: {0}", Evaluate(myMaterial, myMaterial));

            return result;
        }


        public static string Square(byte square)
        {
            const string col = "abcdefgh";
            const string row = "12345678";

            if (square > 63) throw new Exception("Invalid square");

            return String.Format("{0}{1}", col[square % 8], row[square / 8]);
        }


        public static byte Square(string square)
        {
            string temp;

            temp = square.Substring(0, 2);
            if (temp.Length != 2) throw new Exception("Invalid square");

            temp = temp.ToLower();

            if (temp[0] < 'a' || temp[0] > 'h') throw new Exception("Invalid square");

            if (temp[1] < '1' || temp[1] > '8') throw new Exception("Invalid square");

            return (byte)(temp[0] - 'a' + (temp[1] - '1') * 8);
        }


        private ulong GetZobristKey()
        {
            ulong key = 0;

            for (int p = 0; p < Gl.Sides; p++)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (myBoard[p, i] != Gl.Empty)
                    {
                        key ^= ZobristC.PositionKeys[p, myBoard[p, i], i];
                    }
                }
            }

            if (myPassants[myMemoryIndex] != 64)
            {
                key ^= ZobristC.PassantKeys[myPassants[myMemoryIndex]];
            }

            return key;
        }
        private void Shift(byte from, byte to)
        {
            int x = Arm[myPlayer, from];
            Arm[myPlayer, from] = 0;
            Arm[myPlayer, to] = x;
        }

        public int[] Fight(byte from, byte to)
        {
            int[] t = new int[2];
            int temp = Arm[myPlayer, to];
            if (temp == 0)
            {
                Shift(from, to);
                t[0] = 0;
                t[1] = Arm[myPlayer, to];
                return t;
            }
            if (Arm[myPlayer, from] >= Arm[myPlayer, to])
            {
                Shift(from, to);
                t[0] = 0;
                t[1] = Arm[myPlayer, to];
                return t;
            }
            else
            {
                t[0] = 1;
                t[1] = 0;
                return t;
            }

        }


    }
}

