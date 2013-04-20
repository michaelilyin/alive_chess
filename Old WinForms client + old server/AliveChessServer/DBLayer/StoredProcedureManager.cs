namespace AliveChessServer.DBLayer
{
    public class StoredProcedureManager
    {
        //private AliveChessDataClassesDataContext _context;

        //public StoredProcedureManager(AliveChessDataClassesDataContext context)
        //{
        //    this._context = context;
        //}

        //public void Insert(SingleObject singleObject)
        //{
        //    _context.InsertSingleObject(singleObject.DbId, singleObject.MapId, singleObject.SingleObjectType,
        //        singleObject.ViewOnMap.ImageId, singleObject.X, singleObject.Y, singleObject.ViewOnMap.WayCost);
        //}

        //public void Insert(MultyObject multyObject)
        //{
        //    if (multyObject.ViewOnMap == null) throw new NullReferenceException("View on map is null");
        //    _context.InsertMultyObject(multyObject.DbId, multyObject.MapId, multyObject.MultyObjectType,
        //        multyObject.ViewOnMap.ImageId, multyObject.ViewOnMap.LeftX, multyObject.ViewOnMap.TopY,
        //        multyObject.ViewOnMap.Width, multyObject.ViewOnMap.Height, multyObject.ViewOnMap.WayCost);
        //}

        //public void Insert(Resource resource)
        //{
        //    Guid? vaultId = null;
        //    if (resource.Vault != null) vaultId = resource.Vault.DbId;
        //    if (resource.ViewOnMap == null) throw new NullReferenceException("View on map is null");
        //    _context.InsertResource(resource.DbId, resource.MapId, vaultId, resource.ResourceType,
        //        resource.CountResource, resource.ViewOnMap.ImageId, resource.X, resource.Y, resource.ViewOnMap.WayCost);
        //}

        //public void Insert(BasePoint landscape)
        //{
        //    _context.InsertBasePointObject(landscape.DbId, landscape.MapId, landscape.LandscapeType,
        //        landscape.ViewOnMap.ImageId, landscape.X, landscape.Y, landscape.ViewOnMap.WayCost);
        //}

        //public void Insert(Castle castle)
        //{
        //    Guid? kingId = null;
        //    if (castle.King != null) kingId = castle.King.DbId;
        //    if (castle.ViewOnMap == null) throw new NullReferenceException("View on map is null");
        //    if (castle.Vicegerent == null) throw new NullReferenceException("Vicegerent is null");
        //    if (castle.Vicegerent.Vault == null) throw new NullReferenceException("Vault is null");
        //    _context.InsertCastle(castle.DbId, castle.MapId, kingId, castle.ViewOnMap.ImageId, 
        //        castle.ViewOnMap.LeftX, castle.ViewOnMap.TopY, castle.ViewOnMap.Width, castle.ViewOnMap.Height, 
        //        castle.ViewOnMap.WayCost, castle.Vicegerent.Name);
        //}

        //public void Insert(Mine mine)
        //{
        //    Guid? kingId = null;
        //    if (mine.King != null) kingId = mine.King.DbId;
        //    if (mine.ViewOnMap == null) throw new NullReferenceException("View on map is null");
        //    _context.InsertMine(mine.DbId, mine.MapId, kingId, mine.MineType, mine.ViewOnMap.ImageId, 
        //        mine.ViewOnMap.LeftX, mine.ViewOnMap.TopY, mine.ViewOnMap.Width, mine.ViewOnMap.Height, mine.ViewOnMap.WayCost);
        //}

        //public void Insert(Player player)
        //{
        //    if (player.King == null) throw new NullReferenceException("King is null");
        //    if (player.King.ViewOnMap == null) throw new NullReferenceException("View on map is null");
        //    if (player.King.StartCastle == null) throw new NullReferenceException("Castle is null");
        //    if (player.King.StartCastle.ResourceStore == null) throw new NullReferenceException("Vault is null");
        //    _context.InsertPlayer(player.DbId, player.LevelId, player.King.StartCastle.DbId, player.Login, 
        //        player.Password, player.King.Name, player.King.Experience, player.King.MilitaryRank, player.King.ViewOnMap.ImageId, 
        //        player.King.ViewOnMap.X, player.King.ViewOnMap.Y, player.King.ViewOnMap.WayCost);
        //}

        //public void Insert(Level level)
        //{
        //    if (level.Map == null) throw new NullReferenceException("Map is null");
        //    _context.InsertLevel(level.DbId, level.LevelType, level.Map.SizeX, level.Map.SizeY);
        //}
    }
}
