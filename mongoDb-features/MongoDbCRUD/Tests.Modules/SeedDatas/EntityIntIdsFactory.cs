using System;
using System.Collections.Generic;
using MongoDbCRUD._1_Domain.Entities;

namespace Tests.Modules.SeedDatas;

public static class EntityIntIdsFactory
{
    public static List<EntityIntId> SeedList { get; } = new()
    {
        new(id: 1, name: "Entity_1"),
        new(id: 2, name: "Entity_2"),
        new(id: 3, name: "Entity_3"),
        new(id: 4, name: "Entity_4")
    };
}