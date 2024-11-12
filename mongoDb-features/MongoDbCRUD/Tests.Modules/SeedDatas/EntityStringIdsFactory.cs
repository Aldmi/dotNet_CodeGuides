using System;
using System.Collections.Generic;
using MongoDbCRUD._1_Domain.Entities;

namespace Tests.Modules.SeedDatas;

public static class EntityStringIdsFactory
{
    public static List<EntityStringId> SeedList { get; } = new()
    {
        new(key: "1", name: "Entity_1"),
        new(key: "2", name: "Entity_2"),
        new(key: "3", name: "Entity_3"),
        new(key: "4", name: "Entity_4")
    };
}