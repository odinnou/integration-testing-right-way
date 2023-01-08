using AutoFixture;
using Service.DrivenAdapters.DatabaseAdapters;
using Service.DrivenAdapters.DatabaseAdapters.Entities;
using System.Globalization;

namespace Tests.Fixtures;

public static class PandaData
{
    public static class Constants
    {
        public static Guid PandaId = Guid.Parse("4d84d305-6648-466f-b109-bca8f8af1606");
        public static Guid PandaId2 = Guid.Parse("4d84d305-5555-2222-0000-bca8f8af1606");
        public const string Latitude = "47.247703075231634";
        public const string Longitude = "1.3531078346932406";
        public const string Address = "ZooParc de Beauval, Av. du Blanc, 41110 Saint-Aignan";
    }

    public static async Task PopulateDataTestForFetching(PandaContext pandaContext, IFixture fixture)
    {
        PandaEntity pandaEntity = fixture.Build<PandaEntity>()
                                        .With(panda => panda.Id, Constants.PandaId)
                                        .With(panda => panda.Longitude, decimal.Parse(Constants.Longitude, CultureInfo.InvariantCulture))
                                        .With(panda => panda.Latitude, decimal.Parse(Constants.Latitude, CultureInfo.InvariantCulture))
                                        .Create();

        PandaEntity pandaEntity2 = fixture.Build<PandaEntity>()
                                        .With(panda => panda.Id, Constants.PandaId2)
                                        .Create();

        pandaContext.Pandas.Add(pandaEntity);
        pandaContext.Pandas.Add(pandaEntity2);

        await pandaContext.SaveChangesAsync();
    }
}