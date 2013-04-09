using System;
using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Characters;

namespace ModuleBatleAliveChess
{
    public class GlobalC
    {
        private byte faces = 6; //Типы фигур(лица)
        private byte sides = 2; //Стороны
        private byte white = 0;
        private byte black = 1;


        private byte[] pawn = new byte[2] {10, 1};
        private byte[] knight = new byte[2] {0, 3};
        private byte[] queen = new byte[2] {1, 7}; //Must be ordered from queen upto bishop
        private byte[] rook = new byte[2] {2, 5};
        private byte[] bishop = new byte[2] {3, 3};
        private byte king = 4;
        private byte wPawn = 5;
        private byte bPawn = 6;
        private byte empty = 7; //пустой

        private byte[] bArm = new byte[16] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
        private byte[] wArm = new byte[16] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1};


        public void DownloadArmy(IList<Unit> armPlayer, IList<Unit> armOpponent)
        {
            byte count;
            bArm[5] = 1;
            wArm[5] = 1;
            for (int i = 0; i < armPlayer.Count; i++)
            {
                if (Convert.ToInt32(armPlayer[i].UnitType) == 2)
                {
                    count = Convert.ToByte(armPlayer[i].UnitCount/2);
                    bArm[0] = count;
                    bArm[7] = count;
                    count = Convert.ToByte(armOpponent[i].UnitCount/2);
                    wArm[0] = count;
                    wArm[7] = count;
                }
                if (armPlayer[i].UnitType == 0)
                {
                    count = Convert.ToByte(armPlayer[i].UnitCount/2);
                    bArm[1] = count;
                    bArm[6] = count;
                    count = Convert.ToByte(armOpponent[i].UnitCount/2);
                    wArm[1] = count;
                    wArm[6] = count;
                }
                if (Convert.ToInt32(armPlayer[i].UnitType) == 3)
                {
                    count = Convert.ToByte(armPlayer[i].UnitCount/2);
                    bArm[2] = count;
                    bArm[5] = count;
                    count = Convert.ToByte(armOpponent[i].UnitCount/2);
                    wArm[2] = count;
                    wArm[5] = count;
                }
                if (Convert.ToInt32(armPlayer[i].UnitType) == 1)
                {
                    count = Convert.ToByte(armPlayer[i].UnitCount/2);
                    bArm[3] = count;
                    count = Convert.ToByte(armOpponent[i].UnitCount/2);
                    wArm[3] = count;
                }

                if (Convert.ToInt32(armPlayer[i].UnitType) == 10)
                {
                    count = Convert.ToByte(armPlayer[i].UnitCount/8);
                    bArm[8] = count;
                    bArm[9] = count;
                    bArm[10] = count;
                    bArm[11] = count;
                    bArm[12] = count;
                    bArm[13] = count;
                    bArm[14] = count;
                    bArm[15] = count;
                    count = Convert.ToByte(armOpponent[i].UnitCount/8);
                    wArm[8] = count;
                    wArm[9] = count;
                    wArm[10] = count;
                    wArm[11] = count;
                    wArm[12] = count;
                    wArm[13] = count;
                    wArm[14] = count;
                    wArm[15] = count;
                }
            }
        }


        public byte getBArm(int i)
        {
            return bArm[i];
        }

        //public void setBArm(int i, byte v)
        //{ bArm[i] = v;}


        public byte getWArm(int i)
        {
            return wArm[i];
        }

        //public byte setWArm(int i, byte v)
        //{ wArm[i] = v; }

        public byte Faces
        {
            get { return faces; }
            /// set { }
        }

        public byte Sides
        {
            get { return sides; }
        }

        public byte White
        {
            get { return white; }
        }

        public byte Black
        {
            get { return black; }
        }

        public byte getPawn(int i)
        {
            return pawn[i];
        }

        public void setQuantityPawn(byte q)
        {
            pawn[1] = q;
        }

        public byte getKnight(int i)
        {
            return knight[i];
        }

        public void setQuantityKnight(byte q)
        {
            knight[1] = q;
        }

        public byte getQueen(int i)
        {
            return queen[i];
        }

        public void setQuantityQueen(byte q)
        {
            queen[1] = q;
        }

        public byte getRook(int i)
        {
            return rook[i];
        }

        public void setQuantityRook(byte q)
        {
            rook[1] = q;
        }

        public byte getBishop(int i)
        {
            return bishop[i];
        }

        public void setQuantityBishop(byte q)
        {
            bishop[1] = q;
        }

        public byte King
        {
            get { return king; }
        }

        public byte WPawn
        {
            get { return wPawn; }
        }

        public byte BPawn
        {
            get { return bPawn; }
        }

        public byte Empty
        {
            get { return empty; }
            set { empty = value; }
        }


        private char[] sideLetter = new char[2] {'w', 'b'};

        private char[,] pieceFace = new char[2,8]
                                        {
                                            {'N', 'Q', 'R', 'B', 'K', 'P', '-', ' '},
                                            {'n', 'q', 'r', 'b', 'k', '-', 'p', ' '}
                                        }; //For printing, extra place for empty  ' '.

        //для печати, дополнительное место для пустых

        public char PieceFace(int i, int j)
        {
            return pieceFace[i, j];
        }

        public char SideLetter(int i)
        {
            return sideLetter[i];
        }

        private int defeat = -30000; //поражение
        private int draw = 0;
        private int victory = 30000;

        public int Victory
        {
            get { return victory; }
        }

        public int Defeat
        {
            get { return defeat; }
        }

        public int Draw
        {
            get { return draw; }
        }
    }
}