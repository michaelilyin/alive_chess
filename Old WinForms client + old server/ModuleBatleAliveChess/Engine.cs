using System;
using System.Threading;

namespace ModuleBatleAliveChess
{
    public class Engine
    {
        GlobalC Gl = new GlobalC();
        private Output o;
        private Move[] moves = new Move[1000];
        private Move[] myPV = new Move[100];
        private int myPVLastPos;
        private enum ClockType { Incremental/*возорастающий*/, Exact/*точный*/, Total/*общее колличество*/};

        private static long myPerft;

        private BoardModFighters myBoard;
        private HashTable myHashTable;
        private History myHistory;

        //Controls thinking indirectly by activating/deactivating ThinkOn command
        //Контроль мышления косвенным путем активизации / отключения ThinkOn команду
        private bool myForce;// моя сила


        //private bool		myPondering;
        private bool myInMove;

        //Thinking control
        private Thread myThinker;// мыслитель
        private bool myThink;//мысль

        private bool myThinking;// мышление


        private int myMaxDepth;// максимальная глубина

        //Search result
        private Move myMove;
        //private byte		myFrom;
        //private byte		myTo;
        //private byte		myPromotion;
        private int myResult;

        //Time control
        private ClockType myClockType;
        private long myTimeLimit; //MilliSeconds
        private long myIncrement; //MilliSeconds
        private long myThinkStart; //Ticks (100 ns parts)

        private byte[] RESULT = new byte[3];
        private byte[,] BoardFild;
        private int[,] Arm;
        private int v = 0;

        public long MyThinkStart
        {
            get { return myThinkStart; }
            set { myThinkStart = value; }
        }

        public int V
        {
            get { return v; }
            //    set { v = value; }
        }

        public bool MyForce
        {
            get { return myForce; }
        }

        public bool MyThinking
        {
            get { return myThinking; }
            //   set { myThinking = value; }
        }

        public bool MyInMove
        {
            get { return myInMove; }
            //   set { myInMove = value; }
        }

        public bool MyThink
        {
            get { return myThink; }
            set { myThink = value; }
        }


        public Engine(Output o, byte[,] myB, int[,] Arm)
        {
            this.o = o;
            BoardFild = myB;
            this.Arm = Arm;
            myBoard = new BoardModFighters(myB, Arm);
            myHashTable = new HashTable();
            myHistory = new History();

            GenerateConstants();

            myThink = false;
            myThinking = false;
            myThinker = new Thread(new ThreadStart(Thinker));
            myThinker.IsBackground = true;
            myThinker.Start();

            myInMove = false;

            Initialize();
        }


        public void Initialize()
        {
            ThinkOff();

            myForce = false;
            myMaxDepth = 50;

            //myPondering = false;
            myInMove = false;

            SetTime(3000);
            myBoard.Initialize();

            myHistory.Clear();
            myHistory.Increase(myBoard.ZobristKey);

            myHashTable.Clear();
        }

        /*
                public void Easy()
                {
                    myPondering = false;
                    if (!myInMove) ThinkOff();
                    //TODO: If ThinkOff() then program won't play???
                }


                public void Hard()
                {
                    myPondering = true;
                    ThinkOn();
                }
        */


        public void Level(int moves, int seconds, int increment)
        {
            if (increment != 0) //ICS incremental clock
            {
                myClockType = ClockType.Incremental;
                myIncrement = increment * 1000;
                myTimeLimit = seconds * 1000;
                return;
            }

            if (moves != 0)
            {
                SetTime((seconds * 1000) / moves);
                return;
            }

            myClockType = ClockType.Total;
            myTimeLimit = seconds * 1000;
        }


        public void Time(int milliseconds)
        {
            if (myClockType == ClockType.Total) myTimeLimit = milliseconds;
        }


        public void SetTime(int milliseconds)
        {
            myClockType = ClockType.Exact;
            myTimeLimit = milliseconds;
        }


        //private void ThinkOn()
        //{
        //    if (myForce) return;

        //    //if (!myPondering && !myInMove) return;
        //    if (!myInMove) return;

        //    if (myThinking) return;

