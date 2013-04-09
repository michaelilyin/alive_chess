namespace ModuleBatleAliveChess
{
    public class DownArmy
    {
        private GlobalC fighters = new GlobalC();
        /* public void DownloadArmy(IList<Unit> armPlayer, IList<Unit> armOpponent)
        {
            byte count;
            fighters.getBArm(5) = 1;
            wArm[5] = 1;
            for (int i = 0; i < armPlayer.Count; i++)
            {
                if (armPlayer[i].Name == 2)
                {
                    count = Convert.ToByte(armPlayer[i].CountUnit / 2);
                    fighters.getBArm(0) = count;
                    fighters.getBArm(7) = count;
                    count = Convert.ToByte(armOpponent[i].CountUnit / 2);
                    wArm[0] = count;
                    wArm[7] = count;
                }
                if (armPlayer[i].Name == 0)
                {
                    count = Convert.ToByte(armPlayer[i].CountUnit / 2);
                    fighters.getBArm(1) = count;
                    fighters.getBArm(6) = count;
                    count = Convert.ToByte(armOpponent[i].CountUnit / 2);
                    wArm[1] = count;
                    wArm[6] = count;
                }
                if (armPlayer[i].Name == 3)
                {
                    count = Convert.ToByte(armPlayer[i].CountUnit / 2);
                    fighters.getBArm(2) = count;
                    fighters.getBArm(5) = count;
                    count = Convert.ToByte(armOpponent[i].CountUnit / 2);
                    wArm[2] = count;
                    wArm[5] = count;
                }
                if (armPlayer[i].Name == 1)
                {
                    count = Convert.ToByte(armPlayer[i].CountUnit / 2);
                    fighters.getBArm(3) = count;
                    count = Convert.ToByte(armOpponent[i].CountUnit / 2);
                    wArm[3] = count;
                }

                if (armPlayer[i].Name == 10)
                {
                    count = Convert.ToByte(armPlayer[i].CountUnit / 8);
                    for (int i = 8; i < 16 ; i++)
                    {
                        fighters.getBArm(i) = count;    
                    }
                    count = Convert.ToByte(armOpponent[i].CountUnit / 8);
                    for (int k = 8; k < 16; i++)
                    {
                        fighters.getWArm(k) = count;
                    }
                }
            }
        }*/
    }
}