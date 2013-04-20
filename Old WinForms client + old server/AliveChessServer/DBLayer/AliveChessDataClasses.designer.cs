namespace AliveChessServer.DBLayer
{
    using System;
    using System.Data.Linq;
    using System.Data.Linq.Mapping;
    using System.Data;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;
    using System.Linq.Expressions;
    using System.ComponentModel;
    using DbLinq.Vendor;
    using MySql.Data.MySqlClient;
    using AliveChessLibrary.GameObjects.Characters;
    using AliveChessLibrary.GameObjects.Buildings;
    using AliveChessLibrary.GameObjects.Resources;
    using AliveChessLibrary.GameObjects.Landscapes;
    using AliveChessLibrary.GameObjects.Abstract;
    using AliveChessLibrary.GameObjects.Objects;
    using AliveChessServer.LogicLayer.Environment;
    using AliveChessServer.LogicLayer.Environment.Aliances;

    [Database(Name = "alive_chess")]
    public class AliveChessDataClassesDataContext : DbLinq.Data.Linq.DataContext
    {

        public AliveChessDataClassesDataContext()
            : base(GetMySqlConnection(), new DbLinq.MySql.MySqlVendor())
        {
        }

        public AliveChessDataClassesDataContext(IVendor vendor)
            : base(GetMySqlConnection(), vendor)
        {
        }

        private static string GetConnectionString()
        {
            return "Database=alive_chess;Data Source=localhost;User Id=root;Password=pw;old guids=true";
        }

        private static MySqlConnection GetMySqlConnection()
        {
            return new MySqlConnection(GetConnectionString());
        }

        [Function(Name = "alive_chess.insert_multy_object")]
        public void InsertMultyObject(
            [Parameter(Name = "multyObjectID", DbType = "binary(16)")] Guid id,
            [Parameter(Name = "mapID", DbType = "binary(16)")] Guid? mapId,
            [Parameter(Name = "multyObjectType", DbType = "int(11)")] MultyObjectTypes multyObjectType,
            [Parameter(Name = "imageID", DbType = "int(11)")] int? imageId,
            [Parameter(Name = "leftX", DbType = "int(11)")] int leftX,
            [Parameter(Name = "topY", DbType = "int(11)")] int topY,
            [Parameter(Name = "sizeX", DbType = "int(11)")] int sizeX,
            [Parameter(Name = "sizeY", DbType = "int(11)")] int sizeY,
            [Parameter(Name = "wayCost", DbType = "float")] float wayCost)
        {
            ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), id, mapId, multyObjectType,
                imageId, leftX, topY, sizeX, sizeY, wayCost);
        }

        [Function(Name = "alive_chess.insert_single_object")]
        public void InsertSingleObject(
            [Parameter(Name = "singleObjectID", DbType = "binary(16)")] Guid id,
            [Parameter(Name = "mapID", DbType = "binary(16)")] Guid? mapId,
            [Parameter(Name = "singleObjectType", DbType = "int(11)")] SingleObjectType singleObjectType,
            [Parameter(Name = "imageID", DbType = "int(11)")] int? imageId,
            [Parameter(Name = "x", DbType = "int(11)")] int x,
            [Parameter(Name = "y", DbType = "int(11)")] int y,
            [Parameter(Name = "wayCost", DbType = "float")] float wayCost)
        {
            ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), id, mapId, singleObjectType,
                imageId, x, y, wayCost);
        }

        [Function(Name = "alive_chess.insert_base_point")]
        public void InsertBasePointObject(
            [Parameter(Name = "landscapeID", DbType = "binary(16)")] Guid id,
            [Parameter(Name = "mapID", DbType = "binary(16)")] Guid? mapId,
            [Parameter(Name = "landscapeType", DbType = "int(11)")] LandscapeTypes landscapeType,
            [Parameter(Name = "imageID", DbType = "int(11)")] int? imageId,
            [Parameter(Name = "x", DbType = "int(11)")] int x,
            [Parameter(Name = "y", DbType = "int(11)")] int y,
            [Parameter(Name = "wayCost", DbType = "float")] float wayCost)
        {
            ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), id, mapId, landscapeType,
                imageId, x, y, wayCost);
        }

        [Function(Name = "alive_chess.insert_resource")]
        public void InsertResource(
            [Parameter(Name = "resourceID", DbType = "binary(16)")] Guid id,
            [Parameter(Name = "mapID", DbType = "binary(16)")] Guid? mapId,
            [Parameter(Name = "vaultID", DbType = "binary(16)")] Guid? vaultId,
            [Parameter(Name = "resourceType", DbType = "int(11)")] ResourceTypes resourceType,
            [Parameter(Name = "resourceCount", DbType = "int(11)")] int resourceCount,
            [Parameter(Name = "imageID", DbType = "int(11)")] int? imageId,
            [Parameter(Name = "x", DbType = "int(11)")] int x,
            [Parameter(Name = "y", DbType = "int(11)")] int y,
            [Parameter(Name = "wayCost", DbType = "float")] float wayCost)
        {
            ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), id, mapId, vaultId,
                resourceType, resourceCount, imageId, x, y, wayCost);
        }

        [Function(Name = "alive_chess.insert_castle")]
        public void InsertCastle(
            [Parameter(Name = "castleID", DbType = "binary(16)")] Guid id,
            [Parameter(Name = "mapID", DbType = "binary(16)")] Guid? mapId,
            [Parameter(Name = "kingID", DbType = "binary(16)")] Guid? kingId,
            [Parameter(Name = "imageID", DbType = "int(11)")] int? imageId,
            [Parameter(Name = "leftX", DbType = "int(11)")] int leftX,
            [Parameter(Name = "topY", DbType = "int(11)")] int topY,
            [Parameter(Name = "sizeX", DbType = "int(11)")] int sizeX,
            [Parameter(Name = "sizeY", DbType = "int(11)")] int sizeY,
            [Parameter(Name = "wayCost", DbType = "float")] float wayCost,
            [Parameter(Name = "vicegerentName", DbType = "varchar(20)")] string vicegerentName)
        {
            ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), id, mapId, kingId,
                imageId, leftX, topY, sizeX, sizeY, wayCost, vicegerentName);
        }

        [Function(Name = "alive_chess.insert_mine")]
        public void InsertMine(
            [Parameter(Name = "mineID", DbType = "binary(16)")] Guid id,
            [Parameter(Name = "mapID", DbType = "binary(16)")] Guid? mapId,
            [Parameter(Name = "kingID", DbType = "binary(16)")] Guid? kingId,
            [Parameter(Name = "mineType", DbType = "binary(16)")] ResourceTypes mineType,
            [Parameter(Name = "imageID", DbType = "int(11)")] int? imageId,
            [Parameter(Name = "leftX", DbType = "int(11)")] int leftX,
            [Parameter(Name = "topY", DbType = "int(11)")] int topY,
            [Parameter(Name = "sizeX", DbType = "int(11)")] int sizeX,
            [Parameter(Name = "sizeY", DbType = "int(11)")] int sizeY,
            [Parameter(Name = "wayCost", DbType = "float")] float wayCost)
        {
            ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), id, mapId, kingId,
                mineType, imageId, leftX, topY, sizeX, sizeY, wayCost);
        }

        [Function(Name = "alive_chess.insert_player")]
        public void InsertPlayer(
            [Parameter(Name = "playerID", DbType = "binary(16)")] Guid id,
            [Parameter(Name = "levelID", DbType = "binary(16)")] Guid? levelId,
            [Parameter(Name = "castleID", DbType = "binary(16)")] Guid? castleId,
            [Parameter(Name = "playerLogin", DbType = "varchar(20)")] string playerLogin,
            [Parameter(Name = "playerPassword", DbType = "varchar(20)")] string playerPassword,
            [Parameter(Name = "kingName", DbType = "varchar(20)")] string kingName,
            [Parameter(Name = "kingExperience", DbType = "int(11)")] int kingExperience,
            [Parameter(Name = "kingMilitaryRank", DbType = "int(11)")] int kingMilitaryRank,
            [Parameter(Name = "imageID", DbType = "int(11)")] int? imageId,
            [Parameter(Name = "x", DbType = "int(11)")] int x,
            [Parameter(Name = "y", DbType = "int(11)")] int y,
            [Parameter(Name = "wayCost", DbType = "float")] float wayCost)
        {
            ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), id, levelId, castleId,
                playerLogin, playerPassword, kingName, kingExperience, kingMilitaryRank, imageId, x, y, wayCost);
        }

        [Function(Name = "alive_chess.insert_level")]
        public void InsertLevel(
            [Parameter(Name = "levelID", DbType = "binary(16)")] Guid id,
            [Parameter(Name = "levelType", DbType = "int(11)")] LevelTypes levelType,
            [Parameter(Name = "mapSizeX", DbType = "int(11)")] int sizeX,
            [Parameter(Name = "mapSizeY", DbType = "int(11)")] int sizeY)
        {
            ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), id, levelType, sizeX,
                sizeY);
        }

        public DbLinq.Data.Linq.Table<Castle> Castles
        {
            get { return this.GetTable<Castle>(); }
        }

        public DbLinq.Data.Linq.Table<InnerBuilding> InnerBuildings
        {
            get { return this.GetTable<InnerBuilding>(); }
        }

        public DbLinq.Data.Linq.Table<King> Kings
        {
            get { return this.GetTable<King>(); }
        }

        public DbLinq.Data.Linq.Table<Map> Maps
        {
            get { return this.GetTable<Map>(); }
        }

        public DbLinq.Data.Linq.Table<Mine> Mines
        {
            get { return this.GetTable<Mine>(); }
        }

        public DbLinq.Data.Linq.Table<Player> Players
        {
            get { return this.GetTable<Player>(); }
        }

        public DbLinq.Data.Linq.Table<Resource> Resources
        {
            get { return this.GetTable<Resource>(); }
        }

        public DbLinq.Data.Linq.Table<Unit> Units
        {
            get { return this.GetTable<Unit>(); }
        }

        public DbLinq.Data.Linq.Table<MapPoint> MapPoints
        {
            get { return this.GetTable<MapPoint>(); }
        }

        public DbLinq.Data.Linq.Table<MapSector> MapSectors
        {
            get { return this.GetTable<MapSector>(); }
        }

        public DbLinq.Data.Linq.Table<MultyObject> MultyObjects
        {
            get { return this.GetTable<MultyObject>(); }
        }

        public DbLinq.Data.Linq.Table<SingleObject> SingleObjects
        {
            get { return this.GetTable<SingleObject>(); }
        }

        public DbLinq.Data.Linq.Table<ResourceStore> ResourceStores
        {
            get { return this.GetTable<ResourceStore>(); }
        }

        public DbLinq.Data.Linq.Table<FigureStore> FigureStores
        {
            get { return this.GetTable<FigureStore>(); }
        }

        public DbLinq.Data.Linq.Table<Vicegerent> Vicegerenrs
        {
            get { return this.GetTable<Vicegerent>(); }
        }

        public DbLinq.Data.Linq.Table<Level> Levels
        {
            get { return this.GetTable<Level>(); }
        }

        public DbLinq.Data.Linq.Table<Store> Stores
        {
            get { return this.GetTable<Store>(); }
        }

        public DbLinq.Data.Linq.Table<Empire> Empires
        {
            get { return this.GetTable<Empire>(); }
        }

        public DbLinq.Data.Linq.Table<Union> Unions
        {
            get { return this.GetTable<Union>(); }
        }

        public DbLinq.Data.Linq.Table<Successor> Successors
        {
            get { return this.GetTable<Successor>(); }
        }

        public DbLinq.Data.Linq.Table<Landscape> Landscapes
        {
            get { return this.GetTable<Landscape>(); }
        }
    }
}