        //    myThink = true;
        //    myThinkStart = DateTime.Now.Ticks;
        //}


        private void ThinkOff()
        {
            if (!myThinking) return;
            myThink = false;
            while (myThinking) Thread.Sleep(1);
        }


        public void Dispose()
        {
            myThinker.Abort();
            o = null;
        }


        private void Controller()
        {
            long stopptime;

            if (myClockType == ClockType.Exact)
            {
                stopptime = myThinkStart + myTimeLimit * 10000 - 150000;
            }
            else if (myClockType == ClockType.Incremental)
            {
                if (myTimeLimit > 0)
                    stopptime = myThinkStart + (myTimeLimit * 10000) / 20 + myIncrement * 10000 - 150000;
                else
                    stopptime = myThinkStart + myIncrement * 10000 - 150000;
            }
            else
            {
                stopptime = myThinkStart + (myTimeLimit * 10000) / 20 - 250000;
            }

            if (DateTime.Now.Ticks >= stopptime) myThink = false;
        }

        private void Thinker()
        {
            while (true)
            {
                Thread.Sleep(1);
                if (myThink)
                {
                    myThinking = true;

                    //myFrom = myTo = 64; myPromotion = GlobalC.Pawn;
                    myMove.from = myMove.to; myMove.promote = Gl.getPawn(0);
                    myResult = Gl.Defeat;// поражение
                    DateTime start, stopp;
                    start = DateTime.Now;

                    myBoard.UpdateGamePhase();

                    for (int i = 1; i <= myMaxDepth && myThink; i++)
                    {
                        //byte tfrom, tto, tpromotion;
                        Move temp;
                        myPerft = 0;
                        myPVLastPos = i;
                        myResult = AlphaBeta(i, Gl.Defeat, Gl.Victory, 0, out temp);

                        if (myThink)
                        {
                            //myFrom = tfrom;
                            //myTo = tto;
                            //myPromotion = tpromotion;
                            myMove = temp;
                            string t = string.Format(" {0,2} {1,6} {2,5} {3,10}",
                                i.ToString(),
                                myResult,
                                (DateTime.Now.Subtract(start).Milliseconds / 10).ToString(),
                                myPerft.ToString()
                                );

                            for (int p = 0; p < i && myPV[p].from != myPV[p].to; p++)
                            {
                                t += " " + PrintMove(myPV[p]);
                            }

                            //o.Out(t);
                        }

                        if (myResult == Gl.Defeat || myResult == Gl.Victory)
                        {
                            if (myResult == Gl.Defeat)
                                o.Log("Mate found =(");
                            else
                                o.Log("Mate found =)");
                            break;
                        }
                    }

                    stopp = DateTime.Now;
                    TimeSpan diff = stopp.Subtract(start);
                    //o.Out(" Time: " + diff.ToString());

                    if (myInMove)
                    {
                        //byte[] res = new byte[3];
                        RESULT = MoveAndGetResult(myResult, myMove);
                        if (myClockType == ClockType.Incremental) myTimeLimit -= myTimeLimit / 20 + 1000;
                        //o.Out(res);
                    }

                    //myHashTable.Clear();

                    myThink = false;
                    myThinking = false;
                }
            }
        }


        public void GenerateConstants()
        {
            BoardC.InitializeBoardC();
        }





        public void Force()
        {
            myForce = true;
            ThinkOff();
        }


        public string MoveNow()
        {
            ThinkOff();
            return "";
            /*
                        if (myThinking)
                        {
                            myInMove = false;
                            string res = MoveAndGetResult(myResult, myFrom, myTo, myPromotion);
                            //if (!res.StartsWith("result")) ThinkOn();
                            return res;
                        }
                        else
                            return "";
            */
        }


        public void SetDepth(int depth)
        {
            myMaxDepth = depth;
        }


        public void HashSize(uint size)
        {
            ThinkOff();
            myHashTable.Size(size);
        }


        public void New()
        {
            Initialize();
        }


        public byte[] UserMove(byte[] move)
        {
            ////if (move.Length < 4) throw new Exception("No move specified");// действие не указанно

            //byte from, to,
            bool tmp = true;
            byte promote = Gl.getPawn(0);

            //from = Board.Square(move);
            //to = Board.Square(move.Substring(2));

            //if (move.Length == 5)
            //{
            //    if (move[4] == Gl.PieceFace(Gl.Black, Gl.getQueen(0))) promote = Gl.getQueen(0);
            //    if (move[4] == Gl.PieceFace(Gl.Black, Gl.getKnight(0))) promote = Gl.getKnight(0);
            //    if (move[4] == Gl.PieceFace(Gl.Black, Gl.getBishop(0))) promote = Gl.getBishop(0);
            //    if (move[4] == Gl.PieceFace(Gl.Black, Gl.getRook(0))) promote = Gl.getRook(0);
            //}

            //ThinkOff();
            try
            {
                myBoard.ValidateMove(move[0], move[1], promote);
            }
            catch
            {
                // ThinkOn();
                tmp = false;
            }
            if (tmp)
            {
                int[] ok = new int[2];
                ok = myBoard.Fight(move[0], move[1]);

                if (ok[0] == 0)
                {
                    myBoard.ActualMove(move[0], move[1], promote);
                    v = ok[1];
                }
                else myBoard.SwitchPlayer();
                bool repetition = !myHistory.Increase(myBoard.ZobristKey);
            }

            //if (myBoard.DrawByMoves || repetition)
            //  return "result 1/2-1/2";

            //myInMove = true;
            //ThinkOn(); //Start thinking

            if (!tmp) return RESULT;
            else return null;
        }


        private void SearchAll(byte depth, int pos)
        {
            if (depth == 0)
            {
                myPerft++;
                return;
            }

            int count = myBoard.GetMoves(moves, pos);
            OrderMoves(pos, count);

            pos += count;
            for (int i = pos - count; i < pos; i++)
            {
#if (DEBUG)
                BoardModFighters b = new BoardModFighters(myBoard, BoardFild, Arm);
#endif
                myBoard.DoMove(moves[i]);
                SearchAll((byte)(depth - 1), pos);
                myBoard.UndoMove(moves[i]);
#if (DEBUG)
                myBoard.CompareWith(b);
#endif
            }
        }


        public string Divide(byte depth)// разделение
        {
            ThinkOff();

            string res = "";

            int count = myBoard.GetMoves(moves, 0);
            int k = 0;
            for (int i = 0; i < count; i++)
            {
                myBoard.DoMove(moves[i]);
                myPerft = 0;
                SearchAll((byte)(depth - 1), count);
                res += String.Format("Moves: {0}, {1}\n", myPerft, PrintMove(moves[i]));
                myBoard.UndoMove(moves[i]);
                k++;
            }

            res += k.ToString() + " positions";

            return res;
        }


        public string Perft(int i)
        {
            ThinkOff();

            DateTime start, stopp;

            string result = "";

            start = DateTime.Now;
            myPerft = 0;
            SearchAll((byte)i, 0);
            stopp = DateTime.Now;
            TimeSpan diff = stopp.Subtract(start);
            result += String.Format("Moves: {0}, Time: {1}", myPerft, diff);

            return result;
        }




        public byte[] MoveAndGetResult(int res, Move move)
        {
            byte[] t = new byte[3];
            if (move.from == move.to)
            {
                if (res == Gl.Defeat)
                {
                    if (myBoard.Player == Gl.Black)
                    {
                        t[0] = 1;
                        t[1] = 0;
                        t[2] = 1;
                        return t;
                    }
                    else
                    {
                        t[0] = 0;
                        t[1] = 1;
                        t[2] = 1;
                        return t;
                    }
                }
                else
                {
                    t[0] = 1;
                    t[1] = 1;
                    t[2] = 1;
                    return t;
                }
            }
            else
            {
                t[0] = move.from;
                t[1] = move.to;
                t[2] = 0;
                //myBoard.ActualMove(move.from, move.to, move.promote);
                //bool repetition = !myHistory.Increase(myBoard.ZobristKey);

                //string result = "move " + PrintMove(move);

                //if (myBoard.DrawByMoves || repetition)
                //{
                //    result += "\nresult 1/2-1/2";
                //    return result;
                //}

                //if (0 == myBoard.GetMoves(moves, 0))
                //{
                //    if (myBoard.InCheck())
                //    {
                //        if (myBoard.Player == Gl.White)
                //        {
                //            result += "\nresult 1-0";
                //        }
                //        else
                //        {
                //            result += "\nresult 0-1";
                //        }
                //    }
                //    else
                //    {
                //        result += "\nresult 1/2-1/2";
                //    }
                //}

                return t;
            }
        }

        public string PrintMove(Move move)
        {
            string res = "";

            res = BoardModFighters.Square(move.from) + BoardModFighters.Square(move.to);

            if (move.promote != Gl.getPawn(0))
            {
                res += String.Format("{0}", Gl.PieceFace(Gl.White, move.promote));
            }

            return res;
        }


        public void SetBoard(string fen)
        {
            ThinkOff();
            myBoard.SetBoardFEN(fen);
        }


        public string SavePos()
        {
            ThinkOff();
            return myBoard.GetBoardFEN();
        }


        public string Print()
        {
            ThinkOff();
            return myBoard.PrintBoard();
        }


        private void OrderMoves(int pos, int count)
        {
            Move Tmp;
            int j, P;

            for (j = 0, P = 0; P < count - j - 1; P++)
            {
                if (moves[pos + P].cmp == 0)
                {
                    Tmp = moves[pos + count - j - 1];
                    moves[pos + count - j - 1] = moves[pos + P];
                    moves[pos + P] = Tmp;
                    j++;
                    P--;
                }
            }

            count -= j;

            for (P = 1; P < count; P++)
            {
                Tmp = moves[pos + P];
                for (j = P; j > 0 && moves[pos + j - 1].cmp <= Tmp.cmp; j--)
                    moves[pos + j] = moves[pos + j - 1];
                moves[pos + j] = Tmp;
            }
        }


        private int AlphaBeta(int depth, int alpha, int beta, int pos, out Move move)
        {
            move = new Move();
            move.from = move.to = 64; move.promote = Gl.getPawn(0);

            if (depth == 0)
            {
                myPerft++;
                if (myPerft % 1024 == 0 && myInMove) Controller();
                return myBoard.Evaluate(alpha, beta);
            }

            int best = Gl.Defeat;//If no move is found the game is lost

            byte sdepth;
            Move temp;

            int res;
            if (myHashTable.GetItem(myBoard.ZobristKey, out res, out sdepth, out move))
            {
                if (sdepth > depth)
                {
                    myPV[myPVLastPos - depth] = move;
                    myPV[myPVLastPos - depth + 1].from = 64;
                    myPV[myPVLastPos - depth + 1].to = 64;
                    return res;
                }

                myBoard.DoMove(move);
                //pos is used as depth to avoid extra variables.
                best = -AlphaBeta(depth - 1, -beta, -alpha, pos, out temp);
                myBoard.UndoMove(move);
            }

            int count = myBoard.GetMoves(moves, pos);

            OrderMoves(pos, count);

            pos += count;
            for (int i = pos - count; i < pos && best < beta && myThink; i++)
            {
                myBoard.DoMove(moves[i]);

                if (!myHistory.Increase(myBoard.ZobristKey) || myBoard.DrawByMoves)
                {
                    res = Gl.Draw;
                }
                else
                {
                    if (best > alpha) alpha = best;
                    res = -AlphaBeta(depth - 1, -beta, -alpha, pos, out temp);
                }

                if (res > best || move.from == move.to)
                {
                    best = res;
                    move = moves[i];
                    myPV[myPVLastPos - depth] = moves[i]; //Copy the move to the pv string
                }

                myHistory.Decrease(myBoard.ZobristKey);
                myBoard.UndoMove(moves[i]);
            }

            if (move.from == move.to)
            {
                depth = 200; //Set depth to a high value to never search this node again
                if (!myBoard.InCheck())
                    best = Gl.Draw;
            }

            if (myThink)
                myHashTable.SetItem(myBoard.ZobristKey, best, (byte)depth, move);

            return best;
        }
    }
}
